using K8055Velleman.Game.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K8055Velleman.Game.UI
{
	internal class GameUI : UIBase
	{
		internal Panel GamePanel { get; private set; }
		Panel selectedStratPanel;
		Label waveNumberLabel;
		internal override void OnCreate()
		{
			GamePanel = new()
			{
				Size = GameWindow.Size,
				Location = new(0, 0),
				BackColor = Color.Black,
			};

			selectedStratPanel = new()
			{
				Width = 522,
				Height = 132,
				Location = new Point(GameWindow.Width / 2 - 522 / 2, 900),
				BorderStyle = BorderStyle.FixedSingle,
				ForeColor = Color.White,
			};
			GamePanel.Controls.Add(selectedStratPanel);
			GameWindow.Controls.Add(GamePanel);
		}

		internal override void OnDestroy()
		{
			GameWindow.Controls.Remove(GamePanel);
			GamePanel.Dispose();
		}

		internal override void OnResize()
		{
			GamePanel.Size = GameWindow.Size;
		}

		internal void UpdateStratagemList(List<StratagemEntityBase> stratagemEntityBases)
		{
			int i = 0;
			foreach(StratagemEntityBase stratagemEntityBase in stratagemEntityBases) 
			{
				if (stratagemEntityBase == null) continue;
                stratagemEntityBase.mainPanel.Location = new Point(2 * (i + 1) + 130 * i, 1);
                selectedStratPanel.Controls.Add(stratagemEntityBase.mainPanel);
                selectedStratPanel.Controls.SetChildIndex(stratagemEntityBase.mainPanel, 0);
				stratagemEntityBase.mainPanel.Enabled = true;
				i++;
            }
        }
	}
}
