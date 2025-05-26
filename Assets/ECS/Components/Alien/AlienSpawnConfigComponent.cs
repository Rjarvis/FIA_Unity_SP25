using ECS.Core;

namespace ECS.Components
{
    public class AlienSpawnConfigComponent : IComponent
    {
        public int WaveSize;
        public bool SpawnBoss;
    }
}