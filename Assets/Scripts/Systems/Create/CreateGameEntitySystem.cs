using System.Linq;
using Components;
using Components.Health;
using Contexts;
using Helpers.Math;
using Systems;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Create
{
    public class CreateGameEntitySystem : MonoBehaviour, ISystem
    {
        protected int entityCount = 0;

        public GameObject CreateEntity(Vector3 position, Color color, int health)
        {
            GameObject entity = new GameObject($"Entity_{entityCount}");
            entity.transform.position = position;
            entityCount++;

            // Attach EntityComponent and assign to Gameplay context
            EntityComponent entityComponent = entity.AddComponent<EntityComponent>();
            entityComponent.SetContext(GameContexts.Gameplay);
            
            // Notify that entity is created
            EntitySystem.NotifyEntityCreated(entityComponent);

            // Add visual representation
            SpriteRenderer renderer = entity.AddComponent<SpriteRenderer>();
            renderer.sprite = CreateRedSquareTexture();
            renderer.color = color;
            renderer.transform.localScale = new Vector3(100f, 100f, 1f); // 100x100 pixels equivalent

            // Add HealthComponent
            HealthComponent healthComponent = entity.AddComponent<HealthComponent>();
            healthComponent.Health = health;
            EntitySystem.NotifyComponentAdded(entityComponent, healthComponent);

            // Add Collider
            entity.AddComponent<BoxCollider2D>();
            
            // Add a button component
            Button button = entity.AddComponent<Button>();

            // Debug.Log($"Entity Created: {entity.name} | Health: {health}");

            return entity;
        }

        private void ChangeColor(SpriteRenderer renderer)
        {
            var randArr = MathHelper.GenerateXNumbersForColorRGB(3);
            // Convert to Unity's 0-1 color range
            Color newColor = new Color(randArr[0] / 255f, randArr[1] / 255f, randArr[2] / 255f);
            renderer.color = newColor;
        }

        private void KillEntity(string entityName)
        {
            var healthComponents = FindObjectsByType<HealthComponent>(FindObjectsSortMode.None);
            var entityToKill = healthComponents.FirstOrDefault((component) =>
            {
                var componentGameObject = component.name.Equals(entityName) ? component.gameObject : null;
                return componentGameObject;
            });
            if(entityToKill)
            {
                
                Destroy(entityToKill.gameObject);
            }
            else Debug.LogWarning($"{entityName} could not be found or killed!");
        }
        
        private Sprite CreateRedSquareTexture()
        {
            Texture2D texture = new Texture2D(1, 1);
            // texture.SetPixel(0, 0, Color.red);
            texture.Apply();

            return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        }

        public void StartSequence()
        {
            
        }

        public void UpdateSequence()
        {
            GetNewEntities();
            MoveEntites();
        }

        private void MoveEntites()
        {
            throw new System.NotImplementedException();
        }

        private void GetNewEntities()
        {
            Contexts.GameContexts.Gameplay.GetAllComponents<AddHealthToEntityComponent>();
        }

        public void EndSequence()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteLastEntity()
        {
            GameObject entityToDelete = GameObject.Find($"Entity_{entityCount-1}");
            Destroy(entityToDelete);
            entityCount--;
        }
    }
}