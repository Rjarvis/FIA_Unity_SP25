using System;
using System.Collections.Generic;
using System.Linq;
using ECS.Core.Interfaces;

namespace ECS.Core
{
    public class Context : Context<Entity>
    {
    }

    public abstract class Context<TEntity> : IContext<TEntity> where TEntity : class, IEntity
    {
        private readonly List<Entity> _entities = new();
        public IReadOnlyList<Entity> Entities => _entities;

        public event Action<TEntity> OnEntityAdded;

        private readonly Dictionary<Type, Action<Entity>> _onComponentAdded = new();
        private readonly Dictionary<Type, Action<Entity>> _onComponentRemoved = new();


        public void SubscribeComponentAdded<T>(Action<Entity> callback) where T : IComponent
        {
            var type = typeof(T);
            if (!_onComponentAdded.ContainsKey(type))
                _onComponentAdded[type] = delegate { };
            _onComponentAdded[type] += callback;
        }

        public void UnsubscribeComponentAdded<T>(Action<Entity> callback) where T : IComponent
        {
            var type = typeof(T);
            if (_onComponentAdded.ContainsKey(type))
                _onComponentAdded[type] -= callback;
        }

        public void NotifyComponentAdded<T>(Entity entity) where T : IComponent
        {
            var type = typeof(T);
            if (_onComponentAdded.TryGetValue(type, out var action))
                action?.Invoke(entity);
        }

        public void NotifyComponentRemoved<T>(Entity entity) where T : IComponent
        {
            var type = typeof(T);
            if (_onComponentRemoved.TryGetValue(type, out var action))
                action?.Invoke(entity);
        }

        public Entity CreateEntity()
        {
            var guid = Guid.NewGuid();
            var entity = new Entity(guid) { Context = this as Context<Entity> };
            _entities.Add(entity);
            OnEntityAdded?.Invoke(entity as TEntity);
            return entity;
        }

        public void DestroyEntity(Entity entity)
        {
            entity.IsAlive = false;
            _entities.Remove(entity);
        }

        public Entity GetEntityByGuid(Guid guid)
        {
            return _entities.FirstOrDefault(e => e.Guid == guid);
        }

        public List<Entity> GetEntitiesWithComponent<T>() where T : class, IComponent =>
            _entities.Where(e => e.HasComponent<T>()).ToList();

        public IEnumerable<Entity> GetEntitiesWithComponents<T1, T2>()
            where T1 : class, IComponent
            where T2 : class, IComponent
        {
            var set1 = new HashSet<Entity>(GetEntitiesWithComponent<T1>());
            set1.IntersectWith(GetEntitiesWithComponent<T2>());
            return set1;
        }

        public ICollector<Entity> CreateCollector<TComponent>() where TComponent : IComponent
        {
            return new Collector<Entity>(this as IContext<Entity>, typeof(TComponent));
        }

        public event ContextEntityChanged OnEntityCreated;
        public event ContextEntityChanged OnEntityWillBeDestroyed;
        public event ContextEntityChanged OnEntityDestroyed;
        public int totalComponents { get; }
        public Stack<IComponent>[] componentPools { get; }
        public int count { get; }
        public int reusableEntitiesCount { get; }
        public int retainedEntitiesCount { get; }
        public void DestroyAllEntities()
        {
            throw new NotImplementedException();
        }

        public void AddEntityIndex(IEntityIndex entityIndex)
        {
            throw new NotImplementedException();
        }

        public IEntityIndex GetEntityIndex(string name)
        {
            throw new NotImplementedException();
        }

        public void ResetCreationIndex()
        {
            throw new NotImplementedException();
        }

        public void ClearComponentPool(int index)
        {
            throw new NotImplementedException();
        }

        public void ClearComponentPools()
        {
            throw new NotImplementedException();
        }

        public void RemoveAllEventHandlers()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}