using ECS.Core;
using UnityEngine;

namespace ECS.Components
{
    public class PositionComponent : IComponent
    {
        public Vector3 Value;
        public PositionComponent(Vector3 value) => Value = value;
    }
}