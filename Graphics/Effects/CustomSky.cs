using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Effects;

public abstract class CustomSky : GameEffect
{
	public abstract void Update(GameTime gameTime);

	public abstract void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth);

	public abstract bool IsActive();

	public abstract void Reset();

	public virtual Color OnTileColor(Color inColor) => inColor;
	public virtual float GetCloudAlpha() => 1f;
	public override bool IsVisible() => true;
}
