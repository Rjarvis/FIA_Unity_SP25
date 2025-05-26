using ECS.Core;
using ECS.Core.Interfaces;

namespace Systems.Create
{
    public class CreateFeature : Feature
    {
        public CreateFeature(Context<Entity> context)
        {
            Add(new CreateGameEntitySystem(context));
        }
    }
}