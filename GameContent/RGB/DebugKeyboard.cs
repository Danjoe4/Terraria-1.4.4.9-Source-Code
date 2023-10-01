using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

internal class DebugKeyboard : RgbDevice
{
	private DebugKeyboard(Fragment fragment)
		: base(RgbDeviceVendor.Virtual, RgbDeviceType.Virtual, fragment, new DeviceColorProfile())
	{
	}

	public static DebugKeyboard Create()
	{
		int num = 400;
		int num2 = 100;
		Point[] array = new Point[num * num2];
		for (int i = 0; i < num2; i++) {
			for (int j = 0; j < num; j++) {
				array[i * num + j] = new Point(j / 10, i / 10);
			}
		}

		Vector2[] array2 = new Vector2[num * num2];
		for (int k = 0; k < num2; k++) {
			for (int l = 0; l < num; l++) {
				array2[k * num + l] = new Vector2((float)l / (float)num2, (float)k / (float)num2);
			}
		}

		return new DebugKeyboard(Fragment.FromCustom(array, array2));
	}

	public override void Present()
	{
	}

	public override void DebugDraw(IDebugDrawer drawer, Vector2 position, float scale)
	{
		for (int i = 0; i < base.LedCount; i++) {
			Vector2 ledCanvasPosition = GetLedCanvasPosition(i);
			drawer.DrawSquare(new Vector4(ledCanvasPosition * scale + position, scale / 100f, scale / 100f), new Color(GetUnprocessedLedColor(i)));
		}
	}
}
