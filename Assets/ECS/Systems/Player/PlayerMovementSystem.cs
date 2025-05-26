using System.Collections.Generic;
using ECS.Core;
using ECS.Core.Interfaces;
using ECS.Components;
using Interfaces;
using NUnit.Framework;
using UnityEngine;

namespace ECS.Systems.Player
{
    public class PlayerMovementSystem : ISystem, IUpdatable
    {
        private readonly Context _playerContext;

        public PlayerMovementSystem(Context playerContext)
        {
            _playerContext = playerContext;
        }

        public void UpdateSystem()
        {
            var entities = _playerContext.GetEntitiesWithComponents<OrbitComponent, ViewComponent>();
            
            foreach (var entity in entities)
            {
                var orbit = entity.GetComponent<OrbitComponent>();
                var view = entity.GetComponent<ViewComponent>();

                if (!view.GameObject || !orbit.Center) continue;

                float input = Input.GetAxis("Horizontal");
                orbit.Angle -= input * orbit.OrbitSpeed * Time.deltaTime;

                float angleRad = orbit.Angle * Mathf.Deg2Rad;
                Vector2 offset = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * orbit.Radius;

                var playerTransform = view.GameObject.transform;
                playerTransform.position = orbit.Center.transform.position + new Vector3(offset.x, offset.y, 0);
                playerTransform.up = (playerTransform.position - orbit.Center.transform.position).normalized;

                // Optional: gravitational pull behavior could go here
            }
        }
    }
}