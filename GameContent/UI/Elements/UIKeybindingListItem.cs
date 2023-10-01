using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Chat;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements;

public class UIKeybindingListItem : UIElement
{
	private InputMode _inputmode;
	private Color _color;
	private string _keybind;

	public UIKeybindingListItem(string bind, InputMode mode, Color color)
	{
		_keybind = bind;
		_inputmode = mode;
		_color = color;
		base.OnLeftClick += OnClickMethod;
	}

	public void OnClickMethod(UIMouseEvent evt, UIElement listeningElement)
	{
		if (PlayerInput.ListeningTrigger != _keybind) {
			if (PlayerInput.CurrentProfile.AllowEditting)
				PlayerInput.ListenFor(_keybind, _inputmode);
			else
				PlayerInput.ListenFor(null, _inputmode);
		}
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		float num = 6f;
		base.DrawSelf(spriteBatch);
		CalculatedStyle dimensions = GetDimensions();
		float num2 = dimensions.Width + 1f;
		Vector2 vector = new Vector2(dimensions.X, dimensions.Y);
		bool flag = PlayerInput.ListeningTrigger == _keybind;
		Vector2 baseScale = new Vector2(0.8f);
		Color value = (flag ? Color.Gold : (base.IsMouseHovering ? Color.White : Color.Silver));
		value = Color.Lerp(value, Color.White, base.IsMouseHovering ? 0.5f : 0f);
		Color color = (base.IsMouseHovering ? _color : _color.MultiplyRGBA(new Color(180, 180, 180)));
		Vector2 position = vector;
		Utils.DrawSettingsPanel(spriteBatch, position, num2, color);
		position.X += 8f;
		position.Y += 2f + num;
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, GetFriendlyName(), position, value, 0f, Vector2.Zero, baseScale, num2);
		position.X -= 17f;
		List<string> list = PlayerInput.CurrentProfile.InputModes[_inputmode].KeyStatus[_keybind];
		string text = GenInput(list);
		if (string.IsNullOrEmpty(text)) {
			text = Lang.menu[195].Value;
			if (!flag)
				value = new Color(80, 80, 80);
		}

		Vector2 stringSize = ChatManager.GetStringSize(FontAssets.ItemStack.Value, text, baseScale);
		position = new Vector2(dimensions.X + dimensions.Width - stringSize.X - 10f, dimensions.Y + 2f + num);
		if (_inputmode == InputMode.XBoxGamepad || _inputmode == InputMode.XBoxGamepadUI)
			position += new Vector2(0f, -3f);

		GlyphTagHandler.GlyphsScale = 0.85f;
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text, position, value, 0f, Vector2.Zero, baseScale, num2);
		GlyphTagHandler.GlyphsScale = 1f;
	}

	private string GenInput(List<string> list)
	{
		if (list.Count == 0)
			return "";

		string text = "";
		switch (_inputmode) {
			case InputMode.XBoxGamepad:
			case InputMode.XBoxGamepadUI: {
				text = GlyphTagHandler.GenerateTag(list[0]);
				for (int j = 1; j < list.Count; j++) {
					text = text + "/" + GlyphTagHandler.GenerateTag(list[j]);
				}

				break;
			}
			case InputMode.Keyboard:
			case InputMode.KeyboardUI:
			case InputMode.Mouse: {
				text = list[0];
				for (int i = 1; i < list.Count; i++) {
					text = text + "/" + list[i];
				}

				break;
			}
		}

		return text;
	}

	private string GetFriendlyName()
	{
		switch (_keybind) {
			default:
				return _keybind;
			case "MouseLeft":
				return Lang.menu[162].Value;
			case "MouseRight":
				return Lang.menu[163].Value;
			case "Up":
				return Lang.menu[148].Value;
			case "Down":
				return Lang.menu[149].Value;
			case "Left":
				return Lang.menu[150].Value;
			case "Right":
				return Lang.menu[151].Value;
			case "Jump":
				return Lang.menu[152].Value;
			case "Throw":
				return Lang.menu[153].Value;
			case "Inventory":
				return Lang.menu[154].Value;
			case "Grapple":
				return Lang.menu[155].Value;
			case "SmartSelect":
				return Lang.menu[160].Value;
			case "SmartCursor":
				return Lang.menu[161].Value;
			case "QuickMount":
				return Lang.menu[158].Value;
			case "QuickHeal":
				return Lang.menu[159].Value;
			case "QuickMana":
				return Lang.menu[156].Value;
			case "QuickBuff":
				return Lang.menu[157].Value;
			case "MapZoomIn":
				return Lang.menu[168].Value;
			case "MapZoomOut":
				return Lang.menu[169].Value;
			case "MapAlphaUp":
				return Lang.menu[171].Value;
			case "MapAlphaDown":
				return Lang.menu[170].Value;
			case "MapFull":
				return Lang.menu[173].Value;
			case "MapStyle":
				return Lang.menu[172].Value;
			case "Hotbar1":
				return Lang.menu[176].Value;
			case "Hotbar2":
				return Lang.menu[177].Value;
			case "Hotbar3":
				return Lang.menu[178].Value;
			case "Hotbar4":
				return Lang.menu[179].Value;
			case "Hotbar5":
				return Lang.menu[180].Value;
			case "Hotbar6":
				return Lang.menu[181].Value;
			case "Hotbar7":
				return Lang.menu[182].Value;
			case "Hotbar8":
				return Lang.menu[183].Value;
			case "Hotbar9":
				return Lang.menu[184].Value;
			case "Hotbar10":
				return Lang.menu[185].Value;
			case "HotbarMinus":
				return Lang.menu[174].Value;
			case "HotbarPlus":
				return Lang.menu[175].Value;
			case "DpadRadial1":
				return Lang.menu[186].Value;
			case "DpadRadial2":
				return Lang.menu[187].Value;
			case "DpadRadial3":
				return Lang.menu[188].Value;
			case "DpadRadial4":
				return Lang.menu[189].Value;
			case "RadialHotbar":
				return Lang.menu[190].Value;
			case "RadialQuickbar":
				return Lang.menu[244].Value;
			case "DpadSnap1":
				return Lang.menu[191].Value;
			case "DpadSnap2":
				return Lang.menu[192].Value;
			case "DpadSnap3":
				return Lang.menu[193].Value;
			case "DpadSnap4":
				return Lang.menu[194].Value;
			case "LockOn":
				return Lang.menu[231].Value;
			case "ViewZoomIn":
				return Language.GetTextValue("UI.ZoomIn");
			case "ViewZoomOut":
				return Language.GetTextValue("UI.ZoomOut");
			case "ToggleCreativeMenu":
				return Language.GetTextValue("UI.ToggleCreativeMenu");
			case "Loadout1":
				return Language.GetTextValue("UI.Loadout1");
			case "Loadout2":
				return Language.GetTextValue("UI.Loadout2");
			case "Loadout3":
				return Language.GetTextValue("UI.Loadout3");
			case "ToggleCameraMode":
				return Language.GetTextValue("UI.ToggleCameraMode");
		}
	}
}
