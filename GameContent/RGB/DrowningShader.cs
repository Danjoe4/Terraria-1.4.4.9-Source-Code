using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class DrowningShader : ChromaShader
{
	private float _breath = 1f;

	public override void Update(float elapsedTime)
	{
		Player player = Main.player[Main.myPlayer];
		_breath = (float)(player.breath * player.breathCDMax - player.breathCD) / (float)(player.breathMax * player.breathCDMax);
	}

	[RgbProcessor(new EffectDetailLevel[] {
		EffectDetailLevel.Low
	}, IsTransparent = true)]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		for (int i = 0; i < fragment.Count; i++) {
			fragment.GetCanvasPositionOfIndex(i);
			Vector4 color = new Vector4(0f, 0f, 1f, 1f - _breath);
			fragment.SetColor(i, color);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] {
		EffectDetailLevel.High
	}, IsTransparent = true)]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		float num = _breath * 1.2f - 0.1f;
		for (int i = 0; i < fragment.Count; i++) {
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Vector4 color = Vector4.Zero;
			if (canvasPositionOfIndex.Y > num)
				color = new Vector4(0f, 0f, 1f, MathHelper.Clamp((canvasPositionOfIndex.Y - num) * 5f, 0f, 1f));

			fragment.SetColor(i, color);
		}
	}
}
