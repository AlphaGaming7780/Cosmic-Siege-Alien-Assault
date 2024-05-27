using K8055Velleman.Game.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game;

internal class UIManager
{
    internal static GameWindow GameWindow;
    internal static PrivateFontCollection CustomFonts = new();
    private static readonly Dictionary<Type, UIBase> s_UIs = [];
    public UIManager(GameWindow gameWindow) 
    { 
        GameWindow = gameWindow;
        CustomFonts.AddFontFile("Resources\\PixeloidSans.ttf");
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
}
