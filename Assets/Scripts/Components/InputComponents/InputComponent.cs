namespace Components.InputComponents
{
    public class InputComponent : EntityComponent
    {
    }

    public class ShootComponent : EntityComponent
    {
        public float cooldownTime;
        public float lastShotTime;
    }
}