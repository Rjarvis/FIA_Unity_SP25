namespace ECS.Core.Interfaces
{
    /// <summary>
    /// Called when an entity is created in a context.
    /// </summary>
    public interface ICreateSystem : ISystem
    {
        void Initialize(World world);
    }
}