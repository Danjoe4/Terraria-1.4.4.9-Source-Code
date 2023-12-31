namespace Terraria.UI;

public struct StyleDimension
{
	public static StyleDimension Fill = new StyleDimension(0f, 1f);
	public static StyleDimension Empty = new StyleDimension(0f, 0f);
	public float Pixels;
	public float Precent;

	public StyleDimension(float pixels, float precent)
	{
		Pixels = pixels;
		Precent = precent;
	}

	public void Set(float pixels, float precent)
	{
		Pixels = pixels;
		Precent = precent;
	}

	public float GetValue(float containerSize) => Pixels + Precent * containerSize;
	public static StyleDimension FromPixels(float pixels) => new StyleDimension(pixels, 0f);
	public static StyleDimension FromPercent(float percent) => new StyleDimension(0f, percent);
	public static StyleDimension FromPixelsAndPercent(float pixels, float percent) => new StyleDimension(pixels, percent);
}
