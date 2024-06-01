using K8055Velleman.Game.Entities.Enemy;
using K8055Velleman.Game.Systems;
using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace K8055Velleman.Game.Entities.Stratagems
{
    internal class DefaultTurret : TurretStratagemBase
    {
        internal override string Name => "Default Turret";

        internal override int MaxLevel => 8;

        internal override int StartActionSpeed => 1500;

        internal override Color Color => Color.Orange;

        internal override int UiID => 0;

        internal override bool Unlockable => false;

        internal override int UnkockPrice => 0;

        internal override BulletInfo BulletInfo => new() { Damage = 5, Color = Color.Orange, Size = new(25,25), Speed = 2 };
    }
}
