using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Components
{
    public class BulletComponent : MonoBehaviour
    {
        public Vector3 direction;
        public float speed = 10f;
    }
    
    public class HealthComponent : MonoBehaviour
    {
        public int Health;

        public HealthComponent(int value)
        {
            Health = value;
        }
    }

    public class MovementComponent : MonoBehaviour
    {
        public float Speed;
    }

    public class PlayerComponent : MonoBehaviour
    {
        public int Level;
    }

    public class AlienComponent : MonoBehaviour
    {
        public int Health;
        public bool isAlive;
        public bool isBoss;
    }
}