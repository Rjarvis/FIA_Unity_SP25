using ECS.Core;
using ECS.Core.Interfaces;

namespace ECS.Components
{
    public class CollisionComponent : IComponent
    {
        public Entity Other;
    }
}