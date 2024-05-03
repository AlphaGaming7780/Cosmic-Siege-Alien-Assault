using K8055Velleman.Game.Systems;
using System.Collections.Generic;
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
            if (!EntitySystem.EntityExist(this)) return;
            if (Guided) {
                if(target == null ) EntitySystem.DestroyEntity(this);
                else if (EntitySystem.EntityExist(target)) taregtCenterLocation = target.CenterLocation;
				else GetNewTarget();
			}
			else if(taregtCenterLocation == CenterLocation) EntitySystem.DestroyEntity(this);
        }

		internal override void OnCollide(EntityBase entityBase)
		{
			if(entityBase is EnemyEntity) EntitySystem.DestroyEntity(this);
		}

		private void GetNewTarget()
		{
            List<EnemyEntity> enemyEntities = EntitySystem.GetEntitiesByType<EnemyEntity>();

            enemyEntities.Sort(delegate (EnemyEntity x, EnemyEntity y)
            {
                return (x.CenterLocation - this.CenterLocation).sqrMagnitude.CompareTo((y.CenterLocation - this.CenterLocation).sqrMagnitude);
            });
            if (enemyEntities.Count <= 0) { return; } //target = null;

            foreach (EnemyEntity enemy in enemyEntities)
            {
                if (enemy.targeted) continue;
                target = enemy;
                break;
            }
        }

	}
}
