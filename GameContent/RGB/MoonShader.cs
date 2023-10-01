using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class MoonShader : ChromaShader
{
	private readonly Vector4 _moonCoreColor;
	private readonly Vector4 _moonRingColor;
	private readonly Vector4 _skyColor;
	private readonly Vector4 _cloudColor;
	private float _progress;

	public MoonShader(Color skyColor, Color moonRingColor, Color moonCoreColor)
		: this(skyColor, moonRingColor, moonCoreColor, Color.White)
	{
	}

	public MoonShader(Color skyColor, Color moonColor)
		: this(skyColor, moonColor, moonColor)
	{
	}

	public MoonShader(Color skyColor, Color moonRingColor, Color moonCoreColor, Color cloudColor)
	{
		_skyColor = skyColor.ToVector4();
		_moonRingColor = moonRingColor.ToVector4();
		_moonCoreColor = moonCoreColor.ToVector4();
		_cloudColor = cloudColor.ToVector4();
	}

	public override void Update(float elapsedTime)
	{
		if (Main.dayTime)
			_progress = (float)(Main.time / 54000.0);
		else
			_progress = (float)(Main.time / 32400.0);
	}

	[RgbProcessor(new EffectDetailLevel[] {
		EffectDetailLevel.Low
	})]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		for (int i = 0; i < fragment.Count; i++) {
			float dynamicNoise = NoiseHelper.GetDynamicNoise(fragment.GetCanvasPositionOfIndex(i) * new Vector2(0.1f, 0.5f) + new Vector2(time * 0.02f, 0f), time / 40f);
			dynamicNoise = (float)Math.Sqrt(Math.Max(0f, 1f - 2f * dynamicNoise));
			Vector4 color = Vector4.Lerp(_skyColor, _cloudColor, dynamicNoise * 0.1f);
			fragment.SetColor(i, color);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] {
		EffectDetailLevel.High
	})]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		if (device.Type != 0 && device.Type != RgbDeviceType.Virtual) {
			ProcessLowDetail(device, fragment, quality, time);
			return;
		}

		Vector2 vector = new Vector2(2f, 0.5f);
		Vector2 vector2 = new Vector2(2.5f, 1f);
		float num = _progress * (float)Math.PI + (float)Math.PI;
		Vector2 vector3 = new Vector2((float)Math.Cos(num), (float)Math.Sin(num)) * vector2 + vector;
		for (int i = 0; i < fragment.Count; i++) {
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			float dynamicNoise = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * new Vector2(0.1f, 0.5f) + new Vector2(time * 0.02f, 0f), time / 40f);
			dynamicNoise = (float)Math.Sqrt(Math.Max(0f, 1f - 2f * dynamicNoise));
			float num2 = (canvasPositionOfIndex - vector3).Length();
			Vector4 vector4 = Vector4.Lerp(_skyColor, _cloudColor, dynamicNoise * 0.15f);
			if (num2 < 0.8f)
				vector4 = Vector4.Lerp(_moonRingColor, _moonCoreColor, Math.Min(0.1f, 0.8f - num2) / 0.1f);
			else if (num2 < 1f)
				vector4 = Vector4.Lerp(vector4, _moonRingColor, Math.Min(0.2f, 1f - num2) / 0.2f);

			fragment.SetColor(i, vector4);
		}
	}
}
