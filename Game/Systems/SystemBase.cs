using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game
{
    public class SystemBase
    {
        internal bool enabled = true;
        internal GameWindow GameWindow { get {return UIManager.GameWindow;} }
        //public virtual void OnLoad() { }
        internal virtual void OnCreate() { }
        internal virtual void OnDestroy() { }
        internal virtual void OnUpdate() { }
        internal virtual void OnGameStatusChange(GameStatus status) { }
    }


}
