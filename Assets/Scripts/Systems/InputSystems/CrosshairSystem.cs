using Base;
using Interfaces;
using UnityEngine;
using Systems.Player;

namespace Systems.InputSystems
{
    public class CrosshairSystem : MonoBehaviourSingleton<CrosshairSystem>, IUpdatable
    {
        public RectTransform crosshairUI; // Assign a UI Image or prefab
        public Camera mainCamera;

        public void Initialize(RectTransform crosshairUI, Camera mainCamera)
        {
            this.crosshairUI = crosshairUI ? crosshairUI : BootSequence.Instance.crosshairPrefab.GetComponent<RectTransform>();
            this.mainCamera = mainCamera ? mainCamera : Camera.main;
        }

        private void Start()
        {
            if (!mainCamera) mainCamera = Camera.main;
        }

        public void UpdateSystem()
        {
            Vector3 mousePosition = UnityEngine.InputSystem.Mouse.current.position.value;

            // Position UI crosshair in screen space
            crosshairUI.position = mousePosition;

            // Optional: debug raycast into world
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);

                // Example: orient player toward hit point
                // if (PlayerMovementSystem.Instance?.PlayerTransform)
                // {
                //     Vector3 direction = (hit.point - PlayerMovementSystem.Instance.PlayerTransform.transform.position).normalized;
                //     direction.y = 0f; // keep upright
                //     if (direction != Vector3.zero)
                //     {
                //         Quaternion lookRotation = Quaternion.LookRotation(direction);
                //         PlayerMovementSystem.Instance.PlayerTransform.transform.rotation = Quaternion.Slerp(
                //             PlayerMovementSystem.Instance.PlayerTransform.transform.rotation,
                //             lookRotation,
                //             Time.deltaTime * 5f
                //         );
                //     }
                // }
            }
        }
    }

}