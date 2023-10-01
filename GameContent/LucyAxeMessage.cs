using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent;

public static class LucyAxeMessage
{
	public enum MessageSource
	{
		Idle,
		Storage,
		ThrownAway,
		PickedUp,
		ChoppedTree,
		ChoppedGemTree,
		ChoppedCactus,
		Count
	}

	private static byte _variation;
	private static int[] _messageCooldownsByType = new int[7];

	private static string GetCategoryName(MessageSource source)
	{
		switch (source) {
			default:
				return "LucyTheAxe_Idle";
			case MessageSource.Storage:
				return "LucyTheAxe_Storage";
			case MessageSource.ThrownAway:
				return "LucyTheAxe_ThrownAway";
			case MessageSource.PickedUp:
				return "LucyTheAxe_PickedUp";
			case MessageSource.ChoppedTree:
				return "LucyTheAxe_ChoppedTree";
			case MessageSource.ChoppedGemTree:
				return "LucyTheAxe_GemTree";
			case MessageSource.ChoppedCactus:
				return "LucyTheAxe_ChoppedCactus";
		}
	}

	public static void Initialize()
	{
		ItemSlot.OnItemTransferred += ItemSlot_OnItemTransferred;
		Player.Hooks.OnEnterWorld += Hooks_OnEnterWorld;
	}

	private static void Hooks_OnEnterWorld(Player player)
	{
		if (player == Main.LocalPlayer)
			GiveIdleMessageCooldown();
	}

	public static void UpdateMessageCooldowns()
	{
		for (int i = 0; i < _messageCooldownsByType.Length; i++) {
			if (_messageCooldownsByType[i] > 0)
				_messageCooldownsByType[i]--;
		}
	}

	public static void TryPlayingIdleMessage()
	{
		MessageSource messageSource = MessageSource.Idle;
		if (_messageCooldownsByType[(int)messageSource] <= 0) {
			Player localPlayer = Main.LocalPlayer;
			Create(messageSource, localPlayer.Top, new Vector2(Main.rand.NextFloatDirection() * 7f, -2f + Main.rand.NextFloat() * -2f));
		}
	}

	private static void ItemSlot_OnItemTransferred(ItemSlot.ItemTransferInfo info)
	{
		if (info.ItemType != 5095)
			return;

		bool flag = CountsAsStorage(info.FromContenxt);
		bool flag2 = CountsAsStorage(info.ToContext);
		if (flag != flag2) {
			MessageSource messageSource = ((!flag) ? MessageSource.Storage : MessageSource.PickedUp);
			if (_messageCooldownsByType[(int)messageSource] <= 0) {
				PutMessageTypeOnCooldown(messageSource, 420);
				Player localPlayer = Main.LocalPlayer;
				Create(messageSource, localPlayer.Top, new Vector2(localPlayer.direction * 7, -2f));
			}
		}
	}

	private static void GiveIdleMessageCooldown()
	{
		PutMessageTypeOnCooldown(MessageSource.Idle, Main.rand.Next(7200, 14400));
	}

	public static void PutMessageTypeOnCooldown(MessageSource source, int timeInFrames)
	{
		_messageCooldownsByType[(int)source] = timeInFrames;
	}

	private static bool CountsAsStorage(int itemSlotContext)
	{
		if (itemSlotContext == 3 || itemSlotContext == 6 || itemSlotContext == 15)
			return true;

		return false;
	}

	public static void TryCreatingMessageWithCooldown(MessageSource messageSource, Vector2 position, Vector2 velocity, int cooldownTimeInTicks)
	{
		if (Main.netMode != 2 && _messageCooldownsByType[(int)messageSource] <= 0) {
			PutMessageTypeOnCooldown(messageSource, cooldownTimeInTicks);
			Create(messageSource, position, velocity);
		}
	}

	public static void Create(MessageSource source, Vector2 position, Vector2 velocity)
	{
		if (Main.netMode != 2) {
			GiveIdleMessageCooldown();
			SpawnPopupText(source, _variation, position, velocity);
			PlaySound(source, position);
			SpawnEmoteBubble();
			if (Main.netMode == 1)
				NetMessage.SendData(141, -1, -1, null, (int)source, (int)_variation, velocity.X, velocity.Y, (int)position.X, (int)position.Y);

			_variation++;
		}
	}

	private static void SpawnEmoteBubble()
	{
		EmoteBubble.MakeLocalPlayerEmote(149);
	}

	public static void CreateFromNet(MessageSource source, byte variation, Vector2 position, Vector2 velocity)
	{
		SpawnPopupText(source, variation, position, velocity);
		PlaySound(source, position);
	}

	private static void PlaySound(MessageSource source, Vector2 position)
	{
		SoundEngine.PlaySound(SoundID.LucyTheAxeTalk, position);
	}

	private static void SpawnPopupText(MessageSource source, int variationUnwrapped, Vector2 position, Vector2 velocity)
	{
		string textForVariation = GetTextForVariation(source, variationUnwrapped);
		AdvancedPopupRequest request = default(AdvancedPopupRequest);
		request.Text = textForVariation;
		request.DurationInFrames = 420;
		request.Velocity = velocity;
		request.Color = new Color(184, 96, 98) * 1.15f;
		PopupText.NewText(request, position);
	}

	private static string GetTextForVariation(MessageSource source, int variationUnwrapped)
	{
		string categoryName = GetCategoryName(source);
		return LanguageManager.Instance.IndexedFromCategory(categoryName, variationUnwrapped).Value;
	}
}
