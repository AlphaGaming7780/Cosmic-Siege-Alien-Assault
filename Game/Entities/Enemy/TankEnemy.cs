using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game.Entities.Enemy
{
    internal class TankEnemy : EnemyEntity
    {
        internal override int StartHealth => 40;

        internal override int Damage => 4;

        internal override int Cost => 2;

        internal override float Speed => 0.75f;

        internal override Size StartSize => new(50,50);

        internal override Color StartColor => Color.DarkRed;
    }
}
