using Microsoft.Xna.Framework;

namespace Terraria.DataStructures;

public struct ContainerTransferContext
{
	private Vector2 _position;
	public bool CanVisualizeTransfers;

	public static ContainerTransferContext FromProjectile(Projectile projectile) => new ContainerTransferContext(projectile.Center);
	public static ContainerTransferContext FromBlockPosition(int x, int y) => new ContainerTransferContext(new Vector2(x * 16 + 16, y * 16 + 16));

	public static ContainerTransferContext FromUnknown(Player player)
	{
		ContainerTransferContext result = default(ContainerTransferContext);
		result.CanVisualizeTransfers = false;
		return result;
	}

	public ContainerTransferContext(Vector2 position)
	{
		_position = position;
		CanVisualizeTransfers = true;
	}

	public Vector2 GetContainerWorldPosition() => _position;
}
