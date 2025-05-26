using System.Collections.Generic;
using System.Security.Claims;
using Contexts;
using ECS.Core;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems
{
    public class EntityClickSystem : MonoBehaviour, IUpdatable, IEntityListener
    {
        private List<GameObject> entities = new();
        private GameObject clickedEntity;

        public void UpdateSystem()
        {
            //Todo: Abstract this system out to include multiple input types i.e. Controllers;
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero);

                if (hit.collider != null)
                {
                    clickedEntity = hit.collider.gameObject;
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                //Kill The Entity
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero);
                
                if (hit.collider != null)
                {
                    clickedEntity = hit.collider.gameObject;
                    Debug.Log($"You right clicked on {clickedEntity.name}");
                    // clickedEntity.GetComponent<EntityLinker>();//Something that will hold a reference or a look up to the associated entity in the ECS
                    // Destroy(clickedEntity);
                    // clickedEntity = null;
                }
            }
            else if (Input.GetMouseButton(0))//This moves entities on left-click-hold
            {
                if (!clickedEntity) return; 
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var posToUse = new Vector3(mousePos.x, mousePos.y, 1);
                clickedEntity.transform.SetPositionAndRotation(posToUse, Quaternion.identity);
            }
            else
            {
                if (clickedEntity) clickedEntity = null;
            }
        }
        
        public void OnEntityCreated(Entity entity)
        {
            // if (entity.GetContext() == GameContexts.Gameplay) // Ensure correct context
            // {
            //     entities.Add(entity.gameObject);
            // }
        }
    }
}