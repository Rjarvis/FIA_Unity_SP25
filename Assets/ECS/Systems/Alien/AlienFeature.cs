using ECS.Core;
using ECS.Core.Interfaces;
using ECS.Systems.Alien.Reactive;

namespace ECS.Systems
{
    public class AlienFeature : Feature
    {
        public AlienFeature(Context context)
        {
            Add(new AlienMovementSystem(context));
            Add(new AlienSpawnReactiveSystem(context, BootSequence.Instance.alienPrefab, BootSequence.Instance.alienBoss));
        }
    }
}