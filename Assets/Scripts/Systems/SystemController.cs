using System;
using Contexts;
using Interfaces;
using Systems.Create;
using Systems.Health;
using Systems.InputSystems;
using Systems.Level;
using Systems.Player;
using UnityEngine;

namespace Systems
{
    public class SystemController : MonoBehaviour
    {
        public BulletSystem BulletSystem;
        public ShootingSystem ShootingSystem;
        public PlayerMovementSystem MovementSystem;
        public CrosshairSystem CrosshairSystem;
        public PlayerCreateSystem PlayerCreateSystem;
        public LevelCreateSystem LevelCreateSystem;
        public UIButtonListenerSystem UIButtonListenerSystem;
        public UISystem UISystem;

        public void UpdateSystems() => EntitySystem.Update();

        public void Update()
        {
            UpdateSystems();
        }

        public void ShutdownSystems()
        {
            Debug.Log("Shutting down systems...");
        
            EntitySystem.UnRegisterSystem<EntityClickSystem>(GameContexts.Input);
            EntitySystem.UnRegisterSystem<CreateGameEntitySystem>(GameContexts.Create);
            EntitySystem.UnRegisterSystem<UISystem>(GameContexts.UI);
            EntitySystem.UnRegisterSystem<UIButtonListenerSystem>(GameContexts.UI);
            EntitySystem.UnRegisterSystem<GameplayHealthSystem>(GameContexts.Gameplay);
            EntitySystem.UnRegisterSystem<PlayerCreateSystem>(GameContexts.Player);
            EntitySystem.UnRegisterSystem<PlayerMovementSystem>(GameContexts.Player);
            EntitySystem.UnRegisterSystem<CrosshairSystem>(GameContexts.Input);
            EntitySystem.UnRegisterSystem<BulletSystem>(GameContexts.Gameplay);
            EntitySystem.UnRegisterSystem<ShootingSystem>(GameContexts.Player);
        }

        public void InitializeSystems(GameObject uiPrefab, GameObject uiInstance, GameObject playerPrefab,
            GameObject crosshairPrefab, GameObject bulletPrefab, UISystem uiSystem,
            UIButtonListenerSystem uiButtonListenerSystem)
        {
            // Ensure an instance of CreateGameEntitySystem exists
            GameObject createGameSystemObj = null;
            if (FindObjectOfType<CreateGameEntitySystem>() == null)
            {
                createGameSystemObj = new GameObject("CreateGameEntitySystem");
                createGameSystemObj.AddComponent<CreateGameEntitySystem>();
            }

            var entityCreator = createGameSystemObj.GetComponent<CreateGameEntitySystem>();
            EntitySystem.RegisterSystem(GameContexts.Create, entityCreator);

            // Initialize UI System
            if (uiSystem == null)
                uiSystem = FindFirstObjectByType<UISystem>();

            if (uiSystem != null)
            {
                uiSystem.Initialize(uiPrefab);
                EntitySystem.RegisterSystem(GameContexts.UI, uiSystem);
                uiInstance = uiSystem.GetInstance();
                UISystem = uiSystem;
                
                // Initialize button events
                uiButtonListenerSystem = FindFirstObjectByType<UIButtonListenerSystem>();
                if (uiButtonListenerSystem != null)
                {
                    uiButtonListenerSystem.Initialize(uiInstance, entityCreator);
                    EntitySystem.RegisterSystem(GameContexts.UI, uiButtonListenerSystem);
                    UIButtonListenerSystem = uiButtonListenerSystem;
                }
            }
            else
            {
                Debug.LogError("UISystem not found in scene.");            
            }
            
            // Initialize LevelCreateSystem
            GameObject levelCreateObj = new GameObject("LevelCreateSystem");
            var levelCreateSystem = levelCreateObj.AddComponent<LevelCreateSystem>();
            levelCreateSystem.Initialize(entityCreator);
            EntitySystem.RegisterSystem(GameContexts.Gameplay, levelCreateSystem);
            LevelCreateSystem = levelCreateSystem;
            
            // Initialize PlayerCreateSystem
            PlayerCreateSystem playerCreateSystem = Systems.Player.Initial.InitializePlayerCreateSystems.Instance.Initialize();
            EntitySystem.RegisterSystem(GameContexts.Player, playerCreateSystem);
            PlayerCreateSystem = playerCreateSystem;
            
            // Initialize CrosshairSystem
            CrosshairSystem = InputSystems.Initial.InitializeInputSystems.Instance.Initialize(crosshairPrefab, uiInstance);


            //Register the playerData to the move system
            playerCreateSystem.RegisterPlayerDataToMoveSystem();
            MovementSystem = PlayerMovementSystem.Instance;

            // Find EntityClickSystem and assign it
            EntityClickSystem clickSystem = FindFirstObjectByType<EntityClickSystem>();
            if (clickSystem == null)
            {
                GameObject clickSystemObj = new GameObject("EntityClickSystem");
                clickSystem = clickSystemObj.AddComponent<EntityClickSystem>();
                EntitySystem.RegisterSystem(GameContexts.Input, clickSystem);
            }
            
            GameObject shootingSystemObj = new GameObject("ShootingSystem");
            ShootingSystem = shootingSystemObj.AddComponent<ShootingSystem>();
            BulletSystem = shootingSystemObj.AddComponent<BulletSystem>();
            
            var crosshairTransform = CrosshairSystem.Instance.crosshairUI;
            ShootingSystem.Initialize(Camera.main, crosshairTransform, bulletPrefab);
            
            EntitySystem.RegisterSystem(GameContexts.Player, ShootingSystem);
            EntitySystem.RegisterSystem(GameContexts.Physics, BulletSystem);
            EntitySystem.RegisterSystem(GameContexts.UI, CrosshairSystem);
        }
    }
}