using ECS.Core;
using UnityEngine;

namespace ECS.Components
{
    public class OrbitComponent : IComponent
    {
        public GameObject Center;
        public float Radius;
        public float Angle;
        public float OrbitSpeed;
    }
}