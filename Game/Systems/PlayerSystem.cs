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
        EntitySystem _entitySystem;
        GameSystem _gameSystem;
        internal PlayerUI playerUI;
        internal PlayerEntity player;

        internal override void OnCreate()
        {
            base.OnCreate();
            enabled = false;
            _entitySystem = GameManager.GetOrCreateSystem<EntitySystem>();
            _gameSystem = GameManager.GetOrCreateSystem<GameSystem>();
            player = _entitySystem.CreateEntity<PlayerEntity>();
            playerUI = UIManager.GetOrCreateUI<PlayerUI>();
            playerUI.PlayerLife.Text = $"❤️ : {player.Health}";
            if (!_entitySystem.GameUI.IsStratInfoPanelShowed) _gameSystem.UpdateDigitalChannels(player.Health);
        }

        internal void DamagePlayer(int value)
        {
            player.Health -= value;
            playerUI.PlayerLife.Text = $"❤️ : {player.Health}";
            if (player.Health <= 0) GameManager.Load(GameStatus.EndGame);
            if(!_entitySystem.GameUI.IsStratInfoPanelShowed) _gameSystem.UpdateDigitalChannels(player.Health);
        }

        internal void PayPlayer(int value)
        {
            player.Money += value;
            player.TotalMoney += value;
            playerUI.PlayerMoney.Text = $"💲 : {player.Money}";
        }

        internal void IndebtedPlayer(int value)
        {
            player.Money -= value;
            playerUI.PlayerMoney.Text = $"💲 : {player.Money}";
        }   

        internal override void OnDestroy()
        {
            base.OnDestroy();
            GameManager.DestroySystem<EntitySystem>();
            _entitySystem.DestroyEntity(player);
            player = null;
            UIManager.DestroyUI<PlayerUI>();
        }

    }
}
