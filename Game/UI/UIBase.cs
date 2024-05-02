using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game.UI
{
    internal abstract class UIBase
    {
        internal GameWindow GameWindow { get { return UIManager.GameWindow; } }
        internal Float2 UIRatio { get { return UIManager.uiRatio; } }

        internal const int RightOffeset = 16;

        internal abstract void OnCreate();
        internal abstract void OnDestroy();
        //internal abstract void OnResize();
    }
}
