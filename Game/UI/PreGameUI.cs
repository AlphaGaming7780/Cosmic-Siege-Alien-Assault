using K8055Velleman.Game.Entities;
using K8055Velleman.Game.Saves;
using K8055Velleman.Game.Systems;
using K8055Velleman.Lib;
using K8055Velleman.Lib.ClassExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI;

internal class PreGameUI : UIBase
{

	Control preGameUI;
	Label playerMoney;
	Control stratagemList;
	BButton backButton;
	BButton startGameButton;
	Panel selectedStratPanel;

	Panel StratagemInfo;

	EntitySystem entitySystem;


    Dictionary<string, StratagemEntityBase> stratagemEntities = [];
	internal List<StratagemEntityBase> selectedStratagemEntities = [null, null, null, null];
	List<Point> oldPos = [new(), new(), new(), new()];
	List<Control> slotSelectors = [];
	int currentStratagemIndex = 0;

	internal override void OnCreate()
	{
		entitySystem = GameManager.GetOrCreateSystem<EntitySystem>();

		preGameUI = new()
		{
			Width = GameWindow.Width,
			Height = GameWindow.Height,
			BackColor = Color.Black,
		};

		playerMoney = new()
		{
			Text = $"{SaveManager.CurrentPlayerData?.Money}💲",
			Top = 25,
            AutoSize = true,
			TextAlign = ContentAlignment.MiddleRight,
            Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
			BorderStyle = BorderStyle.FixedSingle,
            ForeColor = Color.WhiteSmoke,
        };
		playerMoney.Left = preGameUI.Width - playerMoney.Width;

        backButton = new()
		{
			Width = 300,
			Height = 100,
			Location = new Point(20, 900),
			Text = "Retour",
			ForeColor = Color.White,
			Font = new(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
		};
		backButton.Click += (sender, e) => { GameManager.instance.Load(GameStatus.MainMenu); };

		startGameButton = new()
		{
			Width = 300,
			Height = 100,
			Location = new Point(1600, 900),
			Text = "Jouer",
			ForeColor = Color.White,
			Font = new(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
		};
		startGameButton.Click += (sender, e) => { GameManager.instance.Load(GameStatus.Game); };

		stratagemList = new Panel()
		{
			Width = 522,
			Height = 768,
			Location = new Point(11,64),
			//BackColor = Color.White,
			BorderStyle = BorderStyle.FixedSingle,
			ForeColor = Color.White,
		};

		foreach (Type t in Utility.GetAllSubclassOf(typeof(StratagemEntityBase)))
		{
			if (t.IsAbstract) continue;
			StratagemEntityBase stratagemEntity = entitySystem.CreateEntity<StratagemEntityBase>(t);
			stratagemEntities.Add(stratagemEntity.Name, stratagemEntity);
			int y = (int)Math.Floor(stratagemEntity.UiID / 4f);
			int x = stratagemEntity.UiID - y * 4;
            stratagemEntity.Size = new Size(128, 128);
			stratagemEntity.Location = new Point( 2 * (x + 1) + stratagemEntity.Size.Width * x, 2 * (y + 1) + stratagemEntity.Size.Height * y);
			if(stratagemEntity.Unlockable && (!SaveManager.CurrentPlayerData.StratagemsData.TryGetValue(stratagemEntity.Name, out StratagemData stratagemData) || !stratagemData.Unlocked))
			{
                stratagemEntity.mainPanel.Click += UnlockStratagem;
			} else stratagemEntity.mainPanel.Click += SelectStratagem;
			stratagemEntity.mainPanel.MouseEnter += (sender, e) => { ShowStratagemInfo(stratagemEntity); };
			stratagemEntity.mainPanel.MouseLeave += (sender, e) => { HideStratagemInfo(); };
            stratagemList.Controls.Add(stratagemEntity.mainPanel);
		}

		selectedStratPanel = new()
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
				currentStratagemIndex = 0; 
				foreach (Control control in slotSelectors) 
				{ 
					if (sender == control) break; 
					currentStratagemIndex++; 
				}
				SelectSlot();
			};
			slotSelectors.Add(slotSelector);
			selectedStratPanel.Controls.Add(slotSelector);
		}
		SelectSlot();

		preGameUI.Controls.Add(backButton);
		preGameUI.Controls.Add(startGameButton);
		preGameUI.Controls.Add(selectedStratPanel);
		preGameUI.Controls.Add(stratagemList);
		preGameUI.Controls.Add(playerMoney);
		GameWindow.Controls.Add(preGameUI);

	}

    private void UnlockStratagem(object sender, EventArgs e)
    {
		Control c = sender as Control;
		UnlockStratagem(stratagemEntities[c.Name]);
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
		stratagemEntity.mainPanel.Click -= UnlockStratagem;
		stratagemEntity.mainPanel.Click += SelectStratagem;
		HideStratagemInfo();
		ShowStratagemInfo(stratagemEntity);
    }

