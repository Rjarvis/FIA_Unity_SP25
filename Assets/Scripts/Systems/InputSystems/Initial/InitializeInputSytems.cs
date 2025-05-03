using Base;
using Unity.VisualScripting;
using UnityEngine;

namespace Systems.InputSystems.Initial
{
    public class InitializeInputSytems : MonoBehaviourSingleton<InitializeInputSytems>
    {
        public void InitializeInputSystems()
        {
        }


        public void InitializeInputSystems(GameObject crosshairPrefab, GameObject uiInstance)
        {
            // Initialize InputSystems
            GameObject inputSystems = new GameObject("InputSystems");
            var crosshairSystem = inputSystems.AddComponent<CrosshairSystem>();
            var crosshairInst = Instantiate(crosshairPrefab, uiInstance.transform, true);
            var rectTransform = crosshairInst.GetComponent<RectTransform>();
            crosshairSystem.Initialize(rectTransform, Camera.main);
        }
    }
}