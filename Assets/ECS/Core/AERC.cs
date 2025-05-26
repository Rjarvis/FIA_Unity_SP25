using System;
using System.Collections.Generic;
using ECS.Core.Interfaces;

public class AERC : IAERC
{
    private readonly HashSet<object> _owners = new();
    public int RetainCount => _owners.Count;

    public void Retain(object owner)
    {
        if (!_owners.Add(owner))
        {
            throw new Exception($"Owner '{owner}' already retains this entity.");
        }
    }

    public void Release(object owner)
    {
        if (!_owners.Remove(owner))
        {
            throw new Exception($"Owner '{owner}' does not retain this entity.");
        }

        // Optional: Automatically destroy or return to pool when no longer retained.
        // You might notify the Context or World here.
    }
}