using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI
{
	internal class PlayerUI : UIBase
	{
		private GroupBox PlayerInfo;
		internal Label PlayerLife { get; private set; }
		internal Label PlayerMoney { get; private set; }

        GameUI gameUI;

		internal override void OnCreate()
		{

			gameUI = UIManager.GetOrCreateUI<GameUI>();

			PlayerInfo = new()
			{
				Text = "Player Informations",
                Location = new(10, 10),
                Font = new(UIManager.CustomFonts.Families[0], 10f, FontStyle.Bold),
                ForeColor = Color.White,
				AutoSize = true,
                BackColor = Color.Transparent,
            };

			PlayerLife = new()
			{
				Text = $"❤️ : ERROR",
				Location = new(19, 30),
				Height = 50,
				Font = new(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
				ForeColor = Color.Red,
				BackColor = Color.Transparent,
				AutoSize = true,
			};

			PlayerMoney = new()
			{
				Text = "💲 : 0",
                Location = new(10, 70),
                Font = new(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
                ForeColor = Color.Green,
                BackColor = Color.Transparent,
                AutoSize = true,
                Height = 50,
            };

			//foreach (FontFamily fontFamily in FontFamily.Families)
			//{
			//	Console.WriteLine(fontFamily.Name);
			//}
			PlayerInfo.Controls.Add(PlayerLife);
			PlayerInfo.Controls.Add(PlayerMoney);
            gameUI.GamePanel.Controls.Add(PlayerInfo);
		}

		internal override void OnDestroy()
		{
			gameUI.GamePanel.Controls.Remove(PlayerLife);
			PlayerLife.Dispose();
		}

        internal override void OnConnectionChange()
        {
			
        }

        internal override void OnDigitalChannelsChange(K8055.DigitalChannel digitalChannel)
        {
            
        }
    }
}
