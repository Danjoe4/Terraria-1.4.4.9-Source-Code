using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class SandstormShader : ChromaShader
{
	private readonly Vector4 _backColor = new Vector4(0.2f, 0f, 0f, 1f);
	private readonly Vector4 _frontColor = new Vector4(1f, 0.5f, 0f, 1f);

	[RgbProcessor(new EffectDetailLevel[] {
		EffectDetailLevel.Low,
		EffectDetailLevel.High
	})]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		if (quality == EffectDetailLevel.Low)
			time *= 0.25f;

		for (int i = 0; i < fragment.Count; i++) {
			float staticNoise = NoiseHelper.GetStaticNoise(fragment.GetCanvasPositionOfIndex(i) * 0.3f + new Vector2(time, 0f - time) * 0.5f);
			Vector4 color = Vector4.Lerp(_backColor, _frontColor, staticNoise);
			fragment.SetColor(i, color);
		}
	}
}
