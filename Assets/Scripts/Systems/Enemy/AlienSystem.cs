// using System.Collections.Generic;
// using Components;
// using Contexts;
// using ECS.Components;
// using Helpers;
// using Interfaces;
// using Unity.VisualScripting;
// using UnityEngine;
//
//
// namespace Systems.Enemy
// {
//     //Todo Refactor into a ReactiveSystem in the namespace ECS.Systems.Alien.Create
//     public class AlienSystem : MonoBehaviour, IUpdatable
//     {
//         private GameObject alienPrefab;
//         private GameObject alienBoss;
//         private int waveCount = 0;
//         private int waveSize = 5;
//         
//         //timer variables
//         private float timeSinceLastWave = 0f;
//         private float waveCooldown = 5f; // seconds to wait before checking for new wave
//         [SerializeField] private bool waveInProgress = false;
//
//
//
//         public void Start()
//         {
//             //Get the alienPrefab from bootSequence;
//             alienPrefab = BootSequence.Instance.alienPrefab;
//             alienBoss = BootSequence.Instance.alienBoss;
//         }
//
//         public void UpdateSystem()
//         {
//             var alienEntities = GameContexts.Alien.GetEntitiesWithComponent<AlienComponent>();
//             if (alienEntities == null)
//             {
//                 Debug.LogWarning($"alienEntities is null; returning;");
//                 return;
//             }
//
//             if (waveInProgress)
//             {
//                 // timeSinceLastWave += Time.deltaTime;
//
//                 // Wait until all aliens are cleared OR cooldown expires
//                 // if (alienEntities.Count == 0 || timeSinceLastWave >= waveCooldown)
//                 if (alienEntities.Count == 0)
//                 {
//                     Debug.Log("Conditions met to start next wave.");
//                     waveInProgress = false;
//                     timeSinceLastWave = 0f;
//                 }
//                 // MoveAliens(alienCount); //Moves the aliens in a pattern towards the planet //Maybe move to AlienMoveSystem
//                 // CheckDied(); //Checks if the aliens were hit by bulletObj this frame
//                 // CheckIfCanShoot(); //Checks if the aliens can shoot this frame 
//                 return;
//             }
//
//             // If not in progress and we have room to spawn, spawn a new wave
//             if (waveCount == 0 && !waveInProgress)
//             {
//                 SpawnAlienWave(alienEntities);
//                 waveInProgress = true;
//             }
//         }
//
//         
//
//         private void CheckIfCanShoot()
//         {
//             //Get all the entities
//             
//         }
//
//         public void CountDied()
//         {
//             waveCount--;
//         }
//
//         private void MoveAliens(List<IEntityComponent> entityComponents)
//         {
//             //Get the centerPoint to move to
//             var centerPoint = GameContexts.Level.GetEntitiesWithComponent<LevelComponent>();
//             var centerPointObj = ReturnCenterPointObj(centerPoint);
//             if (!centerPointObj)
//             {
//                 Debug.LogError("CenterPoint obj could not be retrieved!");
//                 return;
//             }
//             
//             foreach (var component in entityComponents)
//             {
//                 //Get the component's gameObject
//                 var gameObject = component.GetGameObject();
//                 //Get the direction from the componentGameObject to the CenterPointObject
//                 var direction = centerPointObj.transform.position;
//                 //Translate the component's gameObject to the CenterPoint
//                 // gameObject.transform.position += 
//             }
//         }
//
//         private GameObject ReturnCenterPointObj(List<IEntityComponent> centerPoint)
//         {
//             foreach (var obj in centerPoint)
//             {
//                 var gameObj = obj.GetGameObject();
//                 if (gameObj.transform.childCount > 0) return gameObj.transform.GetChild(0).gameObject;
//             }
//
//             return null;
//         }
//
//         private void SpawnAlienWave(List<IEntityComponent> entityComponents)
//         {
//             var playerEntity = GameContexts.Player.GetEntitiesWithComponent<PlayerComponent>()[0];
//             var levelEntity = GameContexts.Level.GetEntitiesWithComponent<LevelComponent>();
//
//             for (int i = 0; i < waveSize; i++)
//             {
//                 SpawnAlien(playerEntity, levelEntity);
//                 waveCount += 1;
//             }
//         }
//
//
//         private void SpawnAlien(IEntityComponent playerEntityComponent, List<IEntityComponent> levelEntityComponent)
//         {
//             playerEntityComponent.TryGetComponent(out PlayerComponent playerComponent);
//             var playerLevel = playerComponent.Level;
//             
//             //Determine spawn location
//             var worldLevelPos = levelEntityComponent[0];//.GetTransform().position;
//             var worldLevelTransform = worldLevelPos.GetTransform();
//             var playerZRot = playerEntityComponent.GetTransform().rotation.z;
//             // Debug.Log($"playerZRot:{playerZRot} \n worldLevelPos.position.x:{worldLevelTransform.position.x}");
//
//             var spawnPos = GetAlienSpawnPosition();
//             var newAlien = Instantiate(alienPrefab);
//             newAlien.name = $"Alien_{waveCount}";
//             
//             //Apply Components to obj
//             var alienEntity = newAlien.AddComponent<EntityComponent>();
//             alienEntity.SetContext(GameContexts.Alien);
//             EntitySystem.NotifyEntityCreated(alienEntity);
//             
//             var alienComponent = new AlienComponent() { };
//             alienComponent.SetContext(GameContexts.Alien);
//             alienEntity.AddComponent(alienComponent);
//             // Debug.Log($"AlienComponent added to: {alienEntity.name} | HasAlienComponent: {alienEntity.HasComponent<AlienComponent>()}");
//             
//             newAlien.AddComponent<BulletCollisionHandler2D>();
//         }
//
//         private Vector3 GetAlienSpawnPosition()
//         {
//             Camera cam = Camera.main;
//             Vector3 spawnPos = Vector3.zero;
//
//             int edge = Random.Range(0, 4); // 0 = top, 1 = right, 2 = bottom, 3 = left
//             float offset = 1f; // How far offscreen to spawn (in world units)
//
//             switch (edge)
//             {
//                 case 0: // Top
//                     spawnPos = cam.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height + 10, cam.nearClipPlane));
//                     break;
//                 case 1: // Right
//                     spawnPos = cam.ScreenToWorldPoint(new Vector3(Screen.width + 10, Random.Range(0, Screen.height), cam.nearClipPlane));
//                     break;
//                 case 2: // Bottom
//                     spawnPos = cam.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), -10, cam.nearClipPlane));
//                     break;
//                 case 3: // Left
//                     spawnPos = cam.ScreenToWorldPoint(new Vector3(-10, Random.Range(0, Screen.height), cam.nearClipPlane));
//                     break;
//             }
//
//             spawnPos.z = 0f; // Fix Z if needed for 2D
//             return spawnPos;
//         }
//
//
//         public void Initialize()
//         {
//             //Get the alienPrefab from bootSequence;
//             alienPrefab = BootSequence.Instance.alienPrefab;
//             alienBoss = BootSequence.Instance.alienBoss;
//             Debug.Log($"alienPrefabIsNull: {alienPrefab.name}; \n bossPrefabIsNull: {alienBoss.name}");
//             EntitySystem.RegisterSystem(GameContexts.Alien, this);//Register
//         }
//     }
// }