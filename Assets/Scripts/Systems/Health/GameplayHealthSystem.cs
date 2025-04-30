using Components;
using Contexts;
using Interfaces;
using UnityEngine;

namespace Systems.Health
{
    public class GameplayHealthSystem : IUpdatable
    {
        public void UpdateSystem()
        {
            var healthEntities = GameContexts.Gameplay.GetEntitiesWithComponent<HealthComponent>();

            foreach (var entity in healthEntities)
            {
                if (entity.TryGetComponent<HealthComponent>(out var health))
                {
                    if (health.Health <= 0)
                    {
                        Debug.Log($"Entity {entity} has died.");
                        // Add death animation, remove entity, etc.
                    }
                }
            }
        }
    }
}