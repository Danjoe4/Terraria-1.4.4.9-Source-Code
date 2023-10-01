using System;
using System.Collections.Generic;
using System.Threading;
using Steamworks;
using Terraria.Social.Base;

namespace Terraria.Social.Steam;

public class AchievementsSocialModule : Terraria.Social.Base.AchievementsSocialModule
{
	private const string FILE_NAME = "/achievements-steam.dat";
	private Callback<UserStatsReceived_t> _userStatsReceived;
	private bool _areStatsReceived;
	private Dictionary<string, int> _intStatCache = new Dictionary<string, int>();
	private Dictionary<string, float> _floatStatCache = new Dictionary<string, float>();

	public override void Initialize()
	{
		_userStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
		SteamUserStats.RequestCurrentStats();
		while (!_areStatsReceived) {
			CoreSocialModule.Pulse();
			Thread.Sleep(10);
		}
	}

	public override void Shutdown()
	{
		_userStatsReceived.Unregister();
		StoreStats();
	}

	public override bool IsAchievementCompleted(string name)
	{
		bool pbAchieved;
		return SteamUserStats.GetAchievement(name, out pbAchieved) && pbAchieved;
	}

	public override byte[] GetEncryptionKey()
	{
		byte[] array = new byte[16];
		byte[] bytes = BitConverter.GetBytes(SteamUser.GetSteamID().m_SteamID);
		Array.Copy(bytes, array, 8);
		Array.Copy(bytes, 0, array, 8, 8);
		return array;
	}

	public override string GetSavePath() => "/achievements-steam.dat";

	private int GetIntStat(string name)
	{
		if (_intStatCache.TryGetValue(name, out var value))
			return value;

		if (SteamUserStats.GetStat(name, out value))
			_intStatCache.Add(name, value);

		return value;
	}

	private float GetFloatStat(string name)
	{
		if (_floatStatCache.TryGetValue(name, out var value))
			return value;

		if (SteamUserStats.GetStat(name, out value))
			_floatStatCache.Add(name, value);

		return value;
	}

	private bool SetFloatStat(string name, float value)
	{
		_floatStatCache[name] = value;
		return SteamUserStats.SetStat(name, value);
	}

	public override void UpdateIntStat(string name, int value)
	{
		if (GetIntStat(name) < value)
			SetIntStat(name, value);
	}

	private bool SetIntStat(string name, int value)
	{
		_intStatCache[name] = value;
		return SteamUserStats.SetStat(name, value);
	}

	public override void UpdateFloatStat(string name, float value)
	{
		if (GetFloatStat(name) < value)
			SetFloatStat(name, value);
	}

	public override void StoreStats()
	{
		SteamUserStats.StoreStats();
	}

	public override void CompleteAchievement(string name)
	{
		SteamUserStats.SetAchievement(name);
	}

	private void OnUserStatsReceived(UserStatsReceived_t results)
	{
		if (results.m_nGameID == 105600 && results.m_steamIDUser == SteamUser.GetSteamID())
			_areStatsReceived = true;
	}
}
