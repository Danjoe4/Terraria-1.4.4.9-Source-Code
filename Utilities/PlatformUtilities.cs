using System;
using System.IO;

namespace Terraria.Utilities;

public static class PlatformUtilities
{
	public static void SavePng(Stream stream, int width, int height, int imgWidth, int imgHeight, byte[] data)
	{
		throw new NotSupportedException("Use Bitmap to save png images on windows");
	}
}
