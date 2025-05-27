using System;
using System.Collections.Generic;
using Interfaces;

public class Context
{
    private readonly List<IEntityComponent> entities = new();
    public readonly string Name;

    public Context(string name)
    {
        Name = name;
    }

    public void AddEntity(IEntityComponent entity)
    {
        if (!entities.Contains(entity))
            entities.Add(entity);
    }

    public void RemoveEntity(IEntityComponent entity)
    {
        entities.Remove(entity);
    }

    public bool ContainsEntity(IEntityComponent entity)
    {
        return entities.Contains(entity);
    }

    public IReadOnlyList<IEntityComponent> GetAllEntities() => entities;

    // 🆕 Query all entities that have a specific component type
    public List<IEntityComponent> GetEntitiesWithComponent<T>() where T : class
    {
        var results = new List<IEntityComponent>();
        foreach (var entity in entities)
        {
            if (entity.TryGetComponent<T>(out _))
            {
                results.Add(entity);
            }
        }
        return results;
    }


    // 🆕 Optional: Query all components of type T directly (e.g., HealthComponent)
    public List<T> GetAllComponentsOfType<T>() where T : class
    {
        var results = new List<T>();
        foreach (var entity in entities)
        {
            if (entity.TryGetComponent<T>(out var component))
            {
                results.Add(component);
            }
        }
        return results;
    }
}