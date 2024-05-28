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

internal static class GameManager
{

    private static readonly Dictionary<Type, SystemBase> s_systems = [];

    /// <summary>
    /// The status of the game.
    /// </summary>
    internal static GameStatus GameStatus = GameStatus.unknown;

    internal static Random Random { get; private set; } = new();

    /// <summary>
    /// Called when the game need to load a new status.
    /// </summary>
    /// <param name="gameStatus">The new status.</param>
    internal static void Load(GameStatus gameStatus) 
    {
        GameStatus = gameStatus;
        switch (GameStatus)
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
        foreach (SystemBase system in new List<SystemBase>(s_systems.Values))
        {
            system.OnGameStatusChange(GameStatus);
        }
    }

    /// <summary>
    /// Update all the systems.
    /// </summary>
    internal static void Update()
    {
        List<SystemBase> temp = new(s_systems.Values);
        foreach (SystemBase system in temp)
        {
            if(s_systems.Values.Contains(system) && system.enabled) system.OnUpdate();
        }
    }

    /// <summary>
    /// Get an exesting system.
    /// </summary>
    /// <typeparam name="T">The type of the system to get.</typeparam>
    /// <returns></returns>
    internal static T GetSystem<T>() where T : SystemBase
    {
        if (s_systems.ContainsKey(typeof(T))) return (T)s_systems[typeof(T)];
        return null;
    }

    /// <summary>
    /// Check if a system exist.
    /// </summary>
    /// <typeparam name="T">The type of the system to check.</typeparam>
    /// <returns>True if the system exists.</returns>
    internal static bool SystemExists<T>() where T : SystemBase
    {
        return s_systems.ContainsKey(typeof(T));
    }

    /// <summary>
    /// Get or create a system.
    /// </summary>
    /// <typeparam name="T">The type of the wnated system.</typeparam>
    /// <returns>The wanted system.</returns>
    internal static T GetOrCreateSystem<T>() where T: SystemBase, new()
    {
        if (s_systems.ContainsKey(typeof(T))) return (T)s_systems[typeof(T)];
        T system = new();
        s_systems.Add(typeof(T), system);
        system.OnCreate(); 
        return system;
    }

    /// <summary>
    /// Destroy a system.
    /// </summary>
    /// <typeparam name="T">The type of the system to destroy.</typeparam>
    /// <returns>True if the system have been destroyed, False if the system doesn't exists.</returns>
    internal static bool DestroySystem<T>() where T : SystemBase
    {
        if (!s_systems.ContainsKey(typeof(T))) return false;
        s_systems[typeof(T)].OnDestroy();
        s_systems.Remove(typeof(T));
        return true;
    }

}
