using K8055Velleman.Game.Systems;
using System.Collections.Generic;
using System.Drawing;

namespace K8055Velleman.Game.Entities;

internal class AmmunitionEntity : MovingEntity
{
    //abstract internal int Damage { get; }

    //abstract internal Size BulletSize { get; }
    //abstract internal Color BulletColor { get; }

    //abstract internal bool Guided { get; }

    private float speed = 1;
    internal float Damage { get; private set; } = 1;
    internal Size BulletSize { get; private set; } = new Size(25,25);
    internal Color BulletColor { get; private set; } = Color.White;
    internal bool Guided { get; private set; } = false;

    internal override float Speed => speed;

    private PlayerSystem playerSystem;

	internal EntityBase target;

	internal override void OnCreate(EntitySystem entitySystem)
	{
        MainPanel = new()
        {
            Width = BulletSize.Width,
            Height = BulletSize.Height,
            BackColor = BulletColor,
        };
        base.OnCreate(entitySystem);
        enabled = false;
	}

	internal void Create(BulletInfo bulletInfo)
	{
        speed = bulletInfo.Speed;
        Damage = bulletInfo.Damage;
        BulletSize = bulletInfo.Size;
        BulletColor = bulletInfo.Color;
        Guided = bulletInfo.Guided;
        UpdateMainPanel();
        enabled = true;
        playerSystem = GameManager.GetOrCreateSystem<PlayerSystem>();
        CenterLocation = playerSystem.player.CenterLocation;
        GameUI.GamePanel.Controls.Add(MainPanel);
    }

    internal override void OnUpdate()
    {
        base.OnUpdate();
        if (!EntitySystem.EntityExists(this)) return;
        if (Guided) {
            if (target == null) GetNewTarget();
            else if (EntitySystem.EntityExists(target)) TaregtCenterLocation = target.CenterLocation;
            else target = null;
		}
		else if(TaregtCenterLocation == CenterLocation) EntitySystem.DestroyEntity(this);
    }

	internal override void OnCollide(EntityBase entityBase)
	{
		if(entityBase is EnemyEntity) EntitySystem.DestroyEntity(this);
	}

	private void GetNewTarget()
	{
        List<EnemyEntity> enemyEntities = EntitySystem.GetEntitiesByType<EnemyEntity>();
        if (enemyEntities.Count <= 0) { return; } //target = null;

        enemyEntities.Sort(delegate (EnemyEntity x, EnemyEntity y)
        {
            return (x.CenterLocation - this.CenterLocation).sqrMagnitude.CompareTo((y.CenterLocation - this.CenterLocation).sqrMagnitude);
        });

        foreach (EnemyEntity enemy in enemyEntities)
        {
            if (enemy.targeted) continue;
            target = enemy;
            break;
        }
        target ??= enemyEntities[0];
    }

    private void UpdateMainPanel()
    {
        MainPanel.Size = BulletSize;
        MainPanel.BackColor = BulletColor;
    }
}
