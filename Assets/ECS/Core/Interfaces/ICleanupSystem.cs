namespace ECS.Core.Interfaces
{
    /// <summary>
    /// Called after main update loop to perform cleanup (e.g. removing entities, freeing resources).
    /// </summary>
    public interface ICleanupSystem : ISystem
    {
        void Cleanup();
    }
}