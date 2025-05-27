using Base;
using Contexts;
using Systems;
using UnityEngine;

public class BootSequence : MonoBehaviourSingleton<BootSequence>
{
    [SerializeField] private SystemController systemController;
    
    #region GameObjects
    public GameObject uiPrefab;
    public GameObject playerPrefab;
    public GameObject crosshairPrefab;
    public GameObject bulletPrefab;
    #endregion

    #region Sounds

    public AudioClip alienDied;
    public AudioClip bulletPew;
    public AudioClip planetHit;
    public AudioClip alienSound;
    public AudioClip alienBossSound;

    #endregion
    
    protected UISystem uiSystem;
    protected UIButtonListenerSystem uiButtonListenerSystem;
    public GameObject alienPrefab;
    public GameObject alienBoss;


    void Start()
    {
        InitializeContexts();
        systemController = gameObject.AddComponent<SystemController>();
        uiSystem = gameObject.GetComponent<UISystem>();
        uiButtonListenerSystem = gameObject.GetComponent<UIButtonListenerSystem>();
        systemController.InitializeSystems(
            uiPrefab,
            crosshairPrefab,
            bulletPrefab,
            uiSystem,
            uiButtonListenerSystem
            );
    }

    private void Update()
    {
        systemController.UpdateSystems();
    }

    private void OnDestroy()
    {
        systemController.ShutdownSystems();
    }

    private void InitializeContexts()
    {
        // Initialize different contexts
        GameContexts.Gameplay = EntitySystem.CreateAndRegisterContext("Gameplay");
        GameContexts.Create = EntitySystem.CreateAndRegisterContext("Create");
        GameContexts.UI = EntitySystem.CreateAndRegisterContext("UI");
        GameContexts.Physics = EntitySystem.CreateAndRegisterContext("Physics");
        GameContexts.Input = EntitySystem.CreateAndRegisterContext("Input");
        GameContexts.Level = EntitySystem.CreateAndRegisterContext("Level");
        GameContexts.Player = EntitySystem.CreateAndRegisterContext("Player");
        GameContexts.Alien = EntitySystem.CreateAndRegisterContext("Alien");
        GameContexts.Sound = EntitySystem.CreateAndRegisterContext("Sound");

        GameContexts.AllContexts = new Context[]
        {
            GameContexts.Gameplay,
            GameContexts.Create,
            GameContexts.UI,
            GameContexts.Physics,
            GameContexts.Input,
            GameContexts.Level,
            GameContexts.Player,
            GameContexts.Alien,
            GameContexts.Sound
        };
    }
}