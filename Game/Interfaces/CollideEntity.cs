using K8055Velleman.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game.Interfaces
{
    internal interface ICollideEntity
    {
        /// <summary>
        /// Called when the entity collide with another entity.
        /// </summary>
        /// <param name="entityBase">The entity that collide with this entity.</param>
        internal abstract void OnCollide(EntityBase entityBase);
    }
}
