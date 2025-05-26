using ECS.Core;
using UnityEngine;

namespace ECS.Components
{
    public class BulletComponent : IComponent
    {
        public Vector3 direction;
        public float speed = 10f;
    }
}