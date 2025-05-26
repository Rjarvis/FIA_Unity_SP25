using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Systems
{
    /// <summary>
    /// Does not need a full migration to ECS... yet.
    /// </summary>
    public class UISystem : MonoBehaviour
    {
        private GameObject uiInstance;

        public void Initialize(GameObject uiPrefab)
        {
            if (uiPrefab == null)
            {
                Debug.LogError($"UI Prefab reference is null in UISystem.");
                return;
            }

            uiInstance = Instantiate(uiPrefab);
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