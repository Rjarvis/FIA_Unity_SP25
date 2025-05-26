using ECS.Components;
using ECS.Core;
using Helpers;
using UnityEngine;

public class CollisionReporter2D : MonoBehaviour
{
    public Entity LinkedEntity;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LinkedEntity == null) return;

        var otherGO = collision.gameObject;
        if (GameObjectEntityMap.TryGetEntity(otherGO, out var otherEntity))
        {
            LinkedEntity.AddComponent(new CollisionComponent
            {
                Other = otherEntity
            });
        }
    }
}