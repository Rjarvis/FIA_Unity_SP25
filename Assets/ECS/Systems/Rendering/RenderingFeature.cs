using ECS.Core;

namespace ECS.Systems.Rendering
{
    public class RenderingFeature : Feature
    {
        public RenderingFeature(Context context)
        {
            Add(new ImageApplyReactiveSystem(context));
        }
    }
}