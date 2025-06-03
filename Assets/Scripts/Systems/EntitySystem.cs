using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using NUnit.Framework;
using Systems;
using UnityEngine;

public static class EntitySystem
{
    internal static readonly Dictionary<Context, List<object>> SystemsDictionary = new();
    internal static readonly Dictionary<Context, Dictionary<Type, List<IEntityComponent>>> componentRegistry = new();


    public static void RegisterSystem<T>(Context context, T obj) where T : class
    {
        if (!SystemsDictionary.ContainsKey(context))
        {
            SystemsDictionary[context] = new List<object>();
        }

        if (SystemsDictionary[context].Any(x => x is T)) return; // Already registered

        SystemsDictionary[context].Add(obj);
    }

    public static void UnRegisterSystem<T>(Context context) where T : class
    {
        if (SystemsDictionary.ContainsKey(context))
        {
            SystemsDictionary[context].RemoveAll(system => system is T);
            Debug.Log($"[EntitySystem] Unregistered system of type {typeof(T).Name} from {context.GetType().Name}");
        }
    }

    public static object GetSystem<T>(T system)
    {
        foreach (var kvp in SystemsDictionary)
        {
            foreach (var systemInContext in kvp.Value)
            {
                var systemInContextType = systemInContext.GetType();
                if (systemInContextType == typeof(T)) return systemInContext;
            }
        }

        return null;
    }
    
    public static object GetSystem<T>()
    {
        foreach (var kvp in SystemsDictionary)
        {
            foreach (var systemInContext in kvp.Value)
            {
                var systemInContextType = systemInContext.GetType();
                if (systemInContextType == typeof(T)) return systemInContext;
            }
        }

        return null;
    }


    public static Context CreateAndRegisterContext(string name)
    {
        var context = new Context(name);
        SystemsDictionary[context] = new List<object>();
        componentRegistry[context] = new Dictionary<Type, List<IEntityComponent>>();
        return context;
    }

    public static void UnRegisterContext(Context context)
    {
        if (SystemsDictionary.ContainsKey(context)) SystemsDictionary[context].Clear();
    }

    public static void NotifyEntityCreated(EntityComponent entity)
    {
        Debug.Log($"Entity {entity.gameObject.name} created, notifying systems.");

        var entityContext = entity.GetContext();
        if (SystemsDictionary.TryGetValue(entityContext, out var systemList))
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
        var context = entity.GetContext();
        if (!componentRegistry.TryGetValue(context, out var componentMap))
        {
            componentMap = new Dictionary<Type, List<IEntityComponent>>();
            componentRegistry[context] = componentMap;
        }

        var type = typeof(T);
        if (!componentMap.TryGetValue(type, out var list))
        {
            list = new List<IEntityComponent>();
            componentMap[type] = list;
        }

        if (!list.Contains(entity))
            list.Add(entity);
    }

    
    public static void NotifyComponentRemoved<T>(EntityComponent entity, T component) where T : class
    {
        var context = entity.GetContext();
        if (componentRegistry.TryGetValue(context, out var componentMap))
        {
            var type = typeof(T);
            if (componentMap.TryGetValue(type, out var list))
            {
                list.Remove(entity);
            }
        }
    }
    
    public static List<IEntityComponent> GetEntitiesWithComponent<T>(Context context) where T : class
    {
        if (componentRegistry.TryGetValue(context, out var componentMap) &&
            componentMap.TryGetValue(typeof(T), out var list))
        {
            return list;
        }

        return new List<IEntityComponent>();
    }



    public static List<T> GetAllComponents<T>(Context context) where T : class
    {
        if (SystemsDictionary.TryGetValue(context, out var list))
        {
            return list.OfType<T>().ToList();
        }
        return new List<T>();
    }

    public static void Update()
    {
        //TODO Refactor this so it is not a nested loop, but does a linq expression to get only "dirty" flagged components and entities.
        //Lets connect the systems to their interface counter parts
        foreach (var system in SystemsDictionary)
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