using K8055Velleman.Game.Entities;
using K8055Velleman.Game.Systems;
using K8055Velleman.Lib.ClassExtension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI
{
	internal class GameUI : UIBase
	{
		internal Panel GamePanel { get; private set; }
		Panel selectedStratPanel;
		Panel StratInfoPanel;
		Panel PauseMenu;
		Panel EndGameMenu;
		GameSystem gameSystem;

		private bool upgrading = false;

		internal override void OnCreate()
		{
			base.OnCreate();
			gameSystem = GameManager.GetOrCreateSystem<GameSystem>();
			GamePanel = new()
			{
				Size = GameWindow.Size,
				Location = new(0, 0),
				BackColor = Color.Black,
			};

			selectedStratPanel = new()
			{
				Width = 522,
				Height = 132,
				Location = new Point(GameWindow.Width / 2 - 522 / 2, 900),
				BorderStyle = BorderStyle.FixedSingle,
				ForeColor = Color.White,
			};
			GamePanel.Controls.Add(selectedStratPanel);
			GameWindow.Controls.Add(GamePanel);
		}

		internal override void OnDestroy()
		{
			base.OnDestroy();
			GameWindow.Controls.Remove(GamePanel);
			GamePanel.Dispose();
			PauseMenu?.Dispose();
			EndGameMenu?.Dispose();
			GamePanel = null;
			PauseMenu = null;
			EndGameMenu = null;
		}

		//internal override void OnResize()
		//{
		//	GamePanel.Size = GameWindow.Size;
		//}

		internal void UpdateStratagemList(List<StratagemEntityBase> stratagemEntityBases)
		{
			int i = 0;
			foreach(StratagemEntityBase stratagemEntityBase in stratagemEntityBases) 
			{
				stratagemEntityBase.mainPanel.Location = new Point(2 * (i + 1) + 130 * i, 1);
				stratagemEntityBase.mainPanel.Size = new(128,128);
				selectedStratPanel.Controls.Add(stratagemEntityBase.mainPanel);
				selectedStratPanel.Controls.SetChildIndex(stratagemEntityBase.mainPanel, 0);
				stratagemEntityBase.mainPanel.Enabled = true;
				stratagemEntityBase.mainPanel.MouseEnter += (s, e) => { ShowStratInfo(stratagemEntityBase); };
				stratagemEntityBase.mainPanel.MouseLeave += (s, e) => { if (!upgrading) { HideStratInfo(); } };
				stratagemEntityBase.mainPanel.Click += (s, e) => { ShowStratInfo(stratagemEntityBase, true); };
				i++;
			}
		}

		internal void ShowStratInfo(StratagemEntityBase stratagemEntityBase, bool upgrade = false)
		{
			if (StratInfoPanel is not null) HideStratInfo();
			if(stratagemEntityBase.level >= stratagemEntityBase.MaxLevel) upgrade = false;
			upgrading = upgrade;
			StratInfoPanel = new()
			{
				Width = 622,
				Height = upgrade ? 256 : 128,
				BorderStyle = BorderStyle.FixedSingle,
				ForeColor = Color.White,
			};
			StratInfoPanel.Location = new Point(selectedStratPanel.Location.X + selectedStratPanel.Width / 2 - StratInfoPanel.Width / 2, selectedStratPanel.Location.Y - StratInfoPanel.Height - 10);
			//StratInfoPanel.MouseLeave += (s, e) => { HideStratInfo(); };

			Label stratName = new()
			{
				Text = $"{stratagemEntityBase.Name} ({stratagemEntityBase.level}/{stratagemEntityBase.MaxLevel})",
				Width = StratInfoPanel.Width,
				Height = 50,
				Location = new(0, 0),
				//AutoSize = true,
				Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
				TextAlign = ContentAlignment.MiddleCenter,
				ForeColor = Color.White,
				BorderStyle = BorderStyle.FixedSingle,
			};
			StratInfoPanel.Controls.Add(stratName);

			GroupBox upgradeGroupBox = null;
			if(upgrade)
			{
				upgradeGroupBox = new()
				{
					Text = $"Upgrade for {gameSystem.GetStartagemUpgradeCost(stratagemEntityBase.level)}$",
					Width = StratInfoPanel.Width - 20,
					Height = StratInfoPanel.Height / 2 - 10,
					Location = new(10, StratInfoPanel.Height / 2 ),
					ForeColor = Color.White,
                    Font = new Font(UIManager.CustomFonts.Families[0], 12f, FontStyle.Bold),
                };
				StratInfoPanel.Controls.Add(upgradeGroupBox);


				if (gameSystem.playerSystem.player.Money < gameSystem.GetStartagemUpgradeCost(stratagemEntityBase.level))
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
                    Width = StratInfoPanel.Width / 2,
                    ForeColor = Color.White,
					Location = new(0, 55),
				};
				ShotSpeed.MinimumSize = ShotSpeed.Size;
				ShotSpeed.AutoSize = true;
                StratInfoPanel.Controls.Add(ShotSpeed);

                Label Damage = new()
                {
                    Text = $"Bullet Damage : {turretStratagem.bulletInfo.Damage}",
                    Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleRight,
                    ForeColor = Color.White,
                    Location = new(StratInfoPanel.Width / 2, 55),
                    Width = StratInfoPanel.Width / 2,
                };
                Damage.MinimumSize = Damage.Size;
                Damage.AutoSize = true;
                StratInfoPanel.Controls.Add(Damage);

                Label DPS = new()
				{
					Text = $"DPS : {Math.Round(turretStratagem.bulletInfo.Damage/(turretStratagem.ActionSpeed/1000d),3)} D/s",
					Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new(0, 90),
					Width = StratInfoPanel.Width / 2,
                    TextAlign = ContentAlignment.MiddleLeft,
                    AutoSize = true
                };
				StratInfoPanel.Controls.Add(DPS);

				Label Speed = new()
				{
					Text = $"Bullet Speed : {turretStratagem.bulletInfo.Speed}",
					Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleRight,
                    ForeColor = Color.White,
					Location = new(StratInfoPanel.Width / 2, 90),
					Width = StratInfoPanel.Width / 2,
				};
				StratInfoPanel.Controls.Add(Speed);

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
					BButton upgradeShootSpeed = new()
					{
                        Text = "Shoot Speed",
						Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
						ForeColor = Color.White,
                        Width = upgradeGroupBox.Width / 2 - 50,
                        Height = upgradeGroupBox.Height - 50,
						Location = new(25,25),
                    };
					upgradeShootSpeed.GotFocus += (s, e) => { 
						ShotSpeed.Text = $"Shooting speed : {turretStratagem.ActionSpeed / 1000d}s - {UpgradesValue.ActionSpeed/1000d}s"; 
						double dPS = turretStratagem.bulletInfo.Damage / (turretStratagem.ActionSpeed / 1000d); 
						double newDPS = Math.Round((turretStratagem.bulletInfo.Damage / (turretStratagem.ActionSpeed / 1000d - 0.1d)) - dPS, 2); 
						DPS.Text = $"DPS : {dPS} D/s + {newDPS} D/s"; 
					};
                    upgradeShootSpeed.LostFocus += (s, e) => { 
						ShotSpeed.Text = $"Shooting speed : {turretStratagem.ActionSpeed / 1000d}s";
						DPS.Text = $"DPS : {turretStratagem.bulletInfo.Damage / (turretStratagem.ActionSpeed / 1000d)} D/s";
					};
                    upgradeShootSpeed.Click += (s, e) => { gameSystem.UpgradeStratagem(turretStratagem, Upgrades.ActionSpeed); ShowStratInfo(stratagemEntityBase, true); };
                    upgradeGroupBox.Controls.Add(upgradeShootSpeed);

                    BButton upgradeDamage = new()
                    {
                        Text = "Damage",
                        Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                        ForeColor = Color.White,
                        Width = upgradeGroupBox.Width / 2 - 50,
                        Height = upgradeGroupBox.Height - 50,
                        Location = new(upgradeGroupBox.Width / 2 + 25, 25),
                    };
                    upgradeDamage.GotFocus += (s, e) => {
						Damage.Text = $"Bullet Damage : {turretStratagem.bulletInfo.Damage} + {UpgradesValue.BulletDamage}";
                        double dPS = turretStratagem.bulletInfo.Damage / (turretStratagem.ActionSpeed / 1000d);
                        double newDPS = ((turretStratagem.bulletInfo.Damage + UpgradesValue.BulletDamage) / (turretStratagem.ActionSpeed / 1000d)) - dPS;
                        DPS.Text = $"DPS : {Math.Round(dPS, 3)} D/s + {Math.Round(newDPS, 3)} D/s";
                    };
                    upgradeDamage.LostFocus += (s, e) => {
                        Damage.Text = $"Bullet Damage : {turretStratagem.bulletInfo.Damage}";
                        DPS.Text = $"DPS : {Math.Round(turretStratagem.bulletInfo.Damage / (turretStratagem.ActionSpeed / 1000d),3)} D/s";
                    };
					upgradeDamage.Click += (s, e) => { gameSystem.UpgradeStratagem(turretStratagem, Upgrades.BulletDamage); ShowStratInfo(stratagemEntityBase, true); };
                    upgradeGroupBox.Controls.Add(upgradeDamage);
                }

			}

			GamePanel.Controls.Add(StratInfoPanel);
			GamePanel.Controls.SetChildIndex(StratInfoPanel, 0);

		}

		internal void HideStratInfo()
		{
			GamePanel.Controls.Remove(StratInfoPanel);
			StratInfoPanel.Dispose();
			StratInfoPanel = null;
		}

		internal void HidePauseMenu()
		{
			GamePanel.Enabled = true;
			GameWindow.Controls.Remove(PauseMenu);
			PauseMenu?.Dispose();
		}

		internal void ShowPauseMenu()
		{
			GamePanel.Enabled = false;
			PauseMenu = new()
			{
				Width = 720,
				Height = 325,
				ForeColor = Color.White,
				BorderStyle = BorderStyle.FixedSingle,
			};
			PauseMenu.Location = new(GameWindow.Width / 2 - PauseMenu.Width / 2, GameWindow.Height / 2 - PauseMenu.Height / 2);

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
			gamePausedText.Location = new(PauseMenu.Width / 2 - gamePausedText.Width / 2, 25);

			BButton resumeButton = new()
			{
				Text = "Resume",
				Width = 300,
				Height = 50,
				ForeColor = Color.White,
				Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
			};
			resumeButton.Click += (s, e) => { gameSystem.UnPauseGame(); };
			resumeButton.Location = new(PauseMenu.Width / 2 - resumeButton.Width / 2, 100);

			BButton settingsButton = new()
			{
				Text = "Settings",
				Width = 300,
				Height = 50,
				ForeColor = Color.White,
				Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
			};
			settingsButton.Click += (s, e) => { gameSystem.UnPauseGame(); GameManager.instance.Load(GameStatus.MainMenu); };
			settingsButton.Location = new(PauseMenu.Width / 2 - settingsButton.Width / 2, 175);

			BButton mainMenuButton = new()
			{
				Text = "Main Menu",
				Width = 300,
				Height = 50,
				ForeColor = Color.White,
				Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
			};
			mainMenuButton.Click += (s, e) => { gameSystem.UnPauseGame(); GameManager.instance.Load(GameStatus.MainMenu); };
			mainMenuButton.Location = new(PauseMenu.Width / 2 - mainMenuButton.Width / 2, 250);

			PauseMenu.Controls.Add(gamePausedText);
			PauseMenu.Controls.Add(resumeButton);
			PauseMenu.Controls.Add(settingsButton);
			PauseMenu.Controls.Add(mainMenuButton);
			GameWindow.Controls.Add(PauseMenu);
			GameWindow.Controls.SetChildIndex(PauseMenu, 0);
		}

		internal void ShowEndGameMenu()
		{
			EndGameMenu = new()
			{
				Width = 1280,
				Height = 720,
				BorderStyle = BorderStyle.FixedSingle,
				ForeColor = Color.White,
			};
			EndGameMenu.Location = new Point(GameWindow.Width / 2 - EndGameMenu.Width / 2, GameWindow.Height /  2 - EndGameMenu.Height / 2);

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
			gameEndedText.Location = new(EndGameMenu.Width / 2 - gameEndedText.Width / 2, 25);

			Label Score = new()
			{
				Text = $"Score : {gameSystem.waveNum} 🌟",
				Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
				ForeColor = Color.White,
				//AutoSize = true,
				Width = 500,
				Height = 50,
				TextAlign = ContentAlignment.MiddleCenter,
				//BorderStyle = BorderStyle.FixedSingle,
			};
			Score.Location = new(EndGameMenu.Width / 4 - Score.Width / 2, 100);

			Label TotalEarnedMoney = new()
			{
				Text = $"Total Earned Money : {gameSystem.playerSystem.player.TotalMoney} 💲",
				Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
				ForeColor = Color.White,
				AutoSize = true,
				Width = 500,
				Height = 50,
				TextAlign = ContentAlignment.MiddleCenter,
				//BorderStyle = BorderStyle.FixedSingle,
			};
			TotalEarnedMoney.Location = new(EndGameMenu.Width / 4 * 3 - TotalEarnedMoney.Width / 2, 100);

			BButton tryAgainButton = new()
			{
				Text = "Try again",
				Width = 250,
				Height = 50,
				ForeColor = Color.White,
				Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
			};
			tryAgainButton.Click += (s, e) => { GameManager.DestroySystem<GameSystem>(); GameManager.instance.Load(GameStatus.PreGame); };
			tryAgainButton.Location = new(EndGameMenu.Width / 4 - tryAgainButton.Width / 2, EndGameMenu.Height - tryAgainButton.Height - 25);

			BButton settingsButton = new()
			{
				Text = "Settings",
				Width = 250,
				Height = 50,
				ForeColor = Color.White,
				Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
			};
			settingsButton.Click += (s, e) => { GameManager.instance.Load(GameStatus.MainMenu); };
			settingsButton.Location = new(EndGameMenu.Width / 2  - settingsButton.Width / 2, EndGameMenu.Height - tryAgainButton.Height - 25);

			BButton mainMenuButton = new()
			{
				Text = "Main Menu",
				Width = 250,
				Height = 50,
				ForeColor = Color.White,
				Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
			};
			mainMenuButton.Click += (s, e) => { GameManager.instance.Load(GameStatus.MainMenu); };
			mainMenuButton.Location = new(EndGameMenu.Width / 4 * 3 - mainMenuButton.Width / 2, EndGameMenu.Height - tryAgainButton.Height - 25);

			EndGameMenu.Controls.Add(gameEndedText);
			EndGameMenu.Controls.Add(Score);
			EndGameMenu.Controls.Add(TotalEarnedMoney);
			EndGameMenu.Controls.Add(tryAgainButton);
			EndGameMenu.Controls.Add(settingsButton);
			EndGameMenu.Controls.Add(mainMenuButton);
			GameWindow.Controls.Add(EndGameMenu);
			GameWindow.Controls.SetChildIndex(EndGameMenu, 0);

		}

        internal override void OnConnectionChange()
        {
            
        }

        internal override void OnDigitalChannelsChange(K8055.DigitalChannel digitalChannel)
        {
            
        }
    }
}
