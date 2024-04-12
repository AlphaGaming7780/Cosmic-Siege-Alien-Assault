using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using K8055Velleman.Game;
using K8055Velleman.Game.Systems;
using K8055Velleman.Properties;

namespace K8055Velleman
{
	public partial class GameWindow : Form
	{
		private readonly GameManager gameManager;
		private readonly UIManager uiManager;
        private readonly InputSystem inputSystem;
		private readonly K8055Manager k8055Manager;
		public GameWindow()
		{
			InitializeComponent();
			gameManager = new();
			uiManager = new(this);
			k8055Manager = new();
            inputSystem = GameManager.GetOrCreateSystem<InputSystem>();
			Clock.Enabled = true;
		}

		private void OnClosingForm(object sender, FormClosingEventArgs e)
		{
			K8055.CloseAllDevices();
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			inputSystem.KeyDown(e.KeyCode);
		}

		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			inputSystem.KeyUp(e.KeyCode);
		}

		private void OnUpdate(object sender, EventArgs e)
		{
			K8055.Update();
            gameManager.Update();
		}

        private void OnLoad(object sender, EventArgs e)
        {
			gameManager.Load(GameStatus.MainMenu);
        }

        private void OnResize(object sender, EventArgs e)
        {
			uiManager?.OnResize();
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        public static void SuspendDrawing(Control ctrl)
        {
            SendMessage(ctrl.Handle, 11, false, 0);
        }


        public static void ResumeDrawing(Control ctrl)
        {
            SendMessage(ctrl.Handle, 11, true, 0);
            ctrl.Refresh();
        }
    }
}
