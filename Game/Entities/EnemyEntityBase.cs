using K8055Velleman.Game.Systems;
using System.Drawing;

namespace K8055Velleman.Game.Entities
{
    internal abstract class EnemyEntityBase : MovingEntity
    {

        private PlayerSystem playerSystem;

        internal float Health { get; set; }
        abstract internal int StartHealth { get; }
        abstract internal int Damage { get; }
        abstract internal int Cost { get; }
        abstract internal Size StartSize { get; }
        abstract internal Color StartColor { get; }

        internal bool targeted = false;

        internal override void OnCreate(EntitySystem entitySystem)
        {
            MainPanel = new()
            {
                Size = StartSize,
                BackColor = StartColor,

            };
            base.OnCreate(entitySystem);
            Health = StartHealth;
        }

        internal void Spawn()
        {
            CenterLocation = GetSpawnLocation();
            playerSystem = GameManager.GetSystem<PlayerSystem>();
            TaregtCenterLocation = playerSystem != null ? playerSystem.player.CenterLocation : new();

            GameUI.GamePanel.Controls.Add(MainPanel);
        }

        internal override void OnUpdate()
        {
            base.OnUpdate();
            if (TaregtCenterLocation == CenterLocation) EntitySystem.DestroyEntity(this);
        }

        private Vector2 GetSpawnLocation()
        {
            int spawnArea = GameManager.Random.Next(0, 4);
            return spawnArea switch
            {
                0 => new(GameManager.Random.Next(0, GameUI.GamePanel.Width + MainPanel.Width), GameManager.Random.Next(- 2 * MainPanel.Height, MainPanel.Height)),
                1 => new(GameManager.Random.Next(GameUI.GamePanel.Width, GameUI.GamePanel.Width + MainPanel.Width), GameManager.Random.Next(0, GameUI.GamePanel.Height + MainPanel.Height)),
                2 => new(GameManager.Random.Next(-MainPanel.Width, GameUI.GamePanel.Width), GameManager.Random.Next(GameUI.GamePanel.Height, GameUI.GamePanel.Height + MainPanel.Height)),
                3 => new(GameManager.Random.Next(- 2 * MainPanel.Width, MainPanel.Width), GameManager.Random.Next(-MainPanel.Height, GameUI.GamePanel.Height)),
                _ => new(),
            };
        }

        internal override void OnCollide(EntityBase entityBase)
        {
            if(entityBase is AmmunitionEntity ammunitionEntity)
            {
                Health -= ammunitionEntity.Damage;
                if (Health <= 0)
                {
                    playerSystem.PayPlayer(Cost);
                    AudioManager.PlaySound(AudioFile.EnemyDeath);
                    EntitySystem.DestroyEntity(this);
                }
            }
            else if(entityBase is PlayerEntity)
            {
                playerSystem.DamagePlayer(Damage);
                EntitySystem.DestroyEntity(this);
            }
        }
    }
}
