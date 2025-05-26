using System;
using ECS.Core;
using UnityEngine;

namespace Helpers
{
    public class EntityLinker : MonoBehaviour
    {
        private Entity entity;
        private Guid Guid { get; set; }
        public string GuidDisplay;

        public void SetEntity(Entity entity)
        {
            this.entity = entity;
            Guid = entity.Guid;
            GuidDisplay = Guid.ToString();
        }
    }
}