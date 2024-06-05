using System.Drawing;

namespace K8055Velleman.Game.Entities.Stratagems
{
    internal class GatlingTurret : TurretStratagemBase
    {
        internal override BulletInfo BulletInfo => new() { Damage = 1, Color = Color.Yellow, Size = new(5, 5), Speed = 3 };

        internal override int UiID => 1;

        internal override bool Unlockable => true;

        internal override int UnkockPrice => 100;

        internal override Color Color => Color.Yellow;

        internal override string Name => "Gatling Turret";

        internal override int MaxLevel => 8;

        internal override int StartActionSpeed => 750;
    }
}
