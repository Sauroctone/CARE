
using UnityEngine;

public class terrainGenerator : MonoBehaviour {

    public int Width = 256;
    public int Height = 256;

    public float panX;
    public float panY;

    public float Speed;

    public int Depth = 20;

    public float Scale = 20;
    Terrain terrain;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        
    }

    void Update()
    {
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        panX += Time.deltaTime * Speed;
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = Width + 1;
        terrainData.size = new Vector3(Width, Depth, Height);

        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[Width, Height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }
        return heights;
    }

    float CalculateHeight (int x, int y )
    {
        float xCoord = (float)x / Width * Scale+panX;
        float yCoord = (float)y / Height * Scale+panY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
