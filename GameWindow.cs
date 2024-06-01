using System;
using System.Diagnostics;
using System.Windows.Forms;
using K8055Velleman.Game;

namespace K8055Velleman
{
    public partial class GameWindow : Form
    {
        private const int kClockInternal = 16;
        private readonly Stopwatch _stopwatch;
        public GameWindow()
        {
            InitializeComponent();
            SaveManager.LoadData();
            UIManager.Setup(this);
            AudioManager.Setup();
            AudioManager.PlaySound(AudioFile.BackGroundMusic, true);
            _stopwatch = new Stopwatch();
            this.Text = null;
            Clock.Enabled = true;
            Clock.Interval = 1;
        }

        private void OnClosingForm(object sender, FormClosingEventArgs e)
        {
            //K8055.CloseAllDevices();
            K8055.CloseDevice();
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
            _stopwatch.Reset();
            _stopwatch.Start();
            K8055.Update();
            GameManager.Update();
            _stopwatch.Stop();
            int i = kClockInternal - (int)_stopwatch.ElapsedMilliseconds;
            Clock.Interval = i <= 0 ? 1 : i;
            float x = 1 / ((_stopwatch.ElapsedMilliseconds + Clock.Interval) / 1000f);
            if (x < 60) Console.WriteLine(x);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            GameManager.Load(GameStatus.PlayerSelector);
        }
    }
}