    private void SelectStratagem(object sender, EventArgs e)
	{
		if (sender is not Control control) return;
		if (selectedStratagemEntities[currentStratagemIndex] != null)
		{
			StratagemEntityBase stratagemEntityBase = selectedStratagemEntities[currentStratagemIndex];
			stratagemEntityBase.mainPanel.Enabled = true;
			stratagemEntityBase.mainPanel.Location = oldPos[currentStratagemIndex];
			selectedStratPanel.Controls.Remove(stratagemEntityBase.mainPanel);
			stratagemList.Controls.Add(stratagemEntityBase.mainPanel);
		}
		selectedStratagemEntities[currentStratagemIndex] = stratagemEntities[control.Name];
		oldPos[currentStratagemIndex] = control.Location;
		stratagemList.Controls.Remove(control);
		control.Location = new Point(2 * (currentStratagemIndex + 1) + 130 * currentStratagemIndex, 1);
        selectedStratPanel.Controls.Add(control);
		selectedStratPanel.Controls.SetChildIndex(control, 0);
		control.Enabled = false;
		currentStratagemIndex++;
		SelectSlot();
	}

	private void SelectSlot()
	{
		foreach(Control slotSelector in slotSelectors)
		{
			slotSelector.BackColor = Color.Black;
		}
		slotSelectors[currentStratagemIndex].BackColor = Color.LightBlue;
	}

	private void ShowStratagemInfo(StratagemEntityBase stratagemEntityBase)
	{
		StratagemInfo = new()
		{
			Width = 600,
			Height = 768,
			Location = new Point(600, 64),
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
			Label ShotSpeed = new()
			{
				Text = $"Start shoot speed : {turretStratagem.StartActionSpeed}s",
                Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.White,
				AutoSize = true,
				Location = new(50,175),
            };
            StratagemInfo.Controls.Add(ShotSpeed);

			GroupBox AmmoInfo = new()
			{
				Text = "Ammunition Information",
				Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
				ForeColor = Color.White,
				Location = new(50, 225),
				Width = 500,
				MaximumSize = new(500, 600),
				AutoSize = true,
            };
			StratagemInfo.Controls.Add(AmmoInfo);

			AmmunitionEntity ammunitionEntity = entitySystem.CreateEntity<AmmunitionEntity>(turretStratagem.Ammo);
			ammunitionEntity.mainPanel.Location = new(50, 250);
			AmmoInfo.Controls.Add(ammunitionEntity.mainPanel);

			Label Damage = new()
			{
				Text = $"Damage : {ammunitionEntity.Damage}",
                Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new(50, 50),
                Width = 500,
                AutoSize = true,
            };
            AmmoInfo.Controls.Add(Damage);

            Label Speed = new()
            {
                Text = $"Speed : {ammunitionEntity.Speed}",
                Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new(50, 100),
                Width = 500,
                AutoSize = true,
            };
            AmmoInfo.Controls.Add(Speed);

            Label Size = new()
            {
                Text = $"Bullet Size : {ammunitionEntity.BulletSize.Width}",
                Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new(50, 150),
                Width = 500,
                AutoSize = true,
            };
            AmmoInfo.Controls.Add(Size);

            Label guided = new()
            {
                Text = $"Guided : {ammunitionEntity.Guided}",
                Font = new Font(UIManager.CustomFonts.Families[0], 15f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new(50, 200),
                Width = 500,
                AutoSize = true,
            };
            AmmoInfo.Controls.Add(guided);

        }

		
		if(stratagemEntityBase.Unlockable && (!SaveManager.CurrentPlayerData.StratagemsData.TryGetValue(stratagemEntityBase.Name, out StratagemData stratagemData) || !stratagemData.Unlocked))
		{
			Label locked = new()
			{
				Text = $"Unlock this stratagem for {stratagemEntityBase.UnkockPrice} $",
				Font = new Font(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
				TextAlign = ContentAlignment.MiddleCenter,
				ForeColor = Color.White,
				BorderStyle = BorderStyle.FixedSingle,
				Size = new Size(500, 50),
			};
			locked.Location = new Point(StratagemInfo.Width / 2 - locked.Width / 2, 650);
			StratagemInfo.Controls.Add(locked);
		}

        StratagemInfo.Controls.Add(stratName);

        preGameUI.Controls.Add(StratagemInfo);

    }

	private void HideStratagemInfo()
	{
		preGameUI.Controls.Remove(StratagemInfo);
        StratagemInfo.Dispose();
    }


    internal override void OnDestroy()
	{
		GameWindow.Controls.Remove(preGameUI);
		preGameUI.Dispose();
	}
}
