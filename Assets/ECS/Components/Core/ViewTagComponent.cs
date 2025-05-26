using ECS.Core;

namespace ECS.Components
{
    public class ViewTagComponent : IComponent
    {
        public string Tag;
        public ViewTagComponent(string tag) => Tag = tag;
    }
}