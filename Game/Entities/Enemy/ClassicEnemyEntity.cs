﻿using K8055Velleman.Game.Interface;
using K8055Velleman.Game.Systems;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game.Entities.Enemy
{
    internal class ClassicEnemyEntity : EnemyEntity
    {
        internal override int StartHealth => 10;

        internal override int Damage => 1;

        internal override float Speed => 1.5f;

        internal override void OnCreate(EntitySystem entitySystem)
        {
            
            mainPanel = new()
            {
                Size = new(25, 25),
                BackColor = Color.Red,

            };
            base.OnCreate(entitySystem); 
        }

        internal override void OnCollide(EntityBase entityBase)
        {
            base.OnCollide(entityBase);
        }

    }
}
