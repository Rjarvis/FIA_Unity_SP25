using System;
using System.Collections.Generic;
using Components;
using Contexts;
using Helpers.Level;
using Interfaces;
using UnityEngine;

namespace Systems.Enemy
{
    public class AlienSystem : MonoBehaviour, IUpdatable
    {
        private GameObject alienPrefab;
        private GameObject alienBoss;
        private int waveCount = 0;
        private int waveSize = 5;


        public void Start()
        {
            //Get the alienPrefab from bootSequence;
            alienPrefab = BootSequence.Instance.alienPrefab;
            alienBoss = BootSequence.Instance.alienBoss;
        }

        public void UpdateSystem()
        {
            // Count the current amount of aliens
            var alienCount = GameContexts.Gameplay.GetEntitiesWithComponent<AlienComponent>();
            if (alienCount.Count < waveSize) SpawnAlienWave(alienCount);
            MoveAliens(alienCount); //Moves the aliens in a pattern towards the planet
            // CheckDied(); //Checks if the aliens were hit by bulletObj this frame
            // CheckIfCanShoot(); //Checks is the aliens can shoot this frame 
        }

        private void MoveAliens(List<IEntityComponent> entityComponents)
        {
            //Get the centerPoint to move to
            var centerPoint = GameContexts.Level.GetEntitiesWithComponent<LevelComponent>();
            var centerPointObj = ReturnCenterPointObj(centerPoint);
            if (!centerPointObj)
            {
                Debug.LogError("CenterPoint obj could not be retrieved!");
                return;
            }
            
            foreach (var component in entityComponents)
            {
                //Get the component's gameObject
                var gameObject = component.GetGameObject();
                //Get the direction from the componentGameObject to the CenterPointObject
                var direction = centerPointObj.transform.position;
                //Translate the component's gameObject to the CenterPoint
                // gameObject.transform.position += 
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

        private void SpawnAlienWave(List<IEntityComponent> entityComponents)
        {
            var currentCount = entityComponents.Count;
            while (currentCount < waveSize)
            {
                SpawnAlien();
                currentCount++;
            }
        }

        private void SpawnAlien()
        {
            //Get the AlienPrefab
            var newAlien = GameObject.Instantiate(alienPrefab);
            // TBD: Apply the image component level/art required :TBD
            //Apply Components
            var alienEnity = newAlien.AddComponent<EntityComponent>();
            alienEnity.SetContext(GameContexts.Gameplay);
            var alienComponent = new AlienComponent(){};
            alienComponent.SetContext(GameContexts.Gameplay);
            alienEnity.AddComponent(alienComponent);
        }
    }
}