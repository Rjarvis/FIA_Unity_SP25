using ECS.Core;
using ECS.Core.Interfaces;
using System.Collections.Generic;
using ECS;

public class Feature : IUpdateSystem, ICreateSystem, IDestroySystem, IReactiveSystem
{
    private readonly List<ISystem> _systems = new();

    protected void Add(ISystem system)
    {
        _systems.Add(system);
    }

    public void Initialize(World world)
    {
        foreach (var system in _systems)
            if (system is ICreateSystem createSystem)
                createSystem.Initialize(world);
    }

    public void Shutdown(World world)
    {
        foreach (var system in _systems)
            if (system is IDestroySystem destroySystem)
                destroySystem.Shutdown(world);
    }

    public void StartSequence()
    {
        var world = WorldContainer.Instance;
        foreach (var system in _systems)
            if (system is ICreateSystem createSystem)
                createSystem.Initialize(world);
    }

    public void UpdateSequence()
    {
        foreach (var system in _systems)
            if (system is IUpdateSystem updateSystem)
                updateSystem.Update();
    }

    public void EndSequence()
    {
        var world = WorldContainer.Instance;
        foreach (var system in _systems)
            if (system is IDestroySystem destroySystem)
                destroySystem.Shutdown(world);
    }

    public void Initialize()
    {
        
    }

    public void Update() => UpdateSequence();
    public void Activate() => StartSequence();
    public void Deactivate() => EndSequence();
    public void Clear() => EndSequence();
}