using Base;
using Components;
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
    #region GameObjects
    public GameObject uiPrefab;
    public GameObject uiInstance;
    public GameObject playerPrefab;
    public GameObject crosshairPrefab;
    #endregion
    
    protected UISystem uiSystem;
    protected UIButtonListenerSystem uiButtonListenerSystem;
    

    void Start()
    {
        InitializeContexts();
        StartSystems();
    }

    void Update()
    {
        UpdateSystems();
    }

    private void OnDestroy()
    {
        EndSystems();
    }

    private void InitializeContexts()
    {
        // Initialize different contexts
        GameContexts.Gameplay = new Context();
        GameContexts.Create = new Context();
        GameContexts.UI = new Context();
        GameContexts.Physics = new Context();
        GameContexts.Input = new Context();
        GameContexts.Level = new Context();
        GameContexts.Player = new Context();
    }

    private void StartSystems()
    {
        RegisterSystems();
        InitializeSystems();
    }

    private void InitializeSystems()
    {
        // Ensure an instance of CreateGameEntitySystem exists
        GameObject createGameSystemObj = null;
        if (FindObjectOfType<CreateGameEntitySystem>() == null)
        {
            createGameSystemObj = new GameObject("CreateGameEntitySystem");
            createGameSystemObj.AddComponent<CreateGameEntitySystem>();
        }

        var entityCreator = createGameSystemObj.GetComponent<CreateGameEntitySystem>();

        // Initialize UI System
        uiSystem = FindFirstObjectByType<UISystem>();
        if (uiSystem != null)
        {
            uiSystem.Initialize(uiPrefab);
            uiInstance = uiSystem.GetInstance();
            
            // Initialize button events
            uiButtonListenerSystem = FindFirstObjectByType<UIButtonListenerSystem>();
            if (uiButtonListenerSystem != null) uiButtonListenerSystem.Initialize(uiInstance, entityCreator);
        }
        else
        {
            Debug.LogError("UISystem not found in scene.");            
        }
        
        // Initialize LevelCreateSystem
        GameObject levelCreateObj = new GameObject("LevelCreateSystem");
        var levelCreateSystem = levelCreateObj.AddComponent<LevelCreateSystem>();
        levelCreateSystem.Initialize(entityCreator);
        
        // Initialize PlayerCreateSystem
        PlayerCreateSystem playerCreateSystem = Systems.Player.Initial.InitializePlayerCreateSystems.Instance.Initialize();
        // Initialize CrosshairSystem
        Systems.InputSystems.Initial.InitializeInputSytems.Instance.InitializeInputSystems(crosshairPrefab, uiInstance);

        //Register the playerData to the move system
        playerCreateSystem.RegisterPlayerDataToMoveSystem();

        // Find EntityClickSystem and assign it
        EntityClickSystem clickSystem = FindFirstObjectByType<EntityClickSystem>();
        if (clickSystem == null)
        {
            GameObject clickSystemObj = new GameObject("EntityClickSystem");
            clickSystemObj.AddComponent<EntityClickSystem>();
        }
    }

    private void RegisterSystems()
    {
        EntitySystem.RegisterSystem<EntityClickSystem>(GameContexts.Input);
        EntitySystem.RegisterSystem<CreateGameEntitySystem>(GameContexts.Create);
        EntitySystem.RegisterSystem<UISystem>(GameContexts.UI);
        EntitySystem.RegisterSystem<UIButtonListenerSystem>(GameContexts.UI);
        EntitySystem.RegisterSystem<GameplayHealthSystem>(GameContexts.Gameplay);
        EntitySystem.RegisterSystem<PlayerCreateSystem>(GameContexts.Player);
        EntitySystem.RegisterSystem<PlayerMovementSystem>(GameContexts.Player);
        EntitySystem.RegisterSystem<CrosshairSystem>(GameContexts.Input);
    }

    private void UpdateSystems()
    {
        EntitySystem.Update();
    }

    private void EndSystems()
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
    }
}