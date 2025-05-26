// using System.Collections.Generic;
// using System.ComponentModel;
// using Base;
// using ECS.Core;
// using Interfaces;
// using Systems.Create;
// using Systems.Level.Data;
// using UnityEngine;
//
// namespace Systems.Level
// {
//     public class LevelCreateSystem : MonoBehaviourSingleton<LevelCreateSystem>, IUpdatable
//     {
//         protected CreateGameEntitySystem entityCreator;
//         private List<LevelData> levelData;
//         private Context levelContext;
//         private int levelIndex;
//         private GameObject levelCenterObj;
//         private GameObject currentLevel;
//         
//         public void Initialize(CreateGameEntitySystem createGameEntitySystem)
//         {
//             entityCreator = createGameEntitySystem;
//             levelContext = Contexts.GameContexts.Level;
//             //Get the Level Data
//             levelData = Helpers.Level.Level.GetLevelData();
//             levelIndex = 0;
//             var data = levelData[levelIndex];
//             //CreateLevel initial
//             currentLevel = CreateLevel(data);
//             
//             //CreateLevelCenter and attach to entity;
//             var levelCenter = GameObject.FindGameObjectWithTag("LevelCenter");;
//             if (!levelCenter)
//             {
//                 levelCenter = Resources.Load<GameObject>("Prefabs/LevelCenter");
//                 levelCenterObj = Instantiate(levelCenter, currentLevel.transform, true);
//                 // Debug.LogError("Level center not found. Add a GameObject with tag 'LevelCenter'.");
//                 // return;
//             }
//         }
//
//         public GameObject CreateLevel(LevelData levelData) => entityCreator.CreateLevelEntity(levelData);
//         
//
//         public void UpdateSystem()
//         {
//             //TBD
//             // if (levelContext.GetAllComponents<AdvanceLevel>() > 0)
//             // {
//             //     levelIndex++;
//             //     //DeleteCurrentLevel();
//             //     CreateLevel(levelData[levelIndex]);
//             // }
//         }
//     }
// }