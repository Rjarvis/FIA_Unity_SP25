using ECS.Core;
using ECS.Core.Interfaces;
using UnityEngine;

namespace ECS.Systems
{
    public class AlienMovementSystem : IUpdateSystem
    {
        private readonly Context _context;
        public AlienMovementSystem(Context context)
        {
            _context = context;
        }

        public void Initialize()
        {
            Debug.Log("AlienMovementSystem Initialized!");
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}