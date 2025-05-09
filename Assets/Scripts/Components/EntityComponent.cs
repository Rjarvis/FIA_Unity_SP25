using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class EntityComponent : MonoBehaviour, IEntityComponent
{
    private Context entityContext;
    private readonly Dictionary<Type, object> components = new();

    public GameObject GetGameObject() => gameObject ? gameObject : null;

    public Transform GetTransform() => transform ? transform : null;

    public void SetContext(Context context)
    {
        entityContext = context;
        context.AddEntity(this); // Register this entity with the context
    }

    public Context GetContext() => entityContext;

    public void AddComponent<T>(T component) where T : class
    {
        components[typeof(T)] = component;
        EntitySystem.NotifyComponentAdded(this, component);
    }

    public void RemoveComponent<T>() where T : class
    {
        if (components.Remove(typeof(T), out var component))
        {
            EntitySystem.NotifyComponentRemoved(this, component);
        }
    }

    public bool TryGetComponent<T>(out T component) where T : class
    {
        if (components.TryGetValue(typeof(T), out var obj))
        {
            component = obj as T;
            return true;
        }
        component = null;
        return false;
    }
}