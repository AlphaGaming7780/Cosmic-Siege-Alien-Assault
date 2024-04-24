using K8055Velleman.Game.Systems;
using System.Collections.Generic;
using System.Windows.Forms;

namespace K8055Velleman.Game.Entities;

internal abstract class StratagemEntity : StaticEntity
{
	private int shootSpeed = 1;
	internal int level = 1;
	internal abstract string IconPath { get; }
	internal abstract string Name { get; }
	internal abstract int MaxLevel { get; }
	//internal abstract Type Ammo { get; }
	internal abstract int StartShootSpeed { get; }

	internal virtual int ShootSpeed { get { return shootSpeed; } set { shootSpeed = value; timer.Interval = value; } }

	private System.Timers.Timer timer;
	private EnemyEntity target = null;
	private int targetLife = 0;
	private bool shot = false;

	internal override void OnCreate(EntitySystem entitySystem)
	{
		mainPanel.Name = Name;
		base.OnCreate(entitySystem);
		timer = new System.Timers.Timer(shootSpeed);
		timer.Elapsed += (e, h) => { shot = true; };
		timer.AutoReset = true;
		ShootSpeed = StartShootSpeed;
	}

	internal override void OnUpdate()
	{
		if(shot)
		{
			shot = false;
			Shot();
		}
	}

	internal abstract void Shot();

	internal abstract void OnUpgrade(int newLevel);

	internal void EnableStratagem()
	{
		enabled = true;
		timer.Enabled = true;
	}

	internal void DisableStratagem()
	{
		enabled = false;
		timer.Enabled = false;
	}

	private EnemyEntity GetTarget()
	{
		if(target == null || !EntitySystem.EntityExist(target))
		{
			List<EnemyEntity> enemyEntities = EntitySystem.GetEntitiesByType<EnemyEntity>();
			if (enemyEntities.Count <= 0) return null;
			target = enemyEntities[GameManager.Random.Next(enemyEntities.Count -1)];
			targetLife = target.Health;
		}

		return target;
	}

	internal void ShotOnTarget(AmmunitionEntity ammunitionEntity)
	{
		EnemyEntity enemyEntity = GetTarget();
		if (enemyEntity == null)
		{
			EntitySystem.DestroyEntity(ammunitionEntity);
			return;
		}
		ammunitionEntity.target = enemyEntity;
		ammunitionEntity.taregtCenterLocation = enemyEntity.CenterLocation;
		targetLife -= ammunitionEntity.Damage;
		if (targetLife <= 0) target = null;
	}

}
