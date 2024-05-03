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

        internal override void OnCreate()
        {
            base.OnCreate();
            entitySystem = GameManager.GetOrCreateSystem<EntitySystem>();
            player = entitySystem.CreateEntity<PlayerEnity>();
            playerUI = UIManager.GetOrCreateUI<PlayerUI>();
            playerUI.PlayerLife.Text = $"❤️ : {player.Health}";
        }

        internal void DamagePlayer(int value)
        {
            player.Health -= value;
            playerUI.PlayerLife.Text = $"❤️ : {player.Health}";
            if (player.Health <= 0) GameManager.instance.Load(GameStatus.MainMenu);
        }

        internal void PayPlayer(int value)
        {
            player.Money += value;
            player.TotalMoney += value;
            playerUI.PlayerMoney.Text = $"💲 : {player.Money}";
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();
            GameManager.DestroySystem<EntitySystem>();
            entitySystem.DestroyEntity(player);
            player = null;
            UIManager.DestroyUI<PlayerUI>();
        }

    }
}
