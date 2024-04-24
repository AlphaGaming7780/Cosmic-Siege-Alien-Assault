using K8055Velleman.Game.Entities;
using K8055Velleman.Game.Systems;
using System;
using System.Collections.Generic;
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

	Dictionary<string, StratagemEntity> stratagemEntities = [];

	internal override void OnCreate()
	{
		EntitySystem entitySystem = GameManager.GetOrCreateSystem<EntitySystem>();

		preGameUI = new()
		{
			Width = GameWindow.Width,
			Height = GameWindow.Height,
			BackColor = Color.Black,
		};

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

		foreach (Type t in Utility.GetAllSubclassOf(typeof(StratagemEntity)))
		{
            StratagemEntity stratagemEntity = entitySystem.CreateEntity<StratagemEntity>(t);

			stratagemEntity.Size = new Size(128, 128);
			stratagemEntity.Location = new Point( 2 * x + stratagemEntity.Size.Width * (x-1), 2 * y + stratagemEntity.Size.Height * (y - 1));
			stratagemList.Controls.Add(stratagemEntity.mainPanel);
			x++;
			if(x>4) { x = 1; y++; }
        }
			
		preGameUI.Controls.Add(stratagemList);
		GameWindow.Controls.Add(preGameUI);

	}

	internal override void OnDestroy()
	{
		
	}

	internal override void OnResize()
	{
		
	}
}
