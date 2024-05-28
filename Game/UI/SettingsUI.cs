using K8055Velleman.Lib.ClassExtension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI;

internal class SettingsUI : UIBase
{
	Control _caller;

	Control _currentSelectedSettings;
	readonly List<Control> _settingsList = [];

	Control _settingsControl;
	Panel _settingsPanel;

	BButton _backButton;
	Label _inputLabel;
	Label _potLabel;
	internal override void OnCreate()
	{
		base.OnCreate();
		base.SetupAnalogChannelEvent();

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
		_backButton.Location = new Point(_settingsPanel.Width / 4 * 1 - _backButton.Width / 2, _settingsPanel.Height - _backButton.Height - 25);
		_backButton.Click += (s, e) => { UIManager.DestroyUI<SettingsUI>(); SaveManager.SaveSettings(); };
		_settingsPanel.Controls.Add(_backButton);

		_inputLabel = new()
		{
            Text = "↑ INP1 | INP2 ↓",
            Font = new Font(FontFamily.GenericSansSerif, 20f, FontStyle.Regular),
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.White,
            Width = _backButton.Width,
            Height = _backButton.Height,
            Location = new(_settingsPanel.Width / 4 * 2 - _backButton.Width / 2, _backButton.Location.Y),
			Visible = K8055.IsConnected,
        };
		_settingsPanel.Controls.Add(_inputLabel);

        _potLabel = new()
        {
            Text = "↔ ATT1",
            Font = new Font(FontFamily.GenericSansSerif, 20f, FontStyle.Regular),
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.White,
            Width = _backButton.Width,
            Height = _backButton.Height,
            Location = new(_settingsPanel.Width / 4 * 3 - _backButton.Width / 2, _backButton.Location.Y),
            Visible = K8055.IsConnected,
        };
        _settingsPanel.Controls.Add(_potLabel);

        Control c1 = CreateBaseSetting("Game Volume");
		TrackBar trackBar = new()
		{
			Width = c1.Width / 2,
			Height = c1.Height,
			Location = new(c1.Width / 2, c1.Height / 4),
			Maximum = 100,
			Minimum = 0,
			Cursor = Cursors.SizeWE,
			Value = (int)(SaveManager.Settings.GameVolume * 100),
		};
		trackBar.ValueChanged += (s, e) => { AudioManager.AudioVolume.GameVolume = trackBar.Value / 100f; };
		c1.Controls.Add(trackBar);
		FinishSettings(c1);

		Control c2 = CreateBaseSetting("Music Volume");
		TrackBar trackBar2 = new()
		{
			Width = c2.Width / 2,
			Height = c2.Height,
			Location = new(c2.Width / 2, c2.Height / 4),
			Maximum = 100,
			Minimum = 0,
			Cursor = Cursors.SizeWE,
			Value = (int)(SaveManager.Settings.MusicVolume * 100),
		};
		trackBar2.ValueChanged += (s, e) => { AudioManager.AudioVolume.MusicVolume = trackBar2.Value / 100f; };
		c2.Controls.Add(trackBar2);
		FinishSettings(c2);

		Control c3 = CreateBaseSetting("UI Volume");
		TrackBar trackBar3 = new()
		{
			Width = c3.Width / 2,
			Height = c3.Height,
			Location = new(c3.Width / 2, c3.Height / 4),
			Maximum = 100,
			Minimum = 0,
			Cursor = Cursors.SizeWE,
			Value = (int)(SaveManager.Settings.UiVolume * 100),
		};
		trackBar3.ValueChanged += (s, e) => { AudioManager.AudioVolume.UiVolume = trackBar3.Value / 100f; };
		c3.Controls.Add(trackBar3);
		FinishSettings(c3);

        Control c4 = CreateBaseSetting("Effect Volume");
        TrackBar trackBar4 = new()
        {
            Width = c4.Width / 2,
            Height = c4.Height,
            Location = new(c4.Width / 2, c4.Height / 4),
            Maximum = 100,
            Minimum = 0,
            Cursor = Cursors.SizeWE,
            Value = (int)(SaveManager.Settings.UiVolume * 100),
        };
        trackBar4.ValueChanged += (s, e) => { AudioManager.AudioVolume.EffectVolume = trackBar4.Value / 100f; };
        c4.Controls.Add(trackBar4);
        FinishSettings(c4);

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
		_inputLabel.Visible = K8055.IsConnected;
		_potLabel.Visible = K8055.IsConnected;
	}

	internal override void OnDigitalChannelsChange(K8055.DigitalChannel digitalChannel)
	{
		if (!_settingsControl.Enabled) return;
		if (digitalChannel == K8055.DigitalChannel.I1)
		{
			int index = _settingsList.IndexOf(_currentSelectedSettings) - 1;
			index = index < 0 ? _settingsList.Count - 1 : index;
			OnMouseEnter(_settingsList[index]);
		} 
		else if (digitalChannel == K8055.DigitalChannel.I2) {
			int index = _settingsList.IndexOf(_currentSelectedSettings) + 1;
			index = index >= _settingsList.Count ? 0 : index;
			OnMouseEnter(_settingsList[index]);
		}
		else if(digitalChannel == K8055.DigitalChannel.I5) _backButton.PerformClick();
	}

	internal override void OnAnalogChannelsChange(K8055.AnalogChannel analogChannel, int value)
	{
		base.OnAnalogChannelsChange(analogChannel, value);
		if(!_settingsControl.Enabled) return;
		if(analogChannel == K8055.AnalogChannel.I1)
		{
			if(_currentSelectedSettings != null) foreach (Control control in _currentSelectedSettings.Controls)
			{
				if (control is TrackBar trackBar)
				{
					trackBar.Value = (int)Math.Round( value / 255d * 100);
				}
			}
		}
	}

	internal void Show(Control caller)
	{
		_caller = caller;
		_caller.Enabled = false;
		_settingsControl.Visible = true;
		_settingsControl.Enabled = true;
	}

	private Control CreateBaseSetting(string labelText) 
	{
		Panel panel = new()
		{
			Height = 64,
			Width = _settingsPanel.Width - 50,
			BorderStyle = BorderStyle.FixedSingle,
			ForeColor = Color.White,
		};
		panel.Location = new Point(_settingsPanel.Width/2 - panel.Width/2, (_settingsList.Count + 1) * 25 + _settingsList.Count * panel.Height);
		panel.MouseEnter += (s, e) => { OnMouseEnter(s as Control); };
		panel.MouseLeave += (s, e) => { OnMouseLeave(s as Control); };

		_settingsList.Add(panel);
		_settingsPanel.Controls.Add(panel);

		Label label = new()
		{
			Text = labelText,
			Font = new Font(FontFamily.GenericSansSerif, 20f, FontStyle.Regular),
			TextAlign = ContentAlignment.MiddleLeft,
			ForeColor = Color.White,
			Width = panel.Width / 2,
			Height = panel.Height,
			Location = new(0, 0),
		};
		panel.Controls.Add(label);

		return panel;
	}

	private void FinishSettings(Control control)
	{
		foreach(Control c in control.Controls)
		{
			c.MouseEnter += (s, e) => { OnMouseEnter((s as Control).Parent); };
			c.MouseLeave += (s, e) => { OnMouseLeave((s as Control).Parent); };
		}
	}

	private void OnMouseEnter(Control control)
	{
		if(_currentSelectedSettings != null) OnMouseLeave(_currentSelectedSettings);
		_currentSelectedSettings = control;
		control.BackColor = Color.Gray;
	}

	private void OnMouseLeave(Control control)
	{
		control.BackColor = Color.Black;
	}

}
