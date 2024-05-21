using K8055Velleman.Game.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K8055Velleman.Game.Systems
{
	internal class MainMenuSystem : SystemBase
	{
		MainMenuUI gameMainMenu;
        internal override void OnCreate()
		{
			base.OnCreate();
			Console.WriteLine("Main Menu is created");
            gameMainMenu = UIManager.GetOrCreateUI<MainMenuUI>();
			if (SaveManager.CurrentPlayerData == null) gameMainMenu.ShowPlayerSelector();
            AudioManager.PlaySound(AudioFile.LoadingMusic, true);
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();
			gameMainMenu = null;
            AudioManager.StopSound(AudioFile.LoadingMusic);
            UIManager.DestroyUI<MainMenuUI>();
        }

        internal override void OnUpdate()
		{
			base.OnUpdate();
		}

        internal override void OnGameStatusChange(GameStatus status)
        {
            base.OnGameStatusChange(status);
			switch (status)
			{
				case GameStatus.MainMenu:
					break;
				default:
					GameManager.DestroySystem<MainMenuSystem>();
					break;
			}
        }

		private void OnButtonPlayClick(object sender, EventArgs e)
		{
			GameManager.instance.Load(GameStatus.PreGame);
		}
		//private void OnQuitButtonClick(object sender, EventArgs e)
		//{
		//	GameWindow.Close();
		//}
	}
}
