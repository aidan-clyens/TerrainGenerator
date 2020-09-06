using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMapGenerator : MonoBehaviour {

    public int mapWidth;
    public int mapHeight;
    public int mapDepth;
    public float noiseScale;
    public int mapOffsetX;
    public int mapOffsetY;

    void Start() {
        Terrain terrain = GetComponent<Terrain>();

        float[,] noiseMap = GenerateNoiseMap(mapWidth, mapHeight, noiseScale, mapOffsetX, mapOffsetY);
        terrain.terrainData = GenerateTerrainData(terrain.terrainData, noiseMap, mapWidth, mapHeight, mapDepth);
    }

    public float[,] GenerateNoiseMap(int width, int height, float scale, int offsetX, int offsetY) {
        float[,] noiseMap = new float[width, height];

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float sampleX = (float)x / (float)width * scale + offsetX;
                float sampleY = (float)y / (float)height * scale + offsetY;

                noiseMap[x, y] = Mathf.PerlinNoise(sampleX, sampleY);
            }   
        }

        return noiseMap;
    }

    public TerrainData GenerateTerrainData(TerrainData terrainData, float[,] noise, int width, int height, int depth) {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, noise);

        return terrainData;
    }
}
