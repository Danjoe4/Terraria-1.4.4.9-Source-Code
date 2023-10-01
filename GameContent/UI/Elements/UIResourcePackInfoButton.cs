using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.IO;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIResourcePackInfoButton<T> : UITextPanel<T>
{
	private readonly Asset<Texture2D> _BasePanelTexture;
	private readonly Asset<Texture2D> _hoveredBorderTexture;
	private ResourcePack _resourcePack;

	public ResourcePack ResourcePack {
		get {
			return _resourcePack;
		}
		set {
			_resourcePack = value;
		}
	}

	public UIResourcePackInfoButton(T text, float textScale = 1f, bool large = false)
		: base(text, textScale, large)
	{
		_BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale");
		_hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder");
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		if (_drawPanel) {
			CalculatedStyle dimensions = GetDimensions();
			int num = 10;
			int num2 = 10;
			Utils.DrawSplicedPanel(spriteBatch, _BasePanelTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, num, num, num2, num2, Color.Lerp(Color.Black, _color, 0.8f) * 0.5f);
			if (base.IsMouseHovering)
				Utils.DrawSplicedPanel(spriteBatch, _hoveredBorderTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, num, num, num2, num2, Color.White);
		}

		DrawText(spriteBatch);
	}
}
