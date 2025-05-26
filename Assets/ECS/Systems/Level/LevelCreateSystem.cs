using System.Collections.Generic;
using ECS.Components;
using ECS.Core;
using ECS.Core.Interfaces;
using Helpers;
using Interfaces;
using Systems.Create;
using Systems.Level.Data;
using UnityEngine;

namespace ECS.Systems.Level
{
    public class LevelCreateSystem : ISystem, IUpdatable
    {
        private readonly Context<Entity> _levelContext;
        private readonly CreateGameEntitySystem _entityCreator;
        private List<LevelData> _levelData;
        private int _levelIndex;
        private GameObject _currentLevel;
        private GameObject _levelCenterObj;

        public LevelCreateSystem(Context<Entity> levelContext, CreateGameEntitySystem entityCreator)
        {
            _levelContext = levelContext;
            _entityCreator = entityCreator;
            
            Initialize();
        }

        public void Initialize()
        {
            _levelData = Helpers.Level.GetLevelData(); // Or inject this for testability
            _levelIndex = 0;

            var data = _levelData[_levelIndex];
            _currentLevel = CreateLevel(data);

            var levelCenter = GameObject.FindGameObjectWithTag("LevelCenter");
            if (levelCenter == null)
            {
                levelCenter = Resources.Load<GameObject>("Prefabs/LevelCenter");
                _levelCenterObj = Object.Instantiate(levelCenter, _currentLevel.transform, true);
            }
        }

        private GameObject CreateLevel(LevelData data)
        {
            var entity = _levelContext.CreateEntity();
            entity.AddComponent(new PositionComponent(Vector3.zero));
            entity.AddComponent(new RadiusComponent(data.radius));
            var go = new GameObject($"Level_{data.level}");
            var entityLinker = go.AddComponent<EntityLinker>();
            entityLinker.SetEntity(entity);
            
            var view = new ViewComponent
            {
                GameObject = go,
                EntityGuid = entity.Guid
            };
            entity.AddComponent(view);
            entity.AddComponent(new ImagePathComponent(data.imagePath));
            entity.AddComponent(new ViewTagComponent("LevelCircle")); // prefab or factory tag

            var renderingContext = WorldContainer.Instance.GetContext("UI");
            var renderingEntity = renderingContext.CreateEntity();
            renderingEntity.AddComponent(new ImageApplyComponent(entity));
            return go;
        }

        public void UpdateSystem()
        {
            // TODO: Replace with a signal-based trigger via ECS
            // if (_levelContext.HasComponent<AdvanceLevelComponent>())
            // {
            //     _levelIndex++;
            //     CreateLevel(_levelData[_levelIndex]);
            // }
        }
    }
}