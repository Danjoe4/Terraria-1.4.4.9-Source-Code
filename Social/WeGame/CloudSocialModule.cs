using System.Collections.Generic;
using rail;
using Terraria.Social.Base;

namespace Terraria.Social.WeGame;

public class CloudSocialModule : Terraria.Social.Base.CloudSocialModule
{
	private object ioLock = new object();

	public override void Initialize()
	{
	}

	public override void Shutdown()
	{
	}

	public override IEnumerable<string> GetFiles()
	{
		lock (ioLock) {
			uint fileCount = rail_api.RailFactory().RailStorageHelper().GetFileCount();
			List<string> list = new List<string>((int)fileCount);
			ulong file_size = 0uL;
			for (uint num = 0u; num < fileCount; num++) {
				rail_api.RailFactory().RailStorageHelper().GetFileNameAndSize(num, out var filename, out file_size);
				list.Add(filename);
			}

			return list;
		}
	}

	public override bool Write(string path, byte[] data, int length)
	{
		lock (ioLock) {
			bool result = true;
			IRailFile railFile = null;
			railFile = ((!rail_api.RailFactory().RailStorageHelper().IsFileExist(path)) ? rail_api.RailFactory().RailStorageHelper().CreateFile(path) : rail_api.RailFactory().RailStorageHelper().OpenFile(path));
			if (railFile != null) {
				railFile.Write(data, (uint)length);
				railFile.Close();
			}
			else {
				result = false;
			}

			return result;
		}
	}

	public override int GetFileSize(string path)
	{
		lock (ioLock) {
			IRailFile railFile = rail_api.RailFactory().RailStorageHelper().OpenFile(path);
			if (railFile != null) {
				int size = (int)railFile.GetSize();
				railFile.Close();
				return size;
			}

			return 0;
		}
	}

	public override void Read(string path, byte[] buffer, int size)
	{
		lock (ioLock) {
			IRailFile railFile = rail_api.RailFactory().RailStorageHelper().OpenFile(path);
			if (railFile != null) {
				railFile.Read(buffer, (uint)size);
				railFile.Close();
			}
		}
	}

	public override bool HasFile(string path)
	{
		lock (ioLock) {
			return rail_api.RailFactory().RailStorageHelper().IsFileExist(path);
		}
	}

	public override bool Delete(string path)
	{
		lock (ioLock) {
			RailResult railResult = rail_api.RailFactory().RailStorageHelper().RemoveFile(path);
			return railResult == RailResult.kSuccess;
		}
	}

	public override bool Forget(string path) => Delete(path);
}
