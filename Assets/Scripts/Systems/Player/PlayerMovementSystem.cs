using Base;
using Interfaces;
using UnityEngine;

namespace Systems.Player
{
    public class PlayerMovementSystem : MonoBehaviourSingleton<PlayerMovementSystem>, IUpdatable
    {
        private GameObject centerPoint;
        public GameObject CenterPoint
        {
            get => centerPoint;
            set => centerPoint = value;
        }
        private GameObject playerTransform;
        public GameObject PlayerTransform
        {
            get => playerTransform;
            set => playerTransform = value;
        }

        public float radius = 1f;
        public float orbitSpeed = 90f;
        private float currentAngle = 0f;

        public void UpdateSystem()
        {
            if (Instance.playerTransform && Instance.centerPoint) PlayerMovement();
        }

        private void PlayerMovement()
        {
            float input = Input.GetAxis("Horizontal");
            currentAngle -= input * orbitSpeed * Time.deltaTime;
            float angleRad = currentAngle * Mathf.Deg2Rad;

            Vector2 offset = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * radius;
            Instance.playerTransform.transform.position = Instance.centerPoint.transform.position + new Vector3(offset.x, offset.y, 0);

            Instance.playerTransform.transform.up = (Instance.playerTransform.transform.position - Instance.centerPoint.transform.position).normalized;
        }
    }
}