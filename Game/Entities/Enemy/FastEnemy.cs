using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game.Entities.Enemy
{
    internal class FastEnemy : EnemyEntity
    {
        internal override int StartHealth => 10;

        internal override int Damage => 2;

        internal override int Cost => 2;
        internal override float Speed => 3f;

        internal override Size StartSize => new(10,10);

        internal override Color StartColor => Color.IndianRed;
    }
}
