using System;
using System.Windows.Forms;
using K8055Velleman.Game;

namespace K8055Velleman
{
	public partial class GameWindow : Form
	{
		private readonly GameManager gameManager;
		private readonly UIManager uiManager;
		public GameWindow()
		{
			InitializeComponent();
			SaveManager.LoadData();
			//this.ShowInTaskbar = false;
			//this.ControlBox = false;
			this.Text = null;
			gameManager = new();
			uiManager = new(this);
			Clock.Enabled = true;
        }

		private void OnClosingForm(object sender, FormClosingEventArgs e)
		{
			K8055.CloseAllDevices();
		}

        private void OnKeyDown(object sender, KeyEventArgs e)
		{
			InputManager.KeyDown(e.KeyCode);
		}

		private void OnKeyUp(object sender, KeyEventArgs e)
		{
            InputManager.KeyUp(e.KeyCode);
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
    }
}
