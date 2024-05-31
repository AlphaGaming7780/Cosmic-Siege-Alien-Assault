namespace K8055Velleman.Game.Interfaces
{
    internal enum Upgrades
    {
        ActionSpeed,
        BulletDamage,
    }

    internal struct UpgradesValue()
    {
        internal const int ActionSpeed = 100;
        internal const float BulletDamage = 2.5f;
    }

    //internal interface IUpgradableStratagemEntity
    //{
    //    abstract internal Upgrades FirstUpgrade { get; }
    //    //ref object FirstUpgradeProperty { get; }
    //    //object FirstUpgradeValue { get; }
    //    abstract internal Upgrades SecondeUpgrade { get; }

    //    // Idea
    //    //internal void Upgrade(Upgrades upgrades)
    //    //{
    //    //    if(upgrades == Upgrades.ActionSpeed && FirstUpgradeProperty is int i && FirstUpgradeValue is int y)
    //    //    {
    //    //        i += y;
    //    //    }
    //    //}

    //}
}
