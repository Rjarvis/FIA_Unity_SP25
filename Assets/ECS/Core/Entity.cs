using System;
using System.Collections.Generic;
using ECS.Core;
using ECS.Core.Interfaces;

namespace ECS.Core
{
    public class Entity : IEntity
    {
        private readonly Dictionary<Type, IComponent> _components = new();
        public Context<Entity> Context { get; set; }

        public Guid Guid { get; private set; }
        private bool _isAlive = true;

        public bool IsAlive
        {
            get => _isAlive;
            set => _isAlive = value;
        }

        public event Action<IEntity, IComponent> OnComponentAdded;
        public event Action<IEntity, IComponent> OnComponentRemoved;
        public event Action<IEntity> OnDestroyed;

        public IAERC Aerc { get; }

        public Entity(Guid guid)
        {
            Guid = guid;
            Aerc = new AERC();
        }

        public void AddComponent<T>(T component) where T : IComponent
        {
            var type = typeof(T);
            _components[type] = component;
            Context?.NotifyComponentAdded<T>(this);
        }

        public void RemoveComponent<T>() where T : IComponent
        {
            var type = typeof(T);
            if (_components.TryGetValue(type, out var component))
            {
                _components.Remove(type);
                Context?.NotifyComponentRemoved<T>(this);
            }
        }

        public bool HasComponent<T>() where T : IComponent =>
            _components.ContainsKey(typeof(T));

        public bool TryGetComponent<T>(out T component) where T : IComponent
        {
            if (_components.TryGetValue(typeof(T), out var obj))
            {
                component = obj is T ? (T)obj : default;
                return component != null;
            }
            component = default;
            return false;
        }

        public T GetComponent<T>() where T : IComponent
        {
            if (_components.TryGetValue(typeof(T), out var component))
                return (T)component;

            throw new KeyNotFoundException($"Component of type {typeof(T)} not found on entity.");
        }

        public void Destroy()
        {
            if (!_isAlive) return;

            _isAlive = false;

            foreach (var component in _components.Values)
            {
                OnComponentRemoved?.Invoke(this, component);
            }

            _components.Clear();
            OnDestroyed?.Invoke(this);
        }

        public int RetainCount { get; }

        public void Retain(object owner)
        {
            Aerc.Retain(owner);
        }

        public void Release(object owner)
        {
            Aerc.Release(owner);
            if (Aerc.RetainCount == 0)
            {
                OnNoRetainers();
            }
        }

        private void OnNoRetainers()
        {
            // Pooling, cleanup, or deferred destruction can happen here
            Destroy();
        }
    }
}
