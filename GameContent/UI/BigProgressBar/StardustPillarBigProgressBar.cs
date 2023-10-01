namespace Terraria.GameContent.UI.BigProgressBar;

public class StardustPillarBigProgressBar : LunarPillarBigProgessBar
{
	internal override float GetCurrentShieldValue() => NPC.ShieldStrengthTowerStardust;
	internal override float GetMaxShieldValue() => NPC.ShieldStrengthTowerMax;
	internal override bool IsPlayerInCombatArea() => Main.LocalPlayer.ZoneTowerStardust;
}
