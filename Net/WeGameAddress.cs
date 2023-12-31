using rail;

namespace Terraria.Net;

public class WeGameAddress : RemoteAddress
{
	public readonly RailID rail_id;
	private string nickname;

	public WeGameAddress(RailID id, string name)
	{
		Type = AddressType.WeGame;
		rail_id = id;
		nickname = name;
	}

	public override string ToString() => "WEGAME_0:" + rail_id.id_;
	public override string GetIdentifier() => ToString();

	public override bool IsLocalHost()
	{
		if (Program.LaunchParameters.ContainsKey("-localwegameid"))
			return Program.LaunchParameters["-localwegameid"].Equals(rail_id.id_.ToString());

		return false;
	}

	public override string GetFriendlyName() => nickname;
}
