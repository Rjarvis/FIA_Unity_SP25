using Contexts;
using ECS.Core;
using ECS.Systems.Level;
using ECS.Systems.Rendering;
using Systems.Create;

namespace ECS.Systems.Core
{
    public class CoreFeature : Feature
    {
        public CoreFeature(Context context)
        {
            var createSystem = new CreateGameEntitySystem(GameContexts.Gameplay);
            Add(createSystem);
            var levelCreateSystem = new LevelCreateSystem(GameContexts.Level, createSystem);
            Add(levelCreateSystem);
            Add(new ImageApplyReactiveSystem(context));
        }
    }
}