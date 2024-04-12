using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K8055Velleman.Game.Systems;

internal class InputSystem : SystemBase
{
    private readonly List<Keys> pressedKeys = [];

    public delegate void OnKeyDown(Keys key);
    public event OnKeyDown onKeyDown;

    internal override void OnCreate()
    {
        base.OnCreate();
        enabled = false;
    }

    internal void KeyDown(Keys key)
    {
        pressedKeys.Add(key);
        onKeyDown?.Invoke(key);
    }

    internal void KeyUp(Keys key)
    {
        pressedKeys.Remove(key);
    }

    public bool IsKeyPressed(Keys key) { return pressedKeys.Contains(key); }




}
