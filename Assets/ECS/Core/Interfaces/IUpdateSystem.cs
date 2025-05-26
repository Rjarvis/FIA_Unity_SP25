namespace ECS.Core.Interfaces
{
    /// <summary>
    /// Called every frame to update logic.
    /// </summary>
    public interface IUpdateSystem : ISystem
    {
        void Initialize();
        void Update();
    }
}