using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMapGenerator : MonoBehaviour {

    public GameObject forest;

    public int mapWidth;
    public int mapHeight;
    public int mapDepth;
    public float noiseScale;
    public int noiseOctaves;
    public float persistence;
    public float lacunarity;
    public int mapOffsetX;
    public int mapOffsetY;

    void Start() {
        Terrain terrain = GetComponent<Terrain>();
        ForestGenerator forestGenerator = forest.GetComponent<ForestGenerator>();

        float[,] noiseMap = GenerateNoiseMap(mapWidth, mapHeight, noiseScale, mapOffsetX, mapOffsetY, noiseOctaves, persistence, lacunarity);
        terrain.terrainData = GenerateTerrainData(terrain.terrainData, noiseMap, mapWidth, mapHeight, mapDepth);
    
        forestGenerator.Generate(terrain.terrainData);
    }

    public float[,] GenerateNoiseMap(int width, int height, float scale, int offsetX, int offsetY, int octaves, float persistence, float lacunarity) {
        float[,] noiseMap = new float[width, height];

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) {
                    float sampleX = (float)x / (float)width * scale * frequency + offsetX;
                    float sampleY = (float)y / (float)height * scale * frequency + offsetY;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }   
        }

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
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
