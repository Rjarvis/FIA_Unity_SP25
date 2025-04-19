using System.Collections.Generic;
using System.IO;
using System.Linq;
using Base;
using Systems.Level.Data;
using UnityEngine;

namespace Systems.Level
{
    public class LevelLoader : MonoBehaviourSingleton<LevelLoader>
    {
        public List<LevelData> levelDataList;

        void Awake()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "LevelData.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                levelDataList = JsonUtility.FromJson<LevelDataWrapper>("{\"levels\":" + json + "}").levels;
                Debug.Log($"Loaded {levelDataList.Count} levels.");
            }
            else
            {
                Debug.LogError($"Missing LevelData.json at {path}");
            }
        }

        [System.Serializable]
        private class LevelDataWrapper
        {
            public List<LevelData> levels;
        }

        public LevelData GetLevelData(int levelNumber)
        {
            return levelDataList.FirstOrDefault(l => l.level == levelNumber);
        }
    }
}