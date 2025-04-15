using UnityEngine;

namespace Components
{
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
}