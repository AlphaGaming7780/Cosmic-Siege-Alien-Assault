using K8055Velleman.Game.Entities;
using K8055Velleman.Game.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game;

public enum GameStatus
{
    MainMenu,
    Game,
    Paused,
    settings,
    unknown = 0,
}

internal class GameManager
{

    public static GameManager instance;

    private static readonly Dictionary<Type, SystemBase> Systems = [];

    internal GameStatus gameStatus = GameStatus.unknown;

    public static Random Random { get; private set; } = new();

    public GameManager() 
    {
        instance = this;
    }

    internal void Load(GameStatus gameStatus) 
    {
        switch(gameStatus)
        {
            case GameStatus.MainMenu:
                GetOrCreateSystem<MainMenuSystem>();
                DestroySystem<GameSystem>();
                break;
            case GameStatus.Game:
                GetOrCreateSystem<GameSystem>();
                DestroySystem<MainMenuSystem>();
                break;

        }
        this.gameStatus = gameStatus;
    }

    internal void Update()
    {
        List<SystemBase> temp = new(Systems.Values);
        foreach (SystemBase system in temp)
        {
            if(system.enabled) system.OnUpdate();
        }
    }

    internal static T GetSystem<T>() where T : SystemBase
    {
        if (Systems.ContainsKey(typeof(T))) return (T)Systems[typeof(T)];
        return null;
    }

    internal static T GetOrCreateSystem<T>() where T: SystemBase, new()
    {
        if (Systems.ContainsKey(typeof(T))) return (T)Systems[typeof(T)];
        T system = new();
        Systems.Add(typeof(T), system);
        system.OnCreate(); 
        return system;
    }

    internal static bool DestroySystem<T>() where T : SystemBase
    {
        if (!Systems.ContainsKey(typeof(T))) return false;
        Systems[typeof(T)].OnDestroy();
        Systems.Remove(typeof(T));
        return true;
    }

}
