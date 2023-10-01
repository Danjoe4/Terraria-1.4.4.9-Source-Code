using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria;

public static class IngameOptions
{
	public const int width = 670;
	public const int height = 480;
	public static float[] leftScale = new float[10] {
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f
	};
	public static float[] rightScale = new float[17] {
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f,
		0.7f
	};
	private static Dictionary<int, int> _leftSideCategoryMapping = new Dictionary<int, int> {
		{ 0, 0 },
		{ 1, 1 },
		{ 2, 2 },
		{ 3, 3 }
	};
	public static bool[] skipRightSlot = new bool[20];
	public static int leftHover = -1;
	public static int rightHover = -1;
	public static int oldLeftHover = -1;
	public static int oldRightHover = -1;
	public static int rightLock = -1;
	public static bool inBar;
	public static bool notBar;
	public static bool noSound;
	private static Rectangle _GUIHover;
	public static int category;
	public static Vector2 valuePosition = Vector2.Zero;
	private static string _mouseOverText;
	private static bool _canConsumeHover;

	public static void Open()
	{
		Main.ClosePlayerChat();
		Main.chatText = "";
		Main.playerInventory = false;
		Main.editChest = false;
		Main.npcChatText = "";
		SoundEngine.PlaySound(10);
		Main.ingameOptionsWindow = true;
		category = 0;
		for (int i = 0; i < leftScale.Length; i++) {
			leftScale[i] = 0f;
		}

		for (int j = 0; j < rightScale.Length; j++) {
			rightScale[j] = 0f;
		}

		leftHover = -1;
		rightHover = -1;
		oldLeftHover = -1;
		oldRightHover = -1;
		rightLock = -1;
		inBar = false;
		notBar = false;
		noSound = false;
	}

	public static void Close()
	{
		if (Main.setKey == -1) {
			Main.ingameOptionsWindow = false;
			SoundEngine.PlaySound(11);
			Recipe.FindRecipes();
			Main.playerInventory = true;
			Main.SaveSettings();
		}
	}

