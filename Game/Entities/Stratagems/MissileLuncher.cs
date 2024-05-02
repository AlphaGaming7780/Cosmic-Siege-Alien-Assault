﻿using K8055Velleman.Game.Entities.Ammunition;
using K8055Velleman.Game.Entities.Enemy;
using K8055Velleman.Game.Systems;
using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace K8055Velleman.Game.Entities.Stratagems
{
	internal class MissileLuncher : TurretStratagemBase
	{
		internal override string IconPath => "";

		internal override string Name => "Missile Luncher";

		internal override int MaxLevel => 8;

		internal override int StartActionSpeed => 1000;

        internal override Type Ammo => typeof(Missile);

		internal override Color Color => Color.Gray;

		internal override int UiID => 1;

        internal override bool Unlockable => true;

        internal override int UnkockPrice => 0;

        internal override void OnCollide(EntityBase entityBase) {}

		//internal override void OnCreate(EntitySystem entitySystem)
		//{
		//	base.OnCreate(entitySystem);
		//}

		internal override void OnUpgrade(int newLevel)
		{
			level = newLevel;
			ActionSpeed = StartActionSpeed - (level - 1) / (MaxLevel - 1) * 500;
		}
	}
}
