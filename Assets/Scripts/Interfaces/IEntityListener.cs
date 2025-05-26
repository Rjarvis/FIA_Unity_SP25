using ECS.Core;

namespace Interfaces
{
    public interface IEntityListener
    {
        void OnEntityCreated(Entity entity);
    }
}