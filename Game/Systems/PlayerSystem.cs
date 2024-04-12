using K8055Velleman.Game.Entities;
using K8055Velleman.Game.Entities.Stratagems;
using K8055Velleman.Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game.Systems
{
    internal class PlayerSystem : SystemBase
    {
        EntitySystem entitySystem;
        internal PlayerUI playerUI;
        internal PlayerEnity player;
        private DefaultTurret defaultTurret;

        internal override void OnCreate()
        {
            base.OnCreate();
            entitySystem = GameManager.GetOrCreateSystem<EntitySystem>();
            player = entitySystem.CreateEntity<PlayerEnity>();
            defaultTurret = entitySystem.CreateEntity<DefaultTurret>();
            defaultTurret.EnableStratagem();
            playerUI = UIManager.GetOrCreateUI<PlayerUI>();
            playerUI.PlayerLife.Text = $"❤️ : {player.Health}";
        }

        internal void PlayerHit(EnemyEntity enemyEntity)
        {
            player.Health -= enemyEntity.Damage;
            playerUI.PlayerLife.Text = $"❤️ : {player.Health}";
            if (player.Health <= 0) GameManager.instance.Load(GameStatus.MainMenu);
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();
            GameManager.DestroySystem<EntitySystem>();
            entitySystem.DestroyEntity(player);
            player = null;
            entitySystem.DestroyEntity(defaultTurret);
            defaultTurret = null;
            UIManager.DestroyUI<PlayerUI>();
        }

    }
}
