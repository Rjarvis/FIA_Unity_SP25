using ECS.Core;
using ECS.Core.Interfaces;
using ECS.Components;
using Interfaces;
using UnityEngine;

namespace ECS.Systems.Player
{
    public class BulletSystem : ISystem, IUpdatable
    {
        private readonly Context _physicsContext;
        private readonly Context _gameplayContext;

        public BulletSystem(Context physicsContext, Context gameplayContext)
        {
            _physicsContext = physicsContext;
            _gameplayContext = gameplayContext;
        }

        public void Initialize(World world)
        {
            // Optional initialization logic
        }

        public void UpdateSystem()
        {
            var bullets = _physicsContext.GetEntitiesWithComponent<BulletComponent>();
            foreach (var bullet in bullets)
            {
                if (!bullet.HasComponent<ViewComponent>()) continue;

                var bulletComp = bullet.GetComponent<BulletComponent>();
                var view = bullet.GetComponent<ViewComponent>();

                var transform = view.GameObject?.transform;
                if (transform == null) continue;

                transform.position += bulletComp.direction * bulletComp.speed * Time.deltaTime;

                // Destroy bullet if off screen
                Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height || screenPos.z < 0)
                {
                    Object.Destroy(view.GameObject);
                    _gameplayContext.DestroyEntity(bullet);
                }
            }
        }
    }
}