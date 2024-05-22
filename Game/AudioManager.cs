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
            AudioFile.BackGroundMusic => musicVolume * gameVolume,
            _ => gameVolume,
        };
    }

}

internal static class AudioManager
{
	private static List<string> musicFiles = ["Musics\\Mr-Blackhole - Category.wav", "Musics\\NOmki - Netrunner.wav", "Musics\\NOmki - Time.wav", "Musics\\punkerrr - Virtual Cataclysm.wav", "Musics\\RyuuAkito & SquashHead - Damaged Artificial Nervous System.wav"];

	private static Dictionary<AudioFile, List<MediaPlayer>> mediaPlayers = [];

	static AudioVolume audioVolume = new AudioVolume();

	public static void PlaySound(AudioFile audioFile, bool loop = false)
	{
        MediaPlayer media = new();
		if (mediaPlayers.ContainsKey(audioFile)) mediaPlayers[audioFile].Add(media);
		else mediaPlayers.Add(audioFile, [media]);
		string filePath = new FileInfo("Resources\\Audio\\" + AudioTypeToString(audioFile)).FullName;
		Console.WriteLine(filePath);
		media.Open(new("file:///" + filePath));
        media.MediaEnded += (sender, eventArgs) => { 
			media.Close(); 
			mediaPlayers[audioFile].Remove(media);
            if (loop)
            {
                PlaySound(audioFile, true);
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
            AudioFile.MouseOver => "MouseOver.wav",
            AudioFile.BackGroundMusic => musicFiles[GameManager.Random.Next(0, musicFiles.Count)],
            _ => null,
		};
	}
}
