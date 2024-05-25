using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace K8055Velleman.Game;

public enum AudioFile
{
	MouseOver,
	BackGroundMusic,
}

public struct AudioVolume()
{
    public readonly float GameVolume { get { return SaveManager.Settings.GameVolume; } set { SaveManager.Settings.GameVolume = value; SaveManager.SaveSettings(); AudioManager.UpdateAudioVolume(); } }
	public readonly float UiVolume { get { return SaveManager.Settings.UiVolume; } set { SaveManager.Settings.UiVolume = value; SaveManager.SaveSettings(); AudioManager.UpdateAudioVolume(); } }
	public readonly float MusicVolume { get { return SaveManager.Settings.MusicVolume; } set { SaveManager.Settings.MusicVolume = value; SaveManager.SaveSettings(); AudioManager.UpdateAudioVolume(); } }
    public readonly float EffectVolume { get { return SaveManager.Settings.EffectVolume; } set { SaveManager.Settings.EffectVolume = value; SaveManager.SaveSettings(); AudioManager.UpdateAudioVolume(); } }

	public readonly float GetVolumeByAudioFile(AudioFile audioFile)
	{
        return audioFile switch
        {
            AudioFile.MouseOver => UiVolume * GameVolume,
            AudioFile.BackGroundMusic => MusicVolume * GameVolume,
            _ => GameVolume,
        };
    }

}

internal static class AudioManager
{
	private static List<string> musicFiles = ["Musics\\Mr-Blackhole - Category.wav", "Musics\\NOmki - Netrunner.wav", "Musics\\NOmki - Time.wav", "Musics\\punkerrr - Virtual Cataclysm.wav", "Musics\\RyuuAkito & SquashHead - Damaged Artificial Nervous System.wav"];

	private static readonly Dictionary<AudioFile, List<MediaPlayer>> s_mediaPlayers = [];

	public static readonly AudioVolume AudioVolume = new();

	public static void PlaySound(AudioFile audioFile, bool loop = false)
	{
        MediaPlayer media = new();
		if (s_mediaPlayers.ContainsKey(audioFile)) s_mediaPlayers[audioFile].Add(media);
		else s_mediaPlayers.Add(audioFile, [media]);
		string filePath = new FileInfo("Resources\\Audio\\" + AudioTypeToString(audioFile)).FullName;
		media.Open(new("file:///" + filePath));
        media.MediaEnded += (sender, eventArgs) => { 
			media.Close(); 
			s_mediaPlayers[audioFile].Remove(media);
            if (loop)
            {
                PlaySound(audioFile, true);
            }
        };
		media.Volume = AudioVolume.GetVolumeByAudioFile(audioFile);
        media.Play();

    }

	public static void StopSound(AudioFile audioFile)
	{
		foreach(MediaPlayer mediaPlayer in s_mediaPlayers[audioFile])
		{
			mediaPlayer.Stop();
			mediaPlayer.Close();
		}
	}

	public static void UpdateAudioVolume()
	{
		foreach(AudioFile audioFile in s_mediaPlayers.Keys)
		{
			foreach(MediaPlayer mediaPlayer in s_mediaPlayers[audioFile]) 
			{
				mediaPlayer.Volume = AudioVolume.GetVolumeByAudioFile(audioFile);
            }
		}
	}

	public static string AudioTypeToString(AudioFile audioType)
	{
		return audioType switch
		{
            AudioFile.MouseOver => "MouseOver.wav",
            AudioFile.BackGroundMusic => musicFiles[GameManager.Random.Next(0, musicFiles.Count)],
            _ => null,
		};
	}
}
