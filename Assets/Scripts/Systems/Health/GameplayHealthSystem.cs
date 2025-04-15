using Components;
using Contexts;
using UnityEngine;

namespace Systems.Health
{
    public class GameplayHealthSystem : MonoBehaviour
    {
        private void Update()
        {
            foreach (var health in GameContexts.Gameplay.GetAllComponents<HealthComponent>())
            {
                if (health.Health <= 0)
                {
                    Debug.Log("Gameplay entity destroyed!");
                    //Maybe add a deathComponent here to flag the system;
                }

                health.Health--;
            }
        }

        public void Initialize()
        {
            
        }
    }
}