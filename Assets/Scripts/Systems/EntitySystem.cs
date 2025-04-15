using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Systems;
using UnityEngine;

public static class EntitySystem
{
    internal static readonly Dictionary<Context, List<object>> systemComponents = new();

    public static void RegisterSystem<T>(Context context) where T : class, new()
    {
        if (!systemComponents.ContainsKey(context))
        {
            systemComponents[context] = new List<object>();
        }

        T systemInstance = new T();
        systemComponents[context].Add(systemInstance);
    }
    
    public static void UnRegisterSystem<T>(Context context) where T : class
    {
        if (systemComponents.ContainsKey(context))
        {
            systemComponents[context].RemoveAll(system => system is T);
        }
    }
    
    public static void NotifyEntityCreated(EntityComponent entity)
    {
        Debug.Log($"Entity {entity.gameObject.name} created, notifying systems.");

        var entityContext = entity.GetContext();
        if (systemComponents.TryGetValue(entityContext, out var systemList))
        {
            foreach (var system in systemList)
            {
                if (system is IEntityListener entityListener)
                {
                    entityListener.OnEntityCreated(entity);
                }
            }
        }
    }
    
    public static void NotifyComponentAdded<T>(EntityComponent entity, T component) where T : class
    {
        if (systemComponents.TryGetValue(entity.GetContext(), out var list) == false)
        {
            list.Add(component);
        }
    }
    
    public static void NotifyComponentRemoved<T>(EntityComponent entity,T component) where T : class
    {
        if (systemComponents.TryGetValue(entity.GetContext(), out var list))
        {
            list.Remove(component);
        }
    }

    public static List<T> GetAllComponents<T>(Context context) where T : class
    {
        if (systemComponents.TryGetValue(context, out var list))
        {
            return list.OfType<T>().ToList();
        }
        return new List<T>();
    }

    public static void Update()
    {
        //TODO Refactor this so it is not a nested loop, but does a linq expression to get only "dirty" flagged components and entities.
        //Lets connect the systems to their interface counter parts
        foreach (var system in systemComponents)
        {
            //Update all systems
            foreach (var obj in system.Value)
            {
                if (obj is IUpdatable updatableSystem)
                {
                    updatableSystem.UpdateSystem();
                }
            }
        }
    }
}