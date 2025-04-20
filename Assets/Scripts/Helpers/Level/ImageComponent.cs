using UnityEditor;
using UnityEngine;

namespace Helpers.Level
{
    public class ImageComponent : MonoBehaviour
    {
        public void Initialize(string imagePath)
        {
            CreateVisualImage(imagePath);
        }

        private void CreateVisualImage(string imagePath)
        {
            SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(imagePath);
            spriteRenderer.sprite = sprite;
        }
    }
}