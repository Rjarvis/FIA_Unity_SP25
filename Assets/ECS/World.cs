using System;
using System.Collections.Generic;
using ECS.Core;
using ECS.Core.Interfaces;
using UnityEngine;

public class World
{
    private readonly Dictionary<string, Context> _contexts = new();
    private readonly List<ISystem> _systems = new();

    public Context CreateContext(string name)
    {
        var context = new Context();
        AddContext(name, context);
        return context;
    }

    public void AddContext(string name, Context context)
    {
        if (_contexts.ContainsKey(name))
            throw new InvalidOperationException($"Context '{name}' already exists.");

        _contexts[name] = context;
    }

    public Context GetContext(string name)
    {
        if (_contexts.TryGetValue(name, out var context))
            return context;

        throw new KeyNotFoundException($"Context '{name}' not found.");
    }
    
    public IEnumerable<Context> GetAllContexts() => _contexts.Values;

    public void InitializeSystems(params ISystem[] systems)
    {
        foreach (var system in systems)
            AddSystem(system);
    }

    public void AddSystem(ISystem system)
    {
        _systems.Add(system);

        if (system is ICreateSystem createSystem)
            createSystem.Initialize(this);
        if (system is IUpdateSystem updateSystem) updateSystem.Initialize();
    }

    public void ExecuteSystems()
    {
        foreach (var system in _systems)
        {
            if (system is IUpdateSystem updateSystem) updateSystem.Update();
        }

        CleanupEntities();
    }

    public void ShutdownSystems()
    {
        foreach (var system in _systems)
        {
            if (system is IDestroySystem destroySystem)
                destroySystem.Shutdown(this);
        }

        _systems.Clear();
    }

    private void CleanupEntities()
    {
        foreach (var context in _contexts.Values)
        {
            var toRemove = new List<Entity>();

            foreach (var entity in context.Entities)
            {
                if (!entity.IsAlive && entity.Aerc.RetainCount == 0)
                    toRemove.Add(entity);
            }

            foreach (var entity in toRemove)
            {
                context.DestroyEntity(entity);
                // Optionally return entity to pool here
            }
        }
    }
}
