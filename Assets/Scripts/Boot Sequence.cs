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
        GameContexts.Gameplay = EntitySystem.CreateAndRegisterContext();
        GameContexts.Create = EntitySystem.CreateAndRegisterContext();
        GameContexts.UI = EntitySystem.CreateAndRegisterContext();
        GameContexts.Physics = EntitySystem.CreateAndRegisterContext();
        GameContexts.Input = EntitySystem.CreateAndRegisterContext();
        GameContexts.Level = EntitySystem.CreateAndRegisterContext();
        GameContexts.Player = EntitySystem.CreateAndRegisterContext();
        GameContexts.Alien = EntitySystem.CreateAndRegisterContext();
    }
}