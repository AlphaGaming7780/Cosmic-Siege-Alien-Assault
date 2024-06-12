using K8055Velleman.Game.UI;
using K8055Velleman.Lib.ClassExtension;
using OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Windows.Forms;

namespace K8055Velleman.Game;

internal static class UIManager
{
    internal static GameWindow GameWindow;
    internal static Control MainContol;
    internal static PrivateFontCollection CustomFonts = new();
    private static readonly Dictionary<Type, UIBase> s_UIs = [];

    /// <summary>
    /// Setup the UIManager.
    /// </summary>
    /// <param name="gameWindow">The main game windows.</param>
    internal static void Setup(GameWindow mainWindow)
    {
        CustomFonts.AddFontFile("Resources\\PixeloidSans.ttf");
        GameWindow = mainWindow;
        MainContol = GameWindow;
    }

    internal static void Setup(GameWindow mainWindow, Control mainControl)
    {
        Setup(mainWindow);
        MainContol = mainControl;
        MainContol.Location = new(0, 0);
        MainContol.Size = GameWindow.Size;
    }

    /// <summary>
    /// Create the UI of the input type.
    /// </summary>
    /// <typeparam name="T">The type of the needed UI.</typeparam>
    /// <returns>The UI of type T.</returns>
    internal static T GetOrCreateUI<T>() where T : UIBase, new()
    {
        if (s_UIs.ContainsKey(typeof(T))) return (T)s_UIs[typeof(T)];
        T UI = new();
        UI.OnCreate();
        s_UIs.Add(typeof(T), UI);
        return UI;
    }

    /// <summary>
    /// Destory the UI of type T.
    /// </summary>
    /// <typeparam name="T">The type of the UI to destroy.</typeparam>
    /// <returns>True if the UI have been destroyed, false if it doesn't exist.</returns>
    internal static bool DestroyUI<T>() where T : UIBase, new()
    {
        if (!s_UIs.ContainsKey(typeof(T))) return false;
        s_UIs[typeof(T)].OnDestroy();
        s_UIs.Remove(typeof(T));
        return true;
    }

    /// <summary>
    /// Check if an UI exist.
    /// </summary>
    /// <typeparam name="T">The type of the UI to check.</typeparam>
    /// <returns>TRUE if the UI exist, otherwise FALSE.</returns>
    internal static bool UIExist<T>() where T : UIBase
    {
        return s_UIs.ContainsKey(typeof(T));
    }
}
