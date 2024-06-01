using K8055Velleman.Game.Systems;
using K8055Velleman.Game.UI;
using K8055Velleman.Lib.ClassExtension;
using System.Drawing;
using System.Windows.Forms;

namespace K8055Velleman.Game.Entities
{
    internal abstract class EntityBase
    {
        private Vector2 _location;

        /// <summary>
        /// The location of the entity from is top right corner.
        /// </summary>
        internal Vector2 Location { get => _location; set { MainPanel.Location = value; _location = value; } }

        /// <summary>
        /// The location of the entity from its center.
        /// </summary>
        internal Vector2 CenterLocation { get => Location + Size.Divide(2); set => Location = value - Size.Divide(2); }

        /// <summary>
        /// The Size of the entity.
        /// </summary>
        internal Size Size { get => MainPanel.Size; set => MainPanel.Size = value; }

        /// <summary>
        /// The Control of the entity.
        /// </summary>
        internal Control MainPanel;

        /// <summary>
        /// The EntitySystem
        /// </summary>
        internal EntitySystem EntitySystem { get; private set; }

        /// <summary>
        /// The game UI.
        /// </summary>
        internal GameUI GameUI { get { return EntitySystem.GameUI; } }

        /// <summary>
        /// Controle if the entity should be updated.
        /// </summary>
        internal bool enabled = true;

        /// <summary>
        /// Called when the entity is created.
        /// </summary>
        /// <param name="entitySystem">The EntitySystem that created the entity.</param>
        internal virtual void OnCreate(EntitySystem entitySystem)
        {
            this.EntitySystem = entitySystem;
            _location = MainPanel.Location;
        }

        /// <summary>
        /// Called when the entity is destroyed.
        /// </summary>
        internal virtual void OnDestroy()
        {
            enabled = false;
            if (MainPanel != null)
            {
                MainPanel.Parent?.Controls.Remove(MainPanel);
                MainPanel.Dispose();
                MainPanel = null;
            }
        }

        /// <summary>
        /// Called when the entity is updated.
        /// </summary>
        internal abstract void OnUpdate();

        /// <summary>
        /// Called when the entity collide with another entity.
        /// </summary>
        /// <param name="entityBase">The entity that collide with this entity.</param>
        internal abstract void OnCollide(EntityBase entityBase);

        public static implicit operator Control(EntityBase entityBase)
        {
            return entityBase.MainPanel;
        }

    }
}
