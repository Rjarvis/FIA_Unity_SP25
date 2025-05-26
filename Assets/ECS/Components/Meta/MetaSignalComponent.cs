using ECS.Core;

namespace ECS.Components.Meta
{
    public class MetaSignalComponent<T> : IComponent
    {
        public T Payload;
    }
}