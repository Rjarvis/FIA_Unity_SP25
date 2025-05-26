using Contexts;
using ECS.Components.Meta;
using ECS.Core;
using ECS.Core.Interfaces;

namespace ECS.ECS_Helpers
{
    public static class MetaSignalHelper
    {
        public static Entity Signal<T>(T payload)
        {
            Entity entity = GameContexts.Meta.CreateEntity();
            entity.AddComponent(new MetaSignalComponent<T> { Payload = payload });
            return entity;
        }

    }
}
