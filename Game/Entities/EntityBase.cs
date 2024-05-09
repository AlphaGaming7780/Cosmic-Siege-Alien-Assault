using K8055Velleman.Game.Systems;
using K8055Velleman.Game.UI;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace K8055Velleman.Game.Entities
{
    internal abstract class EntityBase
    {
        private Vector2 location;
        internal Vector2 Location { get => location; set { mainPanel.Location = value; location = value; } }
        internal Vector2 CenterLocation { get => Location + Size.Divide(2); set => Location = value - Size.Divide(2); }
        internal Size Size { get => mainPanel.Size; set => mainPanel.Size = value; }

        internal Control mainPanel;
        internal EntitySystem EntitySystem { get; private set; }
        //internal GameWindow GameWindow { get { return EntitySystem.GameWindow; } }
        internal GameUI GameUI { get { return EntitySystem.GameUI; } }

        internal bool enabled = true;

        internal virtual void OnCreate(EntitySystem entitySystem)
        {
            this.EntitySystem = entitySystem;
            location = mainPanel.Location;
        }
        internal virtual void OnDestroy()
        {
            enabled = false;
            if (mainPanel != null)
            {
                mainPanel.Parent?.Controls.Remove(mainPanel);
                mainPanel.Dispose();
                mainPanel = null;
            }
        }
        internal abstract void OnUpdate();

        internal abstract void OnCollide(EntityBase entityBase);
        internal virtual void OnResize()
        {
            //Console.WriteLine(deltaRatio.x);
            //Console.WriteLine(deltaRatio.y);
            //mainPanel.Size = new((int)(mainPanel.Size.Width * deltaRatio.x), (int)(mainPanel.Size.Width * deltaRatio.y));
            //mainPanel.Location = new((int)(mainPanel.Location.X * deltaRatio.x), (int)(mainPanel.Location.Y * deltaRatio.y));
        }

    }
}
