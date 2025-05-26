using System.Collections.Generic;

namespace ECS.Core.Interfaces
{
    public interface IReactiveSystem : ISystem
    {
        void Activate();
        void Deactivate();
        void Clear();
        void Update();
    }
}