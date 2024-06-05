using K8055Velleman.Game.UI;

namespace K8055Velleman.Game.Systems
{
    internal class PlayerSelectorSystem : SystemBase
    {
        internal override void OnCreate()
        {
            base.OnCreate();
            enabled = false;
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();
            UIManager.DestroyUI<PlayerSelectorUI>();
        }

        internal override void OnGameStatusChange(GameStatus status)
        {
            base.OnGameStatusChange(status);
            switch (status)
            {
                case GameStatus.PlayerSelector:
                    UIManager.GetOrCreateUI<PlayerSelectorUI>();
                    break;
                default:
                    GameManager.DestroySystem<PlayerSelectorSystem>();
                    break;
            }
        }
    }
}
