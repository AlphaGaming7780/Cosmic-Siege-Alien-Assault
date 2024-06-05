using K8055Velleman.Game.Systems;
using K8055Velleman.Game.Interfaces;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace K8055Velleman.Game.Entities;

internal abstract class TurretStratagemBase : StratagemEntityBase
{
    internal BulletInfo bulletInfo;

    internal abstract BulletInfo BulletInfo { get; }

    private EnemyEntityBase target = null, oldTarget = null;
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
        if (Level >= MaxLevel) return false;
        if (upgrade == Upgrades.BulletDamage)
        {
            bulletInfo.Damage += UpgradesValue.BulletDamage;
        }
        return base.Upgrade(upgrade);
    }

    private void GetTarget()
    {
        List<EnemyEntityBase> enemyEntities = EntitySystem.GetEntitiesByType<EnemyEntityBase>();

        PlayerEntity playerEnity = EntitySystem.GetEntitiesByType<PlayerEntity>()[0];
        enemyEntities.Sort(delegate (EnemyEntityBase x, EnemyEntityBase y)
        {
            return (x.CenterLocation - playerEnity.CenterLocation).sqrMagnitude.CompareTo((y.CenterLocation - playerEnity.CenterLocation).sqrMagnitude);
        });
        if (enemyEntities.Count <= 0) { target = null; return; }

        foreach(EnemyEntityBase enemy in enemyEntities)
        {
            if (enemy.targeted || oldTarget == enemy) continue;
            target = enemy;
            break;
        }
        target ??= enemyEntities[GameManager.Random.Next(enemyEntities.Count)];
        targetLife = target.Health;
        target.targeted = true;

        return;
    }

    internal void Shot()
    {
        if (!EntitySystem.EntityExists(target)) target = null;
        if (target == null) GetTarget();
        AmmunitionEntity ammunitionEntity = EntitySystem.CreateEntity<AmmunitionEntity>();
        ammunitionEntity.Create(bulletInfo);
        ammunitionEntity.target = target;
        ammunitionEntity.TaregtCenterLocation = target.CenterLocation;
        targetLife -= ammunitionEntity.Damage;
    }

    internal override void OnCollide(EntityBase entityBase) {}
}
internal struct BulletInfo()
{

    internal float Damage = 1;

    internal float Speed = 1;

    internal Size Size = new(25, 25);

    internal Color Color = Color.White;

    internal bool Guided = false;
}
