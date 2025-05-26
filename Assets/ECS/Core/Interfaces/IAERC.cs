namespace ECS.Core.Interfaces
{
    public interface IAERC
    {
        int RetainCount { get; }
        void Retain(object owner);
        void Release(object owner);
    }
}