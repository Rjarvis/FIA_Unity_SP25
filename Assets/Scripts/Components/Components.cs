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
        public int HealthPool;
        public int Health;

        public HealthComponent(int value)
        {
            Health = value;
            HealthPool = value;
        }

        public void Add(int value) => Health += value;
        public void Subtract(int value) => Health -= value;
        
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

    public class ScoreComponent : MonoBehaviour
    {
        public int Score;
    }
}