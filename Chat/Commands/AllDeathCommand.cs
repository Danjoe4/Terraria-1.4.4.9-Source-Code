using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands;

[ChatCommand("AllDeath")]
public class AllDeathCommand : IChatCommand
{
	private static readonly Color RESPONSE_COLOR = new Color(255, 25, 25);

	public void ProcessIncomingMessage(string text, byte clientId)
	{
		for (int i = 0; i < 255; i++) {
			Player player = Main.player[i];
			if (player != null && player.active) {
				NetworkText text2 = NetworkText.FromKey("LegacyMultiplayer.23", player.name, player.numberOfDeathsPVE);
				if (player.numberOfDeathsPVE == 1)
					text2 = NetworkText.FromKey("LegacyMultiplayer.25", player.name, player.numberOfDeathsPVE);

				ChatHelper.BroadcastChatMessage(text2, RESPONSE_COLOR);
			}
		}
	}

	public void ProcessOutgoingMessage(ChatMessage message)
	{
	}
}
