using System.Collections.Generic;
using ECS.Core.Interfaces;

namespace ECS.Core
{
    public abstract class ReactiveSystem<TEntity> : IReactiveSystem, IUpdateSystem where TEntity : class, IEntity
    {
        private readonly ICollector<TEntity> _collector;
        private readonly List<TEntity> _buffer;

        protected ReactiveSystem(ICollector<TEntity> collector)
        {
            _collector = collector;
            _buffer = new List<TEntity>();
            _collector.OnEntityAdded += OnEntityAdded;
        }

        protected ReactiveSystem(IContext<TEntity> context)
        {
            _collector = GetTrigger(context);
            _buffer = new List<TEntity>();
            ((ICollector<TEntity>)_collector).OnEntityAdded += OnEntityAdded;

        }

        protected abstract ICollector<TEntity> GetTrigger(IContext<TEntity> context);

        public abstract void Execute(List<TEntity> entities);
        protected abstract bool Filter(TEntity entity);

        protected void OnEntityAdded(IEntity entity)
        {
            if (entity is TEntity tEntity && Filter(tEntity) && !_buffer.Contains(tEntity))
            {
                _buffer.Add(tEntity);
            }
        }


        public abstract void Activate();
        public abstract void Deactivate();
        public abstract void Clear();
        public void Initialize() { }
        public void Update()
        {
            if (_buffer.Count == 0) return;

            Execute(_buffer);
            _buffer.Clear();
        }
    }
}