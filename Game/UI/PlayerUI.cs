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
		internal Label PlayerLife { get; private set; }

		GameUI gameUI;

		internal override void OnCreate()
		{

			gameUI = UIManager.GetOrCreateUI<GameUI>();

			PlayerLife = new()
			{
				Text = $"❤️ : ERROR",
				Location = new(10, 10),
				Width = GameWindow.Width / 2,
				Height = 50,
				Font = new(UIManager.CustomFonts.Families[0], 20f, FontStyle.Bold),
				ForeColor = Color.Red,
				BackColor = Color.Transparent,
				AutoSize = true,
			};

			//foreach (FontFamily fontFamily in FontFamily.Families)
			//{
			//	Console.WriteLine(fontFamily.Name);
			//}

			gameUI.GamePanel.Controls.Add(PlayerLife);
		}

		internal override void OnDestroy()
		{
			gameUI.GamePanel.Controls.Remove(PlayerLife);
			PlayerLife.Dispose();
		}
	}
}
