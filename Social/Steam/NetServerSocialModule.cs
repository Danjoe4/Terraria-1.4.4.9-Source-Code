using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Steamworks;
using Terraria.Localization;
using Terraria.Net;
using Terraria.Net.Sockets;

namespace Terraria.Social.Steam;

public class NetServerSocialModule : NetSocialModule
{
	private ServerMode _mode;
	private Callback<P2PSessionRequest_t> _p2pSessionRequest;
	private bool _acceptingClients;
	private SocketConnectionAccepted _connectionAcceptedCallback;

	public NetServerSocialModule()
		: base(1, 2)
	{
	}

	private void BroadcastConnectedUsers()
	{
		List<ulong> list = new List<ulong>();
		foreach (KeyValuePair<CSteamID, ConnectionState> item in _connectionStateMap) {
			if (item.Value == ConnectionState.Connected)
				list.Add(item.Key.m_SteamID);
		}

		byte[] array = new byte[list.Count * 8 + 1];
		using (MemoryStream output = new MemoryStream(array)) {
			using BinaryWriter binaryWriter = new BinaryWriter(output);
			binaryWriter.Write((byte)1);
			foreach (ulong item2 in list) {
				binaryWriter.Write(item2);
			}
		}

		_lobby.SendMessage(array);
	}

	public override void Initialize()
	{
		base.Initialize();
		_reader.SetReadEvent(OnPacketRead);
		_p2pSessionRequest = Callback<P2PSessionRequest_t>.Create(OnP2PSessionRequest);
		if (Program.LaunchParameters.ContainsKey("-lobby")) {
			_mode |= ServerMode.Lobby;
			string text = Program.LaunchParameters["-lobby"];
			if (!(text == "private")) {
				if (text == "friends") {
					_mode |= ServerMode.FriendsCanJoin;
					_lobby.Create(inviteOnly: false, OnLobbyCreated);
				}
				else {
					Console.WriteLine(Language.GetTextValue("Error.InvalidLobbyFlag", "private", "friends"));
				}
			}
			else {
				_lobby.Create(inviteOnly: true, OnLobbyCreated);
			}
		}

		if (Program.LaunchParameters.ContainsKey("-friendsoffriends"))
			_mode |= ServerMode.FriendsOfFriends;
	}

	public override ulong GetLobbyId() => _lobby.Id.m_SteamID;

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
		return true;
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
		CSteamID user = RemoteAddressToSteamId(address);
		Close(user);
	}

	private void Close(CSteamID user)
	{
		if (_connectionStateMap.ContainsKey(user)) {
			SteamUser.EndAuthSession(user);
			SteamNetworking.CloseP2PSessionWithUser(user);
			_connectionStateMap[user] = ConnectionState.Inactive;
			_reader.ClearUser(user);
			_writer.ClearUser(user);
		}
	}

	private void OnLobbyCreated(LobbyCreated_t result, bool failure)
	{
		if (!failure)
			SteamFriends.SetRichPresence("status", Language.GetTextValue("Social.StatusInGame"));
	}

	private bool OnPacketRead(byte[] data, int length, CSteamID userId)
	{
		if (!_connectionStateMap.ContainsKey(userId) || _connectionStateMap[userId] == ConnectionState.Inactive) {
			P2PSessionRequest_t result = default(P2PSessionRequest_t);
			result.m_steamIDRemote = userId;
			OnP2PSessionRequest(result);
			if (!_connectionStateMap.ContainsKey(userId) || _connectionStateMap[userId] == ConnectionState.Inactive)
				return false;
		}

		ConnectionState connectionState = _connectionStateMap[userId];
		if (connectionState == ConnectionState.Authenticating) {
			if (length < 3)
				return false;

			if (((data[1] << 8) | data[0]) != length)
				return false;

			if (data[2] != 93)
				return false;

			byte[] array = new byte[data.Length - 3];
			Array.Copy(data, 3, array, 0, array.Length);
			switch (SteamUser.BeginAuthSession(array, array.Length, userId)) {
				case EBeginAuthSessionResult.k_EBeginAuthSessionResultOK:
					_connectionStateMap[userId] = ConnectionState.Connected;
					BroadcastConnectedUsers();
					break;
				case EBeginAuthSessionResult.k_EBeginAuthSessionResultDuplicateRequest:
					Close(userId);
					break;
				case EBeginAuthSessionResult.k_EBeginAuthSessionResultExpiredTicket:
					Close(userId);
					break;
				case EBeginAuthSessionResult.k_EBeginAuthSessionResultGameMismatch:
					Close(userId);
					break;
				case EBeginAuthSessionResult.k_EBeginAuthSessionResultInvalidTicket:
					Close(userId);
					break;
				case EBeginAuthSessionResult.k_EBeginAuthSessionResultInvalidVersion:
					Close(userId);
					break;
			}

			return false;
		}

		return connectionState == ConnectionState.Connected;
	}

	private void OnP2PSessionRequest(P2PSessionRequest_t result)
	{
		CSteamID steamIDRemote = result.m_steamIDRemote;
		if (_connectionStateMap.ContainsKey(steamIDRemote) && _connectionStateMap[steamIDRemote] != 0) {
			SteamNetworking.AcceptP2PSessionWithUser(steamIDRemote);
		}
		else if (_acceptingClients && (_mode.HasFlag(ServerMode.FriendsOfFriends) || SteamFriends.GetFriendRelationship(steamIDRemote) == EFriendRelationship.k_EFriendRelationshipFriend)) {
			SteamNetworking.AcceptP2PSessionWithUser(steamIDRemote);
			P2PSessionState_t pConnectionState;
			while (SteamNetworking.GetP2PSessionState(steamIDRemote, out pConnectionState) && pConnectionState.m_bConnecting == 1) {
			}

			if (pConnectionState.m_bConnectionActive == 0)
				Close(steamIDRemote);

			_connectionStateMap[steamIDRemote] = ConnectionState.Authenticating;
			_connectionAcceptedCallback(new SocialSocket(new SteamAddress(steamIDRemote)));
		}
	}
}
