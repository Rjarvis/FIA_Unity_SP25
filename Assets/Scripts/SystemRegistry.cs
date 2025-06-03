using System.Collections.Generic;
using UnityEngine;

public class SystemRegistry : MonoBehaviour
{
    private static readonly List<ISystem> systems = new List<ISystem>();

    public static void Register(ISystem system) => systems.Add(system);
    public static void Unregister(ISystem system) => systems.Remove(system);

    // public static void StartAll() { foreach (var system in systems) system.StartSequence(); }
    // public static void UpdateAll() { foreach (var system in systems) system.UpdateSequence(); }
    // public static void EndAll() { foreach (var system in systems) system.EndSequence(); }
    public static List<ISystem> GetAllSystems() => systems;

    public static List<T> GetSystemOfType<T>(bool bringAll) where T : class
    {
        //Todo clean this up a little
        List<T> toReturn = new List<T>();
        foreach (var system1 in systems)
        {
            var system = (T)system1;
            Debug.Log($"systemPreCast: {system1.GetType()}");
            if (null == system) continue;
            if (system.GetType() == typeof(T)) Debug.LogError("Yes you can!");
            if (!bringAll) return new List<T>() { system };
            toReturn.Add(system);
        }

        return toReturn;
    }
}

public interface ISystem
{
    // void StartSequence();
    // void UpdateSequence();
    // void EndSequence();
}