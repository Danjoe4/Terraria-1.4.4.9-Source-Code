using System;
using System.Threading;
using rail;
using Terraria.Social.Base;

namespace Terraria.Social.WeGame;

public class AchievementsSocialModule : Terraria.Social.Base.AchievementsSocialModule
{
	private const string FILE_NAME = "/achievements-wegame.dat";
	private bool _areStatsReceived;
	private bool _areAchievementReceived;
	private RailCallBackHelper _callbackHelper = new RailCallBackHelper();
	private IRailPlayerAchievement _playerAchievement;
	private IRailPlayerStats _playerStats;

	public override void Initialize()
	{
		_callbackHelper.RegisterCallback(RAILEventID.kRailEventStatsPlayerStatsReceived, RailEventCallBack);
		_callbackHelper.RegisterCallback(RAILEventID.kRailEventAchievementPlayerAchievementReceived, RailEventCallBack);
		IRailPlayerStats myPlayerStats = GetMyPlayerStats();
		IRailPlayerAchievement myPlayerAchievement = GetMyPlayerAchievement();
		if (myPlayerStats != null && myPlayerAchievement != null) {
			myPlayerStats.AsyncRequestStats("");
			myPlayerAchievement.AsyncRequestAchievement("");
			while (!_areStatsReceived && !_areAchievementReceived) {
				CoreSocialModule.RailEventTick();
				Thread.Sleep(10);
			}
		}
	}

	public override void Shutdown()
	{
		StoreStats();
	}

	private IRailPlayerStats GetMyPlayerStats()
	{
		if (_playerStats == null) {
			IRailStatisticHelper railStatisticHelper = rail_api.RailFactory().RailStatisticHelper();
			if (railStatisticHelper != null) {
				RailID railID = new RailID();
				railID.id_ = 0uL;
				_playerStats = railStatisticHelper.CreatePlayerStats(railID);
			}
		}

		return _playerStats;
	}

	private IRailPlayerAchievement GetMyPlayerAchievement()
	{
		if (_playerAchievement == null) {
			IRailAchievementHelper railAchievementHelper = rail_api.RailFactory().RailAchievementHelper();
			if (railAchievementHelper != null) {
				RailID railID = new RailID();
				railID.id_ = 0uL;
				_playerAchievement = railAchievementHelper.CreatePlayerAchievement(railID);
			}
		}

		return _playerAchievement;
	}

	public void RailEventCallBack(RAILEventID eventId, EventBase data)
	{
		switch (eventId) {
			case RAILEventID.kRailEventStatsPlayerStatsReceived:
				_areStatsReceived = true;
				break;
			case RAILEventID.kRailEventAchievementPlayerAchievementReceived:
				_areAchievementReceived = true;
				break;
		}
	}

	public override bool IsAchievementCompleted(string name)
	{
		bool achieved = false;
		RailResult railResult = RailResult.kFailure;
		IRailPlayerAchievement myPlayerAchievement = GetMyPlayerAchievement();
		if (myPlayerAchievement != null)
			railResult = myPlayerAchievement.HasAchieved(name, out achieved);

		if (achieved)
			return railResult == RailResult.kSuccess;

		return false;
	}

	public override byte[] GetEncryptionKey()
	{
		RailID railID = rail_api.RailFactory().RailPlayer().GetRailID();
		byte[] array = new byte[16];
		byte[] bytes = BitConverter.GetBytes(railID.id_);
		Array.Copy(bytes, array, 8);
		Array.Copy(bytes, 0, array, 8, 8);
		return array;
	}

	public override string GetSavePath() => "/achievements-wegame.dat";

	private int GetIntStat(string name)
	{
		int data = 0;
		GetMyPlayerStats()?.GetStatValue(name, out data);
		return data;
	}

	private float GetFloatStat(string name)
	{
		double data = 0.0;
		GetMyPlayerStats()?.GetStatValue(name, out data);
		return (float)data;
	}

	private bool SetFloatStat(string name, float value)
	{
		IRailPlayerStats myPlayerStats = GetMyPlayerStats();
		RailResult railResult = RailResult.kFailure;
		if (myPlayerStats != null)
			railResult = myPlayerStats.SetStatValue(name, value);

		return railResult == RailResult.kSuccess;
	}

	public override void UpdateIntStat(string name, int value)
	{
		IRailPlayerStats myPlayerStats = GetMyPlayerStats();
		if (myPlayerStats != null) {
			int data = 0;
			if (myPlayerStats.GetStatValue(name, out data) == RailResult.kSuccess && data < value)
				myPlayerStats.SetStatValue(name, value);
		}
	}

	private bool SetIntStat(string name, int value)
	{
		IRailPlayerStats myPlayerStats = GetMyPlayerStats();
		RailResult railResult = RailResult.kFailure;
		if (myPlayerStats != null)
			railResult = myPlayerStats.SetStatValue(name, value);

		return railResult == RailResult.kSuccess;
	}

	public override void UpdateFloatStat(string name, float value)
	{
		IRailPlayerStats myPlayerStats = GetMyPlayerStats();
		if (myPlayerStats != null) {
			double data = 0.0;
			if (myPlayerStats.GetStatValue(name, out data) == RailResult.kSuccess && (float)data < value)
				myPlayerStats.SetStatValue(name, value);
		}
	}

	public override void StoreStats()
	{
		SaveStats();
		SaveAchievement();
	}

	private void SaveStats()
	{
		GetMyPlayerStats()?.AsyncStoreStats("");
	}

	private void SaveAchievement()
	{
		GetMyPlayerAchievement()?.AsyncStoreAchievement("");
	}

	public override void CompleteAchievement(string name)
	{
		GetMyPlayerAchievement()?.MakeAchievement(name);
	}
}
