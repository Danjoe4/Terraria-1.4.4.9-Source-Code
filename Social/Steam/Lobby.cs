using System;
using System.Collections.Generic;
using Steamworks;

namespace Terraria.Social.Steam;

public class Lobby
{
	private HashSet<CSteamID> _usersSeen = new HashSet<CSteamID>();
	private byte[] _messageBuffer = new byte[1024];
	public CSteamID Id = CSteamID.Nil;
	public CSteamID Owner = CSteamID.Nil;
	public LobbyState State;
	private CallResult<LobbyEnter_t> _lobbyEnter;
	private CallResult<LobbyEnter_t>.APIDispatchDelegate _lobbyEnterExternalCallback;
	private CallResult<LobbyCreated_t> _lobbyCreated;
	private CallResult<LobbyCreated_t>.APIDispatchDelegate _lobbyCreatedExternalCallback;

	public Lobby()
	{
		_lobbyEnter = CallResult<LobbyEnter_t>.Create(OnLobbyEntered);
		_lobbyCreated = CallResult<LobbyCreated_t>.Create(OnLobbyCreated);
	}

	public void Create(bool inviteOnly, CallResult<LobbyCreated_t>.APIDispatchDelegate callResult)
	{
		SteamAPICall_t hAPICall = SteamMatchmaking.CreateLobby((!inviteOnly) ? ELobbyType.k_ELobbyTypeFriendsOnly : ELobbyType.k_ELobbyTypePrivate, 256);
		_lobbyCreatedExternalCallback = callResult;
		_lobbyCreated.Set(hAPICall);
		State = LobbyState.Creating;
	}

	public void OpenInviteOverlay()
	{
		if (State == LobbyState.Inactive)
			SteamFriends.ActivateGameOverlayInviteDialog(new CSteamID(Main.LobbyId));
		else
			SteamFriends.ActivateGameOverlayInviteDialog(Id);
	}

	public void Join(CSteamID lobbyId, CallResult<LobbyEnter_t>.APIDispatchDelegate callResult)
	{
		if (State == LobbyState.Inactive) {
			State = LobbyState.Connecting;
			_lobbyEnterExternalCallback = callResult;
			SteamAPICall_t hAPICall = SteamMatchmaking.JoinLobby(lobbyId);
			_lobbyEnter.Set(hAPICall);
		}
	}

	public byte[] GetMessage(int index)
	{
		CSteamID pSteamIDUser;
		EChatEntryType peChatEntryType;
		int lobbyChatEntry = SteamMatchmaking.GetLobbyChatEntry(Id, index, out pSteamIDUser, _messageBuffer, _messageBuffer.Length, out peChatEntryType);
		byte[] array = new byte[lobbyChatEntry];
		Array.Copy(_messageBuffer, array, lobbyChatEntry);
		return array;
	}

	public int GetUserCount() => SteamMatchmaking.GetNumLobbyMembers(Id);
	public CSteamID GetUserByIndex(int index) => SteamMatchmaking.GetLobbyMemberByIndex(Id, index);
	public bool SendMessage(byte[] data) => SendMessage(data, data.Length);

	public bool SendMessage(byte[] data, int length)
	{
		if (State != LobbyState.Active)
			return false;

		return SteamMatchmaking.SendLobbyChatMsg(Id, data, length);
	}

	public void Set(CSteamID lobbyId)
	{
		Id = lobbyId;
		State = LobbyState.Active;
		Owner = SteamMatchmaking.GetLobbyOwner(lobbyId);
	}

	public void SetPlayedWith(CSteamID userId)
	{
		if (!_usersSeen.Contains(userId)) {
			SteamFriends.SetPlayedWith(userId);
			_usersSeen.Add(userId);
		}
	}

	public void Leave()
	{
		if (State == LobbyState.Active)
			SteamMatchmaking.LeaveLobby(Id);

		State = LobbyState.Inactive;
		_usersSeen.Clear();
	}

	private void OnLobbyEntered(LobbyEnter_t result, bool failure)
	{
		if (State == LobbyState.Connecting) {
			if (failure)
				State = LobbyState.Inactive;
			else
				State = LobbyState.Active;

			Id = new CSteamID(result.m_ulSteamIDLobby);
			Owner = SteamMatchmaking.GetLobbyOwner(Id);
			_lobbyEnterExternalCallback(result, failure);
		}
	}

	private void OnLobbyCreated(LobbyCreated_t result, bool failure)
	{
		if (State == LobbyState.Creating) {
			if (failure)
				State = LobbyState.Inactive;
			else
				State = LobbyState.Active;

			Id = new CSteamID(result.m_ulSteamIDLobby);
			Owner = SteamMatchmaking.GetLobbyOwner(Id);
			_lobbyCreatedExternalCallback(result, failure);
		}
	}
}
