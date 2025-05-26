using ECS.Core;

namespace ECS.Components
{
    public class RadiusComponent : IComponent
    {
        public float Value;
        public RadiusComponent(float value) => Value = value;
    }
}