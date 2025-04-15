using Components;
using Contexts;
using Interfaces;
using Systems;
using Systems.Create;
using Systems.Health;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BootSequence : MonoBehaviour
{
    [SerializeField] private GameObject uiPrefab;
    private UISystem uiSystem;
    private UIButtonListenerSystem uiButtonListenerSystem;
    [SerializeField] private GameObject uiInstance;

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
    }

    private void StartSystems()
    {
        // Ensure an instance of CreateGameEntitySystem exists
        GameObject createGameSystemObj = null;
        if (FindObjectOfType<CreateGameEntitySystem>() == null)
        {
            createGameSystemObj = new GameObject("CreateGameEntitySystem");
            createGameSystemObj.AddComponent<CreateGameEntitySystem>();
        }

        // Initialize UI System
        uiSystem = FindFirstObjectByType<UISystem>();
        if (uiSystem != null)
        {
            uiSystem.Initialize(uiPrefab);
            uiInstance = uiSystem.GetInstance();
            
            // Initialize button events
            uiButtonListenerSystem = FindFirstObjectByType<UIButtonListenerSystem>();
            if (uiButtonListenerSystem != null) uiButtonListenerSystem.Initialize(uiInstance, createGameSystemObj.GetComponent<CreateGameEntitySystem>());
        }
        else
        {
            Debug.LogError("UISystem not found in scene.");            
        }
        
        

        // Find EntityClickSystem and assign it
        EntityClickSystem clickSystem = FindObjectOfType<EntityClickSystem>();
        if (clickSystem == null)
        {
            GameObject clickSystemObj = new GameObject("EntityClickSystem");
            clickSystemObj.AddComponent<EntityClickSystem>();
        }

        RegisterSystems();
    }

    private void RegisterSystems()
    {
        EntitySystem.RegisterSystem<EntityClickSystem>(GameContexts.Input);
        EntitySystem.RegisterSystem<CreateGameEntitySystem>(GameContexts.Create);
        EntitySystem.RegisterSystem<UISystem>(GameContexts.UI);
        EntitySystem.RegisterSystem<UIButtonListenerSystem>(GameContexts.UI);
        EntitySystem.RegisterSystem<GameplayHealthSystem>(GameContexts.Gameplay);
    }

    private void UpdateSystems()
    {
        EntitySystem.Update();
    }

    private void EndSystems()
    {
        Debug.Log("Shutting down systems...");
        
        EntitySystem.UnRegisterSystem<UISystem>(GameContexts.UI);
        EntitySystem.UnRegisterSystem<UIButtonListenerSystem>(GameContexts.UI);
        EntitySystem.UnRegisterSystem<GameplayHealthSystem>(GameContexts.Gameplay);
    }
}

