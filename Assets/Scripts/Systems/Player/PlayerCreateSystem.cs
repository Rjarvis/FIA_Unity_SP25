using System;
using Base;
using Helpers.Level;
using Interfaces;
using UnityEngine;

namespace Systems.Player
{
    public class PlayerCreateSystem : MonoBehaviourSingleton<PlayerCreateSystem>, IUpdatable
    {
        public GameObject playerPrefab;
        private GameObject playerInstance;
        private GameObject centerObj;
        private float radius;

        public void Initialize()
        {
            // Find level center
            centerObj = GameObject.FindGameObjectWithTag("LevelCenter");
            if (!centerObj)
            {
                centerObj = Resources.Load<GameObject>("Prefabs/LevelCenter");
                // Debug.LogError("Level center not found. Add a GameObject with tag 'LevelCenter'.");
                // return;
            }

            // Load the PlayerPrefab from Resources folder
            playerPrefab = Resources.Load<GameObject>("Prefabs/PlayerPrefab");  // Make sure it's under Assets/Resources/Prefabs
            if (!playerPrefab)
            {
                Debug.LogError("PlayerPrefab not found in Resources/Prefabs.");
                return;
            }

            // Instantiate player
            playerInstance = Instantiate(playerPrefab);
            playerInstance.name = "PlayerObject";

            // Set initial position at the orbit radius on x-axis
            radius = GameObject.Find("LevelEntity_1").GetComponent<LevelComponent>().Radius;
            playerInstance.transform.position = centerObj.transform.position + new Vector3(radius, 0f, 0f);

            var imageComponent = playerInstance.AddComponent<ImageComponent>();
            imageComponent.Initialize(Helpers.Data.PlayerSpritePath);
        }

        public void RegisterPlayerDataToMoveSystem()
        {
            // Register data to movement system
            var moveSystem = PlayerMovementSystem.Instance;
            moveSystem.PlayerTransform = playerInstance;
            moveSystem.CenterPoint = centerObj;
            moveSystem.radius = radius;
        }


        public void UpdateSystem() { }
    }
}