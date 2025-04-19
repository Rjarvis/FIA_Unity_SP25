using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Systems.Level.Data;

namespace Helpers.Level
{
    public static class Level
    {
        public static List<LevelData> GetLevelData()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "LevelData.json");

#if UNITY_ANDROID && !UNITY_EDITOR
            // On Android, you need UnityWebRequest
            using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(path))
            {
                www.SendWebRequest();
                while (!www.isDone) { }

                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError("Error loading level data: " + www.error);
                    return new List<LevelData>();
                }

                return ParseJson(www.downloadHandler.text);
            }
#else
            if (!File.Exists(path))
            {
                Debug.LogError($"Level data not found at path: {path}");
                return new List<LevelData>();
            }

            string json = File.ReadAllText(path);
            return ParseJson(json);
#endif
        }

        private static List<LevelData> ParseJson(string json)
        {
            LevelDataList dataList = JsonUtility.FromJson<LevelDataList>(json);
            return dataList.levels;
        }

        [System.Serializable]
        private class LevelDataList
        {
            public List<LevelData> levels;
        }
    }
}