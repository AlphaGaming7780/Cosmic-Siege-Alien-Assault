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
		internal override string Name => "Missile Luncher";

		internal override int MaxLevel => 8;

		internal override int StartActionSpeed => 2000;

		internal override Color Color => Color.Gray;

		internal override int UiID => 1;

        internal override bool Unlockable => true;

        internal override int UnkockPrice => 100;

        internal override BulletInfo BulletInfo => new() { Damage = 10, Speed = 4f, Size = new(10, 10), Color = Color.DarkGray, Guided = false };

        internal override void OnCollide(EntityBase entityBase) {}
	}
}
