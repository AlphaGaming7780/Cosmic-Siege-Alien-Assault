using System.Collections.Generic;
using System.Windows.Forms;

namespace K8055Velleman.Game;

static internal class InputManager
{
    private static readonly List<Keys> pressedKeys = [];

    public delegate void onKeyDown(Keys key);
    public static event onKeyDown OnKeyDown;

    internal static void KeyDown(Keys key)
    {
        pressedKeys.Add(key);
        OnKeyDown?.Invoke(key);
    }

    internal static void KeyUp(Keys key)
    {
        pressedKeys.Remove(key);
    }

    public static bool IsKeyPressed(Keys key) { return pressedKeys.Contains(key); }

}
