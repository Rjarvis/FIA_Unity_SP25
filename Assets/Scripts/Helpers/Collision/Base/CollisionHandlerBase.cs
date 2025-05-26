using Contexts;
using ECS.Core;
using UnityEngine;

namespace Helpers.Base
{
    public class CollisionHandlerBase : MonoBehaviour
    {
        protected void RemoveAndDestroyEntity(Context context)
        {
            var levelView = gameObject.GetComponent<LevelView>();
            if (!levelView) return;
            var entity = levelView.GetEntity();
            context.DestroyEntity(entity);//Make a dirty or destroy flag component so a CleanupSystem can handle entity destruction
            Destroy(gameObject, 0.01f);
        }
    }
}