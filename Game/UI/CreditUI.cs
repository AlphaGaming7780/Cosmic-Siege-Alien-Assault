using K8055Velleman.Lib.ClassExtension;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI;

internal class CreditUI : UIBase
{
	Control mainControl;
	Panel buttonsBox;
	BButton backButton;
	BButton gameCredit;
	BButton musicCredit;

	Panel creditPanelOpned;

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

		buttonsBox = new()
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
		gameCredit.Click += (s, e) => ShowGameCreditPanel();
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
		musicCredit.Click += (s, e) => ShowMuiscCreditPanel();
		buttonsBox.Controls.Add(musicCredit);

		ShowGameCreditPanel();
		gameCredit.Focus();

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
		if (digitalChannel == K8055.DigitalChannel.I1) gameCredit.PerformClick();
		else if (digitalChannel == K8055.DigitalChannel.I2) musicCredit.PerformClick();
		else if (digitalChannel == K8055.DigitalChannel.I5) backButton.PerformClick();
	}

	private void ShowCreditPanel()
	{
		creditPanelOpned?.Dispose();
		creditPanelOpned = new()
		{
			Width = 1080,
			Height = buttonsBox.Height,
			BorderStyle = BorderStyle.FixedSingle,
			ForeColor = Color.White,
			AutoScroll = true,
		};
		creditPanelOpned.Location = new Point(buttonsBox.Left + buttonsBox.Width + 25, buttonsBox.Top);
		mainControl.Controls.Add(creditPanelOpned);


	}


	private void ShowGameCreditPanel()
	{

		ShowCreditPanel();

	}

	private void ShowMuiscCreditPanel()
	{

		ShowCreditPanel();

		AddMusicPanel("NOmki - Netrunner", "https://www.newgrounds.com/audio/listen/1093684", 0);
		AddMusicPanel("NOmki - Time", "https://www.newgrounds.com/audio/listen/1116845", 1);
		AddMusicPanel("punkerrr - Virtual Cataclysm ", "https://www.newgrounds.com/audio/listen/1241343", 2);
		AddMusicPanel("Mr-Blackhole - Category", "https://www.newgrounds.com/audio/listen/1090910", 3);
		AddMusicPanel("RyuuAkito & SquashHead - Damaged Artificial Nervous System", "https://www.newgrounds.com/audio/listen/1197066", 4);
    }

	private void AddMusicPanel(string name, string link, int pos)
	{
        Panel panel = new()
        {
            Width = creditPanelOpned.Width - 50,
            Height = 100,
            BorderStyle = BorderStyle.FixedSingle,
            ForeColor = Color.White,
        };
        panel.Location = new(creditPanelOpned.Width / 2 - panel.Width / 2, 25 + panel.Height * pos + 10 * pos);
        creditPanelOpned.Controls.Add(panel);

        Label label = new()
        {
            Width = panel.Width - 50,
            Height = 50,
            Text = name,
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.White,
            Font = new Font(UIManager.CustomFonts.Families[0], 25f, FontStyle.Bold)
        };
        panel.Controls.Add(label);

        LinkLabel linkLabel = new()
        {
            Width = panel.Width - 50,
            Height = 50,
            Text = link,
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.White,
            Font = new Font(UIManager.CustomFonts.Families[0], 17f, FontStyle.Italic),
            Location = new Point(25, 50),
        };
        linkLabel.Click += (s, e) => { linkLabel.LinkVisited = true; System.Diagnostics.Process.Start(link); };
        panel.Controls.Add(linkLabel);
    }

}
