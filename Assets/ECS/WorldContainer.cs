namespace ECS
{
    public static class WorldContainer
    {
        public static World Instance { get; private set; }

        public static void SetWorld(World world)
        {
            Instance = world;
        }
    }
}