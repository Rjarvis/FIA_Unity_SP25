using ECS.Core;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    private ECS.Core.Entity _entity;
    public float Radius;

    public void Initialize(ECS.Core.Entity entity, float radius)
    {
        _entity = entity;
        Radius = radius;
        AttachCollider(radius);
    }

    private void AttachCollider(float radius)
    {
        var circleCollider2D = gameObject.AddComponent<CircleCollider2D>();
        circleCollider2D.radius = radius;
    }

    public ECS.Core.Entity GetEntity() => _entity;
}