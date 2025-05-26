using System;
using Components;
using Contexts;
using Helpers.Base;
using UnityEngine;

namespace Helpers
{
    public class AlienCollisionHandler : CollisionHandlerBase
    {
        // private void OnCollisionEnter2D(Collision2D collision)
        // {
        //     var collisionGameObj = collision.gameObject;
        //     //Collision with Bullet
        //     if (GameContexts.Physics.ContainsEntity(
        //             collisionGameObj.GetComponent<BulletComponent>())) RemoveAndDestroyEntity();
        //     
        //     //Collision with Level
        //     if (GameContexts.Level.ContainsEntity(
        //             collisionGameObj.GetComponent<LevelComponent>())) HandleLevelCollision();
        //     
        //     //Collision with Player
        //     if (GameContexts.Player.ContainsEntity(
        //             collisionGameObj.GetComponent<PlayerComponent>())) HandlePlayerCollision();
        //     
        //     //Collision with Alien
        //     if (GameContexts.Alien.ContainsEntity(
        //             collisionGameObj.GetComponent<AlienComponent>())) HandleAlienCollision();
        // }
        //
        // private void HandleAlienCollision()
        // {
        //     throw new NotImplementedException();
        // }
        //
        // private void HandlePlayerCollision()
        // {
        //     throw new NotImplementedException();
        // }
        //
        // private void HandleLevelCollision()
        // {
        //     throw new NotImplementedException();
        // }
    }
}