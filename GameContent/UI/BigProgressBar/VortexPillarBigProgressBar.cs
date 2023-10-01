namespace Terraria.GameContent.UI.BigProgressBar;

public class VortexPillarBigProgressBar : LunarPillarBigProgessBar
{
	internal override float GetCurrentShieldValue() => NPC.ShieldStrengthTowerVortex;
	internal override float GetMaxShieldValue() => NPC.ShieldStrengthTowerMax;
	internal override bool IsPlayerInCombatArea() => Main.LocalPlayer.ZoneTowerVortex;
}
