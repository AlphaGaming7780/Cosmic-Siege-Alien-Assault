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
    internal static UIManager instance;
    internal static GameWindow GameWindow;
    internal static PrivateFontCollection CustomFonts = new();
    private static readonly Dictionary<Type, UIBase> UIs = [];
    private static Size initialWindowSize = new(1280,720);
    internal static Float2 uiRatio { get { return (Float2)GameWindow.Size / (Float2)initialWindowSize; } }
    //internal Float2 dynamicUIRatio;
    //private Float2 OldWindowSize;
    public UIManager(GameWindow gameWindow) 
    { 
        instance = this;
        GameWindow = gameWindow;
        //initialWindowSize = GameWindow.Size;
        //OldWindowSize = GameWindow.Size;
        CustomFonts.AddFontFile("Resources\\PixeloidSans.ttf");
    }

    //internal void OnResize()
    //{   
    //    //dynamicUIRatio = GameWindow.Size/OldWindowSize;
    //    //OldWindowSize = GameWindow.Size;
    //    foreach (UIBase UI in UIs.Values)
    //    {
    //        UI.OnResize();
    //    }
    //}

    internal static T GetOrCreateUI<T>() where T : UIBase, new()
    {
        if (UIs.ContainsKey(typeof(T))) return (T)UIs[typeof(T)];
        T UI = new();
        UI.OnCreate();
        UIs.Add(typeof(T), UI);
        return UI;
    }

    internal static bool DestroyUI<T>() where T : UIBase, new()
    {
        if (!UIs.ContainsKey(typeof(T))) return false;
        UIs[typeof(T)].OnDestroy();
        UIs.Remove(typeof(T));
        return true;
    }
}
