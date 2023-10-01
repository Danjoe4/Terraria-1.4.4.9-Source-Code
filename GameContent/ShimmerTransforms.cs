using Terraria.ID;

namespace Terraria.GameContent;

public static class ShimmerTransforms
{
	public static class RecipeSets
	{
		public static bool[] PostSkeletron;
		public static bool[] PostGolem;
	}

	public static int GetDecraftingRecipeIndex(int type)
	{
		int num = ItemID.Sets.IsCrafted[type];
		if (num < 0)
			return -1;

		if (WorldGen.crimson && ItemID.Sets.IsCraftedCrimson[type] >= 0)
			return ItemID.Sets.IsCraftedCrimson[type];

		if (!WorldGen.crimson && ItemID.Sets.IsCraftedCorruption[type] >= 0)
			return ItemID.Sets.IsCraftedCorruption[type];

		return num;
	}

	public static bool IsItemTransformLocked(int type)
	{
		int decraftingRecipeIndex = GetDecraftingRecipeIndex(type);
		if (decraftingRecipeIndex < 0)
			return false;

		if (!NPC.downedBoss3 && RecipeSets.PostSkeletron[decraftingRecipeIndex])
			return true;

		if (!NPC.downedGolemBoss && RecipeSets.PostGolem[decraftingRecipeIndex])
			return true;

		return false;
	}

	public static void UpdateRecipeSets()
	{
		RecipeSets.PostSkeletron = Utils.MapArray(Main.recipe, (Recipe r) => r.ContainsIngredient(154));
		RecipeSets.PostGolem = Utils.MapArray(Main.recipe, (Recipe r) => r.ContainsIngredient(1101));
	}
}
