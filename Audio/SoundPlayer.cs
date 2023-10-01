using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;

namespace Terraria.Audio;

public class SoundPlayer
{
	private readonly SlotVector<ActiveSound> _trackedSounds = new SlotVector<ActiveSound>(4096);

	public SlotId Play(SoundStyle style, Vector2 position)
	{
		if (Main.dedServ || style == null || !style.IsTrackable)
			return SlotId.Invalid;

		if (Vector2.DistanceSquared(Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), position) > 100000000f)
			return SlotId.Invalid;

		ActiveSound value = new ActiveSound(style, position);
		return _trackedSounds.Add(value);
	}

	public SlotId PlayLooped(SoundStyle style, Vector2 position, ActiveSound.LoopedPlayCondition loopingCondition)
	{
		if (Main.dedServ || style == null || !style.IsTrackable)
			return SlotId.Invalid;

		if (Vector2.DistanceSquared(Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), position) > 100000000f)
			return SlotId.Invalid;

		ActiveSound value = new ActiveSound(style, position, loopingCondition);
		return _trackedSounds.Add(value);
	}

	public void Reload()
	{
		StopAll();
	}

	public SlotId Play(SoundStyle style)
	{
		if (Main.dedServ || style == null || !style.IsTrackable)
			return SlotId.Invalid;

		ActiveSound value = new ActiveSound(style);
		return _trackedSounds.Add(value);
	}

	public ActiveSound GetActiveSound(SlotId id)
	{
		if (!_trackedSounds.Has(id))
			return null;

		return _trackedSounds[id];
	}

	public void PauseAll()
	{
		foreach (SlotVector<ActiveSound>.ItemPair item in (IEnumerable<SlotVector<ActiveSound>.ItemPair>)_trackedSounds) {
			item.Value.Pause();
		}
	}

	public void ResumeAll()
	{
		foreach (SlotVector<ActiveSound>.ItemPair item in (IEnumerable<SlotVector<ActiveSound>.ItemPair>)_trackedSounds) {
			item.Value.Resume();
		}
	}

	public void StopAll()
	{
		foreach (SlotVector<ActiveSound>.ItemPair item in (IEnumerable<SlotVector<ActiveSound>.ItemPair>)_trackedSounds) {
			item.Value.Stop();
		}

		_trackedSounds.Clear();
	}

	public void Update()
	{
		foreach (SlotVector<ActiveSound>.ItemPair item in (IEnumerable<SlotVector<ActiveSound>.ItemPair>)_trackedSounds) {
			try {
				item.Value.Update();
				if (!item.Value.IsPlaying)
					_trackedSounds.Remove(item.Id);
			}
			catch {
				_trackedSounds.Remove(item.Id);
			}
		}
	}

	public ActiveSound FindActiveSound(SoundStyle style)
	{
		foreach (SlotVector<ActiveSound>.ItemPair item in (IEnumerable<SlotVector<ActiveSound>.ItemPair>)_trackedSounds) {
			if (item.Value.Style == style)
				return item.Value;
		}

		return null;
	}
}
