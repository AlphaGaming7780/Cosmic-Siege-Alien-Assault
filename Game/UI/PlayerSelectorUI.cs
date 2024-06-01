using K8055Velleman.Game.Saves;
using K8055Velleman.Lib.CustomControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI
{
    internal class PlayerSelectorUI : UIBase
    {
        Control _oldSelected = null;
        PlayerData _selectedPlayer;

        Panel _playerSelectorPanel;
        Panel _playerSelectorScrollPanel;

        BButton _deletePlayerButton;
        BButton _selectePlayerButton;
        BButton _newPlayerButton;
        BButton _createNewPlayerButton;
        BButton _cancelNewPalyerButton;

        internal override void OnCreate()
        {
            base.OnCreate();
            _selectedPlayer = SaveManager.CurrentPlayerData;

            _playerSelectorPanel = new()
            {
                Width = 800,
                Height = 800,
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = Color.White,
            };
            _playerSelectorPanel.Location = new(GameWindow.Width / 2 - _playerSelectorPanel.Width / 2, GameWindow.Height / 2 - _playerSelectorPanel.Height / 2);

            _newPlayerButton = new()
            {
                Width = 200,
                Height = 75,
                Location = new(550, 675),
                Text = K8055.IsConnected ? "New Player (INP4)" : "New Player",
                Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular)
            };
            _newPlayerButton.Click += NewPlayer_Click;


            _deletePlayerButton = new()
            {
                Width = 200,
                Height = 75,
                Location = new(50, 675),
                Text = K8055.IsConnected ? "Delete Player (INP5)" : "Delete Player",
                Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular),
                Visible = _selectedPlayer != null,
                Enabled = _selectedPlayer != null
            };
            _deletePlayerButton.Click += (s, e) => { SaveManager.DeletePlayerData(_selectedPlayer); _selectedPlayer = null; UpdatePlayerList(); };

            _selectePlayerButton = new()
            {
                Width = 200,
                Height = 75,
                Location = new(300, 675),
                Text = K8055.IsConnected ? "Select Player (INP3)" : "Select Player",
                Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular),
                Visible = _selectedPlayer != null,
                Enabled = _selectedPlayer != null
            };
            _selectePlayerButton.Click += (s, e) => { SaveManager.CurrentPlayerData = _selectedPlayer; GameManager.Load(GameStatus.MainMenu); UIManager.DestroyUI<PlayerSelectorUI>(); };

            UpdatePlayerList();

            _playerSelectorPanel.Controls.Add(_newPlayerButton);
            _playerSelectorPanel.Controls.Add(_deletePlayerButton);
            _playerSelectorPanel.Controls.Add(_selectePlayerButton);
            GameWindow.Controls.Add(_playerSelectorPanel);
            GameWindow.Controls.SetChildIndex(_playerSelectorPanel, 0);
        }

        private void UpdatePlayerList()
        {
            if(_playerSelectorScrollPanel is not null)
            {
                _playerSelectorPanel.Controls.Remove(_playerSelectorScrollPanel);
                _playerSelectorScrollPanel.Dispose();
                _playerSelectorScrollPanel = null;
            }

            _deletePlayerButton.Visible = _selectedPlayer != null;
            _deletePlayerButton.Enabled = _selectedPlayer != null;

            _selectePlayerButton.Visible = _selectedPlayer != null;
            _selectePlayerButton.Enabled = _selectedPlayer != null;

            _playerSelectorScrollPanel = new()
            {
                Width = 750,
                Height = 600,
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = Color.White,
                AutoScroll = true,
                Location = new(25, 25),
            };
            _playerSelectorPanel.Controls.Add(_playerSelectorScrollPanel);

            int x = 0;
            foreach (PlayerData playerData in SaveManager.s_playersData)
            {
                Panel Player = new()
                {
                    Name = "Player",
                    Width = 700,
                    Height = 100,
                    BorderStyle = BorderStyle.FixedSingle,
                    ForeColor = Color.White,
                    Location = new(25, 100 * x + 25 * (x + 1)),
                };
                Player.MouseEnter += OnMouseEnter;
                Player.MouseLeave += OnMouseLeave;
                Player.Click += (s, e) =>
                {
                    Control c = s as Control;
                    OnPlayerClick(c, playerData);
                };

                Label name = new()
                {
                    Width = 400,
                    Height = 100,
                    Text = playerData.Name,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font(UIManager.CustomFonts.Families[0], 30f, FontStyle.Bold),
                    ForeColor = Color.White,
                };

                Label Score = new()
                {
                    Width = 300,
                    Height = 100,
                    Text = $"{playerData.HigestScore} 🌟",
                    TextAlign = ContentAlignment.MiddleRight,
                    Font = new Font(UIManager.CustomFonts.Families[0], 30f, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(400, 0),
                };

                Player.Controls.Add(name);
                Player.Controls.Add(Score);

                foreach (Control child in Player.Controls)
                {
                    child.MouseEnter += OnMouseEnter;
                    child.MouseLeave += OnMouseLeave;
                    child.Click += (s, e) =>
                    {
                        Control c = s as Control;
                        OnPlayerClick(c.Parent, playerData);
                    };
                }

                if (_selectedPlayer == playerData) OnPlayerClick(Player, playerData);

                _playerSelectorScrollPanel.Controls.Add(Player);
                x++;
            }
        }

        private void OnPlayerClick(Control player, PlayerData playerData)
        {
            if (_oldSelected != null)
            {
                _oldSelected.MouseEnter += OnMouseEnter;
                _oldSelected.MouseLeave += OnMouseLeave;
                _oldSelected.BackColor = Color.Transparent;
                AddEventToPlayerChild(_oldSelected);
            }
            _oldSelected = player;
            onMouseClickColorChange(player);
            _selectedPlayer = playerData;
            _deletePlayerButton.Visible = true;
            _deletePlayerButton.Enabled = true;
            _selectePlayerButton.Enabled = true;
            _selectePlayerButton.Visible = true;
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
            foreach (Control child in player.Controls)
            {
                child.MouseEnter += OnMouseEnter;
                child.MouseLeave += OnMouseLeave;
            }
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            if (sender is not Control c) return;
            if (c.Name == "Player") onMouseEnterColorChange(c);
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
            _playerSelectorPanel.Enabled = false;
            _playerSelectorPanel.Visible = false;

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
                Location = new(50, 0),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            TextBox textBox = new()
            {
                MinimumSize = new Size(400, 50),
                Location = new(50, 50),
                Font = new Font(UIManager.CustomFonts.Families[0], 30f, FontStyle.Bold),
            };

            _createNewPlayerButton = new()
            {
                Width = 200,
                Height = 50,
                Text = K8055.IsConnected ? "New Player (INP1)" : "New Player",
                Font = new Font(FontFamily.GenericSansSerif, K8055.IsConnected ? 15f : 17f, FontStyle.Regular),
                Location = new(275, 125),
            };
            _createNewPlayerButton.Click += (s, e) =>
            {
                bool already = false;
                foreach (PlayerData playerData in SaveManager.s_playersData)
                {
                    if (playerData.Name == textBox.Text)
                    {
                        already = true;
                        break;
                    }
                }
                if (!already)
                {
                    _selectedPlayer = new()
                    {
                        Name = textBox.Text,
                    };
                    SaveManager.s_playersData.Add(_selectedPlayer);
                    SaveManager.CurrentPlayerData = _selectedPlayer;
                    SaveManager.SaveCurrentPlayerData();
                    panel.Dispose();
                    panel = null;
                    GameManager.Load(GameStatus.MainMenu);
                    UIManager.DestroyUI<PlayerSelectorUI>();
                }
            };

            _cancelNewPalyerButton = new()
            {
                Width = 200,
                Height = 50,
                Text = K8055.IsConnected ? "Cancel (INP5)" : "Cancel",
                Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular),
                Location = new(25, 125),
            };
            _cancelNewPalyerButton.Click += (s, e) => { panel.Dispose(); _playerSelectorPanel.Visible = true; _playerSelectorPanel.Enabled = true; };

            panel.Controls.Add(_createNewPlayerButton);
            panel.Controls.Add(_cancelNewPalyerButton);
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

        internal override void OnDestroy()
        {
            base.OnDestroy();
            GameWindow.Controls.Remove(_playerSelectorPanel);
            _playerSelectorPanel.Dispose();
            _playerSelectorPanel = null;
        }


        internal override void OnConnectionChange()
        {
            _newPlayerButton.Text = K8055.IsConnected ? "New Player (INP4)" : "New Player";
            _deletePlayerButton.Text = K8055.IsConnected ? "Delete Player (INP5)" : "Delete Player";
            _selectePlayerButton.Text = K8055.IsConnected ? "Select Player (INP3)" : "Select Player";
            if(!_playerSelectorPanel.Visible)
            {
                _cancelNewPalyerButton.Text = K8055.IsConnected ? "Cancel (INP5)" : "Cancel";
                _createNewPlayerButton.Text = K8055.IsConnected ? "New Player (INP1)" : "New Player";
            }
        }

        internal override void OnDigitalChannelsChange(K8055.DigitalChannel digitalChannel)
        {
            //if (!_playerSelectorPanel.Enabled) return;
            if (digitalChannel == K8055.DigitalChannel.I1)
            {
                if (_playerSelectorPanel.Visible)
                {
                    int x = SaveManager.s_playersData.IndexOf(_selectedPlayer) - 1;
                    if (x < 0) x = SaveManager.s_playersData.Count - 1;
                    _selectedPlayer = SaveManager.s_playersData[x];
                    UpdatePlayerList();
                }
                else 
                {
                    _createNewPlayerButton?.PerformClick();
                }
            }

            else if (digitalChannel == K8055.DigitalChannel.I2)
            {
                if (_playerSelectorPanel.Visible)
                {
                    int x = SaveManager.s_playersData.IndexOf(_selectedPlayer) + 1;
                    if (x >= SaveManager.s_playersData.Count) x = 0;
                    _selectedPlayer = SaveManager.s_playersData[x];
                    UpdatePlayerList();
                }
            }
            else if (digitalChannel == K8055.DigitalChannel.I3)
            {
                if(_playerSelectorPanel.Visible && _selectedPlayer != null) _selectePlayerButton.PerformClick();  
            }
            else if (digitalChannel == K8055.DigitalChannel.I4)
            {
                if(_playerSelectorPanel.Visible) _newPlayerButton.PerformClick();
            }
            else if (digitalChannel == K8055.DigitalChannel.I5)
            {
                if (_playerSelectorPanel.Visible) { if (_selectedPlayer != null) _deletePlayerButton.PerformClick(); }
                else
                {
                    _cancelNewPalyerButton?.PerformClick();
                }
            }
        }
    }
}
