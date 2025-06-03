using Contexts;
using Systems.Create;
using Systems.Enemy;
using Systems.GamePlay;
using Systems.Health;
using Systems.InputSystems;
using Systems.Level;
using Systems.Player;
using Systems.Player.Initial;
using Systems.Sound;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems
{
    public class SystemController : MonoBehaviour
    {
        public BulletSystem bulletSystem;
        public ShootingSystem shootingSystem;
        public PlayerMovementSystem movementSystem;
        public CrosshairSystem crosshairSystem;
        public PlayerCreateSystem playerCreateSystem;
        public LevelCreateSystem levelCreateSystem;
        public UIButtonListenerSystem uiButtonListenerSystem;
        public UISystem uiSystem;
        public ScoreSystem scoreSystem;

        private CreateGameEntitySystem entityCreator;

        public void UpdateSystems() => EntitySystem.Update();

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
            EntitySystem.UnRegisterSystem<AlienSystem>(GameContexts.Alien);
        }

        public void InitializeSystems(GameObject uiPrefab,
            GameObject crosshairPrefab, GameObject bulletPrefab, UISystem uiSystem,
            UIButtonListenerSystem uiButtonListenerSystem)
        {
            InitializeCreateGameEntitySystem();
            InitializeUISystems(uiPrefab, uiSystem);
            InitializeSoundSystem();
            InitializeButtonEvents(this.uiSystem.GetInstance(), uiButtonListenerSystem);
            InitializeLevelCreateSystem();
            InitializePlayerSystems();
            InitializeCrosshairSystem(crosshairPrefab, this.uiSystem.GetInstance());
            InitializePlayerMovementSystems();
            InitializeClickSystem();
            InitializeShootingSystem(bulletPrefab);
            InitializeScoringSystem(uiSystem);
            
            InitializeAlienSystem();
        }

        private void InitializeScoringSystem(UISystem uiSystem)
        {
            scoreSystem = this.AddComponent<ScoreSystem>();
            scoreSystem.Initialize(uiSystem, GameContexts.Gameplay);
        }

        private void InitializeSoundSystem()
        {
            var soundSystem = Camera.main.gameObject.AddComponent<SoundSystem>();
            soundSystem.alienBoss = BootSequence.Instance.alienBossSound;
            soundSystem.alienDied = BootSequence.Instance.alienDied;
            soundSystem.alienSound = BootSequence.Instance.alienSound;
            soundSystem.bulletPew = BootSequence.Instance.bulletPew;
            soundSystem.planetHit = BootSequence.Instance.planetHit;

            EntitySystem.RegisterSystem(GameContexts.Sound, soundSystem);
        }

        private void InitializeAlienSystem()
        {
            GameObject alienSystemObj = new GameObject("AlienSystem");
            var alienSystem = alienSystemObj.AddComponent<AlienSystem>();
            EntitySystem.RegisterSystem(GameContexts.Alien, alienSystem);
        }

        private void InitializeShootingSystem(GameObject bulletPrefab)
        {
            GameObject shootingSystemObj = new GameObject("ShootingSystem");
            shootingSystem = shootingSystemObj.AddComponent<ShootingSystem>();
            var crosshairTransform = CrosshairSystem.Instance.crosshairUI;
            shootingSystem.Initialize(Camera.main, crosshairTransform, bulletPrefab);
            bulletSystem = shootingSystemObj.AddComponent<BulletSystem>();
            
            EntitySystem.RegisterSystem(GameContexts.Player, shootingSystem);
            EntitySystem.RegisterSystem(GameContexts.Physics, bulletSystem);
            
        }

        private void InitializeClickSystem()
        {
            EntityClickSystem clickSystem = FindFirstObjectByType<EntityClickSystem>();
            if (clickSystem == null)
            {
                GameObject clickSystemObj = new GameObject("EntityClickSystem");
                clickSystem = clickSystemObj.AddComponent<EntityClickSystem>();
                EntitySystem.RegisterSystem(GameContexts.Input, clickSystem);
            }
        }

        private void InitializePlayerMovementSystems()
        {
            playerCreateSystem.RegisterPlayerDataToMoveSystem();
            movementSystem = PlayerMovementSystem.Instance;
        }

        private void InitializeCrosshairSystem(GameObject crosshairPrefab, GameObject uiInstance)
        {
            crosshairSystem = InputSystems.Initial.InitializeInputSystems.Instance.Initialize(crosshairPrefab, uiInstance);
            EntitySystem.RegisterSystem(GameContexts.UI, crosshairSystem);
        }

        private void InitializePlayerSystems()
        {
            PlayerCreateSystem playerCreateSystem = InitializePlayerCreateSystems.Instance.Initialize();
            EntitySystem.RegisterSystem(GameContexts.Player, playerCreateSystem);
            this.playerCreateSystem = playerCreateSystem;
        }

        private void InitializeLevelCreateSystem()
        {
            GameObject levelCreateObj = new GameObject("LevelCreateSystem");
            var levelCreateSystem = levelCreateObj.AddComponent<LevelCreateSystem>();
            levelCreateSystem.Initialize(entityCreator);
            EntitySystem.RegisterSystem(GameContexts.Gameplay, levelCreateSystem);
            this.levelCreateSystem = levelCreateSystem;
        }

        private void InitializeCreateGameEntitySystem()
        {
            var createGameSystemObj = new GameObject("CreateGameEntitySystem");
            entityCreator = createGameSystemObj.AddComponent<CreateGameEntitySystem>();
            EntitySystem.RegisterSystem(GameContexts.Create, entityCreator);
        }

        private void InitializeUISystems(GameObject uiPrefab, UISystem system)
        {
            system.Initialize(uiPrefab);
            EntitySystem.RegisterSystem(GameContexts.UI, system);
            uiSystem = system;
        }

        private void InitializeButtonEvents(GameObject uiInstance, UIButtonListenerSystem uiButtonListenerSystem)
        {
            uiButtonListenerSystem = FindFirstObjectByType<UIButtonListenerSystem>();
            if (uiButtonListenerSystem != null)
            {
                uiButtonListenerSystem.Initialize(uiInstance, entityCreator);
                EntitySystem.RegisterSystem(GameContexts.UI, uiButtonListenerSystem);
                this.uiButtonListenerSystem = uiButtonListenerSystem;
            }
        }
    }
}