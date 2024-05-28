using K8055Velleman.Game.Entities;
using K8055Velleman.Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace K8055Velleman.Game.Systems
{
	internal class GameSystem : SystemBase
	{
		private EntitySystem _entitySystem;
        private List<StratagemEntityBase> _selectedStratagemEntities = [];

		internal int Scores = 0, WaveMoneyBank = 0, WaveMoneyPay = 1;
        internal PlayerSystem playerSystem;
        internal PreGameUI PreGameUI { get; private set; } = null;


		internal override void OnCreate()
		{
			base.OnCreate();
            //PreGameUI = UIManager.GetOrCreateUI<PreGameUI>();
            InputManager.OnKeyDown += OnKeyDown;
            _entitySystem = GameManager.GetOrCreateSystem<EntitySystem>();
        }

        internal override void OnDestroy()
		{
			base.OnDestroy();
            _entitySystem = null;
			playerSystem = null;
            GameManager.DestroySystem<EntitySystem>();
            GameManager.DestroySystem<PlayerSystem>();
			//gameUI = null;
			PreGameUI = null;
			UIManager.DestroyUI<GameUI>();
			UIManager.DestroyUI<PreGameUI>();
			UIManager.DestroyUI<PauseUI>();
			UIManager.DestroyUI<EndGameUI>();
			InputManager.OnKeyDown -= OnKeyDown;
		}

        internal override void OnUpdate()
        {
            base.OnUpdate();
			if(GameManager.GameStatus == GameStatus.Game)
			{
                if (_entitySystem.GetEntitiesByType<EnemyEntity>().Count <= 0)
                {
                    GenerateWave();
					Scores += WaveMoneyPay;
					_entitySystem.GameUI.Score.Text = $"{Scores} 🌟";
                }
            }
		}

		private void GenerateWave()
		{
			List<Type> types = Utility.GetAllSubclassOf(typeof(EnemyEntity)).ToList();
			List<EnemyEntity> enemyEntities = [];
			foreach (Type type in types)
			{
				enemyEntities.Add(_entitySystem.CreateEntity<EnemyEntity>(type));
			}
			int currentMoneyWave = WaveMoneyBank += WaveMoneyPay;
			while(currentMoneyWave > 0) 
			{
				EnemyEntity enemyType = enemyEntities[GameManager.Random.Next(0, enemyEntities.Count)];

                if (currentMoneyWave - enemyType.Cost >= 0)
				{
                    EnemyEntity enemyEntity = _entitySystem.CreateEntity<EnemyEntity>(enemyType.GetType());
                    enemyEntity.Spawn();
					currentMoneyWave -= enemyEntity.Cost;
				}
			}

			foreach(EnemyEntity enemyEntity in enemyEntities)
			{
				_entitySystem.DestroyEntity(enemyEntity);
			}

		}

        internal override void OnGameStatusChange(GameStatus status)
        {
            base.OnGameStatusChange(status);
			switch (status)
			{
				case GameStatus.PreGame:
                    PreGameUI = UIManager.GetOrCreateUI<PreGameUI>();
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
			foreach (StratagemEntityBase stratagemEntityBase in PreGameUI.selectedStratagemEntities)
			{
				if(stratagemEntityBase is null) continue;
				_selectedStratagemEntities.Add(_entitySystem.CreateEntity<StratagemEntityBase>(stratagemEntityBase.GetType()));
				_entitySystem.DestroyEntity(stratagemEntityBase);
			}
			_entitySystem.GameUI = UIManager.GetOrCreateUI<GameUI>();
			_entitySystem.GameUI.UpdateStratagemList(_selectedStratagemEntities);
            playerSystem = GameManager.GetOrCreateSystem<PlayerSystem>();
            PreGameUI = null;
			UIManager.DestroyUI<PreGameUI>();
            foreach (StratagemEntityBase entity in _selectedStratagemEntities)
			{
                entity.EnableStratagem();
			}
			Scores = WaveMoneyBank;
        }

        private void OnKeyDown(Keys key)
        {
            switch (key)
			{
				case Keys.Escape:
					if (GameManager.GameStatus == GameStatus.PreGame) { GameManager.Load(GameStatus.MainMenu); break; }
					else if (GameManager.GameStatus != GameStatus.Game) break;
					PauseLogique();
					break;
			}
        }

		internal void PauseLogique()
		{
            if (_entitySystem.enabled) PauseGame();
            else UnPauseGame();
        }

        internal void PauseGame()
		{
			_entitySystem.enabled = false;
            _entitySystem.GameUI.GamePanel.Enabled = false;
			PauseUI pauseUI = UIManager.GetOrCreateUI<PauseUI>();
			pauseUI.gameSystem = this;
            foreach (StratagemEntityBase stratagemEntityBase in _selectedStratagemEntities)
			{
				stratagemEntityBase?.PauseStratagem();
			}
        }
		internal void UnPauseGame()
		{
            _entitySystem.enabled = true;
            _entitySystem.GameUI.GamePanel.Enabled = true;
            UIManager.DestroyUI<PauseUI>();
            foreach (StratagemEntityBase stratagemEntityBase in _selectedStratagemEntities)
            {
                stratagemEntityBase?.ResumeStratagem();
            }
        }

		private void SetupEndGame()
		{
            _entitySystem.enabled = false;
            _entitySystem.GameUI.GamePanel.Enabled = false;
            SaveManager.CurrentPlayerData.Money += playerSystem.player.TotalMoney;
			if(SaveManager.CurrentPlayerData.HigestScore < Scores) SaveManager.CurrentPlayerData.HigestScore = Scores;
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
				playerSystem.RemoveMoneyFromPlayer(GetStartagemUpgradeCost(stratagemEntityBase.level - 1));
			}
        }

		internal void UpdateDigitalChannels(int value)
		{
            //K8055.ClearAllDigital();
            K8055.WriteAllDigital((int)Math.Pow(2, value) - 1);
        }
    }
}
