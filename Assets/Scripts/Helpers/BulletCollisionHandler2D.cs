using Components;
using Contexts;
using Systems.Enemy;
using Systems.Sound;
using UnityEngine;

namespace Helpers
{
    public class BulletCollisionHandler2D : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        { 
            var thisEntity = GetComponent<EntityComponent>();
            string thisEntityContext = FindMe.WhatsMyContextNamed(thisEntity);
            
            switch (thisEntityContext) // this is a switch
            {
                // A switch has cases
                case "Physics":
                    IAmProbablyABullet(collision);
                    return;
                case "Alien":
                    TakeMeToYourLeader(collision);
                    return;
                case "Level":
                    //This is where the Level will react
                    return;
                //They even have default to catch errors
                default:
                    Debug.Log($"Well Skibidi my toilet how'd that happen?! thisEntityContext:{thisEntityContext}");
                    return;
            }
        }

        private void TakeMeToYourLeader(Collision2D collision2D)
        {
            //An alien has hit something
            //We already have the bullet doing the damage to aliens so lets focus on when the alien lands on Earf!
            var collisionEntity = collision2D.gameObject.GetComponent<EntityComponent>();
            var collidedContext = FindMe.WhatsMyContextNamed(collisionEntity);
            if (collidedContext == GameContexts.Level.Name)
            {
                //Oh no! The aliens have collided with the planet!
                // SoundSystem.Instance.PlayThisSound(SoundSystem.Instance.planetHit);
                collision2D.gameObject.GetComponent<HealthComponent>().Health -= 1;//Make it so bosses do more damage
                var alienComponent = GetComponent<AlienComponent>();
                if (alienComponent) alienComponent.isAlive = false;
            }
        }

        private void IAmProbablyABullet(Collision2D collision)
        {
            //What did I hit?
            GameObject collidedWithThisGameObject = collision.gameObject;
            
            //What was it though?
            var collisionEntity = collidedWithThisGameObject.GetComponent<EntityComponent>();
            var collidedContext = FindMe.WhatsMyContextNamed(collisionEntity);

            if (collidedContext == GameContexts.Alien.Name) IGotOne(collision);
        }

        private void IGotOne(Collision2D collision)
        {
            //Great A bullet hit an Alien now what?
            //Gotta decrease the count in the AlienSystem
            //Well get the AlienSystem from the EntitySystem
            var alienComponent = collision.gameObject.GetComponent<AlienComponent>();
            if (alienComponent)
            {
                var alienSystem = (AlienSystem)EntitySystem.GetSystem<AlienSystem>();
                alienSystem.ScoredAHit(alienComponent);
            }
        }
    }
}