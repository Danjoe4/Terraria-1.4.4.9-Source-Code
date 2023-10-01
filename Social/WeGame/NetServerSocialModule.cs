using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using rail;
using Terraria.Localization;
using Terraria.Net;
using Terraria.Net.Sockets;

namespace Terraria.Social.WeGame;

public class NetServerSocialModule : NetSocialModule
{
	private SocketConnectionAccepted _connectionAcceptedCallback;
	private bool _acceptingClients;
	private ServerMode _mode;
	private RailCallBackHelper _callbackHelper = new RailCallBackHelper();
	private MessageDispatcherClient _client = new MessageDispatcherClient();
	private bool _serverConnected;
	private RailID _serverID = new RailID();
	private Action _ipcConnetedAction;
	private List<RailFriendInfo> _wegameFriendList;

	public NetServerSocialModule()
	{
		_lobby._lobbyCreatedExternalCallback = OnLobbyCreated;
	}

	private void BroadcastConnectedUsers()
	{
	}

	private bool AcceptAnUserSession(RailID local_peer, RailID remote_peer)
	{
		bool result = false;
		WeGameHelper.WriteDebugString("AcceptAnUserSession server:" + local_peer.id_ + " remote:" + remote_peer.id_);
		IRailNetwork railNetwork = rail_api.RailFactory().RailNetworkHelper();
		if (railNetwork != null)
			result = railNetwork.AcceptSessionRequest(local_peer, remote_peer) == RailResult.kSuccess;

		return result;
	}

	private void TerminateRemotePlayerSession(RailID remote_id)
	{
		rail_api.RailFactory().RailPlayer()?.TerminateSessionOfPlayer(remote_id);
	}

	private bool CloseNetWorkSession(RailID remote_peer)
	{
		bool result = false;
		IRailNetwork railNetwork = rail_api.RailFactory().RailNetworkHelper();
		if (railNetwork != null)
			result = railNetwork.CloseSession(_serverID, remote_peer) == RailResult.kSuccess;

		return result;
	}

	private RailID GetServerID()
	{
		RailID railID = null;
		IRailGameServer server = _lobby.GetServer();
		if (server != null)
			railID = server.GetGameServerRailID();

		return railID ?? new RailID();
	}

	private void CloseAndUpdateUserState(RailID remote_peer)
	{
		if (_connectionStateMap.ContainsKey(remote_peer)) {
			WeGameHelper.WriteDebugString("CloseAndUpdateUserState, remote:{0}", remote_peer.id_);
			TerminateRemotePlayerSession(remote_peer);
			CloseNetWorkSession(remote_peer);
			_connectionStateMap[remote_peer] = ConnectionState.Inactive;
			_reader.ClearUser(remote_peer);
			_writer.ClearUser(remote_peer);
		}
	}

	public void OnConnected()
	{
		_serverConnected = true;
		if (_ipcConnetedAction != null)
			_ipcConnetedAction();

		_ipcConnetedAction = null;
		WeGameHelper.WriteDebugString("IPC connected");
	}

	private void OnCreateSessionRequest(CreateSessionRequest data)
	{
		if (!_acceptingClients) {
			WeGameHelper.WriteDebugString(" - Ignoring connection from " + data.remote_peer.id_ + " while _acceptionClients is false.");
			return;
		}

		if (!_mode.HasFlag(ServerMode.FriendsOfFriends) && !IsWeGameFriend(data.remote_peer)) {
			WeGameHelper.WriteDebugString("Ignoring connection from " + data.remote_peer.id_ + ". Friends of friends is disabled.");
			return;
		}

		WeGameHelper.WriteDebugString("pass wegame friend check");
		AcceptAnUserSession(data.local_peer, data.remote_peer);
		_connectionStateMap[data.remote_peer] = ConnectionState.Authenticating;
		if (_connectionAcceptedCallback != null)
			_connectionAcceptedCallback(new SocialSocket(new WeGameAddress(data.remote_peer, "")));
	}

	private void OnCreateSessionFailed(CreateSessionFailed data)
	{
		WeGameHelper.WriteDebugString("CreateSessionFailed, local:{0}, remote:{1}", data.local_peer.id_, data.remote_peer.id_);
		CloseAndUpdateUserState(data.remote_peer);
	}

	private bool GetRailFriendList(List<RailFriendInfo> list)
	{
		bool result = false;
		IRailFriends railFriends = rail_api.RailFactory().RailFriends();
		if (railFriends != null)
			result = railFriends.GetFriendsList(list) == RailResult.kSuccess;

		return result;
	}

	private void OnWegameMessage(IPCMessage message)
	{
		IPCMessageType cmd = message.GetCmd();
		if (cmd == IPCMessageType.IPCMessageTypeNotifyFriendList) {
			message.Parse<WeGameFriendListInfo>(out var value);
			UpdateFriendList(value);
		}
	}

	private void UpdateFriendList(WeGameFriendListInfo friendListInfo)
	{
		_wegameFriendList = friendListInfo._friendList;
		WeGameHelper.WriteDebugString("On update friend list - " + DumpFriendListString(friendListInfo._friendList));
	}

	private bool IsWeGameFriend(RailID id)
	{
		bool result = false;
		if (_wegameFriendList != null) {
			foreach (RailFriendInfo wegameFriend in _wegameFriendList) {
				if (wegameFriend.friend_rail_id == id)
					return true;
			}

			return result;
		}

		return result;
	}

	private string DumpFriendListString(List<RailFriendInfo> list)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (RailFriendInfo item in list) {
			stringBuilder.AppendLine($"friend_id: {item.friend_rail_id.id_}, type: {item.friend_type}, online: {item.online_state.friend_online_state.ToString()}, playing: {item.online_state.game_define_game_playing_state}");
		}

		return stringBuilder.ToString();
	}

	private bool IsActiveUser(RailID user)
	{
		if (_connectionStateMap.ContainsKey(user))
			return _connectionStateMap[user] != ConnectionState.Inactive;

		return false;
	}

	private void UpdateUserStateBySessionAuthResult(GameServerStartSessionWithPlayerResponse data)
	{
		RailID remote_rail_id = data.remote_rail_id;
		RailResult result = data.result;
		if (_connectionStateMap.ContainsKey(remote_rail_id)) {
			if (result == RailResult.kSuccess) {
				WeGameHelper.WriteDebugString("UpdateUserStateBySessionAuthResult Auth Success");
				BroadcastConnectedUsers();
			}
			else {
				WeGameHelper.WriteDebugString("UpdateUserStateBySessionAuthResult Auth Failed");
				CloseAndUpdateUserState(remote_rail_id);
			}
		}
	}

	private bool TryAuthUserByRecvData(RailID user, byte[] data, int length)
	{
		WeGameHelper.WriteDebugString("TryAuthUserByRecvData user:{0}", user.id_);
		if (length < 3) {
			WeGameHelper.WriteDebugString("Failed to validate authentication packet: Too short. (Length: " + length + ")");
			return false;
		}

		int num = (data[1] << 8) | data[0];
		if (num != length) {
			WeGameHelper.WriteDebugString("Failed to validate authentication packet: Packet size mismatch. (" + num + "!=" + length + ")");
			return false;
		}

		if (data[2] != 93) {
			WeGameHelper.WriteDebugString("Failed to validate authentication packet: Packet type is not correct. (Type: " + data[2] + ")");
			return false;
		}

		return true;
	}

	private bool OnPacketRead(byte[] data, int size, RailID user)
	{
		if (!IsActiveUser(user)) {
			WeGameHelper.WriteDebugString("OnPacketRead IsActiveUser false");
			return false;
		}

		ConnectionState connectionState = _connectionStateMap[user];
		if (connectionState == ConnectionState.Authenticating) {
			if (!TryAuthUserByRecvData(user, data, size))
				CloseAndUpdateUserState(user);
			else
				OnAuthSuccess(user);

			return false;
		}

		return connectionState == ConnectionState.Connected;
	}

	private void OnAuthSuccess(RailID remote_peer)
	{
		if (_connectionStateMap.ContainsKey(remote_peer)) {
			_connectionStateMap[remote_peer] = ConnectionState.Connected;
			int num = 3;
			byte[] array = new byte[num];
			array[0] = (byte)((uint)num & 0xFFu);
			array[1] = (byte)((uint)(num >> 8) & 0xFFu);
			array[2] = 93;
			rail_api.RailFactory().RailNetworkHelper().SendReliableData(_serverID, remote_peer, array, (uint)num);
		}
	}

	public void OnRailEvent(RAILEventID event_id, EventBase data)
	{
		WeGameHelper.WriteDebugString("OnRailEvent,id=" + event_id.ToString() + " ,result=" + data.result);
		switch (event_id) {
			case RAILEventID.kRailEventGameServerAuthSessionTicket:
				UpdateUserStateBySessionAuthResult((GameServerStartSessionWithPlayerResponse)data);
				break;
			case RAILEventID.kRailEventNetworkCreateSessionRequest:
				OnCreateSessionRequest((CreateSessionRequest)data);
				break;
			case RAILEventID.kRailEventNetworkCreateSessionFailed:
				OnCreateSessionFailed((CreateSessionFailed)data);
				break;
		}
	}

	private void OnLobbyCreated(RailID lobbyID)
	{
		WeGameHelper.WriteDebugString("SetLocalPeer: {0}", lobbyID.id_);
		_reader.SetLocalPeer(lobbyID);
		_writer.SetLocalPeer(lobbyID);
		_serverID = lobbyID;
		Action action = delegate {
			ReportServerID t = new ReportServerID {
				_serverID = lobbyID.id_.ToString()
			};

			IPCMessage iPCMessage = new IPCMessage();
			iPCMessage.Build(IPCMessageType.IPCMessageTypeReportServerID, t);
			WeGameHelper.WriteDebugString("Send serverID to game client - " + _client.SendMessage(iPCMessage));
		};

		if (_serverConnected) {
			action();
			return;
		}

		_ipcConnetedAction = (Action)Delegate.Combine(_ipcConnetedAction, action);
		WeGameHelper.WriteDebugString("report server id fail, no connection");
	}

	private void RegisterRailEvent()
	{
		RAILEventID[] array = new RAILEventID[4] {
			RAILEventID.kRailEventNetworkCreateSessionRequest,
			RAILEventID.kRailEventNetworkCreateSessionFailed,
			RAILEventID.kRailEventGameServerAuthSessionTicket,
			RAILEventID.kRailEventGameServerGetSessionTicket
		};

		foreach (RAILEventID event_id in array) {
			_callbackHelper.RegisterCallback(event_id, OnRailEvent);
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		_mode |= ServerMode.Lobby;
		RegisterRailEvent();
		_reader.SetReadEvent(OnPacketRead);
		if (Program.LaunchParameters.ContainsKey("-lobby")) {
			_mode |= ServerMode.Lobby;
			string text = Program.LaunchParameters["-lobby"];
			if (!(text == "private")) {
				if (text == "friends") {
					_mode |= ServerMode.FriendsCanJoin;
					_lobby.Create(inviteOnly: false);
				}
				else {
					Console.WriteLine(Language.GetTextValue("Error.InvalidLobbyFlag", "private", "friends"));
				}
			}
			else {
				_lobby.Create(inviteOnly: true);
			}
		}

		if (Program.LaunchParameters.ContainsKey("-friendsoffriends"))
			_mode |= ServerMode.FriendsOfFriends;

		_client.Init("WeGame.Terraria.Message.Client", "WeGame.Terraria.Message.Server");
		_client.OnConnected += OnConnected;
		_client.OnMessage += OnWegameMessage;
		CoreSocialModule.OnTick += _client.Tick;
		_client.Start();
	}

	public override ulong GetLobbyId() => _serverID.id_;

	public override void OpenInviteInterface()
	{
	}

	public override void CancelJoin()
	{
	}

	public override bool CanInvite() => false;

	public override void LaunchLocalServer(Process process, ServerMode mode)
	{
	}

	public override bool StartListening(SocketConnectionAccepted callback)
	{
		_acceptingClients = true;
		_connectionAcceptedCallback = callback;
		return false;
	}

	public override void StopListening()
	{
		_acceptingClients = false;
	}

	public override void Connect(RemoteAddress address)
	{
	}

	public override void Close(RemoteAddress address)
	{
		RailID remote_peer = RemoteAddressToRailId(address);
		CloseAndUpdateUserState(remote_peer);
	}
}
