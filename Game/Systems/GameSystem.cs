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

		int waveMoneyBank = 0, waveNum = 0, waveMoneyPay = 2;
        //readonly List<EnemyEntity> enemyToGenerate = [];

        //internal GameUI gameUI { get; private set; } = null;
        internal PreGameUI preGameUI { get; private set; } = null;
		internal override void OnCreate()
		{
			base.OnCreate();
            //PreGameUI = UIManager.GetOrCreateUI<PreGameUI>();
            InputManager.OnKeyDown += OnKeyDown;
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
			InputManager.OnKeyDown -= OnKeyDown;
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
			List<Type> types = Utility.GetAllSubclassOf(typeof(EnemyEntity)).ToList();
			List<EnemyEntity> enemyEntities = [];
			foreach (Type type in types)
			{
				enemyEntities.Add(entitySystem.CreateEntity<EnemyEntity>(type));
			}
			int currentMoneyWave = waveMoneyBank += waveMoneyPay;
			while(currentMoneyWave > 0) 
			{
				EnemyEntity enemyType = enemyEntities[GameManager.Random.Next(0, enemyEntities.Count)];

                if (currentMoneyWave - enemyType.Cost >= 0)
				{
                    EnemyEntity enemyEntity = entitySystem.CreateEntity<EnemyEntity>(enemyType.GetType());
                    enemyEntity.Spawn();
					currentMoneyWave -= enemyEntity.Cost;
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
				case GameStatus.EndGame:
					SetupEndGame();
					break;
				default:
					GameManager.DestroySystem<GameSystem>();	
					break;
			}
        }

		private void SetupGame()
		{
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
					if (GameManager.instance.gameStatus == GameStatus.PreGame) { GameManager.instance.Load(GameStatus.MainMenu); break; }
					else if (GameManager.instance.gameStatus != GameStatus.Game) break;
                    if(GameWindow.Clock.Enabled) PauseGame();
					else UnPauseGame();
					break;
			}
        }

        internal void PauseGame()
		{
			GameWindow.Clock.Enabled = false;
            entitySystem.GameUI.ShowPauseMenu();
			foreach (StratagemEntityBase stratagemEntityBase in selectedStratagemEntities)
			{
				stratagemEntityBase?.PauseStratagem();
			}
        }
		internal void UnPauseGame()
		{
            GameWindow.Clock.Enabled = true;
            entitySystem.GameUI.HidePauseMenu();
            foreach (StratagemEntityBase stratagemEntityBase in selectedStratagemEntities)
            {
                stratagemEntityBase?.ResumeStratagem();
            }
        }

		private void SetupEndGame()
		{
			PauseGame();
			SaveManager.CurrentPlayerData.Money += playerSystem.player.TotalMoney;
			if(SaveManager.CurrentPlayerData.HigestScore < waveNum) SaveManager.CurrentPlayerData.HigestScore = waveNum;
			SaveManager.SaveCurrentPlayerData();
		}

    }
}
