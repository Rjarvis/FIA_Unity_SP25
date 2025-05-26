namespace ECS.Core.Interfaces
{
    public interface IGroup
    {
        int count { get; }
        void RemoveAllEventHandlers();
    }
}