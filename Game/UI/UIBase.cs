using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace K8055Velleman.Game.UI
{
    internal abstract class UIBase
    {
        internal GameWindow GameWindow { get { return UIManager.GameWindow; } }
        internal Float2 UIRatio { get { return UIManager.uiRatio; } }

        internal const int RightOffeset = 16;

        internal virtual void OnCreate() {
            K8055.OnDigitalChannelsChange += OnDigitalChannelsChange;
            K8055.OnConnectionChanged += OnConnectionChange;
        }
        internal abstract void OnConnectionChange();

        internal abstract void OnDigitalChannelsChange(K8055.DigitalChannel digitalChannel);

        internal virtual void OnDestroy() 
        {
            K8055.OnConnectionChanged -= OnConnectionChange;
            K8055.OnDigitalChannelsChange -= OnDigitalChannelsChange;
        }
        //internal abstract void OnResize();
    }
}
