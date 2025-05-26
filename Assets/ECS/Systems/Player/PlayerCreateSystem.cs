using System.Linq;
using ECS.Core;
using ECS.Core.Interfaces;
using ECS.Components;
using ECS.Components.Meta;
using ECS.Components.Meta.Structs;
using ECS.ECS_Helpers;
using Systems.Level.Data;
using UnityEngine;

namespace ECS.Systems.Player
{
    public class PlayerCreateSystem : ICreateSystem
    {
        private readonly Context _playerContext;
        private readonly Context _gameplayContext;
        private readonly GameObject _playerPrefab;
        private GameObject _levelCenterObj;

        public PlayerCreateSystem(Context playerContext, Context gameplayContext, GameObject playerPrefab)
        {
            _playerContext = playerContext;
            _gameplayContext = gameplayContext;
            _playerPrefab = playerPrefab;
        }

        public void OnEntityCreated(Entity entity, World world)
        {
            throw new System.NotImplementedException();
            //Maybe Entity tracking or flag components here?
        }

        public void Initialize(World world)
        {
            _levelCenterObj = GameObject.FindGameObjectWithTag("LevelCenter") 
                              ?? Object.Instantiate(Resources.Load<GameObject>("Prefabs/LevelCenter"));

            var levelContext = WorldContainer.Instance.GetContext("Level");
            var levelEntity = levelContext.GetEntitiesWithComponent<ViewTagComponent>().FirstOrDefault(entity =>
            {
                entity.TryGetComponent(out ViewTagComponent viewTagComponent);
                return viewTagComponent.Tag.Equals("LevelCircle");
            });
            var levelRadiusComponent = levelEntity?.GetComponent<RadiusComponent>();
            if (levelRadiusComponent == null) return;

            float radius = levelRadiusComponent.Value;

            var playerGO = Object.Instantiate(_playerPrefab);
            playerGO.name = "PlayerObject";
            playerGO.transform.position = _levelCenterObj.transform.position + new Vector3(radius, 0, 0);

            var playerEntity = _playerContext.CreateEntity();

            playerEntity.AddComponent(new ViewComponent { GameObject = playerGO });
            playerEntity.AddComponent(new PlayerComponent { Level = 7 });
            playerEntity.AddComponent(new ShootComponent { cooldownTime = 0.025f, lastShotTime = 0f });
            playerEntity.AddComponent(new OrbitComponent
            {
                Center = _levelCenterObj,
                Radius = radius,
                Angle = 0f,
                OrbitSpeed = 90f
            });
            playerEntity.AddComponent(new ImagePathComponent(Helpers.Data.PlayerSpritePath));
            MetaSignalHelper.Signal(new ViewRequestPayload
            {
                contextName = "Player",
                entityGuid = playerEntity.Guid,
                imagePath = Helpers.Data.PlayerSpritePath
            });
        }
    }
}