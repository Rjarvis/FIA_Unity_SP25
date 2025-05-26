using System;
using ECS.Core.Interfaces;

namespace ECS.ECS_Helpers
{
    public static class EntityExtensions
    {
        public static bool HasComponent(this IEntity entity, Type type)
        {
            return entity.GetType().GetMethod("HasComponent")?
                .MakeGenericMethod(type)
                .Invoke(entity, null) as bool? ?? false;
        }
    }
}