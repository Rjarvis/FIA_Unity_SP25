using ECS.Core;

namespace ECS.Components
{
    public class ImageApplyComponent : IComponent
    {
        public Entity EntityToApplyImageTo;

        public ImageApplyComponent(Entity entity)
        {
            EntityToApplyImageTo = entity;
        }
    }
}