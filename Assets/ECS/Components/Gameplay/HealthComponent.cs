using ECS.Core;

namespace ECS.Components
{
    public class HealthComponent : IComponent
    {
        public int Value;
        public HealthComponent(int value) => Value = value;
    }
}