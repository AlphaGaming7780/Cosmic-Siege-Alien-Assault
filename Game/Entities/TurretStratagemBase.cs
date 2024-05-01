using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game.Entities;

internal abstract class TurretStratagemBase : StratagemEntityBase
{
	internal abstract Type Ammo { get; }

	private EnemyEntity target = null, oldTarget = null;
	private int targetLife = 0;

	internal override void Action()
	{
		Shot();
	}

	private void GetTarget()
	{
		if (target == null || !EntitySystem.EntityExist(target))
		{
			List<EnemyEntity> enemyEntities = EntitySystem.GetEntitiesByType<EnemyEntity>();

			PlayerEnity playerEnity = EntitySystem.GetEntitiesByType<PlayerEnity>()[0];
			enemyEntities.Sort(delegate (EnemyEntity x, EnemyEntity y)
			{
				return (x.CenterLocation - playerEnity.CenterLocation).sqrMagnitude.CompareTo((y.CenterLocation - playerEnity.CenterLocation).sqrMagnitude);
			});
			if (enemyEntities.Count <= 0) { target = null; return; }

			foreach(EnemyEntity enemy in enemyEntities)
			{
				if (enemy.targeted || oldTarget == enemy) continue;
				target = enemy;
				break;
			}
			target ??= enemyEntities[GameManager.Random.Next(enemyEntities.Count - 1)];
			targetLife = target.Health;
			target.targeted = true;
		}

		return;
	}

	internal void Shot()
	{
		GetTarget();
		if (target == null) return;
		AmmunitionEntity ammunitionEntity = EntitySystem.CreateEntity<AmmunitionEntity>(Ammo);
		ammunitionEntity.target = target;
		ammunitionEntity.taregtCenterLocation = target.CenterLocation;
		targetLife -= ammunitionEntity.Damage;
		if (targetLife <= 0)
		{
			target.targeted = false;
			oldTarget = target;
			target = null;
		}
	}
}
