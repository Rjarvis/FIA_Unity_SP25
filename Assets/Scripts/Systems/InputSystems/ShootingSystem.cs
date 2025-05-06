using Components;
using Components.InputComponents;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Contexts;

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
                Debug.Log("<color=red>Got Here</color>");
                // FireBullet();
                var playerEntities = GameContexts.Player.GetEntitiesWithComponent<PlayerComponent>();
                Debug.Log($"<color=blue>playerEntities.Count: {playerEntities.Count}</color>");

                foreach (var entity in playerEntities)
                {
                    if (entity.TryGetComponent<ShootComponent>(out var shootComponent))
                    {
                        Debug.Log("<color=yellow>Got Here</color>");
                        if (Time.time - shootComponent.lastShotTime > shootComponent.cooldownTime)
                        {
                            Debug.Log("<color=green>Got Here</color>");
                            shootComponent.lastShotTime = Time.time;
                            SpawnBullet(entity);
                            Debug.Log("Shot a bullet");
                        }
                    }
                }
            }
        }

        private void SpawnBullet(IEntityComponent player)
        {
            var playerTransform = player.GetTransform();
            
            // Get world point under crosshair
            Vector2 crosshairScreenPos = crosshairTransform.position;
            Ray ray = mainCamera.ScreenPointToRay(crosshairScreenPos);

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out var hit))
                targetPoint = hit.point;
            else
                targetPoint = ray.origin + ray.direction * 100f; // fallback

            Vector3 direction = (targetPoint - playerTransform.position).normalized;

            // Create bullet entity
            var bulletGO = GameObject.Instantiate(bulletPrefab, playerTransform.position, Quaternion.identity);
            var bulletEntity = bulletGO.AddComponent<EntityComponent>();
            bulletEntity.SetContext(GameContexts.Gameplay);

            var bulletComponent = new BulletComponent() { direction = direction, speed = 50 };
            bulletComponent.SetContext(GameContexts.Gameplay);
            bulletEntity.AddComponent(bulletComponent);
            EntitySystem.NotifyComponentAdded(bulletEntity, bulletComponent);
        }
    }
}
