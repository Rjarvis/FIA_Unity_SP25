using Base;
using Components;
using Components.InputComponents;
using Contexts;
using Helpers.Level;
using Interfaces;
using UnityEngine;

namespace Systems.Player
{
    public class PlayerCreateSystem : MonoBehaviourSingleton<PlayerCreateSystem>, IUpdatable
    {
        public GameObject playerPrefab;
        private GameObject playerInstance;
        private GameObject centerObj;
        private float radius;

        public void Initialize()
        {
            // Find level center
            centerObj = GameObject.FindGameObjectWithTag("LevelCenter");
            if (!centerObj) centerObj = Resources.Load<GameObject>("Prefabs/LevelCenter");
            

            // Load the PlayerPrefab from the BootSequence or the Resources folder 
            playerPrefab = BootSequence.Instance.playerPrefab ? BootSequence.Instance.playerPrefab : Resources.Load<GameObject>("Prefabs/PlayerPrefab");  
            if (!playerPrefab)
            {
                Debug.LogError("PlayerPrefab not found in Resources/Prefabs.");
                return;
            }

            // Instantiate player
            playerInstance = Instantiate(playerPrefab);
            playerInstance.name = "PlayerObject";
            
            var levelObj = GameObject.Find("LevelEntity_1");
            if (!levelObj || !levelObj.TryGetComponent(out LevelComponent levelComponent))
            {
                Debug.LogError("Level entity with radius not found.");
                return;
            }
            // Set initial position at the orbit radius on x-axis
            radius = levelComponent.Radius;
            playerInstance.transform.position = centerObj.transform.position + new Vector3(radius, 0f, 0f);

            // Set the image component to the player object
            var imageComponent = playerInstance.AddComponent<ImageComponent>();
            imageComponent.Initialize(Helpers.Data.PlayerSpritePath);
            imageComponent.SetContext(Contexts.GameContexts.Player);
            
            // Add the EntityComponent to the player obj
            var entityComponent = playerInstance.AddComponent<EntityComponent>();
            entityComponent.SetContext(Contexts.GameContexts.Player);
            
            // Create and attach ECS-style data component
            var playerComponent = new PlayerComponent { Level = 0 };
            entityComponent.SetContext(Contexts.GameContexts.Player);
            entityComponent.AddComponent(playerComponent);


            var shootComponent = new ShootComponent { cooldownTime = 0.025f, lastShotTime = 0f};
            shootComponent.SetContext(GameContexts.Player);
            entityComponent.AddComponent(shootComponent);
            
            // Notify the EntitySystem
            EntitySystem.NotifyComponentAdded(entityComponent, shootComponent);
            EntitySystem.NotifyComponentAdded(entityComponent, playerComponent);
            EntitySystem.NotifyComponentAdded(entityComponent, imageComponent);
        }

        public void RegisterPlayerDataToMoveSystem()
        {
            // Register data to movement system
            var moveSystem = PlayerMovementSystem.Instance;
            moveSystem.gameObject.transform.SetParent(gameObject.transform);
            moveSystem.PlayerTransform = playerInstance;
            moveSystem.CenterPoint = centerObj;
            moveSystem.radius = radius;
            moveSystem.Initialize();
            
            EntitySystem.RegisterSystem(GameContexts.Player, moveSystem);
        }


        public void UpdateSystem() { }
    }
}