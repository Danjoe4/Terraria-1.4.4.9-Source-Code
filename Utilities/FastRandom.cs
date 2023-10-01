using System;

namespace Terraria.Utilities;

public struct FastRandom
{
	private const ulong RANDOM_MULTIPLIER = 25214903917uL;
	private const ulong RANDOM_ADD = 11uL;
	private const ulong RANDOM_MASK = 281474976710655uL;

	public ulong Seed { get; private set; }

	public FastRandom(ulong seed)
	{
		this = default(FastRandom);
		Seed = seed;
	}

	public FastRandom(int seed)
	{
		this = default(FastRandom);
		Seed = (ulong)seed;
	}

	public FastRandom WithModifier(ulong modifier) => new FastRandom(NextSeed(modifier) ^ Seed);
	public FastRandom WithModifier(int x, int y) => WithModifier((ulong)(x + 2654435769u + ((long)y << 6)) + ((ulong)y >> 2));
	public static FastRandom CreateWithRandomSeed() => new FastRandom((ulong)Guid.NewGuid().GetHashCode());

	public void NextSeed()
	{
		Seed = NextSeed(Seed);
	}

	private int NextBits(int bits)
	{
		Seed = NextSeed(Seed);
		return (int)(Seed >> 48 - bits);
	}

	public float NextFloat() => (float)NextBits(24) * 5.9604645E-08f;
	public double NextDouble() => (float)NextBits(32) * 4.656613E-10f;

	public int Next(int max)
	{
		if ((max & -max) == max)
			return (int)((long)max * (long)NextBits(31) >> 31);

		int num;
		int num2;
		do {
			num = NextBits(31);
			num2 = num % max;
		} while (num - num2 + (max - 1) < 0);

		return num2;
	}

	public int Next(int min, int max) => Next(max - min) + min;
	private static ulong NextSeed(ulong seed) => (seed * 25214903917L + 11) & 0xFFFFFFFFFFFFuL;
}
