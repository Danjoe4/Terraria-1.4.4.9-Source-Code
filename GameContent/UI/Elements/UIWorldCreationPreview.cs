using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIWorldCreationPreview : UIElement
{
	private readonly Asset<Texture2D> _BorderTexture;
	private readonly Asset<Texture2D> _BackgroundExpertTexture;
	private readonly Asset<Texture2D> _BackgroundNormalTexture;
	private readonly Asset<Texture2D> _BackgroundMasterTexture;
	private readonly Asset<Texture2D> _BunnyExpertTexture;
	private readonly Asset<Texture2D> _BunnyNormalTexture;
	private readonly Asset<Texture2D> _BunnyCreativeTexture;
	private readonly Asset<Texture2D> _BunnyMasterTexture;
	private readonly Asset<Texture2D> _EvilRandomTexture;
	private readonly Asset<Texture2D> _EvilCorruptionTexture;
	private readonly Asset<Texture2D> _EvilCrimsonTexture;
	private readonly Asset<Texture2D> _SizeSmallTexture;
	private readonly Asset<Texture2D> _SizeMediumTexture;
	private readonly Asset<Texture2D> _SizeLargeTexture;
	private byte _difficulty;
	private byte _evil;
	private byte _size;

	public UIWorldCreationPreview()
	{
		_BorderTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewBorder");
		_BackgroundNormalTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyNormal1");
		_BackgroundExpertTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyExpert1");
		_BackgroundMasterTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyMaster1");
		_BunnyNormalTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyNormal2");
		_BunnyExpertTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyExpert2");
		_BunnyCreativeTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyCreative2");
		_BunnyMasterTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewDifficultyMaster2");
		_EvilRandomTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewEvilRandom");
		_EvilCorruptionTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewEvilCorruption");
		_EvilCrimsonTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewEvilCrimson");
		_SizeSmallTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewSizeSmall");
		_SizeMediumTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewSizeMedium");
		_SizeLargeTexture = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/PreviewSizeLarge");
		Width.Set(_BackgroundExpertTexture.Width(), 0f);
		Height.Set(_BackgroundExpertTexture.Height(), 0f);
	}

	public void UpdateOption(byte difficulty, byte evil, byte size)
	{
		_difficulty = difficulty;
		_evil = evil;
		_size = size;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		CalculatedStyle dimensions = GetDimensions();
		Vector2 position = new Vector2(dimensions.X + 4f, dimensions.Y + 4f);
		Color color = Color.White;
		switch (_difficulty) {
			case 0:
			case 3:
				spriteBatch.Draw(_BackgroundNormalTexture.Value, position, Color.White);
				color = Color.White;
				break;
			case 1:
				spriteBatch.Draw(_BackgroundExpertTexture.Value, position, Color.White);
				color = Color.DarkGray;
				break;
			case 2:
				spriteBatch.Draw(_BackgroundMasterTexture.Value, position, Color.White);
				color = Color.DarkGray;
				break;
		}

		switch (_size) {
			case 0:
				spriteBatch.Draw(_SizeSmallTexture.Value, position, color);
				break;
			case 1:
				spriteBatch.Draw(_SizeMediumTexture.Value, position, color);
				break;
			case 2:
				spriteBatch.Draw(_SizeLargeTexture.Value, position, color);
				break;
		}

		switch (_evil) {
			case 0:
				spriteBatch.Draw(_EvilRandomTexture.Value, position, color);
				break;
			case 1:
				spriteBatch.Draw(_EvilCorruptionTexture.Value, position, color);
				break;
			case 2:
				spriteBatch.Draw(_EvilCrimsonTexture.Value, position, color);
				break;
		}

		switch (_difficulty) {
			case 0:
				spriteBatch.Draw(_BunnyNormalTexture.Value, position, color);
				break;
			case 1:
				spriteBatch.Draw(_BunnyExpertTexture.Value, position, color);
				break;
			case 2:
				spriteBatch.Draw(_BunnyMasterTexture.Value, position, color * 1.2f);
				break;
			case 3:
				spriteBatch.Draw(_BunnyCreativeTexture.Value, position, color);
				break;
		}

		spriteBatch.Draw(_BorderTexture.Value, new Vector2(dimensions.X, dimensions.Y), Color.White);
	}
}
