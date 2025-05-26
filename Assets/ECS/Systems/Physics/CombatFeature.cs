using ECS.Core;
using ECS.Systems.Player;

namespace ECS.Systems.Physics
{
    public class CombatFeature : Feature
    {
        public CombatFeature(Context physics)
        {
            Add(new BulletSystem(physics, WorldContainer.Instance.GetContext("Gameplay")));
            Add(new BulletCollisionSystem(physics));
        }
    }
}