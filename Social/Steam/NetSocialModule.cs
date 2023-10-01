using System.Collections.Concurrent;
using System.IO;
using Steamworks;
using Terraria.Net;
using Terraria.Social.Base;

namespace Terraria.Social.Steam;

public abstract class NetSocialModule : Terraria.Social.Base.NetSocialModule
{
	public enum ConnectionState
	{
		Inactive,
		Authenticating,
		Connected
	}

	protected delegate void AsyncHandshake(CSteamID client);

	protected const int ServerReadChannel = 1;
	protected const int ClientReadChannel = 2;
	protected const int LobbyMessageJoin = 1;
	protected const ushort GamePort = 27005;
	protected const ushort SteamPort = 27006;
	protected const ushort QueryPort = 27007;
	protected static readonly byte[] _handshake = new byte[10] {
		10,
		0,
		93,
		114,
		101,
		108,
		111,
		103,
		105,
		99
	};
	protected SteamP2PReader _reader;
	protected SteamP2PWriter _writer;
	protected Lobby _lobby = new Lobby();
	protected ConcurrentDictionary<CSteamID, ConnectionState> _connectionStateMap = new ConcurrentDictionary<CSteamID, ConnectionState>();
	protected object _steamLock = new object();
	private Callback<LobbyChatMsg_t> _lobbyChatMessage;

	protected NetSocialModule(int readChannel, int writeChannel)
	{
		_reader = new SteamP2PReader(readChannel);
		_writer = new SteamP2PWriter(writeChannel);
	}

	public override void Initialize()
	{
		CoreSocialModule.OnTick += _reader.ReadTick;
		CoreSocialModule.OnTick += _writer.SendAll;
		_lobbyChatMessage = Callback<LobbyChatMsg_t>.Create(OnLobbyChatMessage);
	}

	public override void Shutdown()
	{
		_lobby.Leave();
	}

	public override bool IsConnected(RemoteAddress address)
	{
		if (address == null)
			return false;

		CSteamID cSteamID = RemoteAddressToSteamId(address);
		if (!_connectionStateMap.ContainsKey(cSteamID) || _connectionStateMap[cSteamID] != ConnectionState.Connected)
			return false;

		if (GetSessionState(cSteamID).m_bConnectionActive != 1) {
			Close(address);
			return false;
		}

		return true;
	}

	protected virtual void OnLobbyChatMessage(LobbyChatMsg_t result)
	{
		if (result.m_ulSteamIDLobby != _lobby.Id.m_SteamID || result.m_eChatEntryType != 1 || result.m_ulSteamIDUser != _lobby.Owner.m_SteamID)
			return;

		byte[] message = _lobby.GetMessage((int)result.m_iChatID);
		if (message.Length == 0)
			return;

		using MemoryStream memoryStream = new MemoryStream(message);
		using BinaryReader binaryReader = new BinaryReader(memoryStream);
		byte b = binaryReader.ReadByte();
		if (b != 1)
			return;

		while (message.Length - memoryStream.Position >= 8) {
			CSteamID cSteamID = new CSteamID(binaryReader.ReadUInt64());
			if (cSteamID != SteamUser.GetSteamID())
				_lobby.SetPlayedWith(cSteamID);
		}
	}

	protected P2PSessionState_t GetSessionState(CSteamID userId)
	{
		SteamNetworking.GetP2PSessionState(userId, out var pConnectionState);
		return pConnectionState;
	}

	protected CSteamID RemoteAddressToSteamId(RemoteAddress address) => ((SteamAddress)address).SteamId;

	public override bool Send(RemoteAddress address, byte[] data, int length)
	{
		CSteamID user = RemoteAddressToSteamId(address);
		_writer.QueueSend(user, data, length);
		return true;
	}

	public override int Receive(RemoteAddress address, byte[] data, int offset, int length)
	{
		if (address == null)
			return 0;

		CSteamID user = RemoteAddressToSteamId(address);
		return _reader.Receive(user, data, offset, length);
	}

	public override bool IsDataAvailable(RemoteAddress address)
	{
		CSteamID id = RemoteAddressToSteamId(address);
		return _reader.IsDataAvailable(id);
	}
}
