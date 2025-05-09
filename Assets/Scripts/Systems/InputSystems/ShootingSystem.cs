using Components;
using Components.InputComponents;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Contexts;
using Helpers;


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
                // #if DEBUG
                // Debug.Log("<color=red>Got Here</color>");
                // #endif
                var playerEntities = GameContexts.Player.GetEntitiesWithComponent<PlayerComponent>();
                foreach (var entity in playerEntities)
                {
                    if (entity.TryGetComponent<ShootComponent>(out var shootComponent))
                    {
                        // #if DEBUG
                        // Debug.Log("<color=yellow>Got Here</color>");
                        // #endif
                        // Debug.Log($"<color=teal>Time.time:{Time.time};" +
                        //           $"Time.deltaTime:{Time.deltaTime}; " +
                        //           $"shootComponent.lastShotTime:{shootComponent.lastShotTime}" +
                        //           $"Time.time - shootComponent.lastShotTime:{Time.time - shootComponent.lastShotTime};" +
                        //           $"shootComponent.cooldownTime:{shootComponent.cooldownTime}</color>");
                        if (Time.time - shootComponent.lastShotTime > shootComponent.cooldownTime)
                        {
                            // #if DEBUG
                            // Debug.Log("<color=green>Got Here</color>");
                            // #endif
                            shootComponent.lastShotTime = Time.time;
                            SpawnBullet(entity);
                            // Debug.Log("Shot a bullet");
                        }
                    }
                }
            }
        }

        private void SpawnBullet(IEntityComponent player)
        {
            var playerTransform = player.GetTransform();

            Vector2 crosshairScreenPos = crosshairTransform.position;
            Ray ray = mainCamera.ScreenPointToRay(crosshairScreenPos);

            Vector3 targetPoint = Physics.Raycast(ray, out var hit)
                ? hit.point
                : ray.origin + ray.direction * 100f;

            // Ignore Z for 2D — flatten to XY plane
            // Need to think through getting the revolving position that would appear above the playerObj head
            // For instance the player is at the 12 o'clock position then the startPos needs to be about 1.5 units
            // in the y postition. But, in the 3 or 9 o'clock position it would need to add or subtract from the
            // x-position respectively.
            // Maybe using the playerTransform.rotation.z could inform us how we could determine this
            // Maybe something like...

            Vector2 playerPos = new Vector2(playerTransform.position.x, playerTransform.position.y);
            Vector2 targetPos = new Vector2(targetPoint.x, targetPoint.y);
            Vector2 direction = (targetPos - playerPos).normalized;

            var bulletGO = GameObject.Instantiate(bulletPrefab, playerPos, Quaternion.identity);

            // ECS-style entity wrapping
            var bulletEntity = bulletGO.AddComponent<EntityComponent>();
            bulletEntity.SetContext(GameContexts.Gameplay);

            var bulletComponent = new BulletComponent() { direction = direction, speed = 1f };
            bulletComponent.SetContext(GameContexts.Gameplay);
            bulletEntity.AddComponent(bulletComponent);

            // Physics setup (2D)
            bulletGO.AddComponent<BulletCollisionHandler2D>(); // Handles OnCollisionEnter2D
            var collider = bulletGO.AddComponent<CircleCollider2D>();
            collider.radius = 0.2f;

            var rb = bulletGO.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.linearVelocity = direction * bulletComponent.speed;
        }
    }
}