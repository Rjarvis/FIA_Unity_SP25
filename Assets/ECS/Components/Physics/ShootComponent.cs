using ECS.Core;

namespace ECS.Components
{
    public class ShootComponent : IComponent
    {
        public float cooldownTime;
        public float lastShotTime;
    }
}