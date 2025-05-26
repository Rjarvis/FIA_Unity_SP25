using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Contexts;
using ECS.Components;
using ECS.Core;


namespace Systems.Player
{
    public class ShootingSystem : MonoBehaviour, IUpdatable
    {
        public GameObject bulletPrefab;
        private Camera mainCamera;
        private RectTransform crosshairTransform;

        public void Initialize(Camera camera, RectTransform crosshair, GameObject bulletPrefab)
        {
            mainCamera = camera;
            crosshairTransform = crosshair;
            this.bulletPrefab = bulletPrefab;
        }

        public void UpdateSystem()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                var playerEntities = GameContexts.Player.GetEntitiesWithComponent<PlayerComponent>();
                foreach (var entity in playerEntities)
                {
                    if (entity.TryGetComponent<ShootComponent>(out var shootComponent))
                    {
                        if (Time.time - shootComponent.lastShotTime > shootComponent.cooldownTime)
                        {
                            shootComponent.lastShotTime = Time.time;
                            SpawnBullet(entity);
                        }
                    }
                }
            }
        }

        private void SpawnBullet(Entity player)
        {
            var playerTransform = player.GetComponent<ViewComponent>().Transform;

            Vector2 crosshairScreenPos = crosshairTransform.position;
            Ray ray = mainCamera.ScreenPointToRay(crosshairScreenPos);

            Vector3 targetPoint = Physics.Raycast(ray, out var hit)
                ? hit.point
                : ray.origin + ray.direction * 100f;

            Vector2 playerPos = new Vector2(playerTransform.position.x, playerTransform.position.y);
            Vector2 targetPos = new Vector2(targetPoint.x, targetPoint.y);
            Vector2 direction = (targetPos - playerPos).normalized;

            // V2
            // Vector2 offset = direction * 1.5f;
            // Vector2 spawnPos = playerPos + offset;
            // var bulletGO = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            
            // V1
            var bulletGO = Instantiate(bulletPrefab, playerPos, Quaternion.identity);

            // ECS-style entity wrapping
            var bulletEntity = GameContexts.Physics.CreateEntity();
            var bulletComponent = new BulletComponent() { direction = direction, speed = 1f };
            bulletEntity.AddComponent(bulletComponent);
            // EntitySystem.NotifyComponentAdded(bulletEntity, bulletComponent);//Redundant

            // Physics setup (2D)
            // bulletGO.AddComponent<BulletCollisionHandler2D>(); // Handles OnCollisionEnter2D
            var collider = bulletGO.GetComponent<CircleCollider2D>();
            collider.radius = 0.2f;

            var rb = bulletGO.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.linearVelocity = direction * bulletComponent.speed;
        }
    }
}