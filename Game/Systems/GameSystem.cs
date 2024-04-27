using K8055Velleman.Game.Entities;
using K8055Velleman.Game.Entities.Enemy;
using K8055Velleman.Game.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace K8055Velleman.Game.Systems
{
	internal class GameSystem : SystemBase
	{
		EntitySystem entitySystem;
		//internal PlayerEnity player;
		PlayerSystem playerSystem;

        List<StratagemEntityBase> selectedStratagemEntities = [];

        private int nbEnemy = 0;

		//internal GameUI gameUI { get; private set; } = null;
		internal PreGameUI preGameUI { get; private set; } = null;
		internal override void OnCreate()
		{
			base.OnCreate();
			//PreGameUI = UIManager.GetOrCreateUI<PreGameUI>();
			
			entitySystem = GameManager.GetOrCreateSystem<EntitySystem>();
        }

		internal override void OnDestroy()
		{
			base.OnDestroy();
			entitySystem = null;
			playerSystem = null;
            GameManager.DestroySystem<EntitySystem>();
            GameManager.DestroySystem<PlayerSystem>();
			//gameUI = null;
			preGameUI = null;
			UIManager.DestroyUI<GameUI>();
			UIManager.DestroyUI<PreGameUI>();
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

        internal override void OnGameStatusChange(GameStatus status)
        {
            base.OnGameStatusChange(status);
			switch (status)
			{
				case GameStatus.PreGame:
                    preGameUI = UIManager.GetOrCreateUI<PreGameUI>();
                    break;
				case GameStatus.Game:
					SetupGame();
                    break;
				default:
					GameManager.DestroySystem<GameSystem>();	
					break;
			}
        }

		private void SetupGame()
		{
			InputManager.OnKeyDown += OnKeyDown;
			selectedStratagemEntities = preGameUI.selectedStratagemEntities;
			entitySystem.GameUI = UIManager.GetOrCreateUI<GameUI>();
            playerSystem = GameManager.GetOrCreateSystem<PlayerSystem>();
            preGameUI = null;
			UIManager.DestroyUI<PreGameUI>();
        }

        private void OnKeyDown(Keys key)
        {
            switch (key)
			{
				case Keys.Escape:
                    if(GameWindow.Clock.Enabled) PauseGame();
					else UnPauseGame();
					break;
			}
        }

        internal void PauseGame()
		{
			GameWindow.Clock.Enabled = false;
        }
		internal void UnPauseGame()
		{
            GameWindow.Clock.Enabled = true;
        }

    }
}
