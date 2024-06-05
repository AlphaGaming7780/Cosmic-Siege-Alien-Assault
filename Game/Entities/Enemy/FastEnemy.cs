using System.Drawing;

namespace K8055Velleman.Game.Entities.Enemy
{
    internal class FastEnemy : EnemyEntityBase
    {
        internal override int StartHealth => 10;

        internal override int Damage => 2;

        internal override int Cost => 2;
        internal override float Speed => 3f;

        internal override Size StartSize => new(10,10);

        internal override Color StartColor => Color.IndianRed;
    }
}
