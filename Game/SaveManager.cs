using K8055Velleman.Game.Saves;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K8055Velleman.Game;

internal static class SaveManager
{
	internal static PlayerData CurrentPlayerData;
	internal static List<PlayerData> PlayersData = [];

	private static string SavePath;
	private static string PlayersPath;

	//private static List<string> OpenedFiles = [];

	internal static void LoadData()
	{
		SavePath = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Saves");
		PlayersPath = Path.Combine(SavePath, "Players");
		if (Directory.Exists(PlayersPath))
        {
            foreach (FileInfo playerSave in new DirectoryInfo(PlayersPath).GetFiles())
            {
                try
                {
                    PlayerData playerData = JsonSerializer.Deserialize<PlayerData>(File.ReadAllText(playerSave.FullName));
                    _ = playerData ?? new PlayerData();
                    PlayersData.Add(playerData);
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

	internal static void SaveCurrentPlayerData()
	{
        //if (OpenedFiles.Contains(savePath)) return;
        Save(PlayersPath, $"{CurrentPlayerData.Name}.json", CurrentPlayerData);
	}

    internal static void DeletePlayerData(PlayerData playerData)
    {
        string path = Path.Combine(PlayersPath, $"{playerData.Name}.json");
        if (File.Exists(path)) File.Delete(path);
        PlayersData.Remove(playerData);
    }

    private static async Task Save(string path, string fileName, object objectToSave)
	{
        //OpenedFiles.Add(path);
        Directory.CreateDirectory(path);
        using FileStream fileStream = File.Create(Path.Combine(path, fileName));
        await JsonSerializer.SerializeAsync(fileStream, objectToSave, new JsonSerializerOptions { WriteIndented = true });
        //OpenedFiles.Remove(path);
    }

}
