using System;
using System.Collections.Generic;
using Components;
using Contexts;
using Helpers;
using Helpers.Level;
using Interfaces;
using Systems.Sound;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Systems.Enemy
{
    public class AlienSystem : MonoBehaviour, IUpdatable
    {
        private GameObject alienPrefab;
        private GameObject alienBoss;
        private int waveCount = 0;
        private int waveMax = 3;
        private int waveSize = 5;
        private Timer timer;
        public float spawnTimer = 5f; //5 seconds
        public float cumulativeDeltaTime;
        public List<AlienComponent> Aliens;//<--- We track the amount of aliens here.


        public void Start()
        {
            //Get the alienPrefab from bootSequence;
            alienPrefab = BootSequence.Instance.alienPrefab;
            alienBoss = BootSequence.Instance.alienBoss;
            Aliens = new List<AlienComponent>();
        }

        public void UpdateSystem()
        {
            cumulativeDeltaTime += Time.deltaTime;
            if (waveCount > waveMax) return;//<---Progress to Level 2 here
            // Count the current amount of aliens
            if (cumulativeDeltaTime > spawnTimer) SpawnAlienWave();
            
            MoveAliens(); //Moves the aliens in a pattern towards the planet
            CheckDied(); //Checks if the aliens were hit by bulletObj this frame
            Shoot(CheckIfCanShoot()); //Checks is the aliens can shoot this frame 
        }

        private void Shoot(bool checkIfCanShoot)
        {
            if (checkIfCanShoot == false || Aliens.Count == 0) return;
            
            //Get a randomNumber from 1 to aliens.Count
            var yourRandomNumber = Random.Range(1, Aliens.Count);
            var alienComponent = Aliens[yourRandomNumber];
            
            //Now spawn a bullet that will move towards the centerPoint
        }

        private bool CheckIfCanShoot() => cumulativeDeltaTime >= spawnTimer;// flip to <= and see what happens
        

        private void CheckDied()
        {
            // Need to count the aliens onscreen.
            //Lets use a simple for loop
            for (int i = 0; i < Aliens.Count; i++)
            {
                if (Aliens[i].Health <= 0) Aliens[i].isAlive = false;
            }

            foreach (var alienComponent in Aliens)
            {
                if(alienComponent.isAlive == false) IGotOne();
            }

            Aliens = RemoveAllTheDeadAliens();
        }

        private List<AlienComponent> RemoveAllTheDeadAliens()
        {
            List<AlienComponent> toReturn = new List<AlienComponent>();
            List<AlienComponent> toDestroy = new List<AlienComponent>();
            foreach (var alien in Aliens)
            {
                if (alien.isAlive) toReturn.Add(alien);
                else toDestroy.Add(alien);
            }

            foreach (var alien in toDestroy)
            {
                Aliens.Remove(alien);
                Destroy(alien.gameObject);
            }
            return toReturn;
        }

        public void IGotOne()
        {
            //Play a sound
            SoundSystem.Instance.PlayTheOneWhereAnAlienDied();
        }

        public void ScoredAHit(AlienComponent alienComponent)
        {
            alienComponent.Health -= 1;//Subtracts one from the alien's health;
        }


        private void MoveAliens()
        {
            // Get the center point to move toward
            var centerPoint = GameObject.FindFirstObjectByType<LevelComponent>();
            if (!centerPoint)
            {
                Debug.LogError("CenterPoint obj could not be retrieved!");
                return;
            }

            Vector3 targetPosition = centerPoint.transform.position;

            // Move each alien toward the center
            foreach (var alien in Aliens)
            {
                if (alien == null || !alien.isAlive) continue;

                Transform alienTransform = alien.transform;
                float moveSpeed = alien.isBoss ? 1f : 2f; // Bosses move slower

                // Move in small step toward center
                alienTransform.position = Vector3.MoveTowards(
                    alienTransform.position,
                    targetPosition,
                    moveSpeed * Time.deltaTime
                );
            }
        }


        private GameObject ReturnCenterPointObj(List<IEntityComponent> centerPoint)
        {
            foreach (var obj in centerPoint)
            {
                var gameObj = obj.GetGameObject();
                if (gameObj.transform.childCount > 0) return gameObj.transform.GetChild(0).gameObject;
            }

            return null;
        }

        private void SpawnAlienWave()
        {
            cumulativeDeltaTime = 0f;//Reset the timer
            if (Aliens.Count > waveSize) return;//Don't spawn more aliens if there are already enough on screen.
            var currentCount = 0;
            bool isBoss = false;//TODO: Figure out how to use a number to determine a true or false statement
            
            while (currentCount < waveSize)
            {
                SpawnAlien(isBoss);
                currentCount++;
            }

            waveCount++;
        }

        //TODO: Use this to make the isBoss variable true so a boss can spawn
        private bool IsPrimeNumber(int i)// Use this to determine if the number you're using is a prime number
        {
            if (i <= 1) return false;
            if (i == 2) return true;
            if (i % 2 == 0) return false;

            int boundary = (int)Mathf.Sqrt(i);
            for (int j = 3; j <= boundary; j += 2)
            {
                if (i % j == 0)
                    return false;
            }

            return true;
        }


        private void SpawnAlien(bool isBoss)
        {
            //Get the AlienPrefab
            var newAlien = isBoss ? //this is like if(isBoss == true) 
                Instantiate(alienBoss) : // if isBoss == true
                Instantiate(alienPrefab);// if isBoss == false

            var randomDirection = Random.Range(0, 7);//0 is 12 o`clock or northSide of the screen, 4 is 6 o`clock
            var startPos = StartAlienMoving(randomDirection);
            newAlien.transform.position = startPos;
            
            Vector3 directionToCenter = Vector3.zero - startPos;
            newAlien.transform.up = directionToCenter.normalized;


            //Apply Components
            var alienEnity = newAlien.AddComponent<EntityComponent>();
            alienEnity.SetContext(GameContexts.Alien);
            var alienComponent = newAlien.AddComponent<AlienComponent>();//Use this to find it again later
            alienComponent.Health = isBoss ? BossHealth : GruntHealth;
            alienComponent.isAlive = true;
            alienComponent.isBoss = isBoss;
            Aliens.Add(alienComponent);
            alienEnity.AddComponent(alienComponent);
            
            //Need to add a CircleCollider2D
            //Something like
            var collider2D = newAlien.AddComponent<CircleCollider2D>();
            collider2D.radius = isBoss ? 0.5f : 0.12f;
            newAlien.AddComponent<BulletCollisionHandler2D>();
            var rigidbody2D = newAlien.AddComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 0f;

            AudioClip audioClipToPlay = isBoss ? SoundSystem.Instance.alienBoss : SoundSystem.Instance.alienSound;
            // SoundSystem.Instance.PlayThisSound(audioClipToPlay);
        }

        private Vector3 StartAlienMoving(int directionIndex)
        {
            // Direction index 0 to 7, like a clock: 0=N, 1=NE, ..., 7=NW
            float angle = directionIndex * 45f * Mathf.Deg2Rad; // 360 / 8 = 45 degrees
            float spawnRadius = 10f; // Distance from center to spawn alien

            float x = Mathf.Cos(angle) * spawnRadius;
            float y = Mathf.Sin(angle) * spawnRadius;

            return new Vector3(x, y, 0f);
        }


        private const int GruntHealth = 2;

        private const int BossHealth = 10;
    }
}