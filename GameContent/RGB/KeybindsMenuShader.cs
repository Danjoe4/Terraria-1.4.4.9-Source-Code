using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

internal class KeybindsMenuShader : ChromaShader
{
	private static Vector4 _baseColor = new Color(20, 20, 20, 245).ToVector4();

	[RgbProcessor(new EffectDetailLevel[] {
		EffectDetailLevel.Low,
		EffectDetailLevel.High
	}, IsTransparent = true)]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		float num = (float)Math.Cos(time * ((float)Math.PI / 2f)) * 0.2f + 0.8f;
		Vector4 color = _baseColor * num;
		color.W = _baseColor.W;
		for (int i = 0; i < fragment.Count; i++) {
			fragment.SetColor(i, color);
		}
	}
}
