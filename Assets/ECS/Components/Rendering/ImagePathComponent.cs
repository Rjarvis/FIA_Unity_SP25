using ECS.Core;

namespace ECS.Components
{
    public class ImagePathComponent : IComponent
    {
        public string Value;
        public ImagePathComponent(string value) => Value = value;
    }
}