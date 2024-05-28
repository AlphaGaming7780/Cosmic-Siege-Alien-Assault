using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace K8055Velleman.Game.UI
{
    internal abstract class UIBase
    {
        /// <summary>
        /// The application main control.
        /// </summary>
        internal GameWindow GameWindow { get { return UIManager.GameWindow; } }
        //internal Float2 UIRatio { get { return UIManager.uiRatio; } }

        internal const int RightOffeset = 8; //16

        /// <summary>
        /// Called when the UI is created.
        /// </summary>
        internal virtual void OnCreate() {
            K8055.OnDigitalChannelsChange += OnDigitalChannelsChange;
            K8055.OnConnectionChanged += OnConnectionChange;
        }

        /// <summary>
        /// Called when the connection with the K8055 Velleman board change.
        /// </summary>
        internal abstract void OnConnectionChange();

        /// <summary>
        /// Called when one digital channel change to 1.
        /// </summary>
        /// <param name="digitalChannel">The digital channel that changed to 1.</param>
        internal abstract void OnDigitalChannelsChange(K8055.DigitalChannel digitalChannel);

        /// <summary>
        /// Called when the UI is destoyed.
        /// </summary>
        internal virtual void OnDestroy() 
        {
            K8055.OnConnectionChanged -= OnConnectionChange;
            K8055.OnDigitalChannelsChange -= OnDigitalChannelsChange;
            K8055.OnAnalogChannelsChange -= OnAnalogChannelsChange;
        }
        
        /// <summary>
        /// Setup the event for the analog channel.
        /// </summary>
        internal void SetupAnalogChannelEvent()
        {
            K8055.OnAnalogChannelsChange += OnAnalogChannelsChange;
        }

        /// <summary>
        /// Called when one analog channel value change.
        /// </summary>
        /// <param name="analogChannel">The analog channel that changed</param>
        /// <param name="value">The new value of this analog channel</param>
        internal virtual void OnAnalogChannelsChange(K8055.AnalogChannel analogChannel, int value) { }
    }
}
