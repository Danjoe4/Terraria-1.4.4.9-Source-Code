namespace Terraria.GameContent.UI.BigProgressBar;

public class SolarFlarePillarBigProgressBar : LunarPillarBigProgessBar
{
	internal override float GetCurrentShieldValue() => NPC.ShieldStrengthTowerSolar;
	internal override float GetMaxShieldValue() => NPC.ShieldStrengthTowerMax;
	internal override bool IsPlayerInCombatArea() => Main.LocalPlayer.ZoneTowerSolar;
}
