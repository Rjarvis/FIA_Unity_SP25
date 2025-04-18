using Unity.Mathematics.Geometry;
using UnityEngine;

public class LevelComponent : MonoBehaviour
{
    public float Radius;

    public void Initialize(float radius)
    {
        Radius = radius;
        CreateVisualCircle(radius);
    }

    private void CreateVisualCircle(float radius)
    {
        var line = gameObject.AddComponent<LineRenderer>();
        line.loop = true;
        line.positionCount = 100;
        line.widthMultiplier = 1f;
        line.useWorldSpace = false;

        float angle = 0f;
        for (int i = 0; i < line.positionCount; i++)
        {
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            line.SetPosition(i, new Vector3(x, y, 0));
            angle += 2 * Mathf.PI / line.positionCount;
        }

        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.green;
        line.endColor = Color.green;
    }
}
