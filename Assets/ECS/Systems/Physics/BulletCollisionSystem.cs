using ECS;
using ECS.Components;
using ECS.Core;
using ECS.Core.Interfaces;
using UnityEngine;

public class BulletCollisionSystem : IUpdateSystem
{
    private readonly Context _context;
    public BulletCollisionSystem(Context context)
    {
        _context = context;
    }

    public void Initialize()
    {
        Debug.Log("BulletCollisionSystem Initialized");
    }

    public void Update()
    {
        var world = WorldContainer.Instance;
        var physics = _context;
        var alien = world.GetContext("Alien");

        foreach (var bullet in physics.GetEntitiesWithComponent<BulletComponent>())
        {
            if (bullet.TryGetComponent<CollisionComponent>(out var collision))
            {
                var hitEntity = collision.Other;

                if (hitEntity.TryGetComponent<AlienComponent>(out _))
                {
                    if (hitEntity.TryGetComponent<AlienHealthComponent>(out var health))
                    {
                        health.Current -= 1;

                        if (health.Current <= 0)
                        {
                            alien.DestroyEntity(hitEntity);
                        }
                    }

                    physics.DestroyEntity(bullet);
                }

                bullet.RemoveComponent<CollisionComponent>();
            }
        }
    }
}