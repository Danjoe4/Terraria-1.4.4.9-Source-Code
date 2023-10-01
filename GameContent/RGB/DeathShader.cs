using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class DeathShader : ChromaShader
{
	private readonly Vector4 _primaryColor;
	private readonly Vector4 _secondaryColor;

	public DeathShader(Color primaryColor, Color secondaryColor)
	{
		_primaryColor = primaryColor.ToVector4();
		_secondaryColor = secondaryColor.ToVector4();
	}

	[RgbProcessor(new EffectDetailLevel[] {
		EffectDetailLevel.High,
		EffectDetailLevel.Low
	})]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		time *= 3f;
		float amount = 0f;
		float num = time % ((float)Math.PI * 4f);
		if (num < (float)Math.PI)
			amount = (float)Math.Sin(num);

		for (int i = 0; i < fragment.Count; i++) {
			Vector4 color = Vector4.Lerp(_primaryColor, _secondaryColor, amount);
			fragment.SetColor(i, color);
		}
	}
}
