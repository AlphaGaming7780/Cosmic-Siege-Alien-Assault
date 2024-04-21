using K8055Velleman.Game.Entities;
using K8055Velleman.Game.Entities.Enemy;
using K8055Velleman.Game.UI;

namespace K8055Velleman.Game.Systems
{
	internal class GameSystem : SystemBase
	{
		EntitySystem entitySystem;
		//internal PlayerEnity player;
		PlayerSystem playerSystem;

		private int nbEnemy = 0;

		internal GameUI GameUI { get; private set; }
		internal PreGameUI PreGameUI { get; private set; }
		internal override void OnCreate()
		{
			base.OnCreate();
			//GameUI = UIManager.GetOrCreateUI<GameUI>();
			PreGameUI = UIManager.GetOrCreateUI<PreGameUI>();
			//playerSystem = GameManager.GetOrCreateSystem<PlayerSystem>();
			entitySystem = GameManager.GetOrCreateSystem<EntitySystem>();
        }

		internal override void OnDestroy()
		{
			base.OnDestroy();
			entitySystem = null;
			playerSystem = null;
            GameManager.DestroySystem<EntitySystem>();
            GameManager.DestroySystem<PlayerSystem>();
			GameUI = null;
			UIManager.DestroyUI<GameUI>();
		}

        internal override void OnUpdate()
        {
            base.OnUpdate();
			//if(nbEnemy < 5)
			//{
   //             entitySystem.CreateEntity<ClassicEnemyEntity>();
			//	nbEnemy++;
   //         }
        }
    }
}
