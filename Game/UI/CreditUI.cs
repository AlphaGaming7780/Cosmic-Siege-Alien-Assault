using K8055Velleman.Lib.CustomControls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI;

internal class CreditUI : UIBase
{
	Control _mainControl;
	Panel _buttonsBox;
	BButton _backButton;
	BButton _gameCredit;
	BButton _musicCredit;
	BButton _miscCredit;

    Panel _creditPanelOpned;

	internal override void OnCreate()
	{
		base.OnCreate();
		_mainControl = new()
		{
			Size = GameWindow.Size,
			BackColor = Color.Black,
		};
		GameWindow.Controls.Add(_mainControl);
		GameWindow.Controls.SetChildIndex(_mainControl, 0);

		_backButton = new()
		{
			Width = 256,
			Height = 94,
			Text = K8055.IsConnected ? "Back (INP5)" : "Back",
			ForeColor = Color.White,
			Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular),
		};
		_backButton.Location = new(50, _mainControl.Height - _backButton.Height - 50);
		_backButton.Click += (s, e) => BackToMainMenu();
		_mainControl.Controls.Add(_backButton);

		_buttonsBox = new()
		{
			Height = 720,
			Width = 256,
			BorderStyle = BorderStyle.FixedSingle,
			ForeColor = Color.White,
		};
		_buttonsBox.Location = new(256, _mainControl.Height / 2 - _buttonsBox.Height / 2);
		_mainControl.Controls.Add(_buttonsBox);

		_gameCredit = new()
		{
			Width = _buttonsBox.Width - 50,
			Height = 128,
			Text = K8055.IsConnected ? "Game (INP1)" : "Game",
			ForeColor = Color.White,
			Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular),
		};
		_gameCredit.Location = new(_buttonsBox.Width / 2 - _gameCredit.Width / 2, 25 );
		_gameCredit.Click += (s, e) => ShowGameCreditPanel();
		_buttonsBox.Controls.Add(_gameCredit);

		_musicCredit = new()
		{
			Width = _buttonsBox.Width - 50,
			Height = 128,
			Text = K8055.IsConnected ? "Music (INP2)" : "Music",
			ForeColor = Color.White,
			Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular)
		};
		_musicCredit.Location = new(_buttonsBox.Width / 2 - _musicCredit.Width / 2, 50 + 128);
		_musicCredit.Click += (s, e) => ShowMuiscCreditPanel();
		_buttonsBox.Controls.Add(_musicCredit);

        _miscCredit = new()
        {
            Width = _buttonsBox.Width - 50,
            Height = 128,
            Text = K8055.IsConnected ? "Misc (INP3)" : "Misc",
            ForeColor = Color.White,
            Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular)
        };
        _miscCredit.Location = new(_buttonsBox.Width / 2 - _miscCredit.Width / 2, 75 + 256);
        _miscCredit.Click += (s, e) => ShowMiscCreditPanel();
        _buttonsBox.Controls.Add(_miscCredit);

        ShowGameCreditPanel();
		_gameCredit.Focus();

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
		GameWindow.Controls.Remove(_mainControl);
		_mainControl.Dispose();
		_mainControl = null;
		Console.WriteLine("CreditUI Destroyed");
	}

	internal override void OnConnectionChange()
	{
		_gameCredit.Text = K8055.IsConnected ? "Game (INP1)" : "Game";
		_musicCredit.Text = K8055.IsConnected ? "Music (INP2)" : "Music";
		_miscCredit.Text = K8055.IsConnected ? "Misc (INP3)" : "Misc";
		_backButton.Text = K8055.IsConnected ? "Back (INP5)" : "Back";
	}

	internal override void OnDigitalChannelsChange(K8055.DigitalChannel digitalChannel)
	{
		if (digitalChannel == K8055.DigitalChannel.I1) _gameCredit.PerformClick();
		else if (digitalChannel == K8055.DigitalChannel.I2) _musicCredit.PerformClick();
		else if (digitalChannel == K8055.DigitalChannel.I3) _miscCredit.PerformClick();
		else if (digitalChannel == K8055.DigitalChannel.I5) _backButton.PerformClick();
	}

	private void ShowCreditPanel()
	{
		_creditPanelOpned?.Dispose();
		_creditPanelOpned = new()
		{
			Width = 1080,
			Height = _buttonsBox.Height,
			BorderStyle = BorderStyle.FixedSingle,
			ForeColor = Color.White,
			AutoScroll = true,
		};
		_creditPanelOpned.Location = new Point(_buttonsBox.Left + _buttonsBox.Width + 25, _buttonsBox.Top);
		_mainControl.Controls.Add(_creditPanelOpned);


	}


	private void ShowGameCreditPanel()
	{

		ShowCreditPanel();
		Label GameCredit = new()
		{
            Width = _creditPanelOpned.Width - 10,
            Height = _creditPanelOpned.Height - 10,
            Text = "The game have been made by Trioen Loïc, student at HEH: Department of Sciences and Technologies. Year 2023-2024.",
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.White,
            Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Regular)
        };
		_creditPanelOpned.Controls.Add(GameCredit);
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
    private void ShowMiscCreditPanel()
    {

        ShowCreditPanel();

        AddMusicPanel("Pixeloid Font by GGBotNet", "https://www.fontspace.com/pixeloid-font-f69232", 0);
        AddMusicPanel("Dustyroom Casual Game Sound", "https://dustyroom.com/free-casual-game-sounds/", 1);
    }

    private void AddMusicPanel(string name, string link, int pos)
	{
        Panel panel = new()
        {
            Width = _creditPanelOpned.Width - 50,
            Height = 100,
            BorderStyle = BorderStyle.FixedSingle,
            ForeColor = Color.White,
        };
        panel.Location = new(_creditPanelOpned.Width / 2 - panel.Width / 2, 25 + panel.Height * pos + 10 * pos);
        _creditPanelOpned.Controls.Add(panel);

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
