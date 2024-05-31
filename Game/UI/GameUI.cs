using K8055Velleman.Game.Entities;
using K8055Velleman.Game.Systems;
using K8055Velleman.Game.Interfaces;
using K8055Velleman.Lib.ClassExtension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI
{
	internal class GameUI : UIBase
	{
		List<StratagemEntityBase> _StratagemEntityBases;
		StratagemEntityBase _StratagemEntity;

        internal Panel GamePanel { get; private set; }
		Panel _selectedStratPanel;
		Panel _stratInfoPanel;
		GameSystem _gameSystem;

		BButton _FirstUpgrade;
		BButton _SecondeUpgrade;

		internal Label Score;
		Label _gameDiffictyIncreaseLabel;

        private bool _upgrading = false;

        internal bool IsStratInfoPanelShowed { get { return _stratInfoPanel != null; } }

        internal override void OnCreate()
		{
			base.OnCreate();
			_gameSystem = GameManager.GetOrCreateSystem<GameSystem>();
			SetupAnalogChannelEvent();
			GamePanel = new()
			{
				Size = GameWindow.Size,
				Location = new(0, 0),
				BackColor = Color.Black,
			};

			_selectedStratPanel = new()
			{
				Width = 522,
				Height = 132,
				Location = new Point(GameWindow.Width / 2 - 522 / 2, 900),
				BorderStyle = BorderStyle.FixedSingle,
				ForeColor = Color.White,
			};
			GamePanel.Controls.Add(_selectedStratPanel);

            GroupBox GameInfo = new()
            {
                Text = "Game Info",
                Font = new(UIManager.CustomFonts.Families[0], 10f, FontStyle.Bold),
                Width = 128,
                Height = 75,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
            };
			GameInfo.Location = new Point(GamePanel.Width - GameInfo.Width - RightOffeset - 5, 0);
			GamePanel.Controls.Add(GameInfo);

            Score = new()
            {
                Text = $"{_gameSystem.Scores} 🌟",
                Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                ForeColor = Color.LightBlue,
				BackColor = Color.Transparent,
                Width = GameInfo.Width - 10,
                Height = 25,
                TextAlign = ContentAlignment.MiddleRight,
            };
            Score.Location = new(5, 17);
            GameInfo.Controls.Add(Score);

            _gameDiffictyIncreaseLabel = new()
			{
				Text = $"{_gameSystem.WaveMoneyPay} 📛",
				Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
				ForeColor = Color.OrangeRed,
                BackColor = Color.Transparent,
                Width = GameInfo.Width - 10,
				Height = 25, 
				TextAlign = ContentAlignment.MiddleRight,
				Location = new Point(5, Score.Height + Score.Location.Y),
            };
			GameInfo.Controls.Add(_gameDiffictyIncreaseLabel);
            GameWindow.Controls.Add(GamePanel);
			K8055.OutputAnalogChannel(K8055.AnalogChannel.O1, (int)(_gameSystem.WaveMoneyPay / 10f * 255));
		}

		internal override void OnDestroy()
		{
			base.OnDestroy();
			GameWindow.Controls.Remove(GamePanel);
			GamePanel.Dispose();
			GamePanel = null;
			K8055.ClearAllDigital();
		}

		//internal override void OnResize()
		//{
		//	GamePanel.Size = GameWindow.Size;
		//}

		internal void UpdateStratagemList(List<StratagemEntityBase> stratagemEntityBases)
		{
			_StratagemEntityBases = stratagemEntityBases;
			int i = 0;
			foreach(StratagemEntityBase stratagemEntityBase in stratagemEntityBases) 
			{
				stratagemEntityBase.MainPanel.Location = new Point(2 * (i + 1) + 130 * i, 1);
				stratagemEntityBase.MainPanel.Size = new(128,128);
				_selectedStratPanel.Controls.Add(stratagemEntityBase.MainPanel);
				_selectedStratPanel.Controls.SetChildIndex(stratagemEntityBase.MainPanel, 0);
				stratagemEntityBase.MainPanel.Enabled = true;
				stratagemEntityBase.MainPanel.MouseEnter += (s, e) => { ShowStratInfo(stratagemEntityBase); };
				stratagemEntityBase.MainPanel.MouseLeave += (s, e) => { if (!_upgrading) { HideStratInfo(); } };
				stratagemEntityBase.MainPanel.Click += (s, e) => { ShowStratInfo(stratagemEntityBase, true); };
				i++;
			}
		}

        private void ShowStratInfo(int index)
        {
            if (_StratagemEntityBases.Count <= index) return;

            ShowStratInfo(_StratagemEntityBases[index], _StratagemEntity == _StratagemEntityBases[index]);
        }

        private void ShowStratInfo(StratagemEntityBase stratagemEntityBase, bool upgrade = false)
		{
			if (_stratInfoPanel is not null) HideStratInfo();
			if(stratagemEntityBase.level >= stratagemEntityBase.MaxLevel) upgrade = false;
			_upgrading = upgrade;

			_StratagemEntity = stratagemEntityBase;

			if (K8055.IsConnected) _gameSystem.UpdateDigitalChannels(stratagemEntityBase.level);

			_stratInfoPanel = new()
			{
				Width = 622,
				Height = upgrade ? 256 : 128,
				BorderStyle = BorderStyle.FixedSingle,
				ForeColor = Color.White,
			};
			_stratInfoPanel.Location = new Point(_selectedStratPanel.Location.X + _selectedStratPanel.Width / 2 - _stratInfoPanel.Width / 2, _selectedStratPanel.Location.Y - _stratInfoPanel.Height - 10);
			//StratInfoPanel.MouseLeave += (s, e) => { HideStratInfo(); };

			Label stratName = new()
			{
				Text = $"{stratagemEntityBase.Name} ({stratagemEntityBase.level}/{stratagemEntityBase.MaxLevel})",
				Width = _stratInfoPanel.Width,
				Height = 50,
				Location = new(0, 0),
				//AutoSize = true,
				Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
				TextAlign = ContentAlignment.MiddleCenter,
				ForeColor = Color.White,
				BorderStyle = BorderStyle.FixedSingle,
			};
			_stratInfoPanel.Controls.Add(stratName);

			GroupBox upgradeGroupBox = null;
			if(upgrade)
			{
				upgradeGroupBox = new()
				{
					Text = $"Upgrade for {_gameSystem.GetStartagemUpgradeCost(stratagemEntityBase.level)}$",
					Width = _stratInfoPanel.Width - 20,
					Height = _stratInfoPanel.Height / 2 - 10,
					Location = new(10, _stratInfoPanel.Height / 2 ),
					ForeColor = Color.White,
                    Font = new Font(UIManager.CustomFonts.Families[0], 12f, FontStyle.Bold),
                };
				_stratInfoPanel.Controls.Add(upgradeGroupBox);


				if (_gameSystem.playerSystem.player.Money < _gameSystem.GetStartagemUpgradeCost(stratagemEntityBase.level))
				{
					Label label = new()
					{
						Text = "Not Enough Money",
						ForeColor = Color.White,
						Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
						Location = new(25, 25),
						Width = upgradeGroupBox.Width - 50,
						Height = upgradeGroupBox.Height - 50,
						TextAlign = ContentAlignment.MiddleCenter,
                    };
                    upgradeGroupBox.Controls.Add(label);
					upgrade = false;
                }
			}

			if (stratagemEntityBase is TurretStratagemBase turretStratagem)
			{
				Label ShotSpeed = new()
				{
					Text = $"Shooting speed : {turretStratagem.ActionSpeed/1000d}s",
					Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
					TextAlign = ContentAlignment.MiddleLeft,
                    Width = _stratInfoPanel.Width / 2,
                    ForeColor = Color.White,
					Location = new(0, 55),
				};
				ShotSpeed.MinimumSize = ShotSpeed.Size;
				ShotSpeed.AutoSize = true;
                _stratInfoPanel.Controls.Add(ShotSpeed);

                Label Damage = new()
                {
                    Text = $"Bullet Damage : {turretStratagem.bulletInfo.Damage}",
                    Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleRight,
                    ForeColor = Color.White,
                    Location = new(_stratInfoPanel.Width / 2, 55),
                    Width = _stratInfoPanel.Width / 2,
                };
                Damage.MinimumSize = Damage.Size;
                Damage.AutoSize = true;
                _stratInfoPanel.Controls.Add(Damage);

                Label DPS = new()
				{
					Text = $"DPS : {Math.Round(turretStratagem.bulletInfo.Damage/(turretStratagem.ActionSpeed/1000d),3)} D/s",
					Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new(0, 90),
					Width = _stratInfoPanel.Width / 2,
                    TextAlign = ContentAlignment.MiddleLeft,
                    AutoSize = true
                };
				_stratInfoPanel.Controls.Add(DPS);

				Label Speed = new()
				{
					Text = $"Bullet Speed : {turretStratagem.bulletInfo.Speed}",
					Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleRight,
                    ForeColor = Color.White,
					Location = new(_stratInfoPanel.Width / 2, 90),
					Width = _stratInfoPanel.Width / 2,
				};
				_stratInfoPanel.Controls.Add(Speed);

				//Label Size = new()
				//{
				//    Text = $"Bullet Size : {turretStratagem.bulletInfo.Size.Width}",
				//    Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
				//    ForeColor = Color.White,
				//    Location = new(50, 150),
				//    Width = 500,
				//    AutoSize = true,
				//};
				//StratInfoPanel.Controls.Add(Size);

				//Label guided = new()
				//{
				//    Text = $"Guided : {turretStratagem.bulletInfo.Guided}",
				//    Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
				//    ForeColor = Color.White,
				//    Location = new(50, 200),
				//    Width = 500,
				//    AutoSize = true,
				//};
				//StratInfoPanel.Controls.Add(guided);

				if(upgrade)
				{
                    _FirstUpgrade = new()
					{
                        Text = "Shoot Speed",
						Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
						ForeColor = Color.White,
                        Width = upgradeGroupBox.Width / 2 - 50,
                        Height = upgradeGroupBox.Height - 50,
						Location = new(25,25),
                    };
                    _FirstUpgrade.GotFocus += (s, e) => { 
						ShotSpeed.Text = $"Shooting speed : {turretStratagem.ActionSpeed / 1000d}s - {UpgradesValue.ActionSpeed/1000d}s"; 
						double dPS = Math.Round(turretStratagem.bulletInfo.Damage / (turretStratagem.ActionSpeed / 1000d), 3); 
						double newDPS = Math.Round((turretStratagem.bulletInfo.Damage / (turretStratagem.ActionSpeed / 1000d - 0.1d)) - dPS, 3); 
						DPS.Text = $"DPS : {dPS} D/s + {newDPS} D/s"; 
					};
                    _FirstUpgrade.LostFocus += (s, e) => { 
						ShotSpeed.Text = $"Shooting speed : {turretStratagem.ActionSpeed / 1000d}s";
						DPS.Text = $"DPS : {turretStratagem.bulletInfo.Damage / (turretStratagem.ActionSpeed / 1000d)} D/s";
					};
                    _FirstUpgrade.Click += (s, e) => { _gameSystem.UpgradeStratagem(turretStratagem, Upgrades.ActionSpeed); ShowStratInfo(stratagemEntityBase, true); };
                    upgradeGroupBox.Controls.Add(_FirstUpgrade);

                    _SecondeUpgrade = new()
                    {
                        Text = "Damage",
                        Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                        ForeColor = Color.White,
                        Width = upgradeGroupBox.Width / 2 - 50,
                        Height = upgradeGroupBox.Height - 50,
                        Location = new(upgradeGroupBox.Width / 2 + 25, 25),
                    };
                    _SecondeUpgrade.GotFocus += (s, e) => {
						Damage.Text = $"Bullet Damage : {turretStratagem.bulletInfo.Damage} + {UpgradesValue.BulletDamage}";
                        double dPS = turretStratagem.bulletInfo.Damage / (turretStratagem.ActionSpeed / 1000d);
                        double newDPS = ((turretStratagem.bulletInfo.Damage + UpgradesValue.BulletDamage) / (turretStratagem.ActionSpeed / 1000d)) - dPS;
                        DPS.Text = $"DPS : {Math.Round(dPS, 3)} D/s + {Math.Round(newDPS, 3)} D/s";
                    };
                    _SecondeUpgrade.LostFocus += (s, e) => {
                        Damage.Text = $"Bullet Damage : {turretStratagem.bulletInfo.Damage}";
                        DPS.Text = $"DPS : {Math.Round(turretStratagem.bulletInfo.Damage / (turretStratagem.ActionSpeed / 1000d),3)} D/s";
                    };
                    _SecondeUpgrade.Click += (s, e) => { _gameSystem.UpgradeStratagem(turretStratagem, Upgrades.BulletDamage); ShowStratInfo(stratagemEntityBase, true); };
                    upgradeGroupBox.Controls.Add(_SecondeUpgrade);
                }

			}

			if(upgrade && K8055.IsConnected)
			{
				_FirstUpgrade.Text = $"{_FirstUpgrade.Text} (INP1)";
				_SecondeUpgrade.Text = $"{_SecondeUpgrade.Text} (INP3)";
            }

			GamePanel.Controls.Add(_stratInfoPanel);
			GamePanel.Controls.SetChildIndex(_stratInfoPanel, 0);

		}

        internal void HideStratInfo()
		{
			_FirstUpgrade = null;
			_SecondeUpgrade = null;
			_upgrading = false;
			_StratagemEntity = null;
			GamePanel.Controls.Remove(_stratInfoPanel);
			_stratInfoPanel.Dispose();
			_stratInfoPanel = null;
			_gameSystem.UpdateDigitalChannels(_gameSystem.playerSystem.player.Health);
		}

        internal override void OnConnectionChange()
        {
			if (_FirstUpgrade != null) _FirstUpgrade.Text = K8055.IsConnected ? $"{_FirstUpgrade.Text} (INP1)" : _FirstUpgrade.Text.Replace(" (INP1)", "");
			if (_SecondeUpgrade != null) _SecondeUpgrade.Text = K8055.IsConnected ? $"{_SecondeUpgrade.Text} (INP3)" : _SecondeUpgrade.Text.Replace(" (INP3)", "");
        }

        internal override void OnDigitalChannelsChange(K8055.DigitalChannel digitalChannel)
        {
			if(!GamePanel.Enabled) return;
			if (digitalChannel == K8055.DigitalChannel.I1)
			{
                if (!_upgrading) ShowStratInfo(0);
				else if(_FirstUpgrade != null)
				{
					if(!_FirstUpgrade.Focused) _FirstUpgrade.Focus();
					else _FirstUpgrade.PerformClick();
				}
            }
			else if (digitalChannel == K8055.DigitalChannel.I2)
			{
                if (!_upgrading) ShowStratInfo(1);
            }
			else if (digitalChannel == K8055.DigitalChannel.I3)
			{
                if (!_upgrading) ShowStratInfo(2);
                else if (_SecondeUpgrade != null)
                {
                    if (!_SecondeUpgrade.Focused) _SecondeUpgrade.Focus();
                    else _SecondeUpgrade.PerformClick();
                }
            }
			else if (digitalChannel == K8055.DigitalChannel.I4)
			{
                if (!_upgrading) ShowStratInfo(3);
            }
			else if (digitalChannel == K8055.DigitalChannel.I5)
			{
				if(_stratInfoPanel != null) HideStratInfo();
				else _gameSystem.PauseLogique();
			}
        }

        internal override void OnAnalogChannelsChange(K8055.AnalogChannel analogChannel, int value)
        {
            if (!GamePanel.Enabled) return;
            base.OnAnalogChannelsChange(analogChannel, value);
			if(analogChannel == K8055.AnalogChannel.I2)
			{
				int i = (int)(value / 255f * 9) + 1;
				if(i != _gameSystem.WaveMoneyPay)
				{
					_gameSystem.WaveMoneyPay = i;
                    _gameDiffictyIncreaseLabel.Text = $"{_gameSystem.WaveMoneyPay} 📛";
                    K8055.OutputAnalogChannel(K8055.AnalogChannel.O1, (int)(_gameSystem.WaveMoneyPay / 10f * 255));
                }
            }
        }
    }
}
