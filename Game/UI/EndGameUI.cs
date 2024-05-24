using K8055Velleman.Game.Systems;
using K8055Velleman.Lib.ClassExtension;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI;

internal class EndGameUI : UIBase
{
    Panel _endGameMenu;
    GameSystem _gameSystem;
    BButton _tryAgainButton;
    BButton _settingsButton;
    BButton _mainMenuButton;
    internal override void OnCreate()
    {
        base.OnCreate();

        _gameSystem = GameManager.GetOrCreateSystem<GameSystem>();

        _endGameMenu = new ()
        {
            Width = 1280,
            Height = 720,
            BorderStyle = BorderStyle.FixedSingle,
            ForeColor = Color.White,
        };
        _endGameMenu.Location = new Point(GameWindow.Width / 2 - _endGameMenu.Width / 2, GameWindow.Height / 2 - _endGameMenu.Height / 2);

        Label gameEndedText = new()
        {
            Text = "You died.",
            Font = new Font(UIManager.CustomFonts.Families[0], 30f, FontStyle.Bold),
            ForeColor = Color.White,
            //AutoSize = true,
            Width = 250,
            Height = 50,
            TextAlign = ContentAlignment.MiddleCenter,
            BorderStyle = BorderStyle.FixedSingle,
        };
        gameEndedText.Location = new(_endGameMenu.Width / 2 - gameEndedText.Width / 2, 25);

        Label Score = new()
        {
            Text = $"Score : {_gameSystem.waveNum} 🌟",
            Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
            ForeColor = Color.White,
            //AutoSize = true,
            Width = 500,
            Height = 50,
            TextAlign = ContentAlignment.MiddleCenter,
            //BorderStyle = BorderStyle.FixedSingle,
        };
        Score.Location = new(_endGameMenu.Width / 4 - Score.Width / 2, 100);

        Label TotalEarnedMoney = new()
        {
            Text = $"Total Earned Money : {_gameSystem.playerSystem.player.TotalMoney} 💲",
            Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = true,
            Width = 500,
            Height = 50,
            TextAlign = ContentAlignment.MiddleCenter,
            //BorderStyle = BorderStyle.FixedSingle,
        };
        TotalEarnedMoney.Location = new(_endGameMenu.Width / 4 * 3 - TotalEarnedMoney.Width / 2, 100);

        _tryAgainButton = new()
        {
            Text = K8055.IsConnected ? "Try again (INP1)" : "Try again",
            Width = 250,
            Height = 50,
            ForeColor = Color.White,
            Font = new Font(UIManager.CustomFonts.Families[0], K8055.IsConnected ? 10f : 20f, FontStyle.Bold),
        };
        _tryAgainButton.Click += (s, e) => { GameManager.DestroySystem<GameSystem>(); GameManager.instance.Load(GameStatus.PreGame); };
        _tryAgainButton.Location = new(_endGameMenu.Width / 4 - _tryAgainButton.Width / 2, _endGameMenu.Height - _tryAgainButton.Height - 25);

        _settingsButton = new()
        {
            Text = K8055.IsConnected ? "Settings (INP3)" : "Settings",
            Width = 250,
            Height = 50,
            ForeColor = Color.White,
            Font = new Font(UIManager.CustomFonts.Families[0], K8055.IsConnected ? 10f : 20f, FontStyle.Bold),
        };
        _settingsButton.Click += (s, e) => { GameManager.instance.Load(GameStatus.MainMenu); };
        _settingsButton.Location = new(_endGameMenu.Width / 2 - _settingsButton.Width / 2, _endGameMenu.Height - _tryAgainButton.Height - 25);

        _mainMenuButton = new()
        {
            Text = K8055.IsConnected ? "Main Menu (INP5)" : "Main Menu",
            Width = 250,
            Height = 50,
            ForeColor = Color.White,
            Font = new Font(UIManager.CustomFonts.Families[0], K8055.IsConnected ? 10f : 20f, FontStyle.Bold),
        };
        _mainMenuButton.Click += (s, e) => { GameManager.instance.Load(GameStatus.MainMenu); };
        _mainMenuButton.Location = new(_endGameMenu.Width / 4 * 3 - _mainMenuButton.Width / 2, _endGameMenu.Height - _tryAgainButton.Height - 25);

        _endGameMenu.Controls.Add(gameEndedText);
        _endGameMenu.Controls.Add(Score);
        _endGameMenu.Controls.Add(TotalEarnedMoney);
        _endGameMenu.Controls.Add(_tryAgainButton);
        _endGameMenu.Controls.Add(_settingsButton);
        _endGameMenu.Controls.Add(_mainMenuButton);
        GameWindow.Controls.Add(_endGameMenu);
        GameWindow.Controls.SetChildIndex(_endGameMenu, 0);
    }

    internal override void OnDestroy()
    {
        base.OnDestroy();
        _gameSystem = null;
        GameWindow.Controls.Remove(_endGameMenu);
        _endGameMenu.Dispose();
        _endGameMenu = null;
    }

    internal override void OnConnectionChange()
    {
        _tryAgainButton.Text = K8055.IsConnected ? "Try again (INP1)" : "Try again";
        _tryAgainButton.Font = new Font(UIManager.CustomFonts.Families[0], K8055.IsConnected ? 10f : 20f, FontStyle.Bold);
        _settingsButton.Text = K8055.IsConnected ? "Settings (INP3)" : "Settings";
        _settingsButton.Font = new Font(UIManager.CustomFonts.Families[0], K8055.IsConnected ? 10f : 20f, FontStyle.Bold);
        _mainMenuButton.Text = K8055.IsConnected ? "Main Menu (INP5)" : "Main Menu";
        _mainMenuButton.Font = new Font(UIManager.CustomFonts.Families[0], K8055.IsConnected ? 10f : 20f, FontStyle.Bold);
    }

    internal override void OnDigitalChannelsChange(K8055.DigitalChannel digitalChannel)
    {
        if (!_endGameMenu.Enabled) return;
        if(digitalChannel == K8055.DigitalChannel.B1) _tryAgainButton.PerformClick();
        if(digitalChannel == K8055.DigitalChannel.B3) _settingsButton.PerformClick();
        if(digitalChannel == K8055.DigitalChannel.B5) _mainMenuButton.PerformClick();
    }
}
