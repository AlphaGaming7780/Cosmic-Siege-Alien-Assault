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
		private GroupBox _playerInfo;
		internal Label PlayerLife { get; private set; }
		internal Label PlayerMoney { get; private set; }

        GameUI _gameUI;

		internal override void OnCreate()
		{

			_gameUI = UIManager.GetOrCreateUI<GameUI>();

			_playerInfo = new()
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
			_playerInfo.Controls.Add(PlayerLife);
			_playerInfo.Controls.Add(PlayerMoney);
            _gameUI.GamePanel.Controls.Add(_playerInfo);
		}

		internal override void OnDestroy()
		{
			_gameUI.GamePanel.Controls.Remove(PlayerLife);
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
