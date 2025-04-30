using Base;
using Components;
using Contexts;
using Interfaces;
using Systems.Create;
using Unity.VisualScripting;
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

        public void Initialize()
        {
            var movementComponent = gameObject.AddComponent<MovementComponent>();
            movementComponent.Speed = orbitSpeed;
            movementComponent.SetContext(GameContexts.Player);
            var entityComponent = gameObject.AddComponent<EntityComponent>();
            entityComponent.SetContext(GameContexts.Player);
            EntitySystem.NotifyComponentAdded(entityComponent, movementComponent);
        }

        public void UpdateSystem()
        {
            if (Instance.playerTransform && Instance.centerPoint) PlayerMovement();
        }

        private void PlayerMovement()
        {
            //Set local variables
            var playerTransform = Instance.playerTransform;
            var centerPoint = Instance.centerPoint;

            //Get the Level obj
            var centerPointParent = centerPoint.transform.parent.gameObject;
            if (centerPointParent) UpdateRadius(centerPointParent);

            //Get MovementComponent
            float input = Input.GetAxis("Horizontal");
            currentAngle -= input * orbitSpeed * Time.deltaTime;
            float angleRad = currentAngle * Mathf.Deg2Rad;

            Vector2 offset = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * radius;
            playerTransform.transform.position = centerPoint.transform.position + new Vector3(offset.x, offset.y, 0);

            playerTransform.transform.up = (playerTransform.transform.position - centerPoint.transform.position).normalized;
            
            //Fall towards Center Point
            if (centerPoint.transform.parent)
            {
                //Move the playerTransform to centerPoint until contact with centerPoint's circle Collider
            }
        }

        private void UpdateRadius(GameObject centerPointParent)
        {
            var levelComponent = centerPointParent.GetComponent<LevelComponent>();
            radius = levelComponent.Radius;
        }
    }
}