using K8055Velleman.Game.Entities;
using K8055Velleman.Game.Saves;
using K8055Velleman.Lib.ClassExtension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace K8055Velleman.Game.UI
{
	internal class MainMenuUI : UIBase
	{

		internal Panel mainMenu;
		internal Panel VellmanBoardStatus;
		internal Label VellmanBoardStatusLabel;
        internal Label GameName;
        internal Label PlayerName;
		internal BButton PlayButton;
		internal BButton SettingsButton;
		internal BButton QuitButton;
		private BButton changePlayer;
        private BButton creditMenuButton;
        private Panel scoreboard;

		internal override void OnCreate()
		{
			base.OnCreate();
            mainMenu = new()
			{
				Location = new Point(0, 0),
				Width = GameWindow.Width,
				Height = GameWindow.Height,
				BackColor = Color.Black,
				Enabled = true,
				Visible = true,
				Name = "MainMenuPanel"
			};

			GameName = new()
			{
				Name = "MainMenuGameName",
				Text = "Cosmic Siege: Alien Assault",
				Width = (int)(500 * UIRatio.x),
				Height = (int)(100 * UIRatio.y),
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new(FontFamily.GenericSerif, 45f, FontStyle.Bold),
				ForeColor = Color.WhiteSmoke,
				BorderStyle = BorderStyle.FixedSingle,
			};
			GameName.Location = new(mainMenu.Width/2 - GameName.Width/2, (int)(100 * UIRatio.y));

			PlayerName = new()
			{
				Text = SaveManager.CurrentPlayerData?.Name,
				Location = new Point(25,25),
				AutoSize = true,
                Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
				ForeColor= Color.WhiteSmoke,
            };

			VellmanBoardStatus = new()
			{
				Width = 150,//(int)(150 * UIRatio.x),
				Height = 30,//(int)(30 * UIRatio.y),
				Name = "MainMenuStatusPanel",
				BackColor = K8055.IsConnected ? Color.Green : Color.Red
			};
			VellmanBoardStatus.Location = new(mainMenu.Right - 10 - VellmanBoardStatus.Width - RightOffeset, mainMenu.Location.Y + 10);

			VellmanBoardStatusLabel = new()
			{
				Name = "MainMenuStatusPanelLabel",
				Height = VellmanBoardStatus.Height,
				Width = VellmanBoardStatus.Width,
				Text = K8055.IsConnected ? "Connected" : "Disconnected",
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new(FontFamily.GenericSerif, 15f, FontStyle.Regular),
			};

			PlayButton = new()
			{
				Name = "MainMenuButtonPlay",
				Height = (int)(50 * UIRatio.y),
				Width = (int)(200 * UIRatio.x),
				Text = K8055.IsConnected ? "Play (INP1)" : "Play",
				ForeColor = Color.White,
				Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular)
			};
			PlayButton.Location = new(mainMenu.Left + mainMenu.Width / 2 - PlayButton.Width/2, (int)(275 * UIRatio.y));
			PlayButton.Click += (s, e) => { GameManager.instance.Load(GameStatus.PreGame); };

			SettingsButton = new()
			{
				Name = "MainMenuButtonSettings",
				Height = (int)(50 * UIRatio.y),
				Width = (int)(200 * UIRatio.x),
				Text = K8055.IsConnected ? "Settings (INP2)" : "Settings",
                ForeColor = Color.White,
                Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular)
			};
			SettingsButton.Location = new(mainMenu.Left + mainMenu.Width / 2 - SettingsButton.Width / 2, (int)(375 * UIRatio.y));

			QuitButton = new()
			{
				Name = "MainMenuButtonQuit",
				Height = (int)(50 * UIRatio.y),
				Width = (int)(200 * UIRatio.x),
				Text = K8055.IsConnected ? "Quit (INP5)" : "Quit",
                ForeColor = Color.White,
                Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular)
			};
			QuitButton.Location = new(mainMenu.Left + mainMenu.Width / 2 - QuitButton.Width / 2, (int)(475 * UIRatio.y));
			QuitButton.Click += (s, e) => { GameWindow.Close(); };

			UpdateScoreboard();

			creditMenuButton = new()
			{
				Text = K8055.IsConnected ? "Credit (INP4)" : "Credit",
                Width = 256,
				Height = 64,
                ForeColor = Color.White,
                Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular)
            };
			creditMenuButton.Location = new(mainMenu.Width - creditMenuButton.Width - 50, mainMenu.Height - creditMenuButton.Height - 50 );
			creditMenuButton.Click += (s, e) => { ShowCreditMenu(); };

			VellmanBoardStatus.Controls.Add(VellmanBoardStatusLabel);
			mainMenu.Controls.Add(VellmanBoardStatus);
			mainMenu.Controls.Add(GameName);
            mainMenu.Controls.Add(PlayerName);
            mainMenu.Controls.Add(PlayButton);
			mainMenu.Controls.Add(SettingsButton);
			mainMenu.Controls.Add(QuitButton);
			mainMenu.Controls.Add(creditMenuButton);
			GameWindow.Controls.Add(mainMenu);
            Console.WriteLine("Main menu destroyed");
        }

        private void ShowCreditMenu()
        {
			UIManager.GetOrCreateUI<CreditUI>();
			UIManager.DestroyUI<MainMenuUI>();
        }

        internal override void OnDestroy()
		{	
			base.OnDestroy();
			Console.WriteLine("Main menu destroyed");
			AudioManager.StopSound(AudioFile.MouseOver);
			GameWindow.Controls.Remove(mainMenu);
			mainMenu.Dispose();
		}

        internal override void OnDigitalChannelsChange(K8055.DigitalChannel digitalChannel)
		{
			if(digitalChannel == K8055.DigitalChannel.B1) PlayButton.PerformClick();
			else if(digitalChannel == K8055.DigitalChannel.B2) SettingsButton.PerformClick();
			else if(digitalChannel == K8055.DigitalChannel.B3) changePlayer.PerformClick();
			else if(digitalChannel == K8055.DigitalChannel.B4) creditMenuButton.PerformClick();
			else if(digitalChannel == K8055.DigitalChannel.B5) QuitButton.PerformClick();
		}

        internal override void OnConnectionChange()
		{
			Console.WriteLine(K8055.IsConnected);
			VellmanBoardStatus.BackColor = K8055.IsConnected ? Color.Green : Color.Red;
			VellmanBoardStatusLabel.Text = K8055.IsConnected ? "Connected" : "Disconnected";
			PlayButton.Text = K8055.IsConnected ? "Play (INP1)" : "Play";
			SettingsButton.Text = K8055.IsConnected ? "Settings (INP2)" : "Settings";
            changePlayer.Text = K8055.IsConnected ? "Change Player (INP3)" : "Change Player";
            creditMenuButton.Text = K8055.IsConnected ? "Credit (INP4)" : "Credit";
            QuitButton.Text = K8055.IsConnected ? "Quit (INP5)" : "Quit";
        }

		private void UpdateScoreboard()
		{
			if(scoreboard != null)
			{
				mainMenu.Controls.Remove(scoreboard);
				scoreboard.Dispose();
				scoreboard = null;
			}
			scoreboard = new()
            {
                Width = 300,
                Height = 500,
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = Color.White,
            };
            scoreboard.Location = new(mainMenu.Width / 10 * 9 - scoreboard.Width / 2, mainMenu.Height / 2 - scoreboard.Height / 2);

			Label label = new()
			{
				Width = scoreboard.Width,
				Height = 50,
				Text = "Scoreboard",
				Location = new(0,0),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
                ForeColor = Color.White,
            };

            Panel PlayerListScroll = new()
            {
                Width = scoreboard.Width - 50,
                Height = scoreboard.Height - 100 - label.Height,
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = Color.White,
                AutoScroll = true,
                Location = new(25, label.Height),
            };

			changePlayer = new()
			{
				Width = scoreboard.Width - 50,
                Height = 50,
                Location = new(300, 675),
                Text = K8055.IsConnected ? "Change Player (INP3)" : "Change Player",
				Font = new Font(FontFamily.GenericSansSerif, K8055.IsConnected ? 15f : 17f, FontStyle.Regular),
            };
			changePlayer.Location = new(25, scoreboard.Height - changePlayer.Height - 25);
            changePlayer.Click += (s, e) => { GameManager.instance.Load(GameStatus.PlayerSelector); };

            int x = 0;
			int offsetSpace = 20;
			List<PlayerData> playerDatas = new(SaveManager.PlayersData);
			playerDatas.Sort(delegate (PlayerData x, PlayerData y)
            {
                return y.HigestScore.CompareTo(x.HigestScore);
            });
            foreach (PlayerData playerData in playerDatas)
            {
                Panel Player = new()
                {
                    Width = PlayerListScroll.Width - offsetSpace * 2,
                    Height = 50,
                    BorderStyle = BorderStyle.FixedSingle,
                    ForeColor = Color.White,
					BackColor = (playerData == SaveManager.CurrentPlayerData) ? Color.LightBlue : Color.Transparent,
                };
				Player.Location = new(offsetSpace, Player.Height * x + offsetSpace * (x + 1));

                Label name = new()
                {
                    Width = Player.Width / 3 * 2,
                    Height = Player.Height,
                    Text = playerData.Name,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Regular),
                    ForeColor = Color.White,
                };

                Label Score = new()
                {
                    Width = Player.Width / 3,
                    Height = Player.Height,
                    Text = $"{playerData.HigestScore} 🌟",
                    TextAlign = ContentAlignment.MiddleRight,
                    Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Regular),
                    ForeColor = Color.White,
                    Location = new Point(name.Width, 0),
                };

                Player.Controls.Add(name);
                Player.Controls.Add(Score);
                PlayerListScroll.Controls.Add(Player);	
                x++;
            }
			scoreboard.Controls.Add(label);
			scoreboard.Controls.Add(PlayerListScroll);
			scoreboard.Controls.Add(changePlayer);

            mainMenu.Controls.Add(scoreboard);
        }
	}
}
