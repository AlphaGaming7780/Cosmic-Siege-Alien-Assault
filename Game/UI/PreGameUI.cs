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

	internal override void OnCreate()
	{
		EntitySystem entitySystem = GameManager.GetOrCreateSystem<EntitySystem>();

		preGameUI = new()
		{
			Width = GameWindow.Width,
			Height = GameWindow.Height,
			BackColor = Color.Black,
		};

		stratagemList = new()
		{
			Width = 512,
			Height = 1024,
			Location = new Point(16,16),
			BackColor = Color.White,
		};

		foreach (Type t in Utility.GetAllSubclassOf(typeof(StratagemEntity)))
		{
            StratagemEntity stratagemEntity = entitySystem.CreateEntity<StratagemEntity>(t);



			stratagemList.Controls.Add(stratagemEntity.Control);
        }

		preGameUI.Controls.Add(stratagemList);
		GameWindow.Controls.Add(preGameUI);

	}

	internal override void OnDestroy()
	{
		throw new NotImplementedException();
	}

	internal override void OnResize()
	{
		throw new NotImplementedException();
	}
}
