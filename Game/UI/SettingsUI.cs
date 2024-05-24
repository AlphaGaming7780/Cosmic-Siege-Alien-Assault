using K8055Velleman.Lib.ClassExtension;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI
{
    internal class SettingsUI : UIBase
    {
        Control _caller;

        Control _settingsControl;
        Panel _settingsPanel;

        BButton _backButton;

        TrackBar _gameVolumeTrackBar;
        internal override void OnCreate()
        {
            base.OnCreate();
            _settingsControl = new()
            {
                Size = GameWindow.Size,
                BackColor = Color.Black,
                Enabled = false,
                Visible = false,
            };
            GameWindow.Controls.Add(_settingsControl);
            GameWindow.Controls.SetChildIndex(_settingsControl, 0);

            _settingsPanel = new()
            {
                Width = 1280,
                Height = 720,
                BackColor= Color.Black,
                ForeColor= Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true,
            };
            _settingsPanel.Location = new(_settingsControl.Width / 2 - _settingsPanel.Width / 2, _settingsControl.Height / 2 - _settingsPanel.Height / 2);
            _settingsControl.Controls.Add(_settingsPanel);

            _backButton = new()
            {
                Height = 64,
                Width = 256,
                Text = K8055.IsConnected ? "Back (INP5)" : "Back",
                Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular),
                ForeColor = Color.White,
            };
            _backButton.Location = new Point(25, _settingsPanel.Height - _backButton.Height - 25);
            _backButton.Click += (s, e) => { UIManager.DestroyUI<SettingsUI>(); SaveManager.SaveSettings(); };
            _settingsPanel.Controls.Add(_backButton);

            _gameVolumeTrackBar = new()
            {

            };
            Console.WriteLine(_gameVolumeTrackBar.CanFocus);
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();
            GameWindow.Controls.Remove(_settingsControl);
            _settingsControl.Dispose();
            _caller.Enabled = true;
        }

        internal override void OnConnectionChange()
        {
            _backButton.Text = K8055.IsConnected ? "Back (INP5)" : "Back";
        }

        internal override void OnDigitalChannelsChange(K8055.DigitalChannel digitalChannel)
        {
            if (!_settingsControl.Enabled) return;
            else if(digitalChannel == K8055.DigitalChannel.B5) _backButton.PerformClick();
        }

        internal void Show(Control caller)
        {
            _caller = caller;
            _caller.Enabled = false;
            _settingsControl.Visible = true;
            _settingsControl.Enabled = true;
        }
    }
}
