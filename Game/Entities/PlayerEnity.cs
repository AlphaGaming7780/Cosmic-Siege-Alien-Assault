using K8055Velleman.Game.Interface;
using K8055Velleman.Game.Systems;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K8055Velleman.Game.Entities
{
	internal class PlayerEnity : StaticEntity
	{
        internal int Health { get; set; }
        internal int Money { get; set; }
        internal int TotalMoney { get; set; }

        internal const int StartHealth = 8;

        internal override void OnCreate(EntitySystem entitySystem)
		{
            Health = StartHealth;
			mainPanel = new() //Panel
            {
				Size = new(50, 50),
				BackColor = Color.Green,

			};
            base.OnCreate(entitySystem);
            CenterLocation = new(GameUI.GamePanel.Width / 2, GameUI.GamePanel.Height / 2);

            GameUI.GamePanel.Controls.Add(mainPanel);
		}

		internal override void OnUpdate()
		{

		}

        internal override void OnResize()
        {
            base.OnResize();
			CenterLocation = new(GameUI.GamePanel.Width / 2, GameUI.GamePanel.Height / 2);
        }

        internal override void OnCollide(EntityBase entityBase)
        {
        }
    }
}
