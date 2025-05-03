using Base;
using UnityEngine;

namespace Systems.Player.Initial
{
    public class InitializePlayerCreateSystems: MonoBehaviourSingleton<InitializePlayerCreateSystems>
    {
        public PlayerCreateSystem Initialize()
        {
            // Initialize PlayerCreateSystem
            GameObject playerSystem = new GameObject("PlayerSystems");
            var playerCreateSystem = playerSystem.AddComponent<PlayerCreateSystem>();
            playerCreateSystem.Initialize();
            return playerCreateSystem;
        }
    }
}