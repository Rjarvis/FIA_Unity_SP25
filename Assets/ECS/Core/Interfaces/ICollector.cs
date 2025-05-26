using System;
using System.Collections.Generic;

namespace ECS.Core.Interfaces
{
    public interface ICollector<TEntity> where TEntity : class, IEntity
    {
        void Activate();
        void Deactivate();
        void ClearCollectedEntities();
        int Count { get; }
        List<TEntity> CollectedEntities { get; }
        event Action<IEntity> OnEntityAdded;
    }
}