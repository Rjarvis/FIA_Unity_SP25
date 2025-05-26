using System;

namespace ECS.Core.Interfaces
{
    public interface IEntity : IAERC
    {
        event Action<IEntity, IComponent> OnComponentAdded;
        event Action<IEntity, IComponent> OnComponentRemoved;
        event Action<IEntity> OnDestroyed;

        bool IsAlive { get; }
        T GetComponent<T>() where T : IComponent;
        bool TryGetComponent<T>(out T component) where T : IComponent;
        bool HasComponent<T>() where T : IComponent;
        void AddComponent<T>(T component) where T : IComponent;
        void RemoveComponent<T>() where T : IComponent;
        void Destroy();
    }
}