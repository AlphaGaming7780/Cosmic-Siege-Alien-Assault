using K8055Velleman.Game.Saves;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;

namespace K8055Velleman.Game;

internal static class SaveManager
{
    const string kExtension = ".txt";

    internal static Settings Settings;
    internal static PlayerData CurrentPlayerData;
    internal static List<PlayerData> s_playersData = [];

    private static string _savePath;
    private static string _settingsPath;
    private static string _playersPath;

    private static readonly List<string> s_openedFiles = [];
    private static readonly Dictionary<string, object> s_objectsToSave = [];

    /// <summary>
    /// Load the game data (settings & players data) from files.
    /// </summary>
    internal static void LoadData()
    {
        _savePath = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Saves");
        _settingsPath = Path.Combine(_savePath, $"Settings{kExtension}");
        _playersPath = Path.Combine(_savePath, "Players");

        try
        {
            if (File.Exists(_settingsPath)) Settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(_settingsPath));
            else Settings = new ();
        } catch
        {
            MessageBox.Show($"Failed to load the settings.\n\nThe Settings have been reseted?", "Error when loading the settings file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            File.Delete(_settingsPath);
            Settings = new ();
        }

        if (Directory.Exists(_playersPath))
        {
            foreach (FileInfo playerSave in new DirectoryInfo(_playersPath).GetFiles())
            {
                try
                {
                    PlayerData playerData = JsonSerializer.Deserialize<PlayerData>(File.ReadAllText(playerSave.FullName));
                    _ = playerData ?? new PlayerData();
                    s_playersData.Add(playerData);
                } catch { 
                    DialogResult dialogResult = MessageBox.Show($"Failed to load the save of the {playerSave.Name} player.\n\nDo you want to delete this save ?", "Error when loading the saves.", MessageBoxButtons.YesNo, MessageBoxIcon.Error); 
                    if(dialogResult == DialogResult.OK)
                    {
                        File.Delete(playerSave.FullName);
                    }
                }
            }
        }
    }
    /// <summary>
    /// Save the settings in the file.
    /// </summary>
    internal static void SaveSettings()
    {
        Save(_settingsPath, Settings);
    }

    /// <summary>
    /// Save current selected player data in is file.
    /// </summary>
    internal static void SaveCurrentPlayerData()
    {
        Save(_playersPath, $"{CurrentPlayerData.Name}{kExtension}", CurrentPlayerData);
    }

    /// <summary>
    /// Delete player data from files.
    /// </summary>
    /// <param name="playerData">The player data to delete</param>
    internal static void DeletePlayerData(PlayerData playerData)
    {
        string path = Path.Combine(_playersPath, $"{playerData.Name}{kExtension}");
        if (File.Exists(path)) File.Delete(path);
        s_playersData.Remove(playerData);
    }

    private static async void Save(string fullPath, object objectToSave)
    {
        if (s_openedFiles.Contains(fullPath))
        {
            if (s_objectsToSave.ContainsKey(fullPath)) s_objectsToSave[fullPath] = objectToSave;
            else s_objectsToSave.Add(fullPath, objectToSave);
            return;
        }
        s_openedFiles.Add(fullPath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        using FileStream fileStream = File.Create(fullPath);
        await JsonSerializer.SerializeAsync(fileStream, objectToSave, new JsonSerializerOptions { WriteIndented = true });
        fileStream.Close();
        s_openedFiles.Remove(fullPath);
        if(s_objectsToSave.ContainsKey(fullPath))
        {
            Save(fullPath, s_objectsToSave[fullPath]);
            s_objectsToSave.Remove(fullPath);
        }
    }

    private static void Save(string path, string fileName, object objectToSave)
    {
        Save(Path.Combine(path, fileName), objectToSave);
    }

}
