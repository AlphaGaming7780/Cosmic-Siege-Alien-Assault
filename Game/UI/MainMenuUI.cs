using K8055Velleman.Game.Saves;
using K8055Velleman.Lib.ClassExtension;
using System;
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
		internal BButton PlayButton;
		internal BButton SettingsButton;
		internal BButton QuitButton;
		private Panel PlayerSelector;

		private PlayerData selectedPlayer;

		internal override void OnCreate()
		{
			//AudioManager.LoadAudioFile(AudioType.MouseOver);
			AudioManager.PlaySound(AudioFile.LoadingMusic);
			K8055.OnDigitalChannelsChange += OnDigitalChannelsChange;
			K8055.OnConnectionChanged += OnConnectionChange;
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
			//PlayButton.GotFocus += (object sender, EventArgs e) => AudioManager.PlaySound(AudioFile.MouseOver);
			//PlayButton.MouseEnter += (object sender, EventArgs e) => AudioManager.PlaySound(AudioFile.MouseOver);

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
			//SettingsButton.GotFocus += (object sender, EventArgs e) => AudioManager.PlaySound(AudioFile.MouseOver);
			//SettingsButton.MouseEnter += (object sender, EventArgs e) => AudioManager.PlaySound(AudioFile.MouseOver);

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
			//QuitButton.GotFocus += (object sender, EventArgs e) => AudioManager.PlaySound(AudioFile.MouseOver);
			//QuitButton.MouseEnter += (object sender, EventArgs e) => AudioManager.PlaySound(AudioFile.MouseOver);

			VellmanBoardStatus.Controls.Add(VellmanBoardStatusLabel);
			mainMenu.Controls.Add(VellmanBoardStatus);
			mainMenu.Controls.Add(GameName);
			mainMenu.Controls.Add(PlayButton);
			mainMenu.Controls.Add(SettingsButton);
			mainMenu.Controls.Add(QuitButton);
			GameWindow.Controls.Add(mainMenu);
		}

		internal override void OnDestroy()
		{
			Console.WriteLine("Main menu destroyed");
			K8055.OnConnectionChanged -= OnConnectionChange;
			K8055.OnDigitalChannelsChange -= OnDigitalChannelsChange;
			AudioManager.StopSound(AudioFile.MouseOver);
			AudioManager.StopSound(AudioFile.LoadingMusic);
			GameWindow.Controls.Remove(mainMenu);
			mainMenu.Dispose();
		}

		internal override void OnResize()
		{
			mainMenu.Width = GameWindow.Width;
			mainMenu.Height = GameWindow.Height;
			VellmanBoardStatus.Location = new(mainMenu.Right - 10 - VellmanBoardStatus.Width - RightOffeset, mainMenu.Location.Y + 10);
			PlayButton.Height = (int)(50 * UIRatio.y);
			PlayButton.Width = (int)(200 * UIRatio.x);
			PlayButton.Location = new(mainMenu.Left + mainMenu.Width / 2 - PlayButton.Width / 2, (int)(275 * UIRatio.y));
			SettingsButton.Height = (int)(50 * UIRatio.y);
			SettingsButton.Width = (int)(200 * UIRatio.x);
			SettingsButton.Location = new(mainMenu.Left + mainMenu.Width / 2 - SettingsButton.Width / 2, (int)(375 * UIRatio.y));
			QuitButton.Height = (int)(50 * UIRatio.y);
			QuitButton.Width = (int)(200 * UIRatio.x);
			QuitButton.Location = new(mainMenu.Left + mainMenu.Width / 2 - QuitButton.Width / 2, (int)(475 * UIRatio.y));
			GameName.Height = (int)(100 * UIRatio.y);
			GameName.Width = (int)(500 * UIRatio.x);
			GameName.Font = new(GameName.Font.FontFamily, 20 * UIRatio.moyenne, FontStyle.Bold);
			GameName.Location = new(mainMenu.Width / 2 - GameName.Width / 2, (int)(100 * UIRatio.y));
		}

		private void OnDigitalChannelsChange(K8055.DigitalChannel digitalChannel)
		{
			if(digitalChannel == K8055.DigitalChannel.B1) PlayButton.PerformClick();
			else if(digitalChannel == K8055.DigitalChannel.B2) SettingsButton.PerformClick();
			else if(digitalChannel == K8055.DigitalChannel.B5) QuitButton.PerformClick();
		}

		private void OnConnectionChange()
		{
			VellmanBoardStatus.BackColor = K8055.IsConnected ? Color.Green : Color.Red;
			VellmanBoardStatusLabel.Text = K8055.IsConnected ? "Connected" : "Disconnected";
			PlayButton.Text = K8055.IsConnected ? "Play (INP1)" : "Play";
			SettingsButton.Text = K8055.IsConnected ? "Settings (INP2)" : "Settings";
			QuitButton.Text = K8055.IsConnected ? "Quit (INP5)" : "Quit";
		}

		internal void ShowPlayerSelector()
		{
			mainMenu.Visible = false;
			mainMenu.Enabled = false;

			Control oldSelected = null;

			PlayerSelector = new()
			{
				Width = 800,
				Height = 800,
				BorderStyle = BorderStyle.FixedSingle,
				ForeColor = Color.White,
			};
			PlayerSelector.Location = new(GameWindow.Width / 2 - PlayerSelector.Width / 2, GameWindow.Height / 2 - PlayerSelector.Height / 2);

			BButton NewPlayer = new()
			{
				Width = 200,
				Height = 75,
				Location = new(550, 675),
				Text = "New Player",
				Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular)
			};
			NewPlayer.Click += NewPlayer_Click;


			BButton DeletePlayer = new()
			{
				Width = 200,
				Height = 75,
				Location = new(50, 675),
				Text = "Delete Player",
				Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular),
				Visible = selectedPlayer != null,
				Enabled = selectedPlayer != null
			};
			DeletePlayer.Click += (s, e) => { SaveManager.DeletePlayerData(selectedPlayer); selectedPlayer = null; PlayerSelector.Dispose(); ShowPlayerSelector(); };

			BButton SelectePlayer = new()
			{
				Width = 200,
				Height = 75,
				Location = new(300, 675),
				Text = "Select Player",
				Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular),
				Visible = selectedPlayer != null,
				Enabled = selectedPlayer != null
			};
			SelectePlayer.Click += (s, e) => { SaveManager.CurrentPlayerData = selectedPlayer; mainMenu.Visible = true; mainMenu.Enabled = true; PlayerSelector.Dispose(); };

			Panel PlayerSelectorScroll = new()
			{
				Width = 750,
				Height = 600,
				BorderStyle = BorderStyle.FixedSingle,
				ForeColor = Color.White,
				AutoScroll = true,
				Location = new(25, 25),
			};

			int x = 0;
			foreach (PlayerData playerData in SaveManager.PlayersData)
			{
				Panel Player = new()
				{
					Name = "Player",
					Width = 700,
					Height = 100,
					BorderStyle = BorderStyle.FixedSingle,
					ForeColor = Color.White,
					Location = new(25, 100 * x + 25 * (x+1) )
				};
				Player.MouseEnter += OnMouseEnter;
				Player.MouseLeave += OnMouseLeave;
				Player.Click += (s, e) =>
				{
					Control c = s as Control;
					if (oldSelected != null)
					{
						oldSelected.MouseEnter += OnMouseEnter;
						oldSelected.MouseLeave += OnMouseLeave;
						oldSelected.BackColor = Color.Transparent;
						AddEventToPlayerChild(c);
					}
					oldSelected = c;
					onMouseClickColorChange(c);
					selectedPlayer = playerData;
					DeletePlayer.Visible = true;
					DeletePlayer.Enabled = true;
					SelectePlayer.Enabled = true;
					SelectePlayer.Visible = true;
				};

				Label name = new()
				{
					Width = 400,
					Height = 75,
					Text = playerData.Name,
					Font = new Font(UIManager.CustomFonts.Families[0], 30f, FontStyle.Bold),
					ForeColor = Color.White,
				};

				Player.Controls.Add(name);

				foreach(Control child in Player.Controls)
				{
					child.MouseEnter += OnMouseEnter;
					child.MouseLeave += OnMouseLeave;
					child.Click += (s, e) =>
					{
						Control c = s as Control;
						if (oldSelected != null)
						{
							oldSelected.MouseEnter += OnMouseEnter;
							oldSelected.MouseLeave += OnMouseLeave;
							oldSelected.BackColor = Color.Transparent;
							AddEventToPlayerChild(c.Parent);
						}
						oldSelected = c.Parent;
						onMouseClickColorChange(c.Parent);
						selectedPlayer = playerData;
						DeletePlayer.Visible = true;
						DeletePlayer.Enabled = true;
						SelectePlayer.Enabled = true;
						SelectePlayer.Visible = true;
					};
				}

				PlayerSelectorScroll.Controls.Add(Player);
				x++;
			}

			PlayerSelector.Controls.Add(NewPlayer);
			PlayerSelector.Controls.Add(DeletePlayer);
			PlayerSelector.Controls.Add(SelectePlayer);
			PlayerSelector.Controls.Add(PlayerSelectorScroll);
			GameWindow.Controls.Add(PlayerSelector);
			GameWindow.Controls.SetChildIndex(PlayerSelector, 0);
		}

		private void RemoveEventToPlayerChild(Control player)
		{
			foreach (Control child in player.Controls)
			{
				child.MouseEnter -= OnMouseEnter;
				child.MouseLeave -= OnMouseLeave;
			}
		}

		private void AddEventToPlayerChild(Control player)
		{
			foreach(Control child in player.Controls)
			{
				child.MouseEnter += OnMouseEnter;
				child.MouseLeave += OnMouseLeave;
			}
		}

		private void OnMouseEnter(object sender, EventArgs e)
		{
			if (sender is not Control c) return;
			if(c.Name == "Player") onMouseEnterColorChange(c);
			else onMouseEnterColorChange(c.Parent);
		}

		private void OnMouseLeave(object sender, EventArgs e)
		{
			if (sender is not Control c) return;
			if (c.Name == "Player") onMouseLeaveColorChange(c);
			else onMouseLeaveColorChange(c.Parent);
		}

		private void NewPlayer_Click(object sender, EventArgs e)
		{
			PlayerSelector.Enabled = false;
			PlayerSelector.Visible = false;

			Panel panel = new()
			{
				Width = 500,
				Height = 200,
				BorderStyle = BorderStyle.FixedSingle,
				ForeColor = Color.White,
			};
			panel.Location = new(GameWindow.Width / 2 - panel.Width / 2, GameWindow.Height / 2 - panel.Height / 2);

			Label label = new()
			{
				Width = 400,
				Height = 50,
				Text = "Enter the name of your player.",
				Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular),
				Location = new(50,0),
				TextAlign = ContentAlignment.MiddleCenter,
			};

			TextBox textBox = new() 
			{
				MinimumSize = new Size(400,50),
				Location = new(50, 50),
				Font = new Font(UIManager.CustomFonts.Families[0], 30f, FontStyle.Bold),
			};

			BButton NewPlayer = new()
			{
				Width = 200,
				Height = 50,
				Text = "New Player",
				Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular),
				Location= new(275, 125),
			};
			NewPlayer.Click += (s, e) => 
			{
				bool already = false;
				foreach (PlayerData playerData in SaveManager.PlayersData)
				{
					if(playerData.Name == textBox.Text)
					{
						already = true;
						break;
					}
				}
				if( !already )
				{
					selectedPlayer = new()
					{
						Name = textBox.Text,
					};
					SaveManager.PlayersData.Add(selectedPlayer);
					SaveManager.CurrentPlayerData = selectedPlayer;
					SaveManager.SaveCurrentPlayerData();
					mainMenu.Visible = true;
					mainMenu.Enabled = true;
					PlayerSelector.Dispose();
					panel.Dispose();
				}
			};

			BButton CancleButton = new() 
			{
				Width = 200,
				Height = 50,
				Text = "Back",
				Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular),
				Location = new(25, 125),
			};
			CancleButton.Click += (s, e) => { panel.Dispose(); PlayerSelector.Visible = true; PlayerSelector.Enabled = true; };

			panel.Controls.Add(NewPlayer);
			panel.Controls.Add(CancleButton);
			panel.Controls.Add(label);
			panel.Controls.Add(textBox);
			
			GameWindow.Controls.Add(panel);
			GameWindow.Controls.SetChildIndex(panel, 0);


		}

		private void onMouseEnterColorChange(Control player)
		{ 
			player.BackColor = Color.DarkGray;
		}

		private void onMouseLeaveColorChange(Control player)
		{
			player.BackColor = Color.Transparent;
		}

		private void onMouseClickColorChange(Control player)
		{
			player.BackColor = Color.LightBlue;
			player.MouseEnter -= OnMouseEnter;
			player.MouseLeave -= OnMouseLeave;
			RemoveEventToPlayerChild(player);
		}

		internal void HidePlayerSelector()
		{
			mainMenu.Visible = true;
			mainMenu.Enabled = true;
			GameWindow.Controls.Remove(PlayerSelector);
			PlayerSelector.Dispose();
			PlayerSelector = null;
		}
	}
}
