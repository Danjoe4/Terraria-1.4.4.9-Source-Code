using Microsoft.Xna.Framework;

namespace Terraria.UI;

public struct CalculatedStyle
{
	public float X;
	public float Y;
	public float Width;
	public float Height;

	public CalculatedStyle(float x, float y, float width, float height)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}

	public Rectangle ToRectangle() => new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
	public Vector2 Position() => new Vector2(X, Y);
	public Vector2 Center() => new Vector2(X + Width * 0.5f, Y + Height * 0.5f);
}
