using System.Drawing;

namespace K8055Velleman.Game.Entities.Enemy;

internal class ClassicEnemyEntity : EnemyEntityBase
{
    internal override int StartHealth => 10;

    internal override int Damage => 1;

    internal override float Speed => 1.5f;

    internal override int Cost => 1;

    internal override Size StartSize => new(25,25);

    internal override Color StartColor => Color.Red;
}
