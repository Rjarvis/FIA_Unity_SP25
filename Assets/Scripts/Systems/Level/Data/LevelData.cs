using System.Collections.Generic;

namespace Systems.Level.Data
{
    [System.Serializable]
    public class LevelData
    {
        public int level;
        public float radius;
        public float enemySpawnRate;
        public int totalEnemyCount;
        public string imagePath;
    }

    [System.Serializable]
    public class LevelDataCollection
    {
        public LevelData[] levels;
    }
    
    [System.Serializable]
    public class LevelDataList
    {
        public List<LevelData> levels;
    }
}