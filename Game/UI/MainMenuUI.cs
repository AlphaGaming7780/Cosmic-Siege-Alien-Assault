using System;
using System.Drawing;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI
{
	internal class MainMenuUI : UIBase
	{

		internal Panel mainMenu;
		internal Panel VellmanBoardStatus;
		internal Label VellmanBoardStatusLabel;
		internal Label GameName;
		internal Button PlayButton;
		internal Button SettingsButton;
		internal Button QuitButton;

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
				Text = GameWindow.Text,
				Width = (int)(500 * UIRatio.x),
				Height = (int)(100 * UIRatio.y),
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new(FontFamily.GenericSerif, 25f, FontStyle.Bold),
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
				BackColor = Color.Gray,
				Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular)
			};
			PlayButton.Location = new(mainMenu.Left + mainMenu.Width / 2 - PlayButton.Width/2, (int)(275 * UIRatio.y));
			PlayButton.GotFocus += (object sender, EventArgs e) => AudioManager.PlaySound(AudioFile.MouseOver);
			PlayButton.MouseEnter += (object sender, EventArgs e) => AudioManager.PlaySound(AudioFile.MouseOver);

			SettingsButton = new()
			{
				Name = "MainMenuButtonSettings",
				Height = (int)(50 * UIRatio.y),
				Width = (int)(200 * UIRatio.x),
				Text = K8055.IsConnected ? "Settings (INP2)" : "Settings",
				BackColor = Color.Gray,
				Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular)
			};
			SettingsButton.Location = new(mainMenu.Left + mainMenu.Width / 2 - SettingsButton.Width / 2, (int)(375 * UIRatio.y));
			SettingsButton.GotFocus += (object sender, EventArgs e) => AudioManager.PlaySound(AudioFile.MouseOver);
			SettingsButton.MouseEnter += (object sender, EventArgs e) => AudioManager.PlaySound(AudioFile.MouseOver);

			QuitButton = new()
			{
				Name = "MainMenuButtonQuit",
				Height = (int)(50 * UIRatio.y),
				Width = (int)(200 * UIRatio.x),
				Text = K8055.IsConnected ? "Quit (INP5)" : "Quit",
				BackColor = Color.Gray,
				Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular)
			};
			QuitButton.Location = new(mainMenu.Left + mainMenu.Width / 2 - QuitButton.Width / 2, (int)(475 * UIRatio.y));
			QuitButton.GotFocus += (object sender, EventArgs e) => AudioManager.PlaySound(AudioFile.MouseOver);
			QuitButton.MouseEnter += (object sender, EventArgs e) => AudioManager.PlaySound(AudioFile.MouseOver);


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
	}
}
