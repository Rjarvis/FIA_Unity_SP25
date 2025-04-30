using System.Collections.Generic;
using System.ComponentModel;
using Base;
using Interfaces;
using Systems.Create;
using Systems.Level.Data;

namespace Systems.Level
{
    public class LevelCreateSystem : MonoBehaviourSingleton<LevelCreateSystem>, IUpdatable
    {
        protected CreateGameEntitySystem entityCreator;
        private List<LevelData> levelData;
        private Context levelContext;
        private int levelIndex;
        
        public void Initialize(CreateGameEntitySystem createGameEntitySystem)
        {
            entityCreator = createGameEntitySystem;
            levelContext = Contexts.GameContexts.Level;
            //Get the Level Data
            levelData = Helpers.Level.Level.GetLevelData();
            levelIndex = 0;
            var data = levelData[levelIndex];
            //CreateLevel initial
            CreateLevel(data);
        }

        public void CreateLevel(LevelData levelData) => entityCreator.CreateLevelEntity(levelData);
        

        public void UpdateSystem()
        {
            //TBD
            // if (levelContext.GetAllComponents<AdvanceLevel>() > 0)
            // {
            //     levelIndex++;
            //     //DeleteCurrentLevel();
            //     CreateLevel(levelData[levelIndex]);
            // }
        }
    }
}