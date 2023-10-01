using System;
using System.Collections.Generic;
using rail;

namespace Terraria.Social.WeGame;

public class WeGameP2PReader
{
	public class ReadResult
	{
		public byte[] Data;
		public uint Size;
		public uint Offset;

		public ReadResult(byte[] data, uint size)
		{
			Data = data;
			Size = size;
			Offset = 0u;
		}
	}

	public delegate bool OnReadEvent(byte[] data, int size, RailID user);

	public object RailLock = new object();
	private const int BUFFER_SIZE = 4096;
	private Dictionary<RailID, Queue<ReadResult>> _pendingReadBuffers = new Dictionary<RailID, Queue<ReadResult>>();
	private Queue<RailID> _deletionQueue = new Queue<RailID>();
	private Queue<byte[]> _bufferPool = new Queue<byte[]>();
	private OnReadEvent _readEvent;
	private RailID _local_id;

	public void ClearUser(RailID id)
	{
		lock (_pendingReadBuffers) {
			_deletionQueue.Enqueue(id);
		}
	}

	public bool IsDataAvailable(RailID id)
	{
		lock (_pendingReadBuffers) {
			if (!_pendingReadBuffers.ContainsKey(id))
				return false;

			Queue<ReadResult> queue = _pendingReadBuffers[id];
			if (queue.Count == 0 || queue.Peek().Size == 0)
				return false;

			return true;
		}
	}

	public void SetReadEvent(OnReadEvent method)
	{
		_readEvent = method;
	}

	private bool IsPacketAvailable(out uint size)
	{
		lock (RailLock) {
			return rail_api.RailFactory().RailNetworkHelper().IsDataReady(new RailID {
				id_ = GetLocalPeer().id_
			}, out size);
		}
	}

	private RailID GetLocalPeer() => _local_id;

	public void SetLocalPeer(RailID rail_id)
	{
		if (_local_id == null)
			_local_id = new RailID();

		_local_id.id_ = rail_id.id_;
	}

	private bool IsValid()
	{
		if (_local_id != null)
			return _local_id.IsValid();

		return false;
	}

	public void ReadTick()
	{
		if (!IsValid())
			return;

		lock (_pendingReadBuffers) {
			while (_deletionQueue.Count > 0) {
				_pendingReadBuffers.Remove(_deletionQueue.Dequeue());
			}

			uint size;
			while (IsPacketAvailable(out size)) {
				byte[] array = ((_bufferPool.Count != 0) ? _bufferPool.Dequeue() : new byte[Math.Max(size, 4096u)]);
				RailID railID = new RailID();
				bool flag;
				lock (RailLock) {
					flag = rail_api.RailFactory().RailNetworkHelper().ReadData(GetLocalPeer(), railID, array, size) == RailResult.kSuccess;
				}

				if (!flag)
					continue;

				if (_readEvent == null || _readEvent(array, (int)size, railID)) {
					if (!_pendingReadBuffers.ContainsKey(railID))
						_pendingReadBuffers[railID] = new Queue<ReadResult>();

					_pendingReadBuffers[railID].Enqueue(new ReadResult(array, size));
				}
				else {
					_bufferPool.Enqueue(array);
				}
			}
		}
	}

	public int Receive(RailID user, byte[] buffer, int bufferOffset, int bufferSize)
	{
		uint num = 0u;
		lock (_pendingReadBuffers) {
			if (!_pendingReadBuffers.ContainsKey(user))
				return 0;

			Queue<ReadResult> queue = _pendingReadBuffers[user];
			while (queue.Count > 0) {
				ReadResult readResult = queue.Peek();
				uint num2 = Math.Min((uint)bufferSize - num, readResult.Size - readResult.Offset);
				if (num2 == 0)
					return (int)num;

				Array.Copy(readResult.Data, readResult.Offset, buffer, bufferOffset + num, num2);
				if (num2 == readResult.Size - readResult.Offset)
					_bufferPool.Enqueue(queue.Dequeue().Data);
				else
					readResult.Offset += num2;

				num += num2;
			}

			return (int)num;
		}
	}
}
