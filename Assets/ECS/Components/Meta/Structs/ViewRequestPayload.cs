using System;

namespace ECS.Components.Meta.Structs
{
    public struct ViewRequestPayload
    {
        public string contextName;
        public Guid entityGuid;
        public string imagePath;
    }
}