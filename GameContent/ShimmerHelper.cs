using Microsoft.Xna.Framework;

namespace Terraria.GameContent;

public class ShimmerHelper
{
	public static Vector2? FindSpotWithoutShimmer(Entity entity, int startX, int startY, int expand, bool allowSolidTop)
	{
		Vector2 vector = new Vector2(-entity.width / 2, -entity.height);
		for (int i = 0; i < expand; i++) {
			int num = startX - i;
			int num2 = startY - expand;
			Vector2 vector2 = new Vector2(num * 16, num2 * 16) + vector;
			if (IsSpotShimmerFree(entity, vector2, allowSolidTop))
				return vector2;

			vector2 = new Vector2((startX + i) * 16, num2 * 16) + vector;
			if (IsSpotShimmerFree(entity, vector2, allowSolidTop))
				return vector2;

			int num3 = startX - i;
			num2 = startY + expand;
			vector2 = new Vector2(num3 * 16, num2 * 16) + vector;
			if (IsSpotShimmerFree(entity, vector2, allowSolidTop))
				return vector2;

			vector2 = new Vector2((startX + i) * 16, num2 * 16) + vector;
			if (IsSpotShimmerFree(entity, vector2, allowSolidTop))
				return vector2;
		}

		for (int j = 0; j < expand; j++) {
			int num4 = startX - expand;
			int num5 = startY - j;
			Vector2 vector3 = new Vector2(num4 * 16, num5 * 16) + vector;
			if (IsSpotShimmerFree(entity, vector3, allowSolidTop))
				return vector3;

			vector3 = new Vector2((startX + expand) * 16, num5 * 16) + vector;
			if (IsSpotShimmerFree(entity, vector3, allowSolidTop))
				return vector3;

			int num6 = startX - expand;
			num5 = startY + j;
			vector3 = new Vector2(num6 * 16, num5 * 16) + vector;
			if (IsSpotShimmerFree(entity, vector3, allowSolidTop))
				return vector3;

			vector3 = new Vector2((startX + expand) * 16, num5 * 16) + vector;
			if (IsSpotShimmerFree(entity, vector3, allowSolidTop))
				return vector3;
		}

		return null;
	}

	private static bool IsSpotShimmerFree(Entity entity, Vector2 landingPosition, bool allowSolidTop)
	{
		if (Collision.SolidCollision(landingPosition, entity.width, entity.height))
			return false;

		if (!Collision.SolidCollision(landingPosition + new Vector2(0f, entity.height), entity.width, 100, allowSolidTop))
			return false;

		if (Collision.WetCollision(landingPosition, entity.width, entity.height + 100) && Collision.shimmer)
			return false;

		return true;
	}
}
