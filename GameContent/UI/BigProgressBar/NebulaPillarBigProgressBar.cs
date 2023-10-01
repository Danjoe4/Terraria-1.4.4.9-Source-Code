namespace Terraria.GameContent.UI.BigProgressBar;

public class NebulaPillarBigProgressBar : LunarPillarBigProgessBar
{
	internal override float GetCurrentShieldValue() => NPC.ShieldStrengthTowerNebula;
	internal override float GetMaxShieldValue() => NPC.ShieldStrengthTowerMax;
	internal override bool IsPlayerInCombatArea() => Main.LocalPlayer.ZoneTowerNebula;
}
