using K8055Velleman.Game.Entities.Enemy;
using K8055Velleman.Game.Systems;
using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace K8055Velleman.Game.Entities.Stratagems
{
    internal class GuidedMissileLuncher : TurretStratagemBase
    {
        internal override string IconPath => "";

        internal override string Name => "Guided Missile Luncher";

        internal override int MaxLevel => 8;

        internal override int StartActionSpeed => 1000;

        //internal override Type Ammo => typeof(GuidedMissile);

        internal override Color Color => Color.DarkGray;

        internal override int UiID => 2;

        internal override bool Unlockable => true;

        internal override int UnkockPrice => 200;

        internal override BulletInfo BulletInfo => new() { Damage = 5, Speed = 4f, Size = new(10, 10), Color = Color.DarkGray, Guided = true };

        internal override void OnCollide(EntityBase entityBase) {}

        //internal override void OnCreate(EntitySystem entitySystem)
        //{
        //    base.OnCreate(entitySystem);
        //}
    }
}
