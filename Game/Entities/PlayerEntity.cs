using K8055Velleman.Game.Systems;
using System.Drawing;

namespace K8055Velleman.Game.Entities;

internal class PlayerEntity : StaticEntity
{
    internal int Health { get; set; }
    internal int Money { get; set; }
    internal int TotalMoney { get; set; }

    internal const int StartHealth = 8;

    internal override void OnCreate(EntitySystem entitySystem)
    {
        Health = StartHealth;
        MainPanel = new()
        {
            Size = new(50, 50),
            BackColor = Color.Green,

        };
        base.OnCreate(entitySystem);
        CenterLocation = new(GameUI.GamePanel.Width / 2, GameUI.GamePanel.Height / 2);

        GameUI.GamePanel.Controls.Add(MainPanel);
    }

    internal override void OnUpdate()
    {

    }

    internal override void OnCollide(EntityBase entityBase)
    {
    }
}