	public static void Draw(Main mainInstance, SpriteBatch sb)
	{
		_canConsumeHover = true;
		for (int i = 0; i < skipRightSlot.Length; i++) {
			skipRightSlot[i] = false;
		}

		bool flag = GameCulture.FromCultureName(GameCulture.CultureName.Russian).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Portuguese).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Polish).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.French).IsActive;
		bool isActive = GameCulture.FromCultureName(GameCulture.CultureName.Polish).IsActive;
		bool isActive2 = GameCulture.FromCultureName(GameCulture.CultureName.German).IsActive;
		bool flag2 = GameCulture.FromCultureName(GameCulture.CultureName.Italian).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Spanish).IsActive;
		bool flag3 = false;
		int num = 70;
		float scale = 0.75f;
		float num2 = 60f;
		float maxWidth = 300f;
		if (flag)
			flag3 = true;

		if (isActive)
			maxWidth = 200f;

		new Vector2(Main.mouseX, Main.mouseY);
		bool flag4 = Main.mouseLeft && Main.mouseLeftRelease;
		Vector2 vector = new Vector2(Main.screenWidth, Main.screenHeight);
		Vector2 vector2 = new Vector2(670f, 480f);
		Vector2 vector3 = vector / 2f - vector2 / 2f;
		int num3 = 20;
		_GUIHover = new Rectangle((int)(vector3.X - (float)num3), (int)(vector3.Y - (float)num3), (int)(vector2.X + (float)(num3 * 2)), (int)(vector2.Y + (float)(num3 * 2)));
		Utils.DrawInvBG(sb, vector3.X - (float)num3, vector3.Y - (float)num3, vector2.X + (float)(num3 * 2), vector2.Y + (float)(num3 * 2), new Color(33, 15, 91, 255) * 0.685f);
		if (new Rectangle((int)vector3.X - num3, (int)vector3.Y - num3, (int)vector2.X + num3 * 2, (int)vector2.Y + num3 * 2).Contains(new Point(Main.mouseX, Main.mouseY)))
			Main.player[Main.myPlayer].mouseInterface = true;

		Utils.DrawBorderString(sb, Language.GetTextValue("GameUI.SettingsMenu"), vector3 + vector2 * new Vector2(0.5f, 0f), Color.White, 1f, 0.5f);
		if (flag) {
			Utils.DrawInvBG(sb, vector3.X + (float)(num3 / 2), vector3.Y + (float)(num3 * 5 / 2), vector2.X / 3f - (float)num3, vector2.Y - (float)(num3 * 3));
			Utils.DrawInvBG(sb, vector3.X + vector2.X / 3f + (float)num3, vector3.Y + (float)(num3 * 5 / 2), vector2.X * 2f / 3f - (float)(num3 * 3 / 2), vector2.Y - (float)(num3 * 3));
		}
		else {
			Utils.DrawInvBG(sb, vector3.X + (float)(num3 / 2), vector3.Y + (float)(num3 * 5 / 2), vector2.X / 2f - (float)num3, vector2.Y - (float)(num3 * 3));
			Utils.DrawInvBG(sb, vector3.X + vector2.X / 2f + (float)num3, vector3.Y + (float)(num3 * 5 / 2), vector2.X / 2f - (float)(num3 * 3 / 2), vector2.Y - (float)(num3 * 3));
		}

		float num4 = 0.7f;
		float num5 = 0.8f;
		float num6 = 0.01f;
		if (flag) {
			num4 = 0.4f;
			num5 = 0.44f;
		}

		if (isActive2) {
			num4 = 0.55f;
			num5 = 0.6f;
		}

		if (oldLeftHover != leftHover && leftHover != -1)
			SoundEngine.PlaySound(12);

		if (oldRightHover != rightHover && rightHover != -1)
			SoundEngine.PlaySound(12);

		if (flag4 && rightHover != -1 && !noSound)
			SoundEngine.PlaySound(12);

		oldLeftHover = leftHover;
		oldRightHover = rightHover;
		noSound = false;
		bool flag5 = SocialAPI.Network != null && SocialAPI.Network.CanInvite();
		int num7 = (flag5 ? 1 : 0);
		int num8 = 5 + num7 + 2;
		Vector2 vector4 = new Vector2(vector3.X + vector2.X / 4f, vector3.Y + (float)(num3 * 5 / 2));
		Vector2 vector5 = new Vector2(0f, vector2.Y - (float)(num3 * 5)) / (num8 + 1);
		if (flag)
			vector4.X -= 55f;

		UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_LEFT = num8 + 1;
		for (int j = 0; j <= num8; j++) {
			bool flag6 = false;
			if (_leftSideCategoryMapping.TryGetValue(j, out var value))
				flag6 = category == value;

			if (leftHover == j || flag6)
				leftScale[j] += num6;
			else
				leftScale[j] -= num6;

			if (leftScale[j] < num4)
				leftScale[j] = num4;

			if (leftScale[j] > num5)
				leftScale[j] = num5;
		}

		leftHover = -1;
		int num9 = category;
		int num10 = 0;
		if (DrawLeftSide(sb, Lang.menu[114].Value, num10, vector4, vector5, leftScale)) {
			leftHover = num10;
			if (flag4) {
				category = 0;
				SoundEngine.PlaySound(10);
			}
		}

		num10++;
		if (DrawLeftSide(sb, Lang.menu[210].Value, num10, vector4, vector5, leftScale)) {
			leftHover = num10;
			if (flag4) {
				category = 1;
				SoundEngine.PlaySound(10);
			}
		}

		num10++;
		if (DrawLeftSide(sb, Lang.menu[63].Value, num10, vector4, vector5, leftScale)) {
			leftHover = num10;
			if (flag4) {
				category = 2;
				SoundEngine.PlaySound(10);
			}
		}

		num10++;
		if (DrawLeftSide(sb, Lang.menu[218].Value, num10, vector4, vector5, leftScale)) {
			leftHover = num10;
			if (flag4) {
				category = 3;
				SoundEngine.PlaySound(10);
			}
		}

		num10++;
		if (DrawLeftSide(sb, Lang.menu[66].Value, num10, vector4, vector5, leftScale)) {
			leftHover = num10;
			if (flag4) {
				Close();
				IngameFancyUI.OpenKeybinds();
			}
		}

		num10++;
		if (flag5 && DrawLeftSide(sb, Lang.menu[147].Value, num10, vector4, vector5, leftScale)) {
			leftHover = num10;
			if (flag4) {
				Close();
				SocialAPI.Network.OpenInviteInterface();
			}
		}

		if (flag5)
			num10++;

		if (DrawLeftSide(sb, Lang.menu[131].Value, num10, vector4, vector5, leftScale)) {
			leftHover = num10;
			if (flag4) {
				Close();
				IngameFancyUI.OpenAchievements();
			}
		}

		num10++;
		if (DrawLeftSide(sb, Lang.menu[118].Value, num10, vector4, vector5, leftScale)) {
			leftHover = num10;
			if (flag4)
				Close();
		}

		num10++;
		if (DrawLeftSide(sb, Lang.inter[35].Value, num10, vector4, vector5, leftScale)) {
			leftHover = num10;
			if (flag4) {
				Close();
				Main.menuMode = 10;
				Main.gameMenu = true;
				WorldGen.SaveAndQuit();
			}
		}

		num10++;
		if (num9 != category) {
			for (int k = 0; k < rightScale.Length; k++) {
				rightScale[k] = 0f;
			}
		}

		int num11 = 0;
		int num12 = 0;
		switch (category) {
			case 0:
				num12 = 16;
				num4 = 1f;
				num5 = 1.001f;
				num6 = 0.001f;
				break;
			case 1:
				num12 = 11;
				num4 = 1f;
				num5 = 1.001f;
				num6 = 0.001f;
				break;
			case 2:
				num12 = 12;
				num4 = 1f;
				num5 = 1.001f;
				num6 = 0.001f;
				break;
			case 3:
				num12 = 15;
				num4 = 1f;
				num5 = 1.001f;
				num6 = 0.001f;
				break;
		}

		if (flag) {
			num4 -= 0.1f;
			num5 -= 0.1f;
		}

		if (isActive2 && category == 3) {
			num4 -= 0.15f;
			num5 -= 0.15f;
		}

		if (flag2 && (category == 0 || category == 3)) {
			num4 -= 0.2f;
			num5 -= 0.2f;
		}

		UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_RIGHT = num12;
		Vector2 vector6 = new Vector2(vector3.X + vector2.X * 3f / 4f, vector3.Y + (float)(num3 * 5 / 2));
		Vector2 vector7 = new Vector2(0f, vector2.Y - (float)(num3 * 3)) / (num12 + 1);
		if (category == 2)
			vector7.Y -= 2f;

		new Vector2(8f, 0f);
		if (flag)
			vector6.X = vector3.X + vector2.X * 2f / 3f;

		for (int l = 0; l < rightScale.Length; l++) {
			if (rightLock == l || (rightHover == l && rightLock == -1))
				rightScale[l] += num6;
			else
				rightScale[l] -= num6;

			if (rightScale[l] < num4)
				rightScale[l] = num4;

			if (rightScale[l] > num5)
				rightScale[l] = num5;
		}

		inBar = false;
		rightHover = -1;
		if (!Main.mouseLeft)
			rightLock = -1;

		if (rightLock == -1)
			notBar = false;

		if (category == 0) {
			int num13 = 0;
			DrawRightSide(sb, Lang.menu[65].Value, num13, vector6, vector7, rightScale[num13], 1f);
			skipRightSlot[num13] = true;
			num13++;
			vector6.X -= num;
			if (DrawRightSide(sb, Lang.menu[99].Value + " " + Math.Round(Main.musicVolume * 100f) + "%", num13, vector6, vector7, rightScale[num13], (rightScale[num13] - num4) / (num5 - num4))) {
				if (rightLock == -1)
					notBar = true;

				noSound = true;
				rightHover = num13;
			}

			valuePosition.X = vector3.X + vector2.X - (float)(num3 / 2) - 20f;
			valuePosition.Y -= 3f;
			float musicVolume = DrawValueBar(sb, scale, Main.musicVolume);
			if ((inBar || rightLock == num13) && !notBar) {
				rightHover = num13;
				if (Main.mouseLeft && rightLock == num13)
					Main.musicVolume = musicVolume;
			}

			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num3 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num13;
			}

			if (rightHover == num13)
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 2;

			num13++;
			if (DrawRightSide(sb, Lang.menu[98].Value + " " + Math.Round(Main.soundVolume * 100f) + "%", num13, vector6, vector7, rightScale[num13], (rightScale[num13] - num4) / (num5 - num4))) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num13;
			}

			valuePosition.X = vector3.X + vector2.X - (float)(num3 / 2) - 20f;
			valuePosition.Y -= 3f;
			float soundVolume = DrawValueBar(sb, scale, Main.soundVolume);
			if ((inBar || rightLock == num13) && !notBar) {
				rightHover = num13;
				if (Main.mouseLeft && rightLock == num13) {
					Main.soundVolume = soundVolume;
					noSound = true;
				}
			}

			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num3 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num13;
			}

			if (rightHover == num13)
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 3;

			num13++;
			if (DrawRightSide(sb, Lang.menu[119].Value + " " + Math.Round(Main.ambientVolume * 100f) + "%", num13, vector6, vector7, rightScale[num13], (rightScale[num13] - num4) / (num5 - num4))) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num13;
			}

			valuePosition.X = vector3.X + vector2.X - (float)(num3 / 2) - 20f;
			valuePosition.Y -= 3f;
			float ambientVolume = DrawValueBar(sb, scale, Main.ambientVolume);
			if ((inBar || rightLock == num13) && !notBar) {
				rightHover = num13;
				if (Main.mouseLeft && rightLock == num13) {
					Main.ambientVolume = ambientVolume;
					noSound = true;
				}
			}

			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num3 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num13;
			}

			if (rightHover == num13)
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 4;

			num13++;
			vector6.X += num;
			DrawRightSide(sb, "", num13, vector6, vector7, rightScale[num13], 1f);
			skipRightSlot[num13] = true;
			num13++;
			DrawRightSide(sb, Language.GetTextValue("GameUI.ZoomCategory"), num13, vector6, vector7, rightScale[num13], 1f);
			skipRightSlot[num13] = true;
			num13++;
			vector6.X -= num;
			string text = Language.GetTextValue("GameUI.GameZoom", Math.Round(Main.GameZoomTarget * 100f), Math.Round(Main.GameViewMatrix.Zoom.X * 100f));
			if (flag3)
				text = FontAssets.ItemStack.Value.CreateWrappedText(text, maxWidth, Language.ActiveCulture.CultureInfo);

			if (DrawRightSide(sb, text, num13, vector6, vector7, rightScale[num13] * 0.85f, (rightScale[num13] - num4) / (num5 - num4))) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num13;
			}

			valuePosition.X = vector3.X + vector2.X - (float)(num3 / 2) - 20f;
			valuePosition.Y -= 3f;
			float num14 = DrawValueBar(sb, scale, Main.GameZoomTarget - 1f);
			if ((inBar || rightLock == num13) && !notBar) {
				rightHover = num13;
				if (Main.mouseLeft && rightLock == num13)
					Main.GameZoomTarget = num14 + 1f;
			}

			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num3 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num13;
			}

			if (rightHover == num13)
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 10;

			num13++;
			bool flag7 = false;
			if (Main.temporaryGUIScaleSlider == -1f)
				Main.temporaryGUIScaleSlider = Main.UIScaleWanted;

			string text2 = Language.GetTextValue("GameUI.UIScale", Math.Round(Main.temporaryGUIScaleSlider * 100f), Math.Round(Main.UIScale * 100f));
			if (flag3)
				text2 = FontAssets.ItemStack.Value.CreateWrappedText(text2, maxWidth, Language.ActiveCulture.CultureInfo);

			if (DrawRightSide(sb, text2, num13, vector6, vector7, rightScale[num13] * 0.75f, (rightScale[num13] - num4) / (num5 - num4))) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num13;
			}

			valuePosition.X = vector3.X + vector2.X - (float)(num3 / 2) - 20f;
			valuePosition.Y -= 3f;
			float num15 = DrawValueBar(sb, scale, MathHelper.Clamp((Main.temporaryGUIScaleSlider - 0.5f) / 1.5f, 0f, 1f));
			if ((inBar || rightLock == num13) && !notBar) {
				rightHover = num13;
				if (Main.mouseLeft && rightLock == num13) {
					Main.temporaryGUIScaleSlider = num15 * 1.5f + 0.5f;
					Main.temporaryGUIScaleSlider = (float)(int)(Main.temporaryGUIScaleSlider * 100f) / 100f;
					Main.temporaryGUIScaleSliderUpdate = true;
					flag7 = true;
				}
			}

			if (!flag7 && Main.temporaryGUIScaleSliderUpdate && Main.temporaryGUIScaleSlider != -1f) {
				Main.UIScale = Main.temporaryGUIScaleSlider;
				Main.temporaryGUIScaleSliderUpdate = false;
			}

			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num3 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num13;
			}

			if (rightHover == num13)
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 11;

			num13++;
			vector6.X += num;
			DrawRightSide(sb, "", num13, vector6, vector7, rightScale[num13], 1f);
			skipRightSlot[num13] = true;
			num13++;
			DrawRightSide(sb, Language.GetTextValue("GameUI.Gameplay"), num13, vector6, vector7, rightScale[num13], 1f);
			skipRightSlot[num13] = true;
			num13++;
			if (DrawRightSide(sb, Main.autoSave ? Lang.menu[67].Value : Lang.menu[68].Value, num13, vector6, vector7, rightScale[num13], (rightScale[num13] - num4) / (num5 - num4))) {
				rightHover = num13;
				if (flag4)
					Main.autoSave = !Main.autoSave;
			}

			num13++;
			if (DrawRightSide(sb, Main.autoPause ? Lang.menu[69].Value : Lang.menu[70].Value, num13, vector6, vector7, rightScale[num13], (rightScale[num13] - num4) / (num5 - num4))) {
				rightHover = num13;
				if (flag4)
					Main.autoPause = !Main.autoPause;
			}

			num13++;
			if (DrawRightSide(sb, Main.ReversedUpDownArmorSetBonuses ? Lang.menu[220].Value : Lang.menu[221].Value, num13, vector6, vector7, rightScale[num13], (rightScale[num13] - num4) / (num5 - num4))) {
				rightHover = num13;
				if (flag4)
					Main.ReversedUpDownArmorSetBonuses = !Main.ReversedUpDownArmorSetBonuses;
			}

			num13++;
			string textValue;
			switch (DoorOpeningHelper.PreferenceSettings) {
				default:
					textValue = Language.GetTextValue("UI.SmartDoorsDisabled");
					break;
				case DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForEverything:
					textValue = Language.GetTextValue("UI.SmartDoorsEnabled");
					break;
				case DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForGamepadOnly:
					textValue = Language.GetTextValue("UI.SmartDoorsGamepad");
					break;
			}

			if (DrawRightSide(sb, textValue, num13, vector6, vector7, rightScale[num13], (rightScale[num13] - num4) / (num5 - num4))) {
				rightHover = num13;
				if (flag4)
					DoorOpeningHelper.CyclePreferences();
			}

			num13++;
			string textValue2;
			if (Player.Settings.HoverControl != 0) {
				_ = 1;
				textValue2 = Language.GetTextValue("UI.HoverControlSettingIsClick");
			}
			else {
				textValue2 = Language.GetTextValue("UI.HoverControlSettingIsHold");
			}

			if (DrawRightSide(sb, textValue2, num13, vector6, vector7, rightScale[num13], (rightScale[num13] - num4) / (num5 - num4))) {
				rightHover = num13;
				if (flag4)
					Player.Settings.CycleHoverControl();
			}

			num13++;
			if (DrawRightSide(sb, Language.GetTextValue(Main.SettingsEnabled_AutoReuseAllItems ? "UI.AutoReuseAllOn" : "UI.AutoReuseAllOff"), num13, vector6, vector7, rightScale[num13], (rightScale[num13] - num4) / (num5 - num4))) {
				rightHover = num13;
				if (flag4)
					Main.SettingsEnabled_AutoReuseAllItems = !Main.SettingsEnabled_AutoReuseAllItems;
			}

			num13++;
			DrawRightSide(sb, "", num13, vector6, vector7, rightScale[num13], 1f);
			skipRightSlot[num13] = true;
			num13++;
		}

		if (category == 1) {
			int num16 = 0;
			if (DrawRightSide(sb, Main.showItemText ? Lang.menu[71].Value : Lang.menu[72].Value, num16, vector6, vector7, rightScale[num16], (rightScale[num16] - num4) / (num5 - num4))) {
				rightHover = num16;
				if (flag4)
					Main.showItemText = !Main.showItemText;
			}

			num16++;
			if (DrawRightSide(sb, Lang.menu[123].Value + " " + Lang.menu[124 + Main.invasionProgressMode], num16, vector6, vector7, rightScale[num16], (rightScale[num16] - num4) / (num5 - num4))) {
				rightHover = num16;
				if (flag4) {
					Main.invasionProgressMode++;
					if (Main.invasionProgressMode >= 3)
						Main.invasionProgressMode = 0;
				}
			}

			num16++;
			if (DrawRightSide(sb, Main.placementPreview ? Lang.menu[128].Value : Lang.menu[129].Value, num16, vector6, vector7, rightScale[num16], (rightScale[num16] - num4) / (num5 - num4))) {
				rightHover = num16;
				if (flag4)
					Main.placementPreview = !Main.placementPreview;
			}

			num16++;
			if (DrawRightSide(sb, ItemSlot.Options.HighlightNewItems ? Lang.inter[117].Value : Lang.inter[116].Value, num16, vector6, vector7, rightScale[num16], (rightScale[num16] - num4) / (num5 - num4))) {
				rightHover = num16;
				if (flag4)
					ItemSlot.Options.HighlightNewItems = !ItemSlot.Options.HighlightNewItems;
			}

			num16++;
			if (DrawRightSide(sb, Main.MouseShowBuildingGrid ? Lang.menu[229].Value : Lang.menu[230].Value, num16, vector6, vector7, rightScale[num16], (rightScale[num16] - num4) / (num5 - num4))) {
				rightHover = num16;
				if (flag4)
					Main.MouseShowBuildingGrid = !Main.MouseShowBuildingGrid;
			}

			num16++;
			if (DrawRightSide(sb, Main.GamepadDisableInstructionsDisplay ? Lang.menu[241].Value : Lang.menu[242].Value, num16, vector6, vector7, rightScale[num16], (rightScale[num16] - num4) / (num5 - num4))) {
				rightHover = num16;
				if (flag4)
					Main.GamepadDisableInstructionsDisplay = !Main.GamepadDisableInstructionsDisplay;
			}

			num16++;
			string textValue3 = Language.GetTextValue("UI.MinimapFrame_" + Main.MinimapFrameManagerInstance.ActiveSelectionKeyName);
			if (DrawRightSide(sb, Language.GetTextValue("UI.SelectMapBorder", textValue3), num16, vector6, vector7, rightScale[num16], (rightScale[num16] - num4) / (num5 - num4))) {
				rightHover = num16;
				if (flag4)
					Main.MinimapFrameManagerInstance.CycleSelection();
			}

			num16++;
			vector6.X -= num;
			string text3 = Language.GetTextValue("GameUI.MapScale", Math.Round(Main.MapScale * 100f));
			if (flag3)
				text3 = FontAssets.ItemStack.Value.CreateWrappedText(text3, maxWidth, Language.ActiveCulture.CultureInfo);

			if (DrawRightSide(sb, text3, num16, vector6, vector7, rightScale[num16] * 0.85f, (rightScale[num16] - num4) / (num5 - num4))) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num16;
			}

			valuePosition.X = vector3.X + vector2.X - (float)(num3 / 2) - 20f;
			valuePosition.Y -= 3f;
			float num17 = DrawValueBar(sb, scale, (Main.MapScale - 0.5f) / 0.5f);
			if ((inBar || rightLock == num16) && !notBar) {
				rightHover = num16;
				if (Main.mouseLeft && rightLock == num16)
					Main.MapScale = num17 * 0.5f + 0.5f;
			}

			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num3 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num16;
			}

			if (rightHover == num16)
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 12;

			num16++;
			vector6.X += num;
			string activeSetKeyName = Main.ResourceSetsManager.ActiveSetKeyName;
			string textValue4 = Language.GetTextValue("UI.HealthManaStyle_" + activeSetKeyName);
			if (DrawRightSide(sb, Language.GetTextValue("UI.SelectHealthStyle", textValue4), num16, vector6, vector7, rightScale[num16], (rightScale[num16] - num4) / (num5 - num4))) {
				rightHover = num16;
				if (flag4)
					Main.ResourceSetsManager.CycleResourceSet();
			}

			num16++;
			string textValue5 = Language.GetTextValue(BigProgressBarSystem.ShowText ? "UI.ShowBossLifeTextOn" : "UI.ShowBossLifeTextOff");
			if (DrawRightSide(sb, textValue5, num16, vector6, vector7, rightScale[num16], (rightScale[num16] - num4) / (num5 - num4))) {
				rightHover = num16;
				if (flag4)
					BigProgressBarSystem.ToggleShowText();
			}

			num16++;
			if (DrawRightSide(sb, Main.SettingsEnabled_OpaqueBoxBehindTooltips ? Language.GetTextValue("GameUI.HoverTextBoxesOn") : Language.GetTextValue("GameUI.HoverTextBoxesOff"), num16, vector6, vector7, rightScale[num16], (rightScale[num16] - num4) / (num5 - num4))) {
				rightHover = num16;
				if (flag4)
					Main.SettingsEnabled_OpaqueBoxBehindTooltips = !Main.SettingsEnabled_OpaqueBoxBehindTooltips;
			}

			num16++;
		}

		if (category == 2) {
			int num18 = 0;
			if (DrawRightSide(sb, Main.graphics.IsFullScreen ? Lang.menu[49].Value : Lang.menu[50].Value, num18, vector6, vector7, rightScale[num18], (rightScale[num18] - num4) / (num5 - num4))) {
				rightHover = num18;
				if (flag4)
					Main.ToggleFullScreen();
			}

			num18++;
			if (DrawRightSide(sb, Lang.menu[51].Value + ": " + Main.PendingResolutionWidth + "x" + Main.PendingResolutionHeight, num18, vector6, vector7, rightScale[num18], (rightScale[num18] - num4) / (num5 - num4))) {
				rightHover = num18;
				if (flag4) {
					int num19 = 0;
					for (int m = 0; m < Main.numDisplayModes; m++) {
						if (Main.displayWidth[m] == Main.PendingResolutionWidth && Main.displayHeight[m] == Main.PendingResolutionHeight) {
							num19 = m;
							break;
						}
					}

					num19++;
					if (num19 >= Main.numDisplayModes)
						num19 = 0;

					Main.PendingResolutionWidth = Main.displayWidth[num19];
					Main.PendingResolutionHeight = Main.displayHeight[num19];
					Main.SetResolution(Main.PendingResolutionWidth, Main.PendingResolutionHeight);
				}
			}

			num18++;
			vector6.X -= num;
			if (DrawRightSide(sb, Lang.menu[52].Value + ": " + Main.bgScroll + "%", num18, vector6, vector7, rightScale[num18], (rightScale[num18] - num4) / (num5 - num4))) {
				if (rightLock == -1)
					notBar = true;

				noSound = true;
				rightHover = num18;
			}

			valuePosition.X = vector3.X + vector2.X - (float)(num3 / 2) - 20f;
			valuePosition.Y -= 3f;
			float num20 = DrawValueBar(sb, scale, (float)Main.bgScroll / 100f);
			if ((inBar || rightLock == num18) && !notBar) {
				rightHover = num18;
				if (Main.mouseLeft && rightLock == num18) {
					Main.bgScroll = (int)(num20 * 100f);
					Main.caveParallax = 1f - (float)Main.bgScroll / 500f;
				}
			}

			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num3 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num18;
			}

			if (rightHover == num18)
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 1;

			num18++;
			vector6.X += num;
			if (DrawRightSide(sb, Lang.menu[(int)(247 + Main.FrameSkipMode)].Value, num18, vector6, vector7, rightScale[num18], (rightScale[num18] - num4) / (num5 - num4))) {
				rightHover = num18;
				if (flag4)
					Main.CycleFrameSkipMode();
			}

			num18++;
			if (DrawRightSide(sb, Language.GetTextValue("UI.LightMode_" + Lighting.Mode), num18, vector6, vector7, rightScale[num18], (rightScale[num18] - num4) / (num5 - num4))) {
				rightHover = num18;
				if (flag4)
					Lighting.NextLightMode();
			}

			num18++;
			if (DrawRightSide(sb, Lang.menu[59 + Main.qaStyle].Value, num18, vector6, vector7, rightScale[num18], (rightScale[num18] - num4) / (num5 - num4))) {
				rightHover = num18;
				if (flag4) {
					Main.qaStyle++;
					if (Main.qaStyle > 3)
						Main.qaStyle = 0;
				}
			}

			num18++;
			if (DrawRightSide(sb, Main.BackgroundEnabled ? Lang.menu[100].Value : Lang.menu[101].Value, num18, vector6, vector7, rightScale[num18], (rightScale[num18] - num4) / (num5 - num4))) {
				rightHover = num18;
				if (flag4)
					Main.BackgroundEnabled = !Main.BackgroundEnabled;
			}

			num18++;
			if (DrawRightSide(sb, ChildSafety.Disabled ? Lang.menu[132].Value : Lang.menu[133].Value, num18, vector6, vector7, rightScale[num18], (rightScale[num18] - num4) / (num5 - num4))) {
				rightHover = num18;
				if (flag4)
					ChildSafety.Disabled = !ChildSafety.Disabled;
			}

			num18++;
			if (DrawRightSide(sb, Language.GetTextValue("GameUI.HeatDistortion", Main.UseHeatDistortion ? Language.GetTextValue("GameUI.Enabled") : Language.GetTextValue("GameUI.Disabled")), num18, vector6, vector7, rightScale[num18], (rightScale[num18] - num4) / (num5 - num4))) {
				rightHover = num18;
				if (flag4)
					Main.UseHeatDistortion = !Main.UseHeatDistortion;
			}

			num18++;
			if (DrawRightSide(sb, Language.GetTextValue("GameUI.StormEffects", Main.UseStormEffects ? Language.GetTextValue("GameUI.Enabled") : Language.GetTextValue("GameUI.Disabled")), num18, vector6, vector7, rightScale[num18], (rightScale[num18] - num4) / (num5 - num4))) {
				rightHover = num18;
				if (flag4)
					Main.UseStormEffects = !Main.UseStormEffects;
			}

			num18++;
			string textValue6;
			switch (Main.WaveQuality) {
				case 1:
					textValue6 = Language.GetTextValue("GameUI.QualityLow");
					break;
				case 2:
					textValue6 = Language.GetTextValue("GameUI.QualityMedium");
					break;
				case 3:
					textValue6 = Language.GetTextValue("GameUI.QualityHigh");
					break;
				default:
					textValue6 = Language.GetTextValue("GameUI.QualityOff");
					break;
			}

			if (DrawRightSide(sb, Language.GetTextValue("GameUI.WaveQuality", textValue6), num18, vector6, vector7, rightScale[num18], (rightScale[num18] - num4) / (num5 - num4))) {
				rightHover = num18;
				if (flag4)
					Main.WaveQuality = (Main.WaveQuality + 1) % 4;
			}

			num18++;
			if (DrawRightSide(sb, Language.GetTextValue("UI.TilesSwayInWind" + (Main.SettingsEnabled_TilesSwayInWind ? "On" : "Off")), num18, vector6, vector7, rightScale[num18], (rightScale[num18] - num4) / (num5 - num4))) {
				rightHover = num18;
				if (flag4)
					Main.SettingsEnabled_TilesSwayInWind = !Main.SettingsEnabled_TilesSwayInWind;
			}

			num18++;
		}

		if (category == 3) {
			int num21 = 0;
			float num22 = num;
			if (flag)
				num2 = 126f;

			Vector3 hSLVector = Main.mouseColorSlider.GetHSLVector();
			Main.mouseColorSlider.ApplyToMainLegacyBars();
			DrawRightSide(sb, Lang.menu[64].Value, num21, vector6, vector7, rightScale[num21], 1f);
			skipRightSlot[num21] = true;
			num21++;
			vector6.X -= num22;
			if (DrawRightSide(sb, "", num21, vector6, vector7, rightScale[num21], (rightScale[num21] - num4) / (num5 - num4))) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num21;
			}

			valuePosition.X = vector3.X + vector2.X - (float)(num3 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num2;
			DelegateMethods.v3_1 = hSLVector;
			float x = DrawValueBar(sb, scale, hSLVector.X, 0, DelegateMethods.ColorLerp_HSL_H);
			if ((inBar || rightLock == num21) && !notBar) {
				rightHover = num21;
				if (Main.mouseLeft && rightLock == num21) {
					hSLVector.X = x;
					noSound = true;
				}
			}

			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num3 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num21;
			}

			if (rightHover == num21) {
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 5;
				Main.menuMode = 25;
			}

			num21++;
			if (DrawRightSide(sb, "", num21, vector6, vector7, rightScale[num21], (rightScale[num21] - num4) / (num5 - num4))) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num21;
			}

			valuePosition.X = vector3.X + vector2.X - (float)(num3 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num2;
			DelegateMethods.v3_1 = hSLVector;
			x = DrawValueBar(sb, scale, hSLVector.Y, 0, DelegateMethods.ColorLerp_HSL_S);
			if ((inBar || rightLock == num21) && !notBar) {
				rightHover = num21;
				if (Main.mouseLeft && rightLock == num21) {
					hSLVector.Y = x;
					noSound = true;
				}
			}

			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num3 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num21;
			}

			if (rightHover == num21) {
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 6;
				Main.menuMode = 25;
			}

			num21++;
			if (DrawRightSide(sb, "", num21, vector6, vector7, rightScale[num21], (rightScale[num21] - num4) / (num5 - num4))) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num21;
			}

			valuePosition.X = vector3.X + vector2.X - (float)(num3 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num2;
			DelegateMethods.v3_1 = hSLVector;
			DelegateMethods.v3_1.Z = Utils.GetLerpValue(0.15f, 1f, DelegateMethods.v3_1.Z, clamped: true);
			x = DrawValueBar(sb, scale, DelegateMethods.v3_1.Z, 0, DelegateMethods.ColorLerp_HSL_L);
			if ((inBar || rightLock == num21) && !notBar) {
				rightHover = num21;
				if (Main.mouseLeft && rightLock == num21) {
					hSLVector.Z = x * 0.85f + 0.15f;
					noSound = true;
				}
			}

			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num3 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num21;
			}

			if (rightHover == num21) {
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 7;
				Main.menuMode = 25;
			}

			num21++;
			if (hSLVector.Z < 0.15f)
				hSLVector.Z = 0.15f;

			Main.mouseColorSlider.SetHSL(hSLVector);
			Main.mouseColor = Main.mouseColorSlider.GetColor();
			vector6.X += num22;
			DrawRightSide(sb, "", num21, vector6, vector7, rightScale[num21], 1f);
			skipRightSlot[num21] = true;
			num21++;
			hSLVector = Main.mouseBorderColorSlider.GetHSLVector();
			if (PlayerInput.UsingGamepad && rightHover == -1)
				Main.mouseBorderColorSlider.ApplyToMainLegacyBars();

			DrawRightSide(sb, Lang.menu[217].Value, num21, vector6, vector7, rightScale[num21], 1f);
			skipRightSlot[num21] = true;
			num21++;
			vector6.X -= num22;
			if (DrawRightSide(sb, "", num21, vector6, vector7, rightScale[num21], (rightScale[num21] - num4) / (num5 - num4))) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num21;
			}

			valuePosition.X = vector3.X + vector2.X - (float)(num3 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num2;
			DelegateMethods.v3_1 = hSLVector;
			x = DrawValueBar(sb, scale, hSLVector.X, 0, DelegateMethods.ColorLerp_HSL_H);
			if ((inBar || rightLock == num21) && !notBar) {
				rightHover = num21;
				if (Main.mouseLeft && rightLock == num21) {
					hSLVector.X = x;
					noSound = true;
				}
			}

			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num3 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num21;
			}

			if (rightHover == num21) {
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 5;
				Main.menuMode = 252;
			}

			num21++;
			if (DrawRightSide(sb, "", num21, vector6, vector7, rightScale[num21], (rightScale[num21] - num4) / (num5 - num4))) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num21;
			}

			valuePosition.X = vector3.X + vector2.X - (float)(num3 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num2;
			DelegateMethods.v3_1 = hSLVector;
			x = DrawValueBar(sb, scale, hSLVector.Y, 0, DelegateMethods.ColorLerp_HSL_S);
			if ((inBar || rightLock == num21) && !notBar) {
				rightHover = num21;
				if (Main.mouseLeft && rightLock == num21) {
					hSLVector.Y = x;
					noSound = true;
				}
			}

			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num3 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num21;
			}

			if (rightHover == num21) {
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 6;
				Main.menuMode = 252;
			}

			num21++;
			if (DrawRightSide(sb, "", num21, vector6, vector7, rightScale[num21], (rightScale[num21] - num4) / (num5 - num4))) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num21;
			}

			valuePosition.X = vector3.X + vector2.X - (float)(num3 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num2;
			DelegateMethods.v3_1 = hSLVector;
			x = DrawValueBar(sb, scale, hSLVector.Z, 0, DelegateMethods.ColorLerp_HSL_L);
			if ((inBar || rightLock == num21) && !notBar) {
				rightHover = num21;
				if (Main.mouseLeft && rightLock == num21) {
					hSLVector.Z = x;
					noSound = true;
				}
			}

			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num3 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num21;
			}

			if (rightHover == num21) {
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 7;
				Main.menuMode = 252;
			}

			num21++;
			if (DrawRightSide(sb, "", num21, vector6, vector7, rightScale[num21], (rightScale[num21] - num4) / (num5 - num4))) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num21;
			}

			valuePosition.X = vector3.X + vector2.X - (float)(num3 / 2) - 20f;
			valuePosition.Y -= 3f;
			valuePosition.X -= num2;
			DelegateMethods.v3_1 = hSLVector;
			float num23 = Main.mouseBorderColorSlider.Alpha;
			x = DrawValueBar(sb, scale, num23, 0, DelegateMethods.ColorLerp_HSL_O);
			if ((inBar || rightLock == num21) && !notBar) {
				rightHover = num21;
				if (Main.mouseLeft && rightLock == num21) {
					num23 = x;
					noSound = true;
				}
			}

			if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num3 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f) {
				if (rightLock == -1)
					notBar = true;

				rightHover = num21;
			}

			if (rightHover == num21) {
				UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 8;
				Main.menuMode = 252;
			}

			num21++;
			Main.mouseBorderColorSlider.SetHSL(hSLVector);
			Main.mouseBorderColorSlider.Alpha = num23;
			Main.MouseBorderColor = Main.mouseBorderColorSlider.GetColor();
			vector6.X += num22;
			DrawRightSide(sb, "", num21, vector6, vector7, rightScale[num21], 1f);
			skipRightSlot[num21] = true;
			num21++;
			string txt = "";
			switch (LockOnHelper.UseMode) {
				case LockOnHelper.LockOnMode.FocusTarget:
					txt = Lang.menu[232].Value;
					break;
				case LockOnHelper.LockOnMode.TargetClosest:
					txt = Lang.menu[233].Value;
					break;
				case LockOnHelper.LockOnMode.ThreeDS:
					txt = Lang.menu[234].Value;
					break;
			}

			if (DrawRightSide(sb, txt, num21, vector6, vector7, rightScale[num21] * 0.9f, (rightScale[num21] - num4) / (num5 - num4))) {
				rightHover = num21;
				if (flag4)
					LockOnHelper.CycleUseModes();
			}

			num21++;
			if (DrawRightSide(sb, Player.SmartCursorSettings.SmartBlocksEnabled ? Lang.menu[215].Value : Lang.menu[216].Value, num21, vector6, vector7, rightScale[num21] * 0.9f, (rightScale[num21] - num4) / (num5 - num4))) {
				rightHover = num21;
				if (flag4)
					Player.SmartCursorSettings.SmartBlocksEnabled = !Player.SmartCursorSettings.SmartBlocksEnabled;
			}

			num21++;
			if (DrawRightSide(sb, Main.cSmartCursorModeIsToggleAndNotHold ? Lang.menu[121].Value : Lang.menu[122].Value, num21, vector6, vector7, rightScale[num21], (rightScale[num21] - num4) / (num5 - num4))) {
				rightHover = num21;
				if (flag4)
					Main.cSmartCursorModeIsToggleAndNotHold = !Main.cSmartCursorModeIsToggleAndNotHold;
			}

			num21++;
			if (DrawRightSide(sb, Player.SmartCursorSettings.SmartAxeAfterPickaxe ? Lang.menu[214].Value : Lang.menu[213].Value, num21, vector6, vector7, rightScale[num21] * 0.9f, (rightScale[num21] - num4) / (num5 - num4))) {
				rightHover = num21;
				if (flag4)
					Player.SmartCursorSettings.SmartAxeAfterPickaxe = !Player.SmartCursorSettings.SmartAxeAfterPickaxe;
			}

			num21++;
		}

		if (rightHover != -1 && rightLock == -1)
			rightLock = rightHover;

		for (int n = 0; n < num8 + 1; n++) {
			UILinkPointNavigator.SetPosition(2900 + n, vector4 + vector5 * (n + 1));
		}

		Vector2 zero = Vector2.Zero;
		if (flag)
			zero.X = -40f;

		for (int num24 = 0; num24 < num12; num24++) {
			if (!skipRightSlot[num24]) {
				UILinkPointNavigator.SetPosition(2930 + num11, vector6 + zero + vector7 * (num24 + 1));
				num11++;
			}
		}

		UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_RIGHT = num11;
		Main.DrawInterface_29_SettingsButton();
		Main.DrawGamepadInstructions();
		Main.mouseText = false;
		Main.instance.GUIBarsDraw();
		Main.instance.DrawMouseOver();
		Main.DrawCursor(Main.DrawThickCursor());
	}

	public static void MouseOver()
	{
		if (Main.ingameOptionsWindow) {
			if (_GUIHover.Contains(Main.MouseScreen.ToPoint()))
				Main.mouseText = true;

			if (_mouseOverText != null)
				Main.instance.MouseText(_mouseOverText, 0, 0);

			_mouseOverText = null;
		}
	}

	public static bool DrawLeftSide(SpriteBatch sb, string txt, int i, Vector2 anchor, Vector2 offset, float[] scales, float minscale = 0.7f, float maxscale = 0.8f, float scalespeed = 0.01f)
	{
		bool flag = false;
		if (_leftSideCategoryMapping.TryGetValue(i, out var value))
			flag = category == value;

		Color color = Color.Lerp(Color.Gray, Color.White, (scales[i] - minscale) / (maxscale - minscale));
		if (flag)
			color = Color.Gold;

		Vector2 vector = Utils.DrawBorderStringBig(sb, txt, anchor + offset * (1 + i), color, scales[i], 0.5f, 0.5f);
		bool flag2 = new Rectangle((int)anchor.X - (int)vector.X / 2, (int)anchor.Y + (int)(offset.Y * (float)(1 + i)) - (int)vector.Y / 2, (int)vector.X, (int)vector.Y).Contains(new Point(Main.mouseX, Main.mouseY));
		if (!_canConsumeHover)
			return false;

		if (flag2) {
			_canConsumeHover = false;
			return true;
		}

		return false;
	}

	public static bool DrawRightSide(SpriteBatch sb, string txt, int i, Vector2 anchor, Vector2 offset, float scale, float colorScale, Color over = default(Color))
	{
		Color color = Color.Lerp(Color.Gray, Color.White, colorScale);
		if (over != default(Color))
			color = over;

		Vector2 vector = Utils.DrawBorderString(sb, txt, anchor + offset * (1 + i), color, scale, 0.5f, 0.5f);
		valuePosition = anchor + offset * (1 + i) + vector * new Vector2(0.5f, 0f);
		bool flag = new Rectangle((int)anchor.X - (int)vector.X / 2, (int)anchor.Y + (int)(offset.Y * (float)(1 + i)) - (int)vector.Y / 2, (int)vector.X, (int)vector.Y).Contains(new Point(Main.mouseX, Main.mouseY));
		if (!_canConsumeHover)
			return false;

		if (flag) {
			_canConsumeHover = false;
			return true;
		}

		return false;
	}

	public static Rectangle GetExpectedRectangleForNotification(int itemIndex, Vector2 anchor, Vector2 offset, int areaWidth) => Utils.CenteredRectangle(anchor + offset * (1 + itemIndex), new Vector2(areaWidth, offset.Y - 4f));

	public static bool DrawValue(SpriteBatch sb, string txt, int i, float scale, Color over = default(Color))
	{
		Color color = Color.Gray;
		Vector2 vector = FontAssets.MouseText.Value.MeasureString(txt) * scale;
		bool flag = new Rectangle((int)valuePosition.X, (int)valuePosition.Y - (int)vector.Y / 2, (int)vector.X, (int)vector.Y).Contains(new Point(Main.mouseX, Main.mouseY));
		if (flag)
			color = Color.White;

		if (over != default(Color))
			color = over;

		Utils.DrawBorderString(sb, txt, valuePosition, color, scale, 0f, 0.5f);
		valuePosition.X += vector.X;
		if (!_canConsumeHover)
			return false;

		if (flag) {
			_canConsumeHover = false;
			return true;
		}

		return false;
	}

	public static float DrawValueBar(SpriteBatch sb, float scale, float perc, int lockState = 0, Utils.ColorLerpMethod colorMethod = null)
	{
		if (colorMethod == null)
			colorMethod = Utils.ColorLerp_BlackToWhite;

		Texture2D value = TextureAssets.ColorBar.Value;
		Vector2 vector = new Vector2(value.Width, value.Height) * scale;
		valuePosition.X -= (int)vector.X;
		Rectangle rectangle = new Rectangle((int)valuePosition.X, (int)valuePosition.Y - (int)vector.Y / 2, (int)vector.X, (int)vector.Y);
		Rectangle destinationRectangle = rectangle;
		sb.Draw(value, rectangle, Color.White);
		int num = 167;
		float num2 = (float)rectangle.X + 5f * scale;
		float num3 = (float)rectangle.Y + 4f * scale;
		for (float num4 = 0f; num4 < (float)num; num4 += 1f) {
			float percent = num4 / (float)num;
			sb.Draw(TextureAssets.ColorBlip.Value, new Vector2(num2 + num4 * scale, num3), null, colorMethod(percent), 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
		}

		rectangle.Inflate((int)(-5f * scale), 0);
		bool flag = rectangle.Contains(new Point(Main.mouseX, Main.mouseY));
		if (lockState == 2)
			flag = false;

		if (flag || lockState == 1)
			sb.Draw(TextureAssets.ColorHighlight.Value, destinationRectangle, Main.OurFavoriteColor);

		sb.Draw(TextureAssets.ColorSlider.Value, new Vector2(num2 + 167f * scale * perc, num3 + 4f * scale), null, Color.White, 0f, new Vector2(0.5f * (float)TextureAssets.ColorSlider.Width(), 0.5f * (float)TextureAssets.ColorSlider.Height()), scale, SpriteEffects.None, 0f);
		if (Main.mouseX >= rectangle.X && Main.mouseX <= rectangle.X + rectangle.Width) {
			inBar = flag;
			return (float)(Main.mouseX - rectangle.X) / (float)rectangle.Width;
		}

		inBar = false;
		if (rectangle.X >= Main.mouseX)
			return 0f;

		return 1f;
	}
}
