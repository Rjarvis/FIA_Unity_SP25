using System;
using Base;
using Components;
using Components.InputComponents;
using Contexts;
using Interfaces;
using Systems;
using Systems.Create;
using Systems.Health;
using Systems.InputSystems;
using Systems.Level;
using Systems.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BootSequence : MonoBehaviourSingleton<BootSequence>
{
    [SerializeField] private SystemController systemController;
    
    #region GameObjects
    public GameObject uiPrefab;
    public GameObject uiInstance;
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
        systemController.InitializeSystems(
            uiPrefab,
            uiInstance,
            playerPrefab,
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
    }
}