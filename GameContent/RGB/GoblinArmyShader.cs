using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class GoblinArmyShader : ChromaShader
{
	private readonly Vector4 _primaryColor;
	private readonly Vector4 _secondaryColor;

	public GoblinArmyShader(Color primaryColor, Color secondaryColor)
	{
		_primaryColor = primaryColor.ToVector4();
		_secondaryColor = secondaryColor.ToVector4();
	}

	[RgbProcessor(new EffectDetailLevel[] {
		EffectDetailLevel.Low
	})]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		time *= 0.5f;
		for (int i = 0; i < fragment.Count; i++) {
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			canvasPositionOfIndex.Y = 1f;
			float staticNoise = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * 0.3f + new Vector2(12.5f, time * 0.2f));
			staticNoise = Math.Max(0f, 1f - staticNoise * staticNoise * 4f * staticNoise);
			staticNoise = MathHelper.Clamp(staticNoise, 0f, 1f);
			Vector4 value = Vector4.Lerp(_primaryColor, _secondaryColor, staticNoise);
			value = Vector4.Lerp(value, Vector4.One, staticNoise * staticNoise);
			Vector4 color = Vector4.Lerp(new Vector4(0f, 0f, 0f, 1f), value, staticNoise);
			fragment.SetColor(i, color);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] {
		EffectDetailLevel.High
	})]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		for (int i = 0; i < fragment.Count; i++) {
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			float staticNoise = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * 0.3f + new Vector2(12.5f, time * 0.2f));
			staticNoise = Math.Max(0f, 1f - staticNoise * staticNoise * 4f * staticNoise * (1.2f - canvasPositionOfIndex.Y)) * canvasPositionOfIndex.Y * canvasPositionOfIndex.Y;
			staticNoise = MathHelper.Clamp(staticNoise, 0f, 1f);
			Vector4 value = Vector4.Lerp(_primaryColor, _secondaryColor, staticNoise);
			value = Vector4.Lerp(value, Vector4.One, staticNoise * staticNoise * staticNoise);
			Vector4 color = Vector4.Lerp(new Vector4(0f, 0f, 0f, 1f), value, staticNoise);
			fragment.SetColor(i, color);
		}
	}
}
