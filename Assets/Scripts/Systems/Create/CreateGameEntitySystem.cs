using System.Linq;
using Base;
using Components;
using Components.Health;
using Contexts;
using Helpers;
using Helpers.Level;
using Helpers.Math;
using Systems;
using Systems.Level.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Create
{
    public class CreateGameEntitySystem : MonoBehaviourSingleton<CreateGameEntitySystem>, ISystem
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

        public GameObject CreateLevelEntity(Vector3 position, float radius)
        {
            GameObject level = new GameObject($"LevelEntity_{radius}");
            level.transform.position = position;

            var entityComponent = level.AddComponent<EntityComponent>();
            entityComponent.SetContext(GameContexts.Level);

            var levelComponent = level.AddComponent<LevelComponent>();
            levelComponent.Initialize(radius);

            EntitySystem.NotifyComponentAdded(entityComponent, levelComponent);

            return level;
        }

        public GameObject CreateLevelEntity(LevelData levelData)
        {
            GameObject level = new GameObject($"LevelEntity_{levelData.level}");
            level.transform.position = Vector3.zero;

            var entityComponent = level.AddComponent<EntityComponent>();
            entityComponent.SetContext(GameContexts.Level);
            GameContexts.Level.AddEntity(entityComponent);

            var levelComponent = level.AddComponent<LevelComponent>();
            levelComponent.Initialize(levelData.radius);

            var imageComponent = level.AddComponent<ImageComponent>();
            imageComponent.Initialize(levelData.imagePath);

            var healthComponent = level.AddComponent<HealthComponent>();
            healthComponent.Health = 3 * levelData.level;

            level.AddComponent<CircleCollider2D>();
            level.AddComponent<BulletCollisionHandler2D>();
            var rigidbody2D = level.AddComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 0f;
            
            EntitySystem.NotifyComponentAdded(entityComponent, levelComponent);

            return level;
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
            throw new System.NotImplementedException();
        }

        // public void UpdateSequence()
        // {
        //     GetNewEntities();
        //     MoveEntites();
        // }
        //
        // private void MoveEntites()
        // {
        //     throw new System.NotImplementedException();
        // }
        //
        // private void GetNewEntities()
        // {
        //     var healthEntities = GameContexts.Gameplay.GetEntitiesWithComponent<AddHealthToEntityComponent>();
        // }

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