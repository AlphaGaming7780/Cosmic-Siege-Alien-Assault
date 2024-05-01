using K8055Velleman.Game.Systems;
using System.Threading;

namespace K8055Velleman.Game.Entities
{
    internal abstract class EnemyEntity : MovingEntity
    {

        private PlayerSystem playerSystem;

        internal int Health { get; set; }
        abstract internal int StartHealth { get; }
        abstract internal int Damage { get; }
        abstract internal int Cost { get; }

        internal bool targeted = false;

        internal override void OnCreate(EntitySystem entitySystem)
        {
            base.OnCreate(entitySystem);
            Health = StartHealth;
        }

        internal void Spawn()
        {
            CenterLocation = GetSpawnLocation();
            playerSystem = GameManager.GetSystem<PlayerSystem>();
            taregtCenterLocation = playerSystem != null ? playerSystem.player.CenterLocation : new();

            GameUI.GamePanel.Controls.Add(mainPanel);
        }

        internal override void OnUpdate()
        {
            base.OnUpdate();
            if (taregtCenterLocation == CenterLocation) EntitySystem.DestroyEntity(this);
        }

        private Vector2 GetSpawnLocation()
        {
            int spawnArea = GameManager.Random.Next(0, 4);
            return spawnArea switch
            {
                0 => new(GameManager.Random.Next(0, GameUI.GamePanel.Width + mainPanel.Width), GameManager.Random.Next(- mainPanel.Height, 0)),
                1 => new(GameManager.Random.Next(GameUI.GamePanel.Width, GameUI.GamePanel.Width + mainPanel.Width), GameManager.Random.Next(0, GameUI.GamePanel.Height + mainPanel.Height)),
                2 => new(GameManager.Random.Next(-mainPanel.Width, GameUI.GamePanel.Width), GameManager.Random.Next(GameUI.GamePanel.Height, GameUI.GamePanel.Height + mainPanel.Height)),
                3 => new(GameManager.Random.Next(-mainPanel.Width, 0), GameManager.Random.Next(-mainPanel.Height, GameUI.GamePanel.Height)),
                _ => new(),
            };
        }

        internal override void OnCollide(EntityBase entityBase)
        {
            if(entityBase is AmmunitionEntity ammunitionEntity)
            {
                Health -= ammunitionEntity.Damage;
                if (Health <= 0) EntitySystem.DestroyEntity(this);
            }
            if(entityBase is PlayerEnity)
            {
                playerSystem.PlayerHit(this);
                EntitySystem.DestroyEntity(this);
            }
        }
    }
}
