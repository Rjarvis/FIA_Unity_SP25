namespace ECS.Core.Interfaces
{
    public interface IEntityIndex
    {
        string name { get; }
        void Activate();
        void Deactivate();
    }
}