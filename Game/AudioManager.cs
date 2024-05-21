using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace K8055Velleman.Game;

public enum AudioFile
{
	MouseOver,
    LoadingMusic,
}

public struct AudioVolume
{
	float gameVolume = 1f;
	float uiVolume = 1f;
	float musicVolume = 0.05f;
	float effectVolume = 1f;
	public AudioVolume()
	{

	}

	public readonly float GetVolumeByAudioFile(AudioFile audioFile)
	{
        return audioFile switch
        {
            AudioFile.MouseOver => uiVolume * gameVolume,
            AudioFile.LoadingMusic => musicVolume * gameVolume,
            _ => gameVolume,
        };
    }

}

internal static class AudioManager
{
	private static Dictionary<AudioFile, List<MediaPlayer>> mediaPlayers = [];

	static AudioVolume audioVolume = new AudioVolume();

	public static void PlaySound(AudioFile audioFile, bool loop = false)
	{
        MediaPlayer media = new();
		if (mediaPlayers.ContainsKey(audioFile)) mediaPlayers[audioFile].Add(media);
		else mediaPlayers.Add(audioFile, [media]);
		media.Open(new("file:///" + new FileInfo(AudioTypeToString(audioFile)).FullName));
        media.MediaEnded += (sender, eventArgs) => { 
			if (loop) { 
				media.Position += media.Position.Negate(); 
			} else { 
				media.Close(); 
				mediaPlayers[audioFile].Remove(media); 
			} 
		};
		media.Volume = audioVolume.GetVolumeByAudioFile(audioFile);
        media.Play();

    }

	public static void StopSound(AudioFile audioFile)
	{
		foreach(MediaPlayer mediaPlayer in mediaPlayers[audioFile])
		{
			mediaPlayer.Stop();
			mediaPlayer.Close();
		}
	}

	public static string AudioTypeToString(AudioFile audioType)
	{
		return audioType switch
		{
            AudioFile.MouseOver => "Resources\\Audio\\MouseOver.wav",
            AudioFile.LoadingMusic => "Resources\\Audio\\LoadingMusic.wav",
            _ => null,
		};
	}
}
