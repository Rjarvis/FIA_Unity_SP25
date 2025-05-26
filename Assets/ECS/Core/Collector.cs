using System;
using System.Collections.Generic;
using ECS.Core.Interfaces;
using ECS.ECS_Helpers;

namespace ECS.Core
{
    public class Collector<T> : ICollector<T> where T : class, IEntity
    {
        private readonly Type _componentType;
        private readonly List<T> _collectedEntities = new();

        public event Action<T> OnEntityAdded;

        public Collector(IContext<T> context, Type componentType)
        {
            _componentType = componentType;
            context.OnEntityAdded += HandleEntityAdded;
        }

        private void HandleEntityAdded(IEntity entity)
        {
            if (entity.HasComponent(_componentType) && entity is T tEntity)
            {
                _collectedEntities.Add(tEntity);
                OnEntityAdded?.Invoke(tEntity);
            }
        }
        
        private readonly Dictionary<Action<IEntity>, Action<T>> _delegateMap = new();

        event Action<IEntity> ICollector<T>.OnEntityAdded
        {
            add
            {
                Action<T> wrapper = e => value?.Invoke(e);
                _delegateMap[value] = wrapper;
                OnEntityAdded += wrapper;
            }
            remove
            {
                if (_delegateMap.TryGetValue(value, out var wrapper))
                {
                    OnEntityAdded -= wrapper;
                    _delegateMap.Remove(value);
                }
            }
        }


        public int Count => _collectedEntities.Count;
        public List<T> CollectedEntities => _collectedEntities;

        public void Activate() { }
        public void Deactivate() { }
        public void ClearCollectedEntities() => _collectedEntities.Clear();
        public void Clear() => _collectedEntities.Clear();
    }
}