using K8055Velleman.Game.Entities;
using K8055Velleman.Game.Systems;
using K8055Velleman.Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game;

public enum GameStatus
{
    unknown,
    PlayerSelector,
    MainMenu,
    PreGame,
    Game,
    EndGame,
    Settings,
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
        this.gameStatus = gameStatus;
        switch (gameStatus)
        {
            case GameStatus.PlayerSelector:
                GetOrCreateSystem<PlayerSelectorSystem>();
                break;
            case GameStatus.MainMenu:
                GetOrCreateSystem<MainMenuSystem>();
                break;
            case GameStatus.PreGame:
                GetOrCreateSystem<GameSystem>();
                break;

        }
        foreach (SystemBase system in new List<SystemBase>(Systems.Values))
        {
            system.OnGameStatusChange(gameStatus);
        }
    }

    internal void Update()
    {
        List<SystemBase> temp = new(Systems.Values);
        foreach (SystemBase system in temp)
        {
            if(Systems.Values.Contains(system) && system.enabled) system.OnUpdate();
        }
    }

    internal static T GetSystem<T>() where T : SystemBase
    {
        if (Systems.ContainsKey(typeof(T))) return (T)Systems[typeof(T)];
        return null;
    }

    internal static bool SystemExist<T>() where T : SystemBase
    {
        return Systems.ContainsKey(typeof(T));
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
