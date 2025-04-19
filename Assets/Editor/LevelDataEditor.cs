using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Systems.Level.Data;

public class LevelDataEditor : EditorWindow
{
    private List<LevelData> levelList = new List<LevelData>();
    private Vector2 scrollPos;

    [MenuItem("Tools/Level Data Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelDataEditor>("Level Data Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Designer", EditorStyles.boldLabel);

        if (GUILayout.Button("Add New Level"))
        {
            levelList.Add(new LevelData() { level = levelList.Count + 1, radius = 3f, enemySpawnRate = 2f, totalEnemyCount = 10 });
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < levelList.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label($"Level {i + 1}", EditorStyles.boldLabel);

            levelList[i].level = EditorGUILayout.IntField("Level Number", levelList[i].level);
            levelList[i].radius = EditorGUILayout.FloatField("Radius", levelList[i].radius);
            levelList[i].enemySpawnRate = EditorGUILayout.FloatField("Enemy Spawn Rate (sec)", levelList[i].enemySpawnRate);
            levelList[i].totalEnemyCount = EditorGUILayout.IntField("Total Enemies", levelList[i].totalEnemyCount);

            if (GUILayout.Button("Remove Level"))
            {
                levelList.RemoveAt(i);
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        if (GUILayout.Button("Save to JSON"))
        {
            SaveToJSON();
        }

        if (GUILayout.Button("Load from JSON"))
        {
            LoadFromJSON();
        }
    }

    private void SaveToJSON()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "LevelData.json");

        string wrappedJson = JsonUtility.ToJson(new LevelDataWrapper { levels = levelList }, true);

        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }

        File.WriteAllText(path, wrappedJson);
        Debug.Log($"Saved LevelData to {path}");
    }

    private void LoadFromJSON()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "LevelData.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            LevelDataWrapper wrapper = JsonUtility.FromJson<LevelDataWrapper>(json);
            levelList = wrapper.levels;
            Debug.Log("Loaded LevelData from JSON");
        }
        else
        {
            Debug.LogWarning("No LevelData.json found to load.");
        }
    }

    [System.Serializable]
    private class LevelDataWrapper
    {
        public List<LevelData> levels;
    }
}
