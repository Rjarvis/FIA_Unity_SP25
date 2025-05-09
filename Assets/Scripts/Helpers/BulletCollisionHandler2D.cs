using Contexts;
using UnityEngine;

namespace Helpers
{
    public class BulletCollisionHandler2D : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Debug.Log($"<color=lime>{gameObject.name}</color>" +
            //           $"<b>hit</b> " +
            //           $"<color=pink>{collision.collider.gameObject.name}</color>{collision.collider.gameObject.name}");
            var otherColliderLevelComponent= collision.gameObject.GetComponent<LevelComponent>();
            bool containsEntity = GameContexts.Level.ContainsEntity(otherColliderLevelComponent);
            if (containsEntity)
            {
                Debug.Log("<color=green>Got Here</color>");
                return;
            }
            var entityComponent = gameObject.GetComponent<EntityComponent>();
            GameContexts.Gameplay.RemoveEntity(entityComponent);
            Destroy(gameObject, 0.01f);
        }
    }
}