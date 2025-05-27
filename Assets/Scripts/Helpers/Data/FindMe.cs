using Contexts;
using UnityEngine;

namespace Helpers
{
    public static class FindMe
    {
        /// <summary>
        /// Takes in a Context and an EntityComponent then gives you that GameObject.
        /// </summary>
        /// <returns>GameObject</returns>
        public static GameObject FindMeObject(Context context, EntityComponent entityComponent)
        {
            if (context == null) return AGoof();
            if (context.ContainsEntity(entityComponent)) return entityComponent.gameObject;
            return AGoof();//<--- If I got here we done goofed!
        }

        public static Context WhatsMyContext(EntityComponent entityComponent)
        {
            Context toReturn;
            foreach (var context in GameContexts.AllContexts)
            {
                if (context.ContainsEntity(entityComponent)) return context;
                // or
                var entityContext = entityComponent.GetContext();
                if (context == entityContext) return context;
            }

            return null;
        }

        public static string WhatsMyContextNamed(EntityComponent entityComponent)
        {
            return entityComponent.GetContext().Name;
        }

        private static GameObject AGoof()
        {
            var goof = GameObject.CreatePrimitive(PrimitiveType.Cube);
            goof.name = "I'm a goof!";
            return goof;
        }
    }
}