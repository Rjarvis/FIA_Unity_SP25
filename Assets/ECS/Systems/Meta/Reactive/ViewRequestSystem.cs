using System.Collections.Generic;
using ECS.Components;
using ECS.Components.Meta;
using ECS.Components.Meta.Structs;
using ECS.Core;
using ECS.Core.Interfaces;
using UnityEngine;

namespace ECS.Systems.Meta.Reactive
{
    public class ViewRequestSystem : ReactiveSystem<Entity>
    {
        private readonly Context<Entity> _metaContext;

        public ViewRequestSystem(Context<Entity> metaContext) : base(metaContext)//Here
        {
            _metaContext = metaContext;
        }

        protected override ICollector<Entity> GetTrigger(IContext<Entity> context)
        {
            return context.CreateCollector<MetaSignalComponent<ViewRequestPayload>>();
        }

        public override void Execute(List<Entity> entities)
        {
            foreach (var signalEntity in entities)
            {
                var payload = signalEntity.GetComponent<MetaSignalComponent<ViewRequestPayload>>().Payload;

                var context = WorldContainer.Instance.GetContext(payload.contextName);
                if (context == null) continue;

                var target = context.GetEntityByGuid(payload.entityGuid);
                if (target == null || !target.HasComponent<ViewComponent>()) continue;

                var go = target.GetComponent<ViewComponent>().GameObject;
                var renderer = go.GetComponent<SpriteRenderer>() ?? go.AddComponent<SpriteRenderer>();
                var sprite = Resources.Load<Sprite>(payload.imagePath);

                if (sprite != null)
                    renderer.sprite = sprite;
                else
                    Debug.LogWarning($"Sprite not found at path: {payload.imagePath}");

                signalEntity.Destroy(); // Clean up signal
            }
        }

        protected override bool Filter(Entity entity)
        {
            return entity.HasComponent<MetaSignalComponent<ViewRequestPayload>>();
        }

        public override void Activate() { }

        public override void Deactivate() { }

        public override void Clear() { }
    }
}