using ECS.Core;
using UnityEngine;

namespace ECS.Components
{
    public class ColorComponent : IComponent
    {
        public Color Value;
        public ColorComponent(Color value) => Value = value;
    }
}