using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game.Entities;

internal abstract class TurretStratagemBase : StratagemEntityBase
{
	internal abstract Type Ammo { get; }

	private EnemyEntity target = null;
	private int targetLife = 0;

	internal override void Action()
	{
		Shot();
	}

	private EnemyEntity GetTarget()
	{
		if (target == null || !EntitySystem.EntityExist(target))
		{
			List<EnemyEntity> enemyEntities = EntitySystem.GetEntitiesByType<EnemyEntity>();
			if (enemyEntities.Count <= 0) return null;
			target = enemyEntities[GameManager.Random.Next(enemyEntities.Count - 1)];
			targetLife = target.Health;
		}

		return target;
	}

	internal void Shot()
	{
		EnemyEntity enemyEntity = GetTarget();
		if (enemyEntity == null) return;
		AmmunitionEntity ammunitionEntity = EntitySystem.CreateEntity<AmmunitionEntity>(Ammo);
		ammunitionEntity.target = enemyEntity;
		ammunitionEntity.taregtCenterLocation = enemyEntity.CenterLocation;
		targetLife -= ammunitionEntity.Damage;
		if (targetLife <= 0) target = null;
	}
}
