using K8055Velleman.Game.Systems;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace K8055Velleman.Game.Entities;

internal abstract class StratagemEntityBase : StaticEntity
{
	private int actionSpeed = 1;
	internal int level = 1;
	internal abstract string IconPath { get; }
	internal abstract string Name { get; }
	internal abstract int MaxLevel { get; }
	internal abstract int StartActionSpeed { get; }

	internal virtual int ActionSpeed { get { return actionSpeed; } set { actionSpeed = value; timer.Interval = value; } }

	private System.Timers.Timer timer;
    private bool action = false;

    internal override void OnCreate(EntitySystem entitySystem)
	{
		mainPanel.Name = Name;
		base.OnCreate(entitySystem);
		timer = new System.Timers.Timer(actionSpeed);
		timer.Elapsed += (e, h) => { action = true; };
		timer.AutoReset = true;
		ActionSpeed = StartActionSpeed;
	}

	internal override void OnUpdate()
	{
		if(action)
		{
			action = false;
            Action();
		}
	}

	internal abstract void Action();

	internal abstract void OnUpgrade(int newLevel);

	internal void EnableStratagem()
	{
		enabled = true;
		timer.Enabled = true;
	}

	internal void DisableStratagem()
	{
		enabled = false;
		timer.Enabled = false;
	}
}
