using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Systems
{
    public class UISystem : MonoBehaviour, ISystem
    {
        private GameObject uiInstance;
        public GameObject UiInstance { get; private set; }

        public void Initialize(GameObject uiPrefab)
        {
            if (uiPrefab == null)
            {
                Debug.LogError($"UI Prefab reference is null in UISystem.");
                return;
            }

            uiInstance = Instantiate(uiPrefab);
            UiInstance = uiInstance;
            Debug.Log("UI Prefab instantiated successfully from BootSequence.");

            EnsureCanvasSetup();
            EnsureEventSystem();
        }

        private void EnsureCanvasSetup()
        {
            var canvas = uiInstance.GetComponent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("UI Prefab does not have a Canvas component.");
                return;
            }


            if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                Debug.Log("Canvas render mode set to Screen Space - Overlay.");
            }

            if (uiInstance.GetComponent<GraphicRaycaster>() == null)
            {
                uiInstance.AddComponent<GraphicRaycaster>();
                Debug.Log("GraphicRaycaster added to Canvas.");
            }
        }

        private void EnsureEventSystem()
        {
            if (FindFirstObjectByType<EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
                Debug.Log("EventSystem created.");
            }
        }

        public GameObject GetInstance() => uiInstance;
    }
}