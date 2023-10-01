using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using rail;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Net;
using Terraria.Net.Sockets;

namespace Terraria.Social.WeGame;

public class NetClientSocialModule : NetSocialModule
{
	private RailCallBackHelper _callbackHelper = new RailCallBackHelper();
	private bool _hasLocalHost;
	private IPCServer server = new IPCServer();
	private readonly string _serverIDMedataKey = "terraria.serverid";
	private RailID _inviter_id = new RailID();
	private List<PlayerPersonalInfo> _player_info_list;
	private MessageDispatcherServer _msgServer;

	private void OnIPCClientAccess()
	{
		WeGameHelper.WriteDebugString("IPC client access");
		SendFriendListToLocalServer();
	}

	private void LazyCreateWeGameMsgServer()
	{
		if (_msgServer == null) {
			_msgServer = new MessageDispatcherServer();
			_msgServer.Init("WeGame.Terraria.Message.Server");
			_msgServer.OnMessage += OnWegameMessage;
			_msgServer.OnIPCClientAccess += OnIPCClientAccess;
			CoreSocialModule.OnTick += _msgServer.Tick;
			_msgServer.Start();
		}
	}

	private void OnWegameMessage(IPCMessage message)
	{
		if (message.GetCmd() == IPCMessageType.IPCMessageTypeReportServerID) {
			message.Parse<ReportServerID>(out var value);
			OnReportServerID(value);
		}
	}

	private void OnReportServerID(ReportServerID reportServerID)
	{
		WeGameHelper.WriteDebugString("OnReportServerID - " + reportServerID._serverID);
		AsyncSetMyMetaData(_serverIDMedataKey, reportServerID._serverID);
		AsyncSetInviteCommandLine(reportServerID._serverID);
	}

	public override void Initialize()
	{
		base.Initialize();
		RegisterRailEvent();
		AsyncGetFriendsInfo();
		_reader.SetReadEvent(OnPacketRead);
		_reader.SetLocalPeer(GetLocalPeer());
		_writer.SetLocalPeer(GetLocalPeer());
		Main.OnEngineLoad += CheckParameters;
	}

	private void AsyncSetInviteCommandLine(string cmdline)
	{
		rail_api.RailFactory().RailFriends().AsyncSetInviteCommandLine(cmdline, "");
	}

	private void AsyncSetMyMetaData(string key, string value)
	{
		List<RailKeyValue> list = new List<RailKeyValue>();
		RailKeyValue railKeyValue = new RailKeyValue();
		railKeyValue.key = key;
		railKeyValue.value = value;
		list.Add(railKeyValue);
		rail_api.RailFactory().RailFriends().AsyncSetMyMetadata(list, "");
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
		if (!_connectionStateMap.ContainsKey(user))
			return false;

		ConnectionState connectionState = _connectionStateMap[user];
		if (connectionState == ConnectionState.Authenticating) {
			if (!TryAuthUserByRecvData(user, data, size)) {
				WeGameHelper.WriteDebugString(" Auth Server Ticket Failed");
				Close(user);
			}
			else {
				WeGameHelper.WriteDebugString("OnRailAuthSessionTicket Auth Success..");
				OnAuthSuccess(user);
			}

			return false;
		}

		return connectionState == ConnectionState.Connected;
	}

	private void OnAuthSuccess(RailID remote_peer)
	{
		if (_connectionStateMap.ContainsKey(remote_peer)) {
			_connectionStateMap[remote_peer] = ConnectionState.Connected;
			AsyncSetPlayWith(_inviter_id);
			AsyncSetMyMetaData("status", Language.GetTextValue("Social.StatusInGame"));
			AsyncSetMyMetaData(_serverIDMedataKey, remote_peer.id_.ToString());
			Main.clrInput();
			Netplay.ServerPassword = "";
			Main.GetInputText("");
			Main.autoPass = false;
			Main.netMode = 1;
			Netplay.OnConnectedToSocialServer(new SocialSocket(new WeGameAddress(remote_peer, GetFriendNickname(_inviter_id))));
			WeGameHelper.WriteDebugString("OnConnectToSocialServer server:" + remote_peer.id_);
		}
	}

	private bool GetRailConnectIDFromCmdLine(RailID server_id)
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		foreach (string text in commandLineArgs) {
			string text2 = "--rail_connect_cmd=";
			int num = text.IndexOf(text2);
			if (num != -1) {
				ulong result = 0uL;
				if (ulong.TryParse(text.Substring(num + text2.Length), out result)) {
					server_id.id_ = result;
					return true;
				}
			}
		}

		return false;
	}

	private void CheckParameters()
	{
		RailID server_id = new RailID();
		if (!GetRailConnectIDFromCmdLine(server_id))
			return;

		if (server_id.IsValid()) {
			Main.OpenPlayerSelect(delegate (PlayerFileData playerData) {
				Main.ServerSideCharacter = false;
				playerData.SetAsActive();
				Main.menuMode = 882;
				Main.statusText = Language.GetTextValue("Social.Joining");
				WeGameHelper.WriteDebugString(" CheckParameters， lobby.join");
				JoinServer(server_id);
			});
		}
		else {
			WeGameHelper.WriteDebugString("Invalid RailID passed to +connect_lobby");
		}
	}

	public override void LaunchLocalServer(Process process, ServerMode mode)
	{
		if (_lobby.State != 0)
			_lobby.Leave();

		LazyCreateWeGameMsgServer();
		ProcessStartInfo startInfo = process.StartInfo;
		startInfo.Arguments = startInfo.Arguments + " -wegame -localwegameid " + GetLocalPeer().id_;
		if (mode.HasFlag(ServerMode.Lobby)) {
			_hasLocalHost = true;
			if (mode.HasFlag(ServerMode.FriendsCanJoin))
				process.StartInfo.Arguments += " -lobby friends";
			else
				process.StartInfo.Arguments += " -lobby private";

			if (mode.HasFlag(ServerMode.FriendsOfFriends))
				process.StartInfo.Arguments += " -friendsoffriends";
		}

		rail_api.RailFactory().RailUtils().GetLaunchAppParameters(EnumRailLaunchAppType.kRailLaunchAppTypeDedicatedServer, out var parameter);
		ProcessStartInfo startInfo2 = process.StartInfo;
		startInfo2.Arguments = startInfo2.Arguments + " " + parameter;
		WeGameHelper.WriteDebugString("LaunchLocalServer,cmd_line:" + process.StartInfo.Arguments);
		AsyncSetMyMetaData("status", Language.GetTextValue("Social.StatusInGame"));
		Netplay.OnDisconnect += OnDisconnect;
		process.Start();
	}

	public override void Shutdown()
	{
		AsyncSetInviteCommandLine("");
		CleanMyMetaData();
		UnRegisterRailEvent();
		base.Shutdown();
	}

	public override ulong GetLobbyId() => 0uL;
	public override bool StartListening(SocketConnectionAccepted callback) => false;

	public override void StopListening()
	{
	}

	public override void Close(RemoteAddress address)
	{
		CleanMyMetaData();
		RailID remote_peer = RemoteAddressToRailId(address);
		Close(remote_peer);
	}

	public override bool CanInvite()
	{
		if (_hasLocalHost || _lobby.State == LobbyState.Active || Main.LobbyId != 0L)
			return Main.netMode != 0;

		return false;
	}

	public override void OpenInviteInterface()
	{
		_lobby.OpenInviteOverlay();
	}

	private void Close(RailID remote_peer)
	{
		if (_connectionStateMap.ContainsKey(remote_peer)) {
			WeGameHelper.WriteDebugString("CloseRemotePeer, remote:{0}", remote_peer.id_);
			rail_api.RailFactory().RailNetworkHelper().CloseSession(GetLocalPeer(), remote_peer);
			_connectionStateMap[remote_peer] = ConnectionState.Inactive;
			_lobby.Leave();
			_reader.ClearUser(remote_peer);
			_writer.ClearUser(remote_peer);
		}
	}

	public override void Connect(RemoteAddress address)
	{
	}

	public override void CancelJoin()
	{
		if (_lobby.State != 0)
			_lobby.Leave();
	}

	private void RegisterRailEvent()
	{
		RAILEventID[] array = new RAILEventID[7] {
			RAILEventID.kRailEventNetworkCreateSessionRequest,
			RAILEventID.kRailEventNetworkCreateSessionFailed,
			RAILEventID.kRailEventUsersRespondInvitation,
			RAILEventID.kRailEventUsersGetUsersInfo,
			RAILEventID.kRailEventFriendsGetMetadataResult,
			RAILEventID.kRailEventFriendsSetMetadataResult,
			RAILEventID.kRailEventFriendsFriendsListChanged
		};

		foreach (RAILEventID event_id in array) {
			_callbackHelper.RegisterCallback(event_id, OnRailEvent);
		}
	}

	private void UnRegisterRailEvent()
	{
		_callbackHelper.UnregisterAllCallback();
	}

	public void OnRailEvent(RAILEventID id, EventBase data)
	{
		WeGameHelper.WriteDebugString("OnRailEvent,id=" + id.ToString() + " ,result=" + data.result);
		switch (id) {
			case RAILEventID.kRailEventNetworkCreateSessionRequest:
				OnRailCreateSessionRequest((CreateSessionRequest)data);
				break;
			case RAILEventID.kRailEventNetworkCreateSessionFailed:
				OnRailCreateSessionFailed((CreateSessionFailed)data);
				break;
			case RAILEventID.kRailEventUsersRespondInvitation:
				OnRailRespondInvation((RailUsersRespondInvitation)data);
				break;
			case RAILEventID.kRailEventFriendsGetMetadataResult:
				OnGetFriendMetaData((RailFriendsGetMetadataResult)data);
				break;
			case RAILEventID.kRailEventFriendsSetMetadataResult:
				OnRailSetMetaData((RailFriendsSetMetadataResult)data);
				break;
			case RAILEventID.kRailEventUsersGetUsersInfo:
				OnRailGetUsersInfo((RailUsersInfoData)data);
				break;
			case RAILEventID.kRailEventFriendsFriendsListChanged:
				OnFriendlistChange((RailFriendsListChanged)data);
				break;
		}
	}

	private string DumpMataDataString(List<RailKeyValueResult> list)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (RailKeyValueResult item in list) {
			stringBuilder.Append("key: " + item.key + " value: " + item.value);
		}

		return stringBuilder.ToString();
	}

	private string GetValueByKey(string key, List<RailKeyValueResult> list)
	{
		string result = null;
		foreach (RailKeyValueResult item in list) {
			if (item.key == key)
				return item.value;
		}

		return result;
	}

	private bool SendFriendListToLocalServer()
	{
		bool result = false;
		if (_hasLocalHost) {
			List<RailFriendInfo> list = new List<RailFriendInfo>();
			if (GetRailFriendList(list)) {
				WeGameFriendListInfo t = new WeGameFriendListInfo {
					_friendList = list
				};

				IPCMessage iPCMessage = new IPCMessage();
				iPCMessage.Build(IPCMessageType.IPCMessageTypeNotifyFriendList, t);
				result = _msgServer.SendMessage(iPCMessage);
				WeGameHelper.WriteDebugString("NotifyFriendListToServer: " + result);
			}
		}

		return result;
	}

	private bool GetRailFriendList(List<RailFriendInfo> list)
	{
		bool result = false;
		IRailFriends railFriends = rail_api.RailFactory().RailFriends();
		if (railFriends != null)
			result = railFriends.GetFriendsList(list) == RailResult.kSuccess;

		return result;
	}

	private void OnGetFriendMetaData(RailFriendsGetMetadataResult data)
	{
		if (data.result != 0 || data.friend_kvs.Count <= 0)
			return;

		WeGameHelper.WriteDebugString("OnGetFriendMetaData - " + DumpMataDataString(data.friend_kvs));
		string valueByKey = GetValueByKey(_serverIDMedataKey, data.friend_kvs);
		if (valueByKey == null)
			return;

		if (valueByKey.Length > 0) {
			RailID railID = new RailID();
			railID.id_ = ulong.Parse(valueByKey);
			if (railID.IsValid())
				JoinServer(railID);
			else
				WeGameHelper.WriteDebugString("JoinServer failed, invalid server id");
		}
		else {
			WeGameHelper.WriteDebugString("can not find server id key");
		}
	}

	private void JoinServer(RailID server_id)
	{
		WeGameHelper.WriteDebugString("JoinServer:{0}", server_id.id_);
		_connectionStateMap[server_id] = ConnectionState.Authenticating;
		int num = 3;
		byte[] array = new byte[num];
		array[0] = (byte)((uint)num & 0xFFu);
		array[1] = (byte)((uint)(num >> 8) & 0xFFu);
		array[2] = 93;
		rail_api.RailFactory().RailNetworkHelper().SendReliableData(GetLocalPeer(), server_id, array, (uint)num);
	}

	private string GetFriendNickname(RailID rail_id)
	{
		if (_player_info_list != null) {
			foreach (PlayerPersonalInfo item in _player_info_list) {
				if (item.rail_id == rail_id)
					return item.rail_name;
			}
		}

		return "";
	}

	private void OnRailGetUsersInfo(RailUsersInfoData data)
	{
		_player_info_list = data.user_info_list;
	}

	private void OnFriendlistChange(RailFriendsListChanged data)
	{
		if (_hasLocalHost)
			SendFriendListToLocalServer();
	}

	private void AsyncGetFriendsInfo()
	{
		IRailFriends railFriends = rail_api.RailFactory().RailFriends();
		if (railFriends == null)
			return;

		List<RailFriendInfo> list = new List<RailFriendInfo>();
		railFriends.GetFriendsList(list);
		List<RailID> list2 = new List<RailID>();
		foreach (RailFriendInfo item in list) {
			list2.Add(item.friend_rail_id);
		}

		railFriends.AsyncGetPersonalInfo(list2, "");
	}

	private void AsyncSetPlayWith(RailID rail_id)
	{
		List<RailUserPlayedWith> list = new List<RailUserPlayedWith>();
		RailUserPlayedWith railUserPlayedWith = new RailUserPlayedWith();
		railUserPlayedWith.rail_id = rail_id;
		list.Add(railUserPlayedWith);
		rail_api.RailFactory().RailFriends()?.AsyncReportPlayedWithUserList(list, "");
	}

	private void OnRailSetMetaData(RailFriendsSetMetadataResult data)
	{
		WeGameHelper.WriteDebugString("OnRailSetMetaData - " + data.result);
	}

	private void OnRailRespondInvation(RailUsersRespondInvitation data)
	{
		WeGameHelper.WriteDebugString(" request join game");
		if (_lobby.State != 0)
			_lobby.Leave();

		_inviter_id = data.inviter_id;
		Main.OpenPlayerSelect(delegate (PlayerFileData playerData) {
			Main.ServerSideCharacter = false;
			playerData.SetAsActive();
			Main.menuMode = 882;
			Main.statusText = Language.GetTextValue("Social.JoiningFriend", GetFriendNickname(data.inviter_id));
			AsyncGetServerIDByOwener(data.inviter_id);
			WeGameHelper.WriteDebugString("inviter_id: " + data.inviter_id.id_);
		});
	}

	private void AsyncGetServerIDByOwener(RailID ownerID)
	{
		List<string> list = new List<string>();
		list.Add(_serverIDMedataKey);
		rail_api.RailFactory().RailFriends()?.AsyncGetFriendMetadata(ownerID, list, "");
	}

	private void OnRailCreateSessionRequest(CreateSessionRequest result)
	{
		WeGameHelper.WriteDebugString("OnRailCreateSessionRequest");
		if (_connectionStateMap.ContainsKey(result.remote_peer) && _connectionStateMap[result.remote_peer] != 0) {
			WeGameHelper.WriteDebugString("AcceptSessionRequest, local{0}, remote:{1}", result.local_peer.id_, result.remote_peer.id_);
			rail_api.RailFactory().RailNetworkHelper().AcceptSessionRequest(result.local_peer, result.remote_peer);
		}
	}

	private void OnRailCreateSessionFailed(CreateSessionFailed result)
	{
		WeGameHelper.WriteDebugString("OnRailCreateSessionFailed, CloseRemote: local:{0}, remote:{1}", result.local_peer.id_, result.remote_peer.id_);
		Close(result.remote_peer);
	}

	private void CleanMyMetaData()
	{
		rail_api.RailFactory().RailFriends()?.AsyncClearAllMyMetadata("");
	}

	private void OnDisconnect()
	{
		CleanMyMetaData();
		_hasLocalHost = false;
		Netplay.OnDisconnect -= OnDisconnect;
	}
}
