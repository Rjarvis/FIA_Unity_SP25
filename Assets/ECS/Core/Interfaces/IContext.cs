using System;
using System.Collections.Generic;

namespace ECS.Core.Interfaces
{
    public interface IContext<TEntity> : IContext where TEntity : class, IEntity
    {
        event Action<TEntity> OnEntityAdded;
        public void NotifyComponentAdded<T>(Entity entity) where T : IComponent;
        public void NotifyComponentRemoved<T>(Entity entity) where T : IComponent;
        public ICollector<Entity> CreateCollector<T>() where T : IComponent;
    }

    public interface IContext
    {
        event ContextEntityChanged OnEntityCreated;
        event ContextEntityChanged OnEntityWillBeDestroyed;
        event ContextEntityChanged OnEntityDestroyed;
        int totalComponents { get; }
        Stack<IComponent>[] componentPools { get; }
        int count { get; }

        int reusableEntitiesCount { get; }

        int retainedEntitiesCount { get; }

        void DestroyAllEntities();

        void AddEntityIndex(IEntityIndex entityIndex);

        IEntityIndex GetEntityIndex(string name);
        void ResetCreationIndex();
        void ClearComponentPool(int index);
        void ClearComponentPools();
        void RemoveAllEventHandlers();
        void Reset();
        
    }

    public delegate void ContextEntityChanged(IContext context, IEntity entity);
    public delegate void ContextGroupChanged(IContext context, IGroup group);

}