using Contexts;
using UnityEngine;

namespace Helpers
{
    public class BulletCollisionHandler2D : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (GameContexts.Level.ContainsEntity(collision.gameObject.GetComponent<LevelComponent>())) return;

            var entityComponent = gameObject.GetComponent<EntityComponent>();
            if (!entityComponent) return;
            RemoveAndDestroyEntity(entityComponent);
        }

        private void RemoveAndDestroyEntity(EntityComponent entityComponent)
        {
            GameContexts.Gameplay.RemoveEntity(entityComponent);
            Destroy(gameObject, 0.01f);
        }
    }
}