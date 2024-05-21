using K8055Velleman.Lib.ClassExtension;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI
{
	internal class CreditUI : UIBase
	{
		Control mainControl;
		BButton backButton;
		BButton gameCredit;
		BButton musicCredit;

        internal override void OnCreate()
		{
			base.OnCreate();
			mainControl = new()
			{
				Size = GameWindow.Size,
				BackColor = Color.Black,
			};
			GameWindow.Controls.Add(mainControl);
			GameWindow.Controls.SetChildIndex(mainControl, 0);

			backButton = new()
			{
				Width = 256,
				Height = 94,
                Text = K8055.IsConnected ? "Back (INP5)" : "Back",
                ForeColor = Color.White,
                Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular),
            };
			backButton.Location = new(50, mainControl.Height - backButton.Height - 50);
			backButton.Click += (s, e) => BackToMainMenu();
			mainControl.Controls.Add(backButton);

			Panel buttonsBox = new()
			{
				Height = 720,
				Width = 256,
				BorderStyle = BorderStyle.FixedSingle,
				ForeColor = Color.White,
			};
			buttonsBox.Location = new(256, mainControl.Height / 2 - buttonsBox.Height / 2);
			mainControl.Controls.Add(buttonsBox);

			gameCredit = new()
			{
				Width = buttonsBox.Width - 50,
				Height = 128,
                Text = K8055.IsConnected ? "Game (INP1)" : "Game",
                ForeColor = Color.White,
                Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular),
            };
			gameCredit.Location = new(buttonsBox.Width / 2 - gameCredit.Width / 2, 25 );
			buttonsBox.Controls.Add(gameCredit);

			musicCredit = new()
			{
				Width = buttonsBox.Width - 50,
				Height = 128,
                Text = K8055.IsConnected ? "Music (INP2)" : "Music",
                ForeColor = Color.White,
                Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular)
            };
			musicCredit.Location = new(buttonsBox.Width / 2 - musicCredit.Width / 2, 50 + 128);
			buttonsBox.Controls.Add(musicCredit);

			Console.WriteLine("CreditUI created");

		}

		private void BackToMainMenu()
		{
			UIManager.GetOrCreateUI<MainMenuUI>();
			UIManager.DestroyUI<CreditUI>();
		}

		internal override void OnDestroy()
		{
			base.OnDestroy();
			GameWindow.Controls.Remove(mainControl);
			mainControl.Dispose();
			mainControl = null;
            Console.WriteLine("CreditUI Destroyed");
        }

        internal override void OnConnectionChange()
        {
            gameCredit.Text = K8055.IsConnected ? "Game (INP1)" : "Game";
            musicCredit.Text = K8055.IsConnected ? "Music (INP2)" : "Music";
            backButton.Text = K8055.IsConnected ? "Back (INP5)" : "Back";
        }

        internal override void OnDigitalChannelsChange(K8055.DigitalChannel digitalChannel)
        {
            if (digitalChannel == K8055.DigitalChannel.B1) gameCredit.PerformClick();
            else if (digitalChannel == K8055.DigitalChannel.B2) musicCredit.PerformClick();
            else if (digitalChannel == K8055.DigitalChannel.B5) backButton.PerformClick();
        }
    }
}
