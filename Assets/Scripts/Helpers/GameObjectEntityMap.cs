using System.Collections.Generic;
using ECS.Core;
using UnityEngine;

namespace Helpers
{
    public static class GameObjectEntityMap
    {
        private static readonly Dictionary<GameObject, Entity> _map = new();

        public static void Register(GameObject gameObject, Entity entity)
        {
            if (gameObject != null && entity != null)
            {
                _map[gameObject] = entity;
            }
        }

        public static void Unregister(GameObject gameObject)
        {
            if (gameObject != null)
            {
                _map.Remove(gameObject);
            }
        }

        public static bool TryGetEntity(GameObject gameObject, out Entity entity)
        {
            return _map.TryGetValue(gameObject, out entity);
        }

        public static void Clear()
        {
            _map.Clear();
        }
    }
}