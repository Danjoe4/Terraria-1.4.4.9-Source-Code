using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.UI;

namespace Terraria.Map;

public class PingMapLayer : IMapLayer
{
	private struct Ping
	{
		public readonly Vector2 Position;
		public readonly DateTime Time;

		public Ping(Vector2 position)
		{
			Position = position;
			Time = DateTime.Now;
		}
	}

	private const double PING_DURATION_IN_SECONDS = 15.0;
	private const double PING_FRAME_RATE = 10.0;
	private readonly SlotVector<Ping> _pings = new SlotVector<Ping>(100);

	public void Draw(ref MapOverlayDrawContext context, ref string text)
	{
		SpriteFrame frame = new SpriteFrame(1, 5);
		DateTime now = DateTime.Now;
		foreach (SlotVector<Ping>.ItemPair item in (IEnumerable<SlotVector<Ping>.ItemPair>)_pings) {
			Ping value = item.Value;
			double totalSeconds = (now - value.Time).TotalSeconds;
			int num = (int)(totalSeconds * 10.0);
			frame.CurrentRow = (byte)(num % (int)frame.RowCount);
			context.Draw(TextureAssets.MapPing.Value, value.Position, frame, Alignment.Center);
			if (totalSeconds > 15.0)
				_pings.Remove(item.Id);
		}
	}

	public void Add(Vector2 position)
	{
		if (_pings.Count != _pings.Capacity)
			_pings.Add(new Ping(position));
	}
}
