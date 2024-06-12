using System;
using System.Diagnostics;
using System.Windows.Forms;
using K8055Velleman.Game;
using OpenGL;

namespace K8055Velleman
{
    public partial class GameWindow : Form
    {
        private const int kClockInternal = 16;
        private readonly Stopwatch _stopwatch;
        public GameWindow()
        {
            InitializeComponent();
            //this.Size = new(1920, 1080);
            glControl.Render += OnRender;
            glControl.ContextCreated += ContextCreated;
            glControl.Size = this.Size;
            glControl.Location = new(0, 0);
            glControl.Visible = true;

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
            glControl.Invalidate();
            _stopwatch.Stop();
            int i = kClockInternal - (int)_stopwatch.ElapsedMilliseconds;
            Clock.Interval = i <= 0 ? 1 : i;
            float x = 1 / ((_stopwatch.ElapsedMilliseconds + Clock.Interval) / 1000f);
            if (x < 60) Console.WriteLine(x);
        }

        private void ContextCreated(object sender, GlControlEventArgs e)
        {
            Control senderControl = (Control)sender;
            // Here you can allocate resources or initialize state
            Gl.MatrixMode(MatrixMode.Projection);
            //Gl.LoadIdentity();
            Gl.Ortho(0.0, 1920.0, 0.0, 1080.0, 0.0, 1.0);

            //Gl.MatrixMode(MatrixMode.Modelview);
            //Gl.LoadIdentity();
        }

        private void OnRender(object sender, GlControlEventArgs e)
        {
            Control senderControl = (Control)sender;

            Gl.Viewport(0, 0, senderControl.ClientSize.Width, senderControl.ClientSize.Height);
            Gl.Clear(ClearBufferMask.ColorBufferBit);

            Gl.Begin(PrimitiveType.Quads);
            Gl.Color3(1.0f, 0.0f, 0.0f); Gl.Vertex3(0.0, 0.0, 0.0);
            Gl.Color3(0.0f, 1.0f, 0.0f); Gl.Vertex3(720.0, 0.0, 0.0);
            Gl.Color3(0.0f, 0.0f, 1.0f); Gl.Vertex3(720.0, 720.0, 0.0);
            Gl.Color3(1.0f, 1.0f, 1.0f); Gl.Vertex3(0.0, 720.0, 0.0);
            Gl.End();

            //Gl.Flush();

            //Gl.Begin(PrimitiveType.Triangles);
            //Gl.Color3(1.0f, 0.0f, 0.0f); Gl.Vertex2(0.0f, 0.0f);
            //Gl.Color3(0.0f, 1.0f, 0.0f); Gl.Vertex2(0.5f, 1.0f);
            //Gl.Color3(0.0f, 0.0f, 1.0f); Gl.Vertex2(1.0f, 0.0f);
            //Gl.End();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            GameManager.Load(GameStatus.PlayerSelector);
        }
    }
}
