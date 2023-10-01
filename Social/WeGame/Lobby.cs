using System;
using rail;

namespace Terraria.Social.WeGame;

public class Lobby
{
	private RailCallBackHelper _callbackHelper = new RailCallBackHelper();
	public LobbyState State;
	private bool _gameServerInitSuccess;
	private IRailGameServer _gameServer;
	public Action<RailID> _lobbyCreatedExternalCallback;
	private RailID _server_id;

	private IRailGameServer RailServerHelper {
		get {
			if (_gameServerInitSuccess)
				return _gameServer;

			return null;
		}
		set {
			_gameServer = value;
		}
	}

	private IRailGameServerHelper GetRailServerHelper() => rail_api.RailFactory().RailGameServerHelper();

	private void RegisterGameServerEvent()
	{
		if (_callbackHelper != null)
			_callbackHelper.RegisterCallback(RAILEventID.kRailEventGameServerCreated, OnRailEvent);
	}

	public void OnRailEvent(RAILEventID id, EventBase data)
	{
		WeGameHelper.WriteDebugString("OnRailEvent,id=" + id.ToString() + " ,result=" + data.result);
		if (id == RAILEventID.kRailEventGameServerCreated)
			OnGameServerCreated((CreateGameServerResult)data);
	}

	private void OnGameServerCreated(CreateGameServerResult result)
	{
		if (result.result == RailResult.kSuccess) {
			_gameServerInitSuccess = true;
			_lobbyCreatedExternalCallback(result.game_server_id);
			_server_id = result.game_server_id;
		}
	}

	public void Create(bool inviteOnly)
	{
		if (State == LobbyState.Inactive)
			RegisterGameServerEvent();

		IRailGameServer railServerHelper = rail_api.RailFactory().RailGameServerHelper().AsyncCreateGameServer(new CreateGameServerOptions {
			has_password = false,
			enable_team_voice = false
		}, "terraria", "terraria");

		RailServerHelper = railServerHelper;
		State = LobbyState.Creating;
	}

	public void OpenInviteOverlay()
	{
		WeGameHelper.WriteDebugString("OpenInviteOverlay by wegame");
		rail_api.RailFactory().RailFloatingWindow().AsyncShowRailFloatingWindow(EnumRailWindowType.kRailWindowFriendList, "");
	}

	public void Join(RailID local_peer, RailID remote_peer)
	{
		if (State != 0)
			WeGameHelper.WriteDebugString("Lobby connection attempted while already in a lobby. This should never happen?");
		else
			State = LobbyState.Connecting;
	}

	public byte[] GetMessage(int index) => null;
	public int GetUserCount() => 0;
	public RailID GetUserByIndex(int index) => null;
	public bool SendMessage(byte[] data) => SendMessage(data, data.Length);
	public bool SendMessage(byte[] data, int length) => false;

	public void Set(RailID lobbyId)
	{
	}

	public void SetPlayedWith(RailID userId)
	{
	}

	public void Leave()
	{
		State = LobbyState.Inactive;
	}

	public IRailGameServer GetServer() => RailServerHelper;
}
