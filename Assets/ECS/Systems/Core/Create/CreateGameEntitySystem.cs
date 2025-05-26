using System;
using System.Collections.Generic;
using ECS.Core;
using ECS.Core.Interfaces;
using UnityEngine;
using ECS.Components;
using Systems.Level.Data;
using Unity.VisualScripting;
using UnityEditor;

namespace Systems.Create
{
    public class CreateGameEntitySystem : ReactiveSystem<Entity>
    {
        private readonly Context<Entity> _context;
        private int _entityCount = 0;

        public CreateGameEntitySystem(Context<Entity> context) : base(context)
        {
            _context = context;
        }
        
        protected override ICollector<Entity> GetTrigger(IContext<Entity> context) => context.CreateCollector<CreateGameEntityComponent>();
        
        protected override bool Filter(Entity entity)
        {
            Debug.Log("got here in CreateGameEntitySystem");
            return entity.HasComponent<CreateGameEntityComponent>();
        }

        public Entity Create(Vector3 position, Color color, int health)
        {
            var entity = _context.CreateEntity();

            entity.AddComponent(new PositionComponent(position));
            entity.AddComponent(new HealthComponent(health));
            entity.AddComponent(new ColorComponent(color));
            // entity.AddComponent(new SizeComponent(100f, 100f)); // if size is data driven
            entity.AddComponent(new ViewTagComponent("Box")); // tells view system to use a red box prefab
            // entity.AddComponent(new ClickableComponent()); // if intended to be interactive

            return entity;
        }

        public Entity CreateLevelEntity(Vector3 position, float radius)
        {
            var entity = _context.CreateEntity();
            entity.AddComponent(new PositionComponent(position));
            entity.AddComponent(new RadiusComponent(radius));
            entity.AddComponent(new ViewTagComponent("LevelCircle")); // tells ViewSystem what prefab to use
            return entity;
        }

        public Entity CreateLevelEntity(LevelData levelData)
        {
            var entity = _context.CreateEntity();
            entity.AddComponent(new PositionComponent(Vector3.zero));
            entity.AddComponent(new RadiusComponent(levelData.radius));
            entity.AddComponent(new ImagePathComponent(levelData.imagePath));
            entity.AddComponent(new ViewTagComponent("LevelCircle")); // prefab or factory tag
            return entity;
        }

        public override void Execute(List<Entity> entities)
        {
            Debug.Log($"Got here with entities.Count:{entities.Count}");
            // foreach (var entity in entities)
            // {
            //     if (entity.HasComponent<LevelDataComponent>())
            //     {
            //         var levelDataComp = entity.GetComponent<LevelDataComponent>();
            //         var levelData = Helpers.Level.GetLevelData()[levelDataComp.Level];
            //         CreateLevelEntity(levelData);
            //         Debug.Log("LevelEntityCreated");
            //     }
            // }
        }

        public override void Activate() { }

        public override void Deactivate() { }

        public override void Clear() { }
    }
}
