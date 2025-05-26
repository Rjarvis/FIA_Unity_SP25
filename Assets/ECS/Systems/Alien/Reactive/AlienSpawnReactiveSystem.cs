using System.Collections.Generic;
using Contexts;
using ECS.Components;
using ECS.Core;
using ECS.Core.Interfaces;
using UnityEngine;

namespace ECS.Systems.Alien.Reactive
{
    public class AlienSpawnReactiveSystem : ReactiveSystem<Entity> 
    {
        private readonly Context<Entity> _alienContext;
        private readonly GameObject _alienPrefab;
        private readonly GameObject _alienBossPrefab;

        public AlienSpawnReactiveSystem(Context<Entity> context, GameObject alienPrefab, GameObject alienBossPrefab) : base(context)
        {
            _alienContext = context;
            _alienPrefab = alienPrefab;
            _alienBossPrefab = alienBossPrefab;
        }

        protected override ICollector<Entity> GetTrigger(IContext<Entity> context)
        {
            return context.CreateCollector<AlienSpawnWaveComponent>();
        }

        protected override bool Filter(Entity entity)
        {
            return entity.HasComponent<AlienSpawnWaveComponent>() &&
                   entity.HasComponent<AlienSpawnConfigComponent>();
        }

        public override void Activate() { }

        public override void Deactivate() { }

        public override void Clear() { }

        public override void Execute(List<Entity> entities)
        {
            foreach (var signalEntity in entities)
            {
                var config = signalEntity.GetComponent<AlienSpawnConfigComponent>();

                for (int i = 0; i < config.WaveSize; i++)
                    SpawnAlien();

                if (config.SpawnBoss)
                    SpawnBoss();

                signalEntity.RemoveComponent<AlienSpawnWaveComponent>();
            }
        }

        private void SpawnAlien()
        {
            var alienCount = _alienContext.GetEntitiesWithComponent<AlienComponent>().Count;
            var newAlien = Object.Instantiate(_alienPrefab);
            newAlien.name = $"Alien_{alienCount}";

            var entity = _alienContext.CreateEntity();
            entity.AddComponent(new AlienComponent { IsBoss = false });
            entity.AddComponent(new ViewComponent { GameObject = newAlien, EntityGuid = entity.Guid });
        }

        private void SpawnBoss()
        {
            var boss = Object.Instantiate(_alienBossPrefab);
            boss.name = "AlienBoss";

            var entity = _alienContext.CreateEntity();
            entity.AddComponent(new AlienComponent { IsBoss = true });
        }
    }
}

