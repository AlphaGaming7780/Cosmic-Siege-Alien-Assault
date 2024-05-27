using K8055Velleman.Game.Systems;
using System.Collections.Generic;
using System.Drawing;

namespace K8055Velleman.Game.Entities;

internal abstract class TurretStratagemBase : StratagemEntityBase
{
	//internal abstract Type Ammo { get; }

	internal BulletInfo bulletInfo;

    internal abstract BulletInfo BulletInfo { get; }

    private EnemyEntity target = null, oldTarget = null;
	private float targetLife = 0;

    internal override void OnCreate(EntitySystem entitySystem)
    {
        base.OnCreate(entitySystem);
		bulletInfo = BulletInfo;
    }

    internal override void Action()
	{
		Shot();
	}

    internal override bool Upgrade(Upgrades upgrade)
    {
        if (level >= MaxLevel) return false;
        if (upgrade == Upgrades.BulletDamage)
		{
			bulletInfo.Damage += UpgradesValue.BulletDamage;
		}
        return base.Upgrade(upgrade);
    }

    private void GetTarget()
	{
        if (target == null || !EntitySystem.EntityExists(target))
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

        if (targetLife <= 0)
        {
            target.targeted = false;
            oldTarget = target;
            target = null;
        }

        return;
	}

	internal void Shot()
    {
        GetTarget();
        if (target == null || !EntitySystem.EntityExists(target)) return;
		AmmunitionEntity ammunitionEntity = EntitySystem.CreateEntity<AmmunitionEntity>();
		ammunitionEntity.Create(bulletInfo);
		ammunitionEntity.target = target;
		ammunitionEntity.TaregtCenterLocation = target.CenterLocation;
		targetLife -= ammunitionEntity.Damage;
    }
}
internal struct BulletInfo()
{

    internal float Damage = 1;

    internal float Speed = 1;

    internal Size Size = new(25, 25);

    internal Color Color = Color.White;

    internal bool Guided = false;
}
