using System.Collections.Generic;
using ECS.Core;
using UnityEditor;
using UnityEngine;
using ECS.Components;
using ECS.Core.Interfaces;

namespace ECS.Systems.Rendering
{
    public class ImageApplyReactiveSystem : ReactiveSystem<Entity>
    {
        private Context _context;

        public ImageApplyReactiveSystem(Context context) : base(context)
        {
            _context = context;
        }
        
        public override void Activate() { }
        public override void Deactivate() { }
        public override void Clear() {}

        protected override ICollector<Entity> GetTrigger(IContext<Entity> context)
        {
            return context.CreateCollector<ImageApplyComponent>();
        }
        
        protected override bool Filter(Entity entity)
        {
            var containedEntity = entity.GetComponent<ImageApplyComponent>().EntityToApplyImageTo;
            Debug.Log($"ImageApply with an entity that has ImagePath:{entity.HasComponent<ImagePathComponent>()} and View:{entity.HasComponent<ViewComponent>()} ");
            return containedEntity.HasComponent<ImagePathComponent>() && containedEntity.HasComponent<ViewComponent>();
        }

        public override void Execute(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var view = entity.GetComponent<ViewComponent>();
                var image = entity.GetComponent<ImagePathComponent>();
                Sprite sprite;

#if UNITY_EDITOR
                sprite = AssetDatabase.LoadAssetAtPath<Sprite>(image.Value);
#else
        sprite = Resources.Load<Sprite>(image.Value); // Assuming inside Resources
#endif

                if (sprite == null)
                {
                    Debug.LogWarning($"[ImageApplyReactiveSystem] Sprite at path '{image.Value}' could not be loaded.");
                    return;
                }

                var renderer = view.GameObject.GetComponent<SpriteRenderer>();
                if (renderer == null)
                {
                    renderer = view.GameObject.AddComponent<SpriteRenderer>();
                }

                renderer.sprite = sprite;

                // Prevent re-processing
                entity.RemoveComponent<ImagePathComponent>();
            }
        }
    }

}