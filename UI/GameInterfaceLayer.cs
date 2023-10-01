using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;

namespace Terraria.UI;

public class GameInterfaceLayer
{
	public readonly string Name;
	public InterfaceScaleType ScaleType;

	public GameInterfaceLayer(string name, InterfaceScaleType scaleType)
	{
		Name = name;
		ScaleType = scaleType;
	}

	public bool Draw()
	{
		Matrix transformMatrix;
		if (ScaleType == InterfaceScaleType.Game) {
			PlayerInput.SetZoom_World();
			transformMatrix = Main.GameViewMatrix.ZoomMatrix;
		}
		else if (ScaleType == InterfaceScaleType.UI) {
			PlayerInput.SetZoom_UI();
			transformMatrix = Main.UIScaleMatrix;
		}
		else {
			PlayerInput.SetZoom_Unscaled();
			transformMatrix = Matrix.Identity;
		}

		bool result = false;
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transformMatrix);
		try {
			result = DrawSelf();
		}
		catch (Exception e) {
			TimeLogger.DrawException(e);
		}

		Main.spriteBatch.End();
		return result;
	}

	protected virtual bool DrawSelf() => true;
}
