using ECS.Core;
using UnityEngine;
using Contexts;
using ECS;
using ECS.Systems;
using ECS.Systems.Core;
using ECS.Systems.Level;
using ECS.Systems.Meta;
using ECS.Systems.Physics;
using ECS.Systems.Player;
using ECS.Systems.Rendering;
using Systems;
using Systems.Create;
using UnityEngine.XR;

public class BootSequence : MonoBehaviour
{
    private World _world;

    public static BootSequence Instance;
    [Header("Prefabs")]
    public GameObject uiPrefab;
    public GameObject playerPrefab;
    public GameObject crosshairPrefab;
    public GameObject bulletPrefab;
    public GameObject alienPrefab;
    public GameObject alienBoss;
    

    private void Start()
    {
        Instance = this;
        InitializeWorld();
        RegisterContexts();
        RegisterSystems();
    }

    private void Update()
    {
        _world.ExecuteSystems();
    }

    private void OnDestroy()
    {
        _world.ShutdownSystems();
    }

    private void InitializeWorld()
    {
        _world = new World();
        WorldContainer.SetWorld(_world); // optional singleton for debug/dev access
    }

    private void RegisterContexts()
    {
        GameContexts.Meta = _world.CreateContext("Meta");
        GameContexts.Gameplay = _world.CreateContext("Gameplay");
        GameContexts.Create = _world.CreateContext("Create");
        GameContexts.UI = _world.CreateContext("UI");
        GameContexts.Physics = _world.CreateContext("Physics");
        GameContexts.Input = _world.CreateContext("Input");
        GameContexts.Level = _world.CreateContext("Level");
        GameContexts.Player = _world.CreateContext("Player");
        GameContexts.Alien = _world.CreateContext("Alien");
    }

    private void RegisterSystems()
    {
        _world.InitializeSystems(
            new MetaFeature(GameContexts.Meta),
            new CoreFeature(GameContexts.Gameplay), // <- contains Create and Level systems
            new CreateFeature(GameContexts.Create),
            new RenderingFeature(GameContexts.UI),
            new CombatFeature(GameContexts.Physics),
            // new InputFeature(GameContexts.Input),
            new PlayerFeature(GameContexts.Player)
            // new AlienFeature(GameContexts.Alien),
        );
    }
}
