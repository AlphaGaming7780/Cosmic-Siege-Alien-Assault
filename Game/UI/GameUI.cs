using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI
{
    internal class GameUI : UIBase
    {
        internal Panel GamePanel { get; private set; }
        internal override void OnCreate()
        {
            GamePanel = new()
            {
                Size = GameWindow.Size,
                Location = new(0, 0),
                BackColor = Color.Black,
            };
            GameWindow.Controls.Add(GamePanel);
        }

        internal override void OnDestroy()
        {
            GameWindow.Controls.Remove(GamePanel);
            GamePanel.Dispose();
        }

        internal override void OnResize()
        {
            GamePanel.Size = GameWindow.Size;
        }
    }
}
