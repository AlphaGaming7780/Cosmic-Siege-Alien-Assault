using K8055Velleman.Game.Entities;
using K8055Velleman.Game.Systems;
using K8055Velleman.Lib.ClassExtension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI
{
	internal class GameUI : UIBase
	{
		internal Panel GamePanel { get; private set; }
		Panel selectedStratPanel;
		Panel PauseMenu;
		Label waveNumberLabel;
		GameSystem gameSystem;
		internal override void OnCreate()
		{
			gameSystem = GameManager.GetOrCreateSystem<GameSystem>();
			GamePanel = new()
			{
				Size = GameWindow.Size,
				Location = new(0, 0),
				BackColor = Color.Black,
			};

			selectedStratPanel = new()
			{
				Width = 522,
				Height = 132,
				Location = new Point(GameWindow.Width / 2 - 522 / 2, 900),
				BorderStyle = BorderStyle.FixedSingle,
				ForeColor = Color.White,
			};
			GamePanel.Controls.Add(selectedStratPanel);
			GameWindow.Controls.Add(GamePanel);
		}

		internal override void OnDestroy()
		{
			GameWindow.Controls.Remove(GamePanel);
			GamePanel.Dispose();
		}

		//internal override void OnResize()
		//{
		//	GamePanel.Size = GameWindow.Size;
		//}

		internal void HidePauseMenu()
		{
			GamePanel.Enabled = true;
			GameWindow.Controls.Remove(PauseMenu);
			PauseMenu.Dispose();
		}

		internal void ShowPauseMenu()
		{
			GamePanel.Enabled = false;
			PauseMenu = new()
			{
				Width = 720,
				Height = 325,
				ForeColor= Color.White,
				BorderStyle = BorderStyle.FixedSingle,
			};
			PauseMenu.Location = new(GameWindow.Width / 2 - PauseMenu.Width / 2, GameWindow.Height / 2 - PauseMenu.Height / 2);

			Label gamePausedText = new()
			{
                Text = "Game Paused",
                Font = new Font(UIManager.CustomFonts.Families[0], 30f, FontStyle.Bold),
                ForeColor = Color.White,
                //AutoSize = true,
				Width = 500,
				Height = 50,
				TextAlign = ContentAlignment.MiddleCenter,
				BorderStyle= BorderStyle.FixedSingle,
            };
			gamePausedText.Location = new(PauseMenu.Width / 2 - gamePausedText.Width / 2, 25);

			BButton resumeButton = new()
			{
				Text = "Resume",
				Width = 300,
				Height = 50,
				ForeColor= Color.White,
                Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
            };
			resumeButton.Click += (s, e) => { gameSystem.UnPauseGame(); };
            resumeButton.Location = new(PauseMenu.Width / 2 - resumeButton.Width / 2, 100);

            BButton settingsButton = new()
            {
                Text = "Settings",
                Width = 300,
                Height = 50,
                ForeColor = Color.White,
                Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
            };
            settingsButton.Click += (s, e) => { gameSystem.UnPauseGame(); GameManager.instance.Load(GameStatus.MainMenu); };
            settingsButton.Location = new(PauseMenu.Width / 2 - settingsButton.Width / 2, 175);

            BButton mainMenuButton = new()
            {
                Text = "Main Menu",
                Width = 300,
                Height = 50,
                ForeColor = Color.White,
                Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
            };
            mainMenuButton.Click += (s, e) => { gameSystem.UnPauseGame(); GameManager.instance.Load(GameStatus.MainMenu); };
			mainMenuButton.Location = new(PauseMenu.Width / 2 - mainMenuButton.Width / 2, 250);

            PauseMenu.Controls.Add(gamePausedText);
			PauseMenu.Controls.Add(resumeButton);
			PauseMenu.Controls.Add(settingsButton);
			PauseMenu.Controls.Add(mainMenuButton);
			GameWindow.Controls.Add(PauseMenu);
			GameWindow.Controls.SetChildIndex(PauseMenu, 0);
		}


		internal void UpdateStratagemList(List<StratagemEntityBase> stratagemEntityBases)
		{
			int i = 0;
			foreach(StratagemEntityBase stratagemEntityBase in stratagemEntityBases) 
			{
				if (stratagemEntityBase == null) continue;
                stratagemEntityBase.mainPanel.Location = new Point(2 * (i + 1) + 130 * i, 1);
                selectedStratPanel.Controls.Add(stratagemEntityBase.mainPanel);
                selectedStratPanel.Controls.SetChildIndex(stratagemEntityBase.mainPanel, 0);
				stratagemEntityBase.mainPanel.Enabled = true;
				i++;
            }
        }
	}
}
