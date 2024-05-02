using K8055Velleman.Game.Systems;
using System.Drawing;

namespace K8055Velleman.Game.Entities
{
	internal abstract class AmmunitionEntity : MovingEntity
	{
		abstract internal int Damage { get; }

		abstract internal Size BulletSize { get; }
		abstract internal Color BulletColor { get; }

		abstract internal bool Guided { get; }

		private PlayerSystem playerSystem;

		internal EntityBase target;

		internal override void OnCreate(EntitySystem entitySystem)
		{
            mainPanel = new()
            {
                Width = (int)(BulletSize.Width * UIManager.uiRatio.x),
                Height = (int)(BulletSize.Height * UIManager.uiRatio.y),
                BackColor = BulletColor,
            };
            base.OnCreate(entitySystem);
            enabled = false;
		}

		internal void Create()
		{
            enabled = true;
            playerSystem = GameManager.GetOrCreateSystem<PlayerSystem>();
            CenterLocation = playerSystem.player.CenterLocation;
            GameUI.GamePanel.Controls.Add(mainPanel);
        }

        internal override void OnUpdate()
        {
            base.OnUpdate();
			if(Guided) {
				if (EntitySystem.EntityExist(target)) taregtCenterLocation = target.CenterLocation;
				//else EntitySystem.DestroyEntity(this);
			}
			if(taregtCenterLocation == CenterLocation) EntitySystem.DestroyEntity(this);
        }

		internal override void OnCollide(EntityBase entityBase)
		{
			if(entityBase is EnemyEntity) EntitySystem.DestroyEntity(this);
		}
	}
}
