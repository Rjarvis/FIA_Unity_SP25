using System;
using ECS.Core;
using UnityEngine;

namespace ECS.Components
{
    public class ViewComponent : IComponent
    {
        public GameObject GameObject;
        public Transform Transform => GameObject?.transform;
        public Guid EntityGuid;
    }
}