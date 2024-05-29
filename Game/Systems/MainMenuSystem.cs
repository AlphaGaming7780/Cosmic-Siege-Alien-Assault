using K8055Velleman.Game.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K8055Velleman.Game.Systems
{
	internal class MainMenuSystem : SystemBase
	{
        internal override void OnCreate()
        {
            base.OnCreate();
            enabled = false;
        }

        internal override void OnDestroy()
        {
            base.OnDestroy();
            UIManager.DestroyUI<MainMenuUI>();
        }

        internal override void OnGameStatusChange(GameStatus status)
        {
            base.OnGameStatusChange(status);
			switch (status)
			{
				case GameStatus.MainMenu:
                    UIManager.GetOrCreateUI<MainMenuUI>();
                    break;
				default:
					GameManager.DestroySystem<MainMenuSystem>();
					break;
			}
        }
	}
}
