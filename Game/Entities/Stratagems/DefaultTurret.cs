using K8055Velleman.Game.Entities.Ammunition;
using K8055Velleman.Game.Entities.Enemy;
using K8055Velleman.Game.Systems;
using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace K8055Velleman.Game.Entities.Stratagems
{
    internal class DefaultTurret : StratagemEntity
    {
        internal override string IconPath => "";

        internal override string Name => "Default Turret";

        internal override int MaxLevel => 8;

        internal override int StartShootSpeed => 1000;

        internal override Control Control => new() { BackColor = Color.Orange };

        internal override void OnCollide(EntityBase entityBase) {}

        internal override void OnCreate(EntitySystem entitySystem)
        {
            base.OnCreate(entitySystem);
        }

        internal override void Shot()
        {
            Console.WriteLine("Shot");
            ShotOnTarget(EntitySystem.CreateEntity<DefaultTurretAmmo>());
        }

        internal override void OnUpgrade(int newLevel)
        {
            level = newLevel;
            ShootSpeed = StartShootSpeed - (level - 1) / (MaxLevel - 1) * 500;
        }
    }
}
