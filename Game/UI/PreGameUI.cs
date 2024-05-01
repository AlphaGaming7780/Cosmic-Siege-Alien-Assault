using K8055Velleman.Game.Entities;
using K8055Velleman.Game.Systems;
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
	Control stratagemList;
	BButton backButton;
	BButton startGameButton;
	Panel selectedStratPanel;

	Dictionary<string, StratagemEntityBase> stratagemEntities = [];
	internal List<StratagemEntityBase> selectedStratagemEntities = [null, null, null, null];
	List<Point> oldPos = [new(), new(), new(), new()];
	List<Control> slotSelectors = [];
	int currentStratagemIndex = 0;

	internal override void OnCreate()
	{
		EntitySystem entitySystem = GameManager.GetOrCreateSystem<EntitySystem>();

		preGameUI = new()
		{
			Width = GameWindow.Width,
			Height = GameWindow.Height,
			BackColor = Color.Black,
		};

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
			Location = new Point(1500, 900),
			Text = "Jouer",
			ForeColor = Color.White,
			Font = new(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
		};
		startGameButton.Click += (sender, e) => { GameManager.instance.Load(GameStatus.Game); };

		stratagemList = new Panel()
		{
			Width = 522,
			Height = 720,
			Location = new Point(11,16),
			//BackColor = Color.White,
			BorderStyle = BorderStyle.FixedSingle,
			ForeColor = Color.White,
		};

		int x = 1, y = 1;

		foreach (Type t in Utility.GetAllSubclassOf(typeof(StratagemEntityBase)))
		{
			if (t.IsAbstract) continue;
			StratagemEntityBase stratagemEntity = entitySystem.CreateEntity<StratagemEntityBase>(t);
			stratagemEntities.Add(stratagemEntity.Name, stratagemEntity);
			stratagemEntity.Size = new Size(128, 128);
			stratagemEntity.Location = new Point( 2 * x + stratagemEntity.Size.Width * (x-1), 2 * y + stratagemEntity.Size.Height * (y - 1));
			stratagemEntity.mainPanel.Click += SelectStratagem;
			stratagemList.Controls.Add(stratagemEntity.mainPanel);
			x++;
			if(x>4) { x = 1; y++; }
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
		GameWindow.Controls.Add(preGameUI);

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

	internal override void OnDestroy()
	{
		GameWindow.Controls.Remove(preGameUI);
		preGameUI.Dispose();
	}

	internal override void OnResize()
	{
		
	}
}
