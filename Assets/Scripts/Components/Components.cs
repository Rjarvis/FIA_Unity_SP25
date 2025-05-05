using UnityEngine;

namespace Components
{
    public class BulletComponent : EntityComponent
    {
        public Vector3 direction;
        public float speed = 10f;
    }
    
    public class HealthComponent : EntityComponent
    {
        public int Health;

        public HealthComponent(int value)
        {
            Health = value;
        }
    }

    public class MovementComponent : EntityComponent
    {
        public float Speed;
    }

    public class PlayerComponent : EntityComponent
    {
        public int Level;
    }
}