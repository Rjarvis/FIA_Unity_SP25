using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PixelEditor : EditorWindow
{
    private const int gridSize = 16; // 16x16 pixel grid
    private const int cellSize = 20; // Size of each pixel square

    private int[,] pixelGrid = new int[gridSize, gridSize]; // Palette index grid
    private List<Color> palette = new List<Color>(); // Dynamic palette
    private Color currentColor = Color.black;
    private int selectedPaletteIndex = -1;

    [MenuItem("Tools/Pixel Editor")]
    public static void ShowWindow()
    {
        GetWindow<PixelEditor>("Pixel Editor");
    }

    private void OnEnable()
    {
        // Clear grid and initialize with 0s
        for (int y = 0; y < gridSize; y++)
            for (int x = 0; x < gridSize; x++)
                pixelGrid[x, y] = -1;

        // Add default colors to the palette
        palette = new List<Color> {
            Color.black, Color.white, Color.red, Color.green, Color.blue, Color.yellow
        };

        selectedPaletteIndex = 0;
        currentColor = palette[selectedPaletteIndex];
    }

    private void OnGUI()
    {
        DrawGrid();
        DrawColorField();
        DrawPaletteSwatches();
    }

    private void DrawGrid()
    {
        GUILayout.Label("Pixel Grid", EditorStyles.boldLabel);
        Rect gridRect = GUILayoutUtility.GetRect(gridSize * cellSize, gridSize * cellSize);

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                int index = pixelGrid[x, y];
                Color color = (index >= 0 && index < palette.Count) ? palette[index] : Color.clear;

                Rect cellRect = new Rect(gridRect.x + x * cellSize, gridRect.y + y * cellSize, cellSize, cellSize);
                EditorGUI.DrawRect(cellRect, color);

                if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag) && cellRect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.button == 0) // Left-click
                    {
                        int paletteIndex = GetOrAddColorToPalette(currentColor);
                        pixelGrid[x, y] = paletteIndex;
                    }
                    else if (Event.current.button == 1) // Right-click
                    {
                        pixelGrid[x, y] = -1; // Transparent
                    }

                    Event.current.Use(); // Prevent event from propagating
                    Repaint();
                }

                // Draw grid lines
                Handles.color = Color.gray;
                Handles.DrawLine(new Vector2(cellRect.x, cellRect.y), new Vector2(cellRect.x + cellSize, cellRect.y));
                Handles.DrawLine(new Vector2(cellRect.x, cellRect.y), new Vector2(cellRect.x, cellRect.y + cellSize));
            }
        }

        // Bottom and right border lines
        Handles.DrawLine(
            new Vector2(gridRect.x, gridRect.y + gridSize * cellSize),
            new Vector2(gridRect.x + gridSize * cellSize, gridRect.y + gridSize * cellSize)
        );
        Handles.DrawLine(
            new Vector2(gridRect.x + gridSize * cellSize, gridRect.y),
            new Vector2(gridRect.x + gridSize * cellSize, gridRect.y + gridSize * cellSize)
        );
    }

    private void DrawColorField()
    {
        GUILayout.Space(10);
        GUILayout.Label("Brush Color", EditorStyles.boldLabel);

        Color newColor = EditorGUILayout.ColorField(currentColor);
        if (newColor != currentColor)
        {
            currentColor = newColor;
            selectedPaletteIndex = GetOrAddColorToPalette(newColor);
        }
    }

    private void DrawPaletteSwatches()
    {
        GUILayout.Space(10);
        GUILayout.Label("Palette", EditorStyles.boldLabel);

        float swatchSize = 30f;
        float padding = 5f;
        float totalWidth = (swatchSize + padding) * palette.Count;
        Rect area = GUILayoutUtility.GetRect(totalWidth, swatchSize);

        for (int i = 0; i < palette.Count; i++)
        {
            Rect swatchRect = new Rect(area.x + i * (swatchSize + padding), area.y, swatchSize, swatchSize);
            Texture2D tex = MakeTex(1, 1, palette[i]);
            GUI.DrawTexture(swatchRect, tex);

            // Draw border if selected
            if (i == selectedPaletteIndex)
            {
                Handles.color = Color.white;
                Handles.DrawSolidRectangleWithOutline(swatchRect, Color.clear, Color.white);
            }

            // Click detection
            if (Event.current.type == EventType.MouseDown && swatchRect.Contains(Event.current.mousePosition))
            {
                selectedPaletteIndex = i;
                currentColor = palette[i];
                Event.current.Use();
            }
        }
    }


    private int GetOrAddColorToPalette(Color color)
    {
        for (int i = 0; i < palette.Count; i++)
        {
            if (ApproximatelyEqual(palette[i], color))
                return i;
        }

        palette.Add(color);
        return palette.Count - 1;
    }

    private bool ApproximatelyEqual(Color a, Color b, float threshold = 0.01f)
    {
        return Mathf.Abs(a.r - b.r) < threshold &&
               Mathf.Abs(a.g - b.g) < threshold &&
               Mathf.Abs(a.b - b.b) < threshold &&
               Mathf.Abs(a.a - b.a) < threshold;
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i) pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        Repaint();
        return result;
    }
}

public enum EditorTool
{
    Draw,
    Erase
}


