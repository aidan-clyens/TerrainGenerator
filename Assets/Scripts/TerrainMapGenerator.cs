using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMapGenerator : MonoBehaviour {

    public int mapWidth;
    public int mapHeight;
    public int mapDepth;
    public float noiseScale;
    public int noiseOctaves;
    public float persistence;
    public float lacunarity;
    public int mapOffsetX;
    public int mapOffsetY;

    MeshGenerator meshGenerator;
    ForestGenerator forestGenerator;

    void Start() {
        Generate();
    }

    void OnDestroy() {
        Clear();    
    }

    public void Generate() {
        float[,] heightMap = CreateHeightMap();

        CreateMesh(heightMap);
        CreateForest(heightMap);
    }

    public void Clear() {
        forestGenerator.Clear();
    }

    void CreateMesh(float[,] heightMap) {
        meshGenerator = GetComponent<MeshGenerator>();

        meshGenerator.Generate(mapWidth, mapHeight);
        meshGenerator.SetHeights(heightMap);
    }

    void CreateForest(float[,] heightMap) {
        forestGenerator = GetComponent<ForestGenerator>();

        forestGenerator.Clear();
        forestGenerator.Generate(heightMap);
    }

    float[,] CreateHeightMap() {
        float[,] noiseMap = Noise.GeneratePerlinNoiseMap(mapWidth + 1, mapHeight + 1, noiseScale, mapOffsetX, mapOffsetY, noiseOctaves, persistence, lacunarity);
        float[,] heightMap = new float[mapWidth + 1, mapHeight + 1];

        for (int z = 0; z <= mapHeight; z++) {
            for (int x = 0; x <= mapWidth; x++) {
                heightMap[x, z] = noiseMap[x, z] * mapDepth;
            }
        }

        return heightMap;
    }
}
