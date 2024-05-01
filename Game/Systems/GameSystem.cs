using K8055Velleman.Game.Entities;
using K8055Velleman.Game.Entities.Enemy;
using K8055Velleman.Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace K8055Velleman.Game.Systems
{
	internal class GameSystem : SystemBase
	{
		EntitySystem entitySystem;
		PlayerSystem playerSystem;
        List<StratagemEntityBase> selectedStratagemEntities = [];

		int waveMoneyBank = 0, waveNum = 0;
        //readonly List<EnemyEntity> enemyToGenerate = [];

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
			if(GameManager.instance.gameStatus == GameStatus.Game)
			{
                if (entitySystem.GetEntitiesByType<EnemyEntity>().Count <= 0)
                {
                    GenerateWave();
					waveNum++;
                }

            }
		}

		private void GenerateWave()
		{
			//enemyToGenerate.Clear();
			List<Type> types = Utility.GetAllSubclassOf(typeof(EnemyEntity)).ToList();
			int currentMoneyWave = waveMoneyBank += 2;
			while(currentMoneyWave > 0) 
			{
				EnemyEntity enemyEntity = entitySystem.CreateEntity<EnemyEntity>(types[GameManager.Random.Next(0, types.Count)]);
				if(currentMoneyWave - enemyEntity.Cost >= 0)
				{
					//enemyToGenerate.Add(enemyEntity);
					enemyEntity.Spawn();
					currentMoneyWave -= enemyEntity.Cost;
				}else
				{
					entitySystem.DestroyEntity(enemyEntity);
				}
			}
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
			entitySystem.GameUI.UpdateStratagemList(selectedStratagemEntities);
            playerSystem = GameManager.GetOrCreateSystem<PlayerSystem>();
            preGameUI = null;
			UIManager.DestroyUI<PreGameUI>();
			foreach (StratagemEntityBase entity in selectedStratagemEntities)
			{
				entity?.EnableStratagem();
			}
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
