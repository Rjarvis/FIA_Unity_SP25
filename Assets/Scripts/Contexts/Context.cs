using System;
using System.Collections.Generic;
using UnityEngine;

public class Context
{
    private readonly Dictionary<Type, List<object>> components = new();
    private readonly List<EntityComponent> entities = new();

    public void AddEntity(EntityComponent entity)
    {
        entities.Add(entity);
    }

    public void RemoveEntity(EntityComponent entity)
    {
        entities.Remove(entity);
    }

    // public void AddComponent<T>(EntityComponent entity, T component) where T : class
    // {
    //     if (!components.ContainsKey(typeof(T)))
    //         components[typeof(T)] = new List<object>();
    //
    //     components[typeof(T)].Add(component);
    //     EntitySystem.NotifyComponentAdded(entity, component);
    // }
    //
    // public void RemoveComponent<T>(EntityComponent entity, T component) where T : class
    // {
    //     if (components.TryGetValue(typeof(T), out var list))
    //     {
    //         list.Remove(component);
    //         EntitySystem.NotifyComponentRemoved(entity, component);
    //     }
    // }

    public List<T> GetAllComponents<T>() where T : class
    {
        return components.TryGetValue(typeof(T), out var list) ? list.ConvertAll(obj => obj as T) : new List<T>();
    }
}