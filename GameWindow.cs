using System;
using System.Diagnostics;
using System.Windows.Forms;
using K8055Velleman.Game;

namespace K8055Velleman
{
    public partial class GameWindow : Form
	{
		private const int kClockInternal = 16;
        private readonly GameManager gameManager;
		private readonly UIManager uiManager;
		private readonly Stopwatch stopwatch;
		public GameWindow()
		{
			InitializeComponent();
			stopwatch = new Stopwatch();

            SaveManager.LoadData();
			//this.ShowInTaskbar = false;
			//this.ControlBox = false;
			this.Text = null;
			gameManager = new();
			uiManager = new(this);
			Clock.Enabled = true;
			Clock.Interval = 16;
			AudioManager.PlaySound(AudioFile.BackGroundMusic, true);
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
			stopwatch.Reset();
			stopwatch.Start();
			K8055.Update();
            gameManager.Update();
			stopwatch.Stop();
			int i = kClockInternal - (int)stopwatch.ElapsedMilliseconds;
			Clock.Interval = i <= 0 ? 1 : i;
			float x = 1 / ((stopwatch.ElapsedMilliseconds + Clock.Interval + 0f) / 1000f);
            if(x < 60) Console.WriteLine(x);
		}

        private void OnLoad(object sender, EventArgs e)
        {
			gameManager.Load(GameStatus.PlayerSelector);
        }

   //     private void OnResize(object sender, EventArgs e)
   //     {
			//uiManager?.OnResize();
   //     }
    }
}
