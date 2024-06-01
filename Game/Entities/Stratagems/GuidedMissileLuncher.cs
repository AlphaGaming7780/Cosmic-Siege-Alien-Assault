using System.Drawing;

namespace K8055Velleman.Game.Entities.Stratagems
{
    internal class GuidedMissileLuncher : TurretStratagemBase
    {
        internal override string Name => "Guided Missile Luncher";

        internal override int MaxLevel => 8;

        internal override int StartActionSpeed => 2000;

        internal override Color Color => Color.DarkGray;

        internal override int UiID => 3;

        internal override bool Unlockable => true;

        internal override int UnkockPrice => 200;

        internal override BulletInfo BulletInfo => new() { Damage = 10, Speed = 5f, Size = new(10, 10), Color = Color.DarkGray, Guided = true };
    }
}
