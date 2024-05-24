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
		internal PlayerSystem playerSystem;
        List<StratagemEntityBase> selectedStratagemEntities = [];

		int waveMoneyBank = 0, waveMoneyPay = 1;
		internal int waveNum = 0;
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
			UIManager.DestroyUI<PauseUI>();
			UIManager.DestroyUI<EndGameUI>();
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
			foreach (StratagemEntityBase stratagemEntityBase in preGameUI.selectedStratagemEntities)
			{
				if(stratagemEntityBase is null) continue;
				selectedStratagemEntities.Add(entitySystem.CreateEntity<StratagemEntityBase>(stratagemEntityBase.GetType()));
				entitySystem.DestroyEntity(stratagemEntityBase);
			}
			entitySystem.GameUI = UIManager.GetOrCreateUI<GameUI>();
			entitySystem.GameUI.UpdateStratagemList(selectedStratagemEntities);
            playerSystem = GameManager.GetOrCreateSystem<PlayerSystem>();
            preGameUI = null;
			UIManager.DestroyUI<PreGameUI>();
            foreach (StratagemEntityBase entity in selectedStratagemEntities)
			{
                entity.EnableStratagem();
			}
        }

        private void OnKeyDown(Keys key)
        {
            switch (key)
			{
				case Keys.Escape:
					if (GameManager.instance.gameStatus == GameStatus.PreGame) { GameManager.instance.Load(GameStatus.MainMenu); break; }
					else if (GameManager.instance.gameStatus != GameStatus.Game) break;
					PauseLogique();
					break;
			}
        }

		internal void PauseLogique()
		{
            if (entitySystem.enabled) PauseGame();
            else UnPauseGame();
        }

        internal void PauseGame()
		{
			entitySystem.enabled = false;
            entitySystem.GameUI.GamePanel.Enabled = false;
			PauseUI pauseUI = UIManager.GetOrCreateUI<PauseUI>();
			pauseUI.gameSystem = this;
            foreach (StratagemEntityBase stratagemEntityBase in selectedStratagemEntities)
			{
				stratagemEntityBase?.PauseStratagem();
			}
        }
		internal void UnPauseGame()
		{
            entitySystem.enabled = true;
            entitySystem.GameUI.GamePanel.Enabled = true;
            UIManager.DestroyUI<PauseUI>();
            foreach (StratagemEntityBase stratagemEntityBase in selectedStratagemEntities)
            {
                stratagemEntityBase?.ResumeStratagem();
            }
        }

		private void SetupEndGame()
		{
            entitySystem.enabled = false;
            entitySystem.GameUI.GamePanel.Enabled = false;
            SaveManager.CurrentPlayerData.Money += playerSystem.player.TotalMoney;
			if(SaveManager.CurrentPlayerData.HigestScore < waveNum) SaveManager.CurrentPlayerData.HigestScore = waveNum;
			SaveManager.SaveCurrentPlayerData();
			UIManager.GetOrCreateUI<EndGameUI>();
			//GameManager.instance.Load(GameStatus.MainMenu);
		}

		internal int GetStartagemUpgradeCost(int level)
		{
			return (int)(10 * Math.Pow(2, level - 1));

        }

		internal void UpgradeStratagem(StratagemEntityBase stratagemEntityBase, Upgrades upgrades)
		{
            if(stratagemEntityBase.Upgrade(upgrades))
			{
				playerSystem.PayPlayer(-GetStartagemUpgradeCost(stratagemEntityBase.level - 1));
			}
        }
    }
}
