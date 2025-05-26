using ECS.Core;
using ECS.Systems.Meta.Reactive;

namespace ECS.Systems.Meta
{
    public class MetaFeature : Feature
    {
        public MetaFeature(Context<Entity> context)
        {
            Add(new ViewRequestSystem(context));
        }
    }
}