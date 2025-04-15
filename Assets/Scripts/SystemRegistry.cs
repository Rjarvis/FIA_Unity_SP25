using System;
using System.Collections.Generic;
using UnityEngine;

public class SystemRegistry : MonoBehaviour
{
    private static readonly List<ISystem> systems = new List<ISystem>();

    public static void Register(ISystem system) => systems.Add(system);
    public static void Unregister(ISystem system) => systems.Remove(system);

    public static void StartAll() { foreach (var system in systems) system.StartSequence(); }
    public static void UpdateAll() { foreach (var system in systems) system.UpdateSequence(); }
    public static void EndAll() { foreach (var system in systems) system.EndSequence(); }
}

public interface ISystem
{
    void StartSequence();
    void UpdateSequence();
    void EndSequence();
}