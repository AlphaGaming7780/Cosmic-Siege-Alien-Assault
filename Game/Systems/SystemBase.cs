using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game
{
    public class SystemBase
    {
        /// <summary>
        /// Control if the system should be updated each frame.
        /// </summary>
        internal bool enabled = true;

        /// <summary>
        /// Called when the system is created.
        /// </summary>
        internal virtual void OnCreate() { }

        /// <summary>
        /// Called when the system is destroyed.
        /// </summary>
        internal virtual void OnDestroy() { }

        /// <summary>
        /// Called when the system is Updated.
        /// </summary>
        internal virtual void OnUpdate() { }

        /// <summary>
        /// Called when the Game change of status.
        /// </summary>
        /// <param name="status">The new status</param>
        internal virtual void OnGameStatusChange(GameStatus status) { }
    }


}
