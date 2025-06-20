using System;
using System.Linq;
using Components;
using Contexts;
using Helpers;
using Helpers.Math;
using Systems.Create;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Systems
{
    public class UIButtonListenerSystem : MonoBehaviour
    {
        private Transform uiRoot;
        private int entityCount;
        protected CreateGameEntitySystem entityCreator; 

        public void Initialize(GameObject uiInstance, CreateGameEntitySystem createGameEntitySystem)
        {
            if (uiInstance == null)
            {
                Debug.LogError("UI Instance is null in UIButtonListenerSystem.");
                return;
            }

            entityCreator = createGameEntitySystem;
            uiRoot = uiInstance.transform;
            entityCount = 0;
            RegisterButtonListeners(uiInstance);
        }

        private void RegisterButtonListeners(GameObject uiInstance)
        {
            Button[] buttons = uiRoot.GetComponentsInChildren<Button>();

            foreach (Button button in buttons)
            {
                string buttonName = button.gameObject.name;
                switch (buttonName)
                {
                    case UIButtonNames.CreateEntity:
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(() => SpawnEntity());
                        break;
                    case UIButtonNames.CreateLevelEntity:
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(() => CreateLevelEntity());
                        break;
                    case UIButtonNames.DestroyEntity:
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(() => DeleteEntityLastEntity());
                        break;
                    case UIButtonNames.ButtonInputField:
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(() => SpawnMultiEntities(button.gameObject));
                        break;
                    default : Debug.Log($"case not handled; buttonName: {buttonName}");
                        return;
                }
            }
        }

        private void CreateLevelEntity()
        {
            //Get the Level Data
            var levelData = Helpers.Level.Level.GetLevelData();

            //Iterate through the level data and CreateLevelEntity(data)
            foreach (var data in levelData)
            {
                entityCreator.CreateLevelEntity(data);
            }
        }

        private void SpawnMultiEntities(GameObject buttonGameObject)
        {
            TMP_InputField inputField = buttonGameObject.GetComponentInChildren<TMP_InputField>();

            if (inputField == null)
            {
                Debug.LogError("InputField not found!");
                return;
            }

            if (ValidateInputString(InputType.INT, inputField.text, out object value) && value is int entityCount)
            {
                for (int i = 0; i < entityCount; i++)
                {
                    SpawnEntity();
                }
            }
        }

        private bool ValidateInputString(InputType inputType, string input, out object number)
        {
            bool valid = false;
            number = null; // Ensure default value is assigned

            switch (inputType)
            {
                case InputType.INT:
                    valid = int.TryParse(input, out int numberInt);
                    if (valid) number = numberInt;
                    return valid;

                case InputType.FLOAT:
                    valid = float.TryParse(input, out float numberFloat);
                    if (valid) number = numberFloat;
                    return valid;

                case InputType.STRING:
                    return !string.IsNullOrWhiteSpace(input);
        
                default:
                    Debug.LogWarning($"Undefined inputType: {inputType}");
                    return false;
            }
        }

        private void DeleteEntityLastEntity()
        {
            // entityCreator.DeleteLastEntity();
        }

        private void SpawnEntity()
        {
            Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);

            int[] randArr = MathHelper.GenerateXNumbersForColorRGB(3);
    
            // Convert to Unity's 0-1 color range
            Color randomColor = new Color(randArr[0] / 255f, randArr[1] / 255f, randArr[2] / 255f);
    
            Debug.Log($"Random color: R={randomColor.r}, G={randomColor.g}, B={randomColor.b}");

            entityCreator.CreateEntity(randomPos, randomColor, Random.Range(1, 10));
        }
    }

    public enum InputType
    {
        INT,
        FLOAT,
        STRING
    }
}

