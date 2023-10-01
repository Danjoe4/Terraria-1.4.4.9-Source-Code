using rail;
using Terraria.Social.Base;

namespace Terraria.Social.WeGame;

public class FriendsSocialModule : Terraria.Social.Base.FriendsSocialModule
{
	public override void Initialize()
	{
	}

	public override void Shutdown()
	{
	}

	public override string GetUsername()
	{
		rail_api.RailFactory().RailPlayer().GetPlayerName(out var name);
		WeGameHelper.WriteDebugString("GetUsername by wegame" + name);
		return name;
	}

	public override void OpenJoinInterface()
	{
		WeGameHelper.WriteDebugString("OpenJoinInterface by wegame");
		rail_api.RailFactory().RailFloatingWindow().AsyncShowRailFloatingWindow(EnumRailWindowType.kRailWindowFriendList, "");
	}
}
