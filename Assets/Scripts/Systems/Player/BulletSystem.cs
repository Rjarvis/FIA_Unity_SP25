// using Components;
// using Interfaces;
// using UnityEngine;
// using Contexts;
// using ECS.Components;
//
// namespace Systems.Player
// {
//     public class BulletSystem : MonoBehaviour, IUpdatable
//     {
//         public void UpdateSystem()
//         {
//             var bullets = GameContexts.Physics.GetEntitiesWithComponent<BulletComponent>();
//             foreach (var bullet in bullets)
//             {
//                 if (bullet.TryGetComponent<BulletComponent>(out var bulletComp))
//                 {
//                     var transform = bullet.GetTransform();
//                     if(!transform) continue;
//                     transform.position += bulletComp.direction * bulletComp.speed * Time.deltaTime;
//
//                     // Destroy bullet if off screen
//                     Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
//                     if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height || screenPos.z < 0)
//                     {
//                         GameObject.Destroy(bullet.GetGameObject());
//                         GameContexts.Gameplay.RemoveEntity(bullet);
//                     }
//                 }
//             }
//         }
//     }
// }