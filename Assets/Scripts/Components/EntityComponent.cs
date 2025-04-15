using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityComponent : MonoBehaviour
{
    private Context entityContext;
    private readonly Dictionary<Type, object> components = new Dictionary<Type, object>();
    
    public void SetContext(Context context)
    {
        entityContext = context;
        context.AddEntity(this);//Adds the entity to the associated Context
    }

    public Context GetContext()
    {
        return entityContext;
    }

    // public void AddComponent<T>(T component) where T : class
    // {
    //     components[typeof(T)] = component;
    //     EntitySystem.NotifyComponentAdded(this, component);
    // }
    //
    // public void RemoveComponent<T>() where T : class
    // {
    //     if (components.Remove(typeof(T), out var component))
    //     {
    //         EntitySystem.NotifyComponentRemoved(, component);
    //     }
    // }

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