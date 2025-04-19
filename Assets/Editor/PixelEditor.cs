using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class PixelEditor : EditorWindow
{
    // private const int gridSize = 16; // 16x16 pixel grid
    private const int cellSize = 20; // Size of each pixel square
    
    private readonly int[] gridSizes = { 8, 16, 32, 64, 128 };
    private int currentGridSizeIndex = 1; // Start at 16x16
    private int gridSize => gridSizes[currentGridSizeIndex]; // Dynamic property for consistency


    private int[,] pixelGrid = new int[16, 16]; // Palette index grid
    private List<Color> palette = new List<Color>(); // Dynamic palette
    private Color currentColor = Color.black;
    private int selectedPaletteIndex = -1;
    
    private float zoom = 1f;
    private float minZoom = 0.5f;
    private float maxZoom = 2.4f;
    private const float zoomStep = 0.1f;


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
        DrawSaveLoadButton();
        //Must be last-Experimental
        DrawPixelGridScaler();
        HandleZoomScrollv2();
        HandleZoomScrollv1();
    }

    private void DrawPixelGridScaler()
    {
        GUILayout.Label($"Grid Size: {gridSize}x{gridSize}");
        
    }

    private void UpdateGridSize()
    {
        pixelGrid = new int[gridSize, gridSize];
    }

    private void DrawGrid()
    {
        //Zoom Controls
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Zoom In")) zoom = Mathf.Min(zoom + zoomStep, maxZoom);
        if (GUILayout.Button("Zoom Out")) zoom = Mathf.Max(zoom - zoomStep, minZoom);
        GUILayout.Label($"Zoom: {zoom:F1}x");
        GUILayout.EndHorizontal();
        //end Zoom Controls
        
        GUILayout.Label("Pixel Grid", EditorStyles.boldLabel);
        int scaledCellSize = Mathf.RoundToInt(cellSize * zoom);
        Rect gridRect = GUILayoutUtility.GetRect(gridSize * scaledCellSize, gridSize * scaledCellSize);


        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                int index = pixelGrid[x, y];
                Color color = (index >= 0 && index < palette.Count) ? palette[index] : Color.clear;

                Rect cellRect = new Rect(gridRect.x + x * scaledCellSize, gridRect.y + y * scaledCellSize, scaledCellSize, scaledCellSize);
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
                Handles.DrawLine(new Vector2(cellRect.x, cellRect.y), new Vector2(cellRect.x + scaledCellSize, cellRect.y));
                Handles.DrawLine(new Vector2(cellRect.x, cellRect.y), new Vector2(cellRect.x, cellRect.y + scaledCellSize));
            }
        }

        // Bottom and right border lines
        Handles.DrawLine(
            new Vector2(gridRect.x, gridRect.y + gridSize * scaledCellSize),
            new Vector2(gridRect.x + gridSize * scaledCellSize, gridRect.y + gridSize * scaledCellSize)
        );
        Handles.DrawLine(
            new Vector2(gridRect.x + gridSize * scaledCellSize, gridRect.y),
            new Vector2(gridRect.x + gridSize * scaledCellSize, gridRect.y + gridSize * scaledCellSize)
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
    
    private void DrawSaveLoadButton()
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save PNG"))
        {
            SaveToPNG();
        }

        if (GUILayout.Button("Load PNG"))
        {
            LoadFromPNG();
        }

        GUILayout.EndHorizontal();
    }
    
    private void SaveToPNG()
    {
        string defaultDir = "Assets/PixelArt";
        if (!Directory.Exists(defaultDir)) Directory.CreateDirectory(defaultDir);
        
        string path = EditorUtility.SaveFilePanel("Save Pixel Grid as PNG", "Assets/PixelArt", "pixel_art.png", "png");
        if (string.IsNullOrEmpty(path)) return;

        Texture2D texture = new Texture2D(gridSize, gridSize, TextureFormat.RGBA32, false);

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                int index = pixelGrid[x, y];
                Color color = (index >= 0 && index < palette.Count) ? palette[index] : Color.clear;
                texture.SetPixel(x, gridSize - y - 1, color); // Flip Y for correct display
            }
        }

        texture.Apply();
        byte[] pngData = texture.EncodeToPNG();

        if (pngData != null)
        {
            File.WriteAllBytes(path, pngData);
            AssetDatabase.Refresh();
            Debug.Log($"Saved PNG to: {path}");
        }
    }
    
    private void LoadFromPNG()
    {
        string path = EditorUtility.OpenFilePanel("Load Pixel Grid from PNG", "Assets/PixelArt", "png");
        if (string.IsNullOrEmpty(path)) return;

        byte[] fileData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(gridSize, gridSize);
        texture.LoadImage(fileData);

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Color color = texture.GetPixel(x, gridSize - y - 1); // Flip Y back
                int paletteIndex = GetOrAddColorToPalette(color);
                pixelGrid[x, y] = paletteIndex;
            }
        }

        Repaint();
        Debug.Log($"Loaded PNG from: {path}");
    }

    private void ResizeGrid(int newSize)
    {
        int[,] newGrid = new int[newSize, newSize];

        for (int y = 0; y < newSize; y++)
        {
            for (int x = 0; x < newSize; x++)
            {
                if (x < pixelGrid.GetLength(0) && y < pixelGrid.GetLength(1))
                    newGrid[x, y] = pixelGrid[x, y];
                else
                    newGrid[x, y] = -1;
            }
        }

        pixelGrid = newGrid;
    }



    private void HandleZoomScrollv2()
    {
        Event e = Event.current;
        if (e.type == EventType.ScrollWheel)
        {
            float scroll = e.delta.y;

            if (scroll > 0 && currentGridSizeIndex > 0)
            {
                currentGridSizeIndex--;
                ResizeGrid(gridSizes[currentGridSizeIndex]);
            }
            else if (scroll < 0 && currentGridSizeIndex < gridSizes.Length - 1)
            {
                currentGridSizeIndex++;
                ResizeGrid(gridSizes[currentGridSizeIndex]);
            }

            Repaint();
            e.Use();
        }
    }

    private void HandleZoomScrollv1()
    {
        Event e = Event.current;
        if (e.type == EventType.ScrollWheel && e.control)
        {
            float scrollDelta = -e.delta.y; // Up = positive, Down = negative
            zoom += scrollDelta * zoomStep * 0.1f;
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
            e.Use(); // Prevent Unity from also scrolling the editor window
            Repaint();
        }
    }

}

public enum EditorTool
{
    Draw,
    Erase
}


