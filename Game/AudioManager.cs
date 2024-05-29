using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media;

namespace K8055Velleman.Game;

internal static class AudioManager
{
	private static readonly List<string> s_musicFiles = ["Musics\\Mr-Blackhole - Category.wav", "Musics\\NOmki - Netrunner.wav", "Musics\\NOmki - Time.wav", "Musics\\punkerrr - Virtual Cataclysm.wav", "Musics\\RyuuAkito & SquashHead - Damaged Artificial Nervous System.wav"];
	private static readonly List<string> s_enemyDeath = ["EnemyDeath\\Death 1.wav", "EnemyDeath\\Death 2.wav"];

	private static readonly Dictionary<AudioFile, List<MediaPlayer>> s_mediaPlayers = [];

	public static readonly AudioVolume AudioVolume = new();

	public static void Setup()
	{
        K8055.OnConnectionChanged += K8055_OnConnectionChanged;
        K8055.OnAnalogChannelsChange += K8055_OnAnalogChannelsChange;
    }

    private static void K8055_OnAnalogChannelsChange(K8055.AnalogChannel analogChannel, int value)
    {
		if (analogChannel == K8055.AnalogChannel.I1)
		{
			float f = (float)Math.Round(value / 255f, 1);
			if (f != AudioVolume.GameVolume)
			{
				Console.WriteLine(f);
				AudioVolume.GameVolume = f;
			}
        }
    }

    private static void K8055_OnConnectionChanged()
    {
		K8055.OutputAnalogChannel(K8055.AnalogChannel.O2, (int)(AudioVolume.GameVolume * 255));
    }

	/// <summary>
	/// Play the audio file type.
	/// </summary>
	/// <param name="audioFile">The audio file to play.</param>
	/// <param name="loop">If the audio file should loop.</param>
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

	/// <summary>
	/// Stop all media player of this audio file.
	/// </summary>
	/// <param name="audioFile">The Audio file to stop.</param>
	public static void StopSound(AudioFile audioFile)
	{
		foreach(MediaPlayer mediaPlayer in s_mediaPlayers[audioFile])
		{
			mediaPlayer.Stop();
			mediaPlayer.Close();
		}
	}

	internal static void UpdateAudioVolume()
	{
		K8055.OutputAnalogChannel(K8055.AnalogChannel.O2, (int)(AudioVolume.GameVolume * 255));
		foreach(AudioFile audioFile in s_mediaPlayers.Keys)
		{
			foreach(MediaPlayer mediaPlayer in s_mediaPlayers[audioFile]) 
			{
				mediaPlayer.Volume = AudioVolume.GetVolumeByAudioFile(audioFile);
            }
		}
	}

	private static string AudioTypeToString(AudioFile audioType)
	{
		return audioType switch
		{
            AudioFile.MouseOver => "MouseOver.wav",
            AudioFile.BackGroundMusic => s_musicFiles[GameManager.Random.Next(0, s_musicFiles.Count)],
			AudioFile.EnemyDeath => s_enemyDeath[GameManager.Random.Next(0, s_enemyDeath.Count)],
            _ => null,
		};
	}
}

public enum AudioFile
{
    MouseOver,
    BackGroundMusic,
    EnemyDeath,
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
            AudioFile.EnemyDeath => EffectVolume * GameVolume,
            _ => GameVolume,
        };
    }

}
