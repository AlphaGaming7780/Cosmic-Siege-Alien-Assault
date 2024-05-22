using K8055Velleman.Game.Saves;
using K8055Velleman.Lib.ClassExtension;
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
        Control oldSelected = null;
        private PlayerData selectedPlayer;
        private Panel PlayerSelector;
        private Panel PlayerSelectorScroll;

        BButton DeletePlayer;
        BButton SelectePlayer;
        BButton NewPlayer;
        BButton CreateNewPlayerButton;
        BButton CancelNewPalyerButton;

        internal override void OnCreate()
        {
            base.OnCreate();
            selectedPlayer = SaveManager.CurrentPlayerData;

            PlayerSelector = new()
            {
                Width = 800,
                Height = 800,
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = Color.White,
            };
            PlayerSelector.Location = new(GameWindow.Width / 2 - PlayerSelector.Width / 2, GameWindow.Height / 2 - PlayerSelector.Height / 2);

            NewPlayer = new()
            {
                Width = 200,
                Height = 75,
                Location = new(550, 675),
                Text = K8055.IsConnected ? "New Player (INP4)" : "New Player",
                Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular)
            };
            NewPlayer.Click += NewPlayer_Click;


            DeletePlayer = new()
            {
                Width = 200,
                Height = 75,
                Location = new(50, 675),
                Text = K8055.IsConnected ? "Delete Player (INP5)" : "Delete Player",
                Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular),
                Visible = selectedPlayer != null,
                Enabled = selectedPlayer != null
            };
            DeletePlayer.Click += (s, e) => { SaveManager.DeletePlayerData(selectedPlayer); selectedPlayer = null; UpdatePlayerList(); };

            SelectePlayer = new()
            {
                Width = 200,
                Height = 75,
                Location = new(300, 675),
                Text = K8055.IsConnected ? "Select Player (INP3)" : "Select Player",
                Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular),
                Visible = selectedPlayer != null,
                Enabled = selectedPlayer != null
            };
            SelectePlayer.Click += (s, e) => { SaveManager.CurrentPlayerData = selectedPlayer; GameManager.instance.Load(GameStatus.MainMenu); UIManager.DestroyUI<PlayerSelectorUI>(); };

            UpdatePlayerList();

            PlayerSelector.Controls.Add(NewPlayer);
            PlayerSelector.Controls.Add(DeletePlayer);
            PlayerSelector.Controls.Add(SelectePlayer);
            GameWindow.Controls.Add(PlayerSelector);
            GameWindow.Controls.SetChildIndex(PlayerSelector, 0);
        }

        private void UpdatePlayerList()
        {
            if(PlayerSelectorScroll is not null)
            {
                PlayerSelector.Controls.Remove(PlayerSelectorScroll);
                PlayerSelectorScroll.Dispose();
                PlayerSelectorScroll = null;
            }

            DeletePlayer.Visible = selectedPlayer != null;
            DeletePlayer.Enabled = selectedPlayer != null;

            SelectePlayer.Visible = selectedPlayer != null;
            SelectePlayer.Enabled = selectedPlayer != null;

            PlayerSelectorScroll = new()
            {
                Width = 750,
                Height = 600,
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = Color.White,
                AutoScroll = true,
                Location = new(25, 25),
            };
            PlayerSelector.Controls.Add(PlayerSelectorScroll);

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

                if (selectedPlayer == playerData) OnPlayerClick(Player, playerData);

                PlayerSelectorScroll.Controls.Add(Player);
                x++;
            }
        }

        private void OnPlayerClick(Control player, PlayerData playerData)
        {
            if (oldSelected != null)
            {
                oldSelected.MouseEnter += OnMouseEnter;
                oldSelected.MouseLeave += OnMouseLeave;
                oldSelected.BackColor = Color.Transparent;
                AddEventToPlayerChild(oldSelected);
            }
            oldSelected = player;
            onMouseClickColorChange(player);
            selectedPlayer = playerData;
            DeletePlayer.Visible = true;
            DeletePlayer.Enabled = true;
            SelectePlayer.Enabled = true;
            SelectePlayer.Visible = true;
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
                Location = new(50, 0),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            TextBox textBox = new()
            {
                MinimumSize = new Size(400, 50),
                Location = new(50, 50),
                Font = new Font(UIManager.CustomFonts.Families[0], 30f, FontStyle.Bold),
            };

            CreateNewPlayerButton = new()
            {
                Width = 200,
                Height = 50,
                Text = K8055.IsConnected ? "New Player (INP1)" : "New Player",
                Font = new Font(FontFamily.GenericSansSerif, K8055.IsConnected ? 15f : 17f, FontStyle.Regular),
                Location = new(275, 125),
            };
            CreateNewPlayerButton.Click += (s, e) =>
            {
                bool already = false;
                foreach (PlayerData playerData in SaveManager.PlayersData)
                {
                    if (playerData.Name == textBox.Text)
                    {
                        already = true;
                        break;
                    }
                }
                if (!already)
                {
                    selectedPlayer = new()
                    {
                        Name = textBox.Text,
                    };
                    SaveManager.PlayersData.Add(selectedPlayer);
                    SaveManager.CurrentPlayerData = selectedPlayer;
                    SaveManager.SaveCurrentPlayerData();
                    panel.Dispose();
                    panel = null;
                    GameManager.instance.Load(GameStatus.MainMenu);
                    UIManager.DestroyUI<PlayerSelectorUI>();
                }
            };

            CancelNewPalyerButton = new()
            {
                Width = 200,
                Height = 50,
                Text = K8055.IsConnected ? "Cancel (INP5)" : "Cancel",
                Font = new Font(FontFamily.GenericSansSerif, 17f, FontStyle.Regular),
                Location = new(25, 125),
            };
            CancelNewPalyerButton.Click += (s, e) => { panel.Dispose(); PlayerSelector.Visible = true; PlayerSelector.Enabled = true; };

            panel.Controls.Add(CreateNewPlayerButton);
            panel.Controls.Add(CancelNewPalyerButton);
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
            GameWindow.Controls.Remove(PlayerSelector);
            PlayerSelector.Dispose();
            PlayerSelector = null;
        }


        internal override void OnConnectionChange()
        {
            NewPlayer.Text = K8055.IsConnected ? "New Player (INP4)" : "New Player";
            DeletePlayer.Text = K8055.IsConnected ? "Delete Player (INP5)" : "Delete Player";
            SelectePlayer.Text = K8055.IsConnected ? "Select Player (INP3)" : "Select Player";
            if(!PlayerSelector.Visible)
            {
                CancelNewPalyerButton.Text = K8055.IsConnected ? "Cancel (INP5)" : "Cancel";
                CreateNewPlayerButton.Text = K8055.IsConnected ? "New Player (INP1)" : "New Player";
            }
        }

        internal override void OnDigitalChannelsChange(K8055.DigitalChannel digitalChannel)
        {
            if (digitalChannel == K8055.DigitalChannel.B1)
            {
                if (PlayerSelector.Visible)
                {
                    int x = SaveManager.PlayersData.IndexOf(selectedPlayer);
                    int i = x <= 0 ? SaveManager.PlayersData.Count - 1 : x - 1;
                    selectedPlayer = SaveManager.PlayersData[i];
                    UpdatePlayerList();
                }
                else 
                {
                    CreateNewPlayerButton?.PerformClick();
                }
            }

            else if (digitalChannel == K8055.DigitalChannel.B2)
            {
                if (PlayerSelector.Visible)
                {
                    int x = SaveManager.PlayersData.IndexOf(selectedPlayer);
                    int i = x >= SaveManager.PlayersData.Count - 1 ? 0 : x + 1;
                    selectedPlayer = SaveManager.PlayersData[i];
                    UpdatePlayerList();
                }
            }
            else if (digitalChannel == K8055.DigitalChannel.B3)
            {
                if(PlayerSelector.Visible && selectedPlayer != null) SelectePlayer.PerformClick();  
            }
            else if (digitalChannel == K8055.DigitalChannel.B4)
            {
                if(PlayerSelector.Visible) NewPlayer.PerformClick();
            }
            else if (digitalChannel == K8055.DigitalChannel.B5)
            {
                if (PlayerSelector.Visible) { if (selectedPlayer != null) DeletePlayer.PerformClick(); }
                else
                {
                    CancelNewPalyerButton?.PerformClick();
                }
            }
        }
    }
}
