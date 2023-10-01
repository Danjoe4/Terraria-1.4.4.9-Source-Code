using System.IO;
using Microsoft.Xna.Framework.Audio;
using XPT.Core.Audio.MP3Sharp;

namespace Terraria.Audio;

public class MP3AudioTrack : ASoundEffectBasedAudioTrack
{
	private Stream _stream;
	private MP3Stream _mp3Stream;

	public MP3AudioTrack(Stream stream)
	{
		_stream = stream;
		MP3Stream mP3Stream = new MP3Stream(stream);
		int frequency = mP3Stream.Frequency;
		_mp3Stream = mP3Stream;
		CreateSoundEffect(frequency, AudioChannels.Stereo);
	}

	public override void Reuse()
	{
		_mp3Stream.Position = 0L;
	}

	public override void Dispose()
	{
		_soundEffectInstance.Dispose();
		_mp3Stream.Dispose();
		_stream.Dispose();
	}

	protected override void ReadAheadPutAChunkIntoTheBuffer()
	{
		byte[] bufferToSubmit = _bufferToSubmit;
		if (_mp3Stream.Read(bufferToSubmit, 0, bufferToSubmit.Length) < 1)
			Stop(AudioStopOptions.Immediate);
		else
			_soundEffectInstance.SubmitBuffer(_bufferToSubmit);
	}
}
