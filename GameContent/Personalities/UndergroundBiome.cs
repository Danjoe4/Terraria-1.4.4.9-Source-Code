namespace Terraria.GameContent.Personalities;

public class UndergroundBiome : AShoppingBiome
{
	public UndergroundBiome()
	{
		base.NameKey = "NormalUnderground";
	}

	public override bool IsInBiome(Player player) => player.ShoppingZone_BelowSurface;
}
