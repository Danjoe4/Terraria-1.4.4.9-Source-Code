using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public abstract class AWorldListItem : UIPanel
{
	protected WorldFileData _data;
	protected int _glitchFrameCounter;
	protected int _glitchFrame;
	protected int _glitchVariation;

	private void UpdateGlitchAnimation(UIElement affectedElement)
	{
		_ = _glitchFrame;
		int minValue = 3;
		int num = 3;
		if (_glitchFrame == 0) {
			minValue = 15;
			num = 120;
		}

		if (++_glitchFrameCounter >= Main.rand.Next(minValue, num + 1)) {
			_glitchFrameCounter = 0;
			_glitchFrame = (_glitchFrame + 1) % 16;
			if ((_glitchFrame == 4 || _glitchFrame == 8 || _glitchFrame == 12) && Main.rand.Next(3) == 0)
				_glitchVariation = Main.rand.Next(7);
		}

		(affectedElement as UIImageFramed).SetFrame(7, 16, _glitchVariation, _glitchFrame, 0, 0);
	}

	protected void GetDifficulty(out string expertText, out Color gameModeColor)
	{
		expertText = "";
		gameModeColor = Color.White;
		if (_data.GameMode == 3) {
			expertText = Language.GetTextValue("UI.Creative");
			gameModeColor = Main.creativeModeColor;
			return;
		}

		int num = 1;
		switch (_data.GameMode) {
			case 1:
				num = 2;
				break;
			case 2:
				num = 3;
				break;
		}

		if (_data.ForTheWorthy)
			num++;

		switch (num) {
			default:
				expertText = Language.GetTextValue("UI.Normal");
				break;
			case 2:
				expertText = Language.GetTextValue("UI.Expert");
				gameModeColor = Main.mcColor;
				break;
			case 3:
				expertText = Language.GetTextValue("UI.Master");
				gameModeColor = Main.hcColor;
				break;
			case 4:
				expertText = Language.GetTextValue("UI.Legendary");
				gameModeColor = Main.legendaryModeColor;
				break;
		}
	}

	protected Asset<Texture2D> GetIcon()
	{
		if (_data.ZenithWorld)
			return Main.Assets.Request<Texture2D>("Images/UI/Icon" + (_data.IsHardMode ? "Hallow" : "") + "Everything");

		if (_data.DrunkWorld)
			return Main.Assets.Request<Texture2D>("Images/UI/Icon" + (_data.IsHardMode ? "Hallow" : "") + "CorruptionCrimson");

		if (_data.ForTheWorthy)
			return GetSeedIcon("FTW");

		if (_data.NotTheBees)
			return GetSeedIcon("NotTheBees");

		if (_data.Anniversary)
			return GetSeedIcon("Anniversary");

		if (_data.DontStarve)
			return GetSeedIcon("DontStarve");

		if (_data.RemixWorld)
			return GetSeedIcon("Remix");

		if (_data.NoTrapsWorld)
			return GetSeedIcon("Traps");

		return Main.Assets.Request<Texture2D>("Images/UI/Icon" + (_data.IsHardMode ? "Hallow" : "") + (_data.HasCorruption ? "Corruption" : "Crimson"));
	}

	protected UIElement GetIconElement()
	{
		if (_data.DrunkWorld && _data.RemixWorld) {
			Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/IconEverythingAnimated");
			UIImageFramed uIImageFramed = new UIImageFramed(asset, asset.Frame(7, 16));
			uIImageFramed.Left = new StyleDimension(4f, 0f);
			uIImageFramed.OnUpdate += UpdateGlitchAnimation;
			return uIImageFramed;
		}

		return new UIImage(GetIcon()) {
			Left = new StyleDimension(4f, 0f)
		};
	}

	private Asset<Texture2D> GetSeedIcon(string seed) => Main.Assets.Request<Texture2D>("Images/UI/Icon" + (_data.IsHardMode ? "Hallow" : "") + (_data.HasCorruption ? "Corruption" : "Crimson") + seed);
}
