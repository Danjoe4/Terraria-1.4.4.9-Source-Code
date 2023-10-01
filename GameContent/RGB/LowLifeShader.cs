using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class LowLifeShader : ChromaShader
{
	private static Vector4 _baseColor = new Color(40, 0, 8, 255).ToVector4();

	[RgbProcessor(new EffectDetailLevel[] {
		EffectDetailLevel.Low,
		EffectDetailLevel.High
	}, IsTransparent = true)]
	private void ProcessAnyDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		float num = (float)Math.Cos(time * (float)Math.PI) * 0.3f + 0.7f;
		Vector4 color = _baseColor * num;
		color.W = _baseColor.W;
		for (int i = 0; i < fragment.Count; i++) {
			fragment.SetColor(i, color);
		}
	}
}
