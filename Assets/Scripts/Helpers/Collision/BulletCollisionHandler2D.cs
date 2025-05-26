// using Contexts;
// using ECS.Components;
// using Helpers.Base;
// using Interfaces;
// using Systems.Enemy;
// using UnityEngine;
//
// namespace Helpers
// {
//     [RequireComponent(typeof(CircleCollider2D))]
//     public class BulletCollisionHandler2D : CollisionHandlerBase
//     {
//         private void OnCollisionEnter2D(Collision2D collision)
//         {
//             IEntityComponent entityComponent = GetEntityComponent();
//             var entityContext = entityComponent.GetContext();
//
//             if (entityContext == GameContexts.Alien)
//             {
//                 Debug.Log($"{gameObject.name} is in the Alien context");
//                 // Debug.LogError($"{gameObject.name} has EntityComponent:{gameObject.GetComponent<EntityComponent>()}");
//                 var physicsEntitiesWithBulletComponent = GameContexts.Physics.GetEntitiesWithComponent<BulletComponent>();
//                 foreach (var entity in physicsEntitiesWithBulletComponent)
//                 {
//                     entity.TryGetComponent(out ViewComponent viewComponent);
//                     if (viewComponent == null) continue;
//
//                     var bulletGameObject = viewComponent.GameObject;
//                     Debug.Log($"<color=red>ARRGGHHH YOU GOT ME!</color>");
//                     var alienSystem = FindFirstObjectByType<AlienSystem>();
//                     alienSystem.CountDied();
//                     RemoveAndDestroyEntity(entityContext);
//                 }
//
//                 return;
//             }
//
//             if (entityContext == GameContexts.Physics)
//             {
//                 Debug.Log($"{gameObject.name} should be a bullet?");
//                 if (CollisionIsInLevelContext(collision)) return;
//                 if (GameContexts.Level.ContainsEntity(collision.gameObject.GetComponent<EntityComponent>())) return;
//             }
//
//             
//             RemoveAndDestroyEntity(entityContext);
//         }
//
//         private bool CollisionIsInLevelContext(Collision2D collision2D)
//         {
//             var gameObject = collision2D.gameObject;
//             var viewComponent = gameObject.GetComponent<ViewComponent>();
//         }
//
//         private IEntityComponent GetEntityComponent()
//         {
//             var entityComponent = gameObject.GetComponent<EntityComponent>();
//             if (entityComponent) return entityComponent;
//             else
//             {
//                 Debug.LogWarning($"EntityComponent could not be retrieved on {gameObject.name} so I am destroying it.");
//                 Destroy(this);
//                 return null;
//             }
//         }
//     }
// }