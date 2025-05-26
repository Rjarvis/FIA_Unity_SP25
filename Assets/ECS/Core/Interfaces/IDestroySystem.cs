namespace ECS.Core.Interfaces
{
    public interface IDestroySystem : ISystem
    {
        void Shutdown(World world);
    }
}