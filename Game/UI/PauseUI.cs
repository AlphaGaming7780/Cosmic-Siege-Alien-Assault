using K8055Velleman.Game.Systems;
using K8055Velleman.Lib.ClassExtension;
using System.Drawing;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI;
internal class PauseUI : UIBase
{
    internal GameSystem gameSystem;
    Panel _pauseMenu;
    BButton _resumeButton;
    BButton _settingsButton;
    BButton _mainMenuButton;
    internal override void OnCreate()
    {
        base.OnCreate();
        _pauseMenu = new()
        {
            Width = 720,
            Height = 325,
            ForeColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
        };
        _pauseMenu.Location = new(GameWindow.Width / 2 - _pauseMenu.Width / 2, GameWindow.Height / 2 - _pauseMenu.Height / 2);

        Label gamePausedText = new()
        {
            Text = "Game Paused",
            Font = new Font(UIManager.CustomFonts.Families[0], 30f, FontStyle.Bold),
            ForeColor = Color.White,
            //AutoSize = true,
            Width = 500,
            Height = 50,
            TextAlign = ContentAlignment.MiddleCenter,
            BorderStyle = BorderStyle.FixedSingle,
        };
        gamePausedText.Location = new(_pauseMenu.Width / 2 - gamePausedText.Width / 2, 25);

        _resumeButton = new()
        {
            Text = K8055.IsConnected ? "Resume (INP1)" : "Resume",
            Width = 300,
            Height = 50,
            ForeColor = Color.White,
            Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
        };
        _resumeButton.Click += (s, e) => { gameSystem.UnPauseGame(); };
        _resumeButton.Location = new(_pauseMenu.Width / 2 - _resumeButton.Width / 2, 100);

        _settingsButton = new()
        {
            Text = K8055.IsConnected ? "Settings (INP3)" : "Settings",
            Width = 300,
            Height = 50,
            ForeColor = Color.White,
            Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
        };
        _settingsButton.Click += (s, e) => { UIManager.GetOrCreateUI<SettingsUI>().Show(_pauseMenu); };
        _settingsButton.Location = new(_pauseMenu.Width / 2 - _settingsButton.Width / 2, 175);

        _mainMenuButton = new()
        {
            Text = K8055.IsConnected ? "Main Menu (INP5)" : "Main Menu",
            Width = 300,
            Height = 50,
            ForeColor = Color.White,
            Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
        };
        _mainMenuButton.Click += (s, e) => { GameManager.instance.Load(GameStatus.MainMenu); };
        _mainMenuButton.Location = new(_pauseMenu.Width / 2 - _mainMenuButton.Width / 2, 250);

        _pauseMenu.Controls.Add(gamePausedText);
        _pauseMenu.Controls.Add(_resumeButton);
        _pauseMenu.Controls.Add(_settingsButton);
        _pauseMenu.Controls.Add(_mainMenuButton);
        GameWindow.Controls.Add(_pauseMenu);
        GameWindow.Controls.SetChildIndex(_pauseMenu, 0);
    }

    internal override void OnDestroy()
    {
        base.OnDestroy();
        GameWindow.Controls.Remove(_pauseMenu);
        _pauseMenu.Dispose();
        _pauseMenu = null;
    }

    internal override void OnConnectionChange()
    {
        if(!_pauseMenu.Enabled) return;
        _resumeButton.Text = K8055.IsConnected ? "Resume (INP1)" : "Resume";
        _settingsButton.Text = K8055.IsConnected ? "Settings (INP3)" : "Settings";
        _mainMenuButton.Text = K8055.IsConnected ? "Main Menu (INP5)" : "Main Menu";
    }

    internal override void OnDigitalChannelsChange(K8055.DigitalChannel digitalChannel)
    {
        if (digitalChannel == K8055.DigitalChannel.I1) _resumeButton.PerformClick(); 
        else if (digitalChannel == K8055.DigitalChannel.I3) _settingsButton.PerformClick(); 
        else if (digitalChannel == K8055.DigitalChannel.I5) _mainMenuButton.PerformClick();
    }
}
