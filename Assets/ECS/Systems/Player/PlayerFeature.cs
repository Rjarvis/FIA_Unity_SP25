using ECS.Core;

namespace ECS.Systems.Player
{
    public class PlayerFeature : Feature
    {
        public PlayerFeature(Context context)
        {
            var world = WorldContainer.Instance;
            var gameContext = world.GetContext("Gameplay");
            var playerPrefab = BootSequence.Instance.playerPrefab;
            Add(new PlayerCreateSystem(context, gameContext, playerPrefab));
            Add(new PlayerMovementSystem(context));
            var physicsContext = world.GetContext("Physics");//Is this a safe way to do this?
            Add(new BulletSystem(physicsContext, gameContext));
        }
    }
}