using K8055Velleman.Game.Entities;
using K8055Velleman.Game.Saves;
using K8055Velleman.Game.Systems;
using K8055Velleman.Lib;
using K8055Velleman.Lib.CustomControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI;

internal class PreGameUI : UIBase
{

	Control _preGameUI;
	Label _playerMoney;
	Control _stratagemList;
	BButton _backButton;
	BButton _startGameButton;
	Panel _selectedStratPanel;

	Panel _stratagemInfo;

	Label _vellemanInputSlotSelector;
    Label _vellemanInputStratSelector;

	Label _startMoneyBankLabel;
    TrackBar _startMoneyBank;

	EntitySystem _entitySystem;
	GameSystem _gameSystem;

    readonly Dictionary<string, StratagemEntityBase> _stratagemEntities = [];
	internal List<StratagemEntityBase> selectedStratagemEntities = [null, null, null, null];
    readonly List<Control> _oldPos = [new(), new(), new(), new()];
    readonly List<Control> _slotSelectors = [];
	int _currentStratagemIndex = 0;
	Control _lastBackGroundMouseOver;
	bool _vellemanModeStratagemeSelection = false;

	internal override void OnCreate()
	{
		base.OnCreate();
		SetupAnalogChannelEvent();
		_entitySystem = GameManager.GetOrCreateSystem<EntitySystem>();
		_gameSystem = GameManager.GetOrCreateSystem<GameSystem>();

		_preGameUI = new()
		{
			Width = GameWindow.Width,
			Height = GameWindow.Height,
			BackColor = Color.Black,
		};

		_playerMoney = new()
		{
			Text = $"{SaveManager.CurrentPlayerData?.Money} 💎",
			Top = 25,
            AutoSize = true,
			TextAlign = ContentAlignment.MiddleRight,
            Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
			BorderStyle = BorderStyle.FixedSingle,
            ForeColor = Color.WhiteSmoke,
        };
		_playerMoney.Left = _preGameUI.Width - _playerMoney.Width - RightOffeset - 5;

        _backButton = new()
		{
			Width = 300,
			Height = 100,
			Location = new Point(20, 900),
			Text = K8055.IsConnected ? "Main Menu (INP5)" : "Main Menu",
			ForeColor = Color.White,
			Font = new(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
		};
		_backButton.Click += (sender, e) => { GameManager.Load(GameStatus.MainMenu); };

		_startGameButton = new()
		{
			Width = 300,
			Height = 100,
			Location = new Point(1600, 900),
			Text = K8055.IsConnected ? "Start Game (INP4)" : "Start Game",
			ForeColor = Color.White,
			Font = new(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
		};
		_startGameButton.Click += (sender, e) => { GameManager.Load(GameStatus.Game); };

		_stratagemList = new Panel()
		{
			Width = 522,
			Height = 768,
			Location = new Point(11,32),
			//BackColor = Color.White,
			BorderStyle = BorderStyle.FixedSingle,
			ForeColor = Color.White,
		};

		foreach (Type t in Utility.GetAllSubclassOf(typeof(StratagemEntityBase)))
		{
			if (t.IsAbstract) continue;

            StratagemEntityBase stratagemEntity = _entitySystem.CreateEntity<StratagemEntityBase>(t);
            _stratagemEntities.Add(stratagemEntity.Name, stratagemEntity);
            int y = (int)Math.Floor(stratagemEntity.UiID / 4f);
            int x = stratagemEntity.UiID - y * 4;

			Control backGround = new()
			{
				Name = "BACKGROUND_"+stratagemEntity.Name,
				Size = new Size(128, 128),
			};
			backGround.Location = new Point(2 * (x + 1) + backGround.Size.Width * x, 2 * (y + 1) + backGround.Size.Height * y);
            backGround.Controls.Add(stratagemEntity.MainPanel);

            stratagemEntity.Size = new Size(96, 96);
			stratagemEntity.Location = new Point(16 ,16);
			if (stratagemEntity.Unlockable && ( !SaveManager.CurrentPlayerData.StratagemsData.TryGetValue(stratagemEntity.Name, out StratagemData stratagemData) || !stratagemData.Unlocked))
			{
				stratagemEntity.MainPanel.Click += UnlockStratagem;
				backGround.Click += UnlockStratagem;
			}
			else
			{
				stratagemEntity.MainPanel.Click += SelectStratagem;
				backGround.Click += SelectStratagem;
			}
			stratagemEntity.MainPanel.MouseEnter += ShowStratagemInfo;
			stratagemEntity.MainPanel.MouseLeave += HideStratagemInfo;
			backGround.MouseEnter += ShowStratagemInfo;
            backGround.MouseLeave += HideStratagemInfo;

            _stratagemList.Controls.Add(backGround);
		}

		_selectedStratPanel = new()
		{
			Width = 522,
			Height = 132,
			Location = new Point(GameWindow.Width/2 - 522/2, 850),
			BorderStyle = BorderStyle.FixedSingle,
			ForeColor = Color.White,
		};

		for(int i = 0;i < selectedStratagemEntities.Count; i++)
		{
			Control slotSelector = new()
			{
				Name = i.ToString(),
				Width = 132,
				Height = 132,
				Location = new(i * 132, 0),
				//BackColor = Color.LightBlue,
			};
			slotSelector.Click += (sender, e) => {
				_currentStratagemIndex = 0; 
				foreach (Control control in _slotSelectors) 
				{ 
					if (sender == control) break; 
					_currentStratagemIndex++; 
				}
				SelectSlot();
			};
			_slotSelectors.Add(slotSelector);
			_selectedStratPanel.Controls.Add(slotSelector);
		}
		SelectSlot();

		_vellemanInputSlotSelector = new()
		{
			Height = 64,
			Width = _selectedStratPanel.Width,
			Location = new(_selectedStratPanel.Location.X, _selectedStratPanel.Location.Y + _selectedStratPanel.Height + 10),
			ForeColor = Color.White,
			BorderStyle	= BorderStyle.FixedSingle,
			Text = "<= INP1 | INP3 | INP2 =>",
			TextAlign = ContentAlignment.MiddleCenter,
			Font = new(UIManager.CustomFonts.Families[0], 20f, FontStyle.Regular),
			Visible = K8055.IsConnected,
        };

		_vellemanInputStratSelector = new()
		{
            Height = 64,
            Width = _stratagemList.Width,
            Location = new(_stratagemList.Location.X, _stratagemList.Location.Y + _stratagemList.Height + 10),
            ForeColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Text = "<= INP1 | INP3 | INP2 =>",
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new(UIManager.CustomFonts.Families[0], 20f, FontStyle.Regular),
			Visible = false,
        };

		Panel GameSettings = new()
		{
			Width = 396,
			Height = 312,
			ForeColor = Color.White,
			BorderStyle= BorderStyle.FixedSingle,
		};
		GameSettings.Location = new(_preGameUI.Width / 9 * 8 - GameSettings.Width / 2, _preGameUI.Height / 2 - GameSettings.Height / 2 - 50);

		Label GameSettingsLabel = new()
		{
            Height = 64,
            Width = GameSettings.Width,
            ForeColor = Color.White,
            //BorderStyle = BorderStyle.FixedSingle,
            Text = "Game Settings",
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new(UIManager.CustomFonts.Families[0], 25f, FontStyle.Regular),
        };
		GameSettings.Controls.Add(GameSettingsLabel);

		_startMoneyBankLabel = new()
		{
            Height = 50,
            Width = GameSettings.Width,
            ForeColor = Color.White,
            //BorderStyle = BorderStyle.FixedSingle,
            Text = K8055.IsConnected ? "Start Difficulty (ATT2)" : "Start Difficulty",
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new(UIManager.CustomFonts.Families[0], 15, FontStyle.Regular),
        };
		_startMoneyBankLabel.Location = new(0, 64);
		GameSettings.Controls.Add(_startMoneyBankLabel);

        _startMoneyBank = new()
		{
			Width = GameSettings.Width - 50,
			Height = 150,
			Value = _gameSystem.WaveMoneyBank,
			Minimum = 0,
			Maximum = 10,
			Cursor = Cursors.SizeWE,
		};
        _startMoneyBank.ValueChanged += (s, e) => { _gameSystem.WaveMoneyBank = _startMoneyBank.Value; K8055.OutputAnalogChannel(K8055.AnalogChannel.O1, (int)(_startMoneyBank.Value * 255f) / _startMoneyBank.Maximum); };
		_startMoneyBank.Location = new(25,125);
		GameSettings.Controls.Add(_startMoneyBank);

		//K8055.OutputAnalogChannel(K8055.AnalogChannel.O1, (int)(_startMoneyBank.Value * 255f) / _startMoneyBank.Maximum);

        _preGameUI.Controls.Add(GameSettings);
		_preGameUI.Controls.Add(_vellemanInputSlotSelector);
		_preGameUI.Controls.Add(_vellemanInputStratSelector);
		_preGameUI.Controls.Add(_backButton);
		_preGameUI.Controls.Add(_startGameButton);
		_preGameUI.Controls.Add(_selectedStratPanel);
		_preGameUI.Controls.Add(_stratagemList);
		_preGameUI.Controls.Add(_playerMoney);
		GameWindow.Controls.Add(_preGameUI);

	}

    private void UnlockStratagem(object sender, EventArgs e)
    {
        if (sender is not Control control) return;
        if (control.Name.StartsWith("BACKGROUND_"))
        {
            UnlockStratagem(_stratagemEntities[control.Name.Replace("BACKGROUND_", "")]);
        }
        else
        {
            UnlockStratagem(_stratagemEntities[control.Name]);
        }
        
    }

    private void UnlockStratagem(StratagemEntityBase stratagemEntity)
    {
		if (stratagemEntity.UnkockPrice > SaveManager.CurrentPlayerData.Money) return;
		SaveManager.CurrentPlayerData.Money -= stratagemEntity.UnkockPrice;
		if(SaveManager.CurrentPlayerData.StratagemsData.TryGetValue(stratagemEntity.Name, out StratagemData stratagemData))
		{
			stratagemData.Unlocked = true;
		} else
		{
			StratagemData stratagemData1 = new() {
				Unlocked = true,
			};
            SaveManager.CurrentPlayerData.StratagemsData.Add(stratagemEntity.Name, stratagemData1);
        }
		SaveManager.SaveCurrentPlayerData();
		stratagemEntity.MainPanel.Click -= UnlockStratagem;
		stratagemEntity.MainPanel.Click += SelectStratagem;
		stratagemEntity.MainPanel.Parent.Click -= UnlockStratagem;
        stratagemEntity.MainPanel.Parent.Click += SelectStratagem;
		HideStratagemInfo();
		ShowStratagemInfo(stratagemEntity);
    }

    private void SelectStratagem(object sender, EventArgs e)
	{
		if (sender is not Control control) return;
        if (control.Name.StartsWith("BACKGROUND_"))
        {
            SelectStratagem(_stratagemEntities[control.Name.Replace("BACKGROUND_", "")]);
        }
        else
        {
            SelectStratagem(_stratagemEntities[control.Name]);
        }
	}

    private void SelectStratagem(StratagemEntityBase stratagemEntityBase)
	{
        if (selectedStratagemEntities[_currentStratagemIndex] != null)
        {
            StratagemEntityBase OldStratagemEntityBase = selectedStratagemEntities[_currentStratagemIndex];
            OldStratagemEntityBase.MainPanel.Click -= SelectSlot;
            OldStratagemEntityBase.MainPanel.Click += SelectStratagem;
            _slotSelectors[_currentStratagemIndex].Controls.Remove(OldStratagemEntityBase);
            _oldPos[_currentStratagemIndex].Controls.Add(OldStratagemEntityBase);
        }

        selectedStratagemEntities[_currentStratagemIndex] = stratagemEntityBase;
        _oldPos[_currentStratagemIndex] = stratagemEntityBase.MainPanel.Parent;
        _oldPos[_currentStratagemIndex].Controls.Remove(stratagemEntityBase);
		_oldPos[_currentStratagemIndex].MouseEnter -= ShowStratagemInfo;
        _oldPos[_currentStratagemIndex].MouseLeave -= HideStratagemInfo;
        _oldPos[_currentStratagemIndex].BackColor = Color.Black;
        _slotSelectors[_currentStratagemIndex].Controls.Add(stratagemEntityBase);
        _slotSelectors[_currentStratagemIndex].Controls.SetChildIndex(stratagemEntityBase, 0);

        stratagemEntityBase.MainPanel.Click -= SelectStratagem;
        stratagemEntityBase.MainPanel.Click += SelectSlot;
		if (_currentStratagemIndex >= selectedStratagemEntities.Count - 1) _currentStratagemIndex = 0;
		else _currentStratagemIndex++;
        SelectSlot();
    }


    private void SelectSlot(object sender, EventArgs e)
    {
		if (sender is not Control control) return;
        _currentStratagemIndex = 0;
        foreach (Control slotSelector in _slotSelectors)
        {
            if (control.Parent == slotSelector) break;
            _currentStratagemIndex++;
        }
        SelectSlot();
    }

    private void SelectSlot()
	{
		foreach(Control slotSelector in _slotSelectors)
		{
			slotSelector.BackColor = Color.Black;
		}
		_slotSelectors[_currentStratagemIndex].BackColor = Color.WhiteSmoke;
	}

    private void ShowStratagemInfo(object sender, EventArgs e)
	{
		if(sender is not Control control) return ;
		if (_lastBackGroundMouseOver != null) HideStratagemInfo(_lastBackGroundMouseOver, null);
        if (control.Name.StartsWith("BACKGROUND_"))
		{
			control.BackColor = Color.WhiteSmoke;
			_lastBackGroundMouseOver = control;
            ShowStratagemInfo(_stratagemEntities[control.Name.Replace("BACKGROUND_", "")]);
        } else
		{
			if (control.Parent.Name.StartsWith("BACKGROUND_"))
			{
				control.Parent.BackColor = Color.WhiteSmoke;
                _lastBackGroundMouseOver = control.Parent;
            }
			ShowStratagemInfo(_stratagemEntities[control.Name]);
		}
	}


    private void ShowStratagemInfo(StratagemEntityBase stratagemEntityBase)
	{
		_stratagemInfo = new()
		{
			Width = 600,
			Height = 768,
			Location = new Point(_stratagemList.Location.X + _stratagemList.Width + 10, _stratagemList.Location.Y),
			BorderStyle = BorderStyle.FixedSingle,
			ForeColor = Color.White,
		};

		Label stratName = new() 
		{
			Text = stratagemEntityBase.Name,
			Width = 500,
			Height = 100,
			Location = new(50,50),
            //AutoSize = true,
            Font = new Font(UIManager.CustomFonts.Families[0], 30f, FontStyle.Bold),
			TextAlign = ContentAlignment.MiddleLeft,
			ForeColor = Color.White,
			BorderStyle = BorderStyle.FixedSingle,
        };

		if(stratagemEntityBase is TurretStratagemBase turretStratagem)
		{
            Label DPS = new()
            {
                Text = $"DPS : { Math.Round( turretStratagem.BulletInfo.Damage / (turretStratagem.StartActionSpeed / 1000d), 3)}D/s",
                Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.White,
                AutoSize = true,
                Location = new(50, 175),
            };
            _stratagemInfo.Controls.Add(DPS);
            Label ShotSpeed = new()
			{
				Text = $"Start shoot speed : {turretStratagem.StartActionSpeed/1000d}s",
                Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.White,
				AutoSize = true,
				Location = new(50, DPS.Top + 50),
            };
            _stratagemInfo.Controls.Add(ShotSpeed);

			GroupBox AmmoInfo = new()
			{
				Text = "Ammunition Information",
				Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
				ForeColor = Color.White,
				Location = new(50, ShotSpeed.Top + 50),
				Width = 500,
				MaximumSize = new(500, 600),
				AutoSize = true,
            };
			_stratagemInfo.Controls.Add(AmmoInfo);

            Label Damage = new()
			{
				Text = $"Damage : {turretStratagem.BulletInfo.Damage}",
                Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new(50, 50),
                Width = 500,
                AutoSize = true,
            };
            AmmoInfo.Controls.Add(Damage);

            Label Speed = new()
            {
                Text = $"Speed : {turretStratagem.BulletInfo.Speed}",
                Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new(50, 100),
                Width = 500,
                AutoSize = true,
            };
            AmmoInfo.Controls.Add(Speed);

            Label Size = new()
            {
                Text = $"Bullet Size : {turretStratagem.BulletInfo.Size.Width}",
                Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new(50, 150),
                Width = 500,
                AutoSize = true,
            };
            AmmoInfo.Controls.Add(Size);

            Label guided = new()
            {
                Text = $"Guided : {turretStratagem.BulletInfo.Guided}",
                Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new(50, 200),
                Width = 500,
                AutoSize = true,
            };
            AmmoInfo.Controls.Add(guided);

			Control bulletPreview = new()
			{
				Size = turretStratagem.BulletInfo.Size,
				BackColor = turretStratagem.BulletInfo.Color,
				Location = new(50, 250),
			};
            AmmoInfo.Controls.Add(bulletPreview);

        }

		
		if(stratagemEntityBase.Unlockable && (!SaveManager.CurrentPlayerData.StratagemsData.TryGetValue(stratagemEntityBase.Name, out StratagemData stratagemData) || !stratagemData.Unlocked))
		{
			Label locked = new()
			{
				Text = $"Unlock this stratagem for {stratagemEntityBase.UnkockPrice} 💎",
				Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
				TextAlign = ContentAlignment.MiddleCenter,
				ForeColor = Color.White,
				BorderStyle = BorderStyle.FixedSingle,
				Size = new Size(500, 50),
			};
			locked.Location = new Point(_stratagemInfo.Width / 2 - locked.Width / 2, 650);
			_stratagemInfo.Controls.Add(locked);
		}

        _stratagemInfo.Controls.Add(stratName);

        _preGameUI.Controls.Add(_stratagemInfo);

    }

	private void HideStratagemInfo(object sender, EventArgs e)
	{
        if (sender is not Control control) return;
        if (control.Name.StartsWith("BACKGROUND_"))
        {
            control.BackColor = Color.Black;
        }
        else if (control.Parent.Name.StartsWith("BACKGROUND_")) control.Parent.BackColor = Color.Black;
        HideStratagemInfo();
	}

    private void HideStratagemInfo()
    {
		_preGameUI.Controls.Remove(_stratagemInfo);
        _stratagemInfo.Dispose();
    }

    internal override void OnDestroy()
	{
		base.OnDestroy();
		GameWindow.Controls.Remove(_preGameUI);
		_preGameUI.Dispose();
		K8055.ClearAnalogChannel(K8055.AnalogChannel.O1);
	}

    internal override void OnConnectionChange()
    {
        UpdateButtonLabelForMode();
		_startMoneyBankLabel.Text = K8055.IsConnected ? "Start Difficulty (ATT2)" : "Start Difficulty";
    }

    internal override void OnDigitalChannelsChange(K8055.DigitalChannel digitalChannel)
    {
        if(!_preGameUI.Enabled) return;

		if(digitalChannel == K8055.DigitalChannel.I1)
		{
			if(_vellemanModeStratagemeSelection && IsThereAnyStratLeft())
			{
                int index = 0;
                if (_lastBackGroundMouseOver != null) index = _stratagemList.Controls.IndexOf(_lastBackGroundMouseOver) + 1;

                Control control = null;
                while (control == null)
                {
                    if (index >= _stratagemList.Controls.Count) index = 0;
                    control = _stratagemList.Controls[index];
                    if (control.Controls.Count > 0) break;
                    control = null;
                    index++;
                }

                ShowStratagemInfo(control, null);

            } else
			{
                if (_currentStratagemIndex <= 0) _currentStratagemIndex = selectedStratagemEntities.Count - 1;
                else _currentStratagemIndex--;
                SelectSlot();
            }
        }
		else if(digitalChannel == K8055.DigitalChannel.I2)
		{
			if(_vellemanModeStratagemeSelection && IsThereAnyStratLeft())
			{
                int index = _stratagemList.Controls.Count - 1;
                if (_lastBackGroundMouseOver != null) index = _stratagemList.Controls.IndexOf(_lastBackGroundMouseOver) - 1;

                Control control = null;
                while (control == null)
                {
                    if (index < 0) index = _stratagemList.Controls.Count - 1;
                    control = _stratagemList.Controls[index];
                    if (control.Controls.Count > 0) break;
                    control = null;
                    index--;
                }
                ShowStratagemInfo(control, null);
            } else
			{
                if (_currentStratagemIndex >= selectedStratagemEntities.Count - 1) _currentStratagemIndex = 0;
                else _currentStratagemIndex++;
                SelectSlot();
            }
        }
		else if(digitalChannel == K8055.DigitalChannel.I3)
		{
			if(_vellemanModeStratagemeSelection)
			{
				if (_lastBackGroundMouseOver.Controls.Count <= 0) return;
				StratagemEntityBase stratagemEntity = _stratagemEntities[_lastBackGroundMouseOver.Controls[0].Name];
                if (stratagemEntity.Unlockable && (!SaveManager.CurrentPlayerData.StratagemsData.TryGetValue(stratagemEntity.Name, out StratagemData stratagemData) || !stratagemData.Unlocked))
                {
					UnlockStratagem(_lastBackGroundMouseOver, null);
                }
                else
                {
					SelectStratagem(_lastBackGroundMouseOver, null);
					HideStratagemInfo(_lastBackGroundMouseOver, null);
                }
				if (!IsThereAnyStratLeft())
				{
					_vellemanModeStratagemeSelection = false;
					UpdateButtonLabelForMode();
                }
            } else if(IsThereAnyStratLeft())
			{
				_vellemanModeStratagemeSelection = true;
				UpdateButtonLabelForMode();
            }
        }
		else if(digitalChannel == K8055.DigitalChannel.I4)
		{
			if(!_vellemanModeStratagemeSelection) _startGameButton.PerformClick();
        } 
		else if(digitalChannel == K8055.DigitalChannel.I5)
		{
			if (_vellemanModeStratagemeSelection) {
				_vellemanModeStratagemeSelection = false;
				UpdateButtonLabelForMode();
			}
			else _backButton.PerformClick();
		}
    }

    internal override void OnAnalogChannelsChange(K8055.AnalogChannel analogChannel, int value)
    {
        if (!_preGameUI.Enabled) return;
        base.OnAnalogChannelsChange(analogChannel, value);
		if (analogChannel == K8055.AnalogChannel.I2) _startMoneyBank.Value = (int)(value / 255f * _startMoneyBank.Maximum);
    }

    private void UpdateButtonLabelForMode()
	{
		_backButton.Text = (K8055.IsConnected && !_vellemanModeStratagemeSelection) ? "Main Menu (INP5)" : "Main Menu";
        _startGameButton.Text = (K8055.IsConnected && !_vellemanModeStratagemeSelection) ? "Start Game (INP4)" : "Start Game";
		_vellemanInputSlotSelector.Visible = K8055.IsConnected && !_vellemanModeStratagemeSelection;
		_vellemanInputStratSelector.Visible = K8055.IsConnected && _vellemanModeStratagemeSelection;

    }

	private bool IsThereAnyStratLeft()
	{
		foreach(Control control in _stratagemList.Controls)
		{
			if (control.Controls.Count > 0) return true;
		}
		return false;
	}

}
