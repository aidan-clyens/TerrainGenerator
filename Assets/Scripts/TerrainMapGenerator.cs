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
    public float waterLevel;

    public Gradient terrainColourGradient;
    public Material terrainMaterial;

    ForestGenerator forestGenerator;

    GameObject terrainGameObject;

    void Start() {
        Generate();
    }

    void OnDestroy() {
        Clear();    
    }

    public void Generate() {
        float[,] heightMap = CreateHeightMap();

        CreateTerrain(heightMap);
        CreateForest(heightMap);
    }

    public void Clear() {
        DestroyImmediate(terrainGameObject, true);
        forestGenerator.Clear();
    }

    void CreateTerrain(float[,] heightMap) {
        terrainGameObject = new GameObject("Terrain");
        terrainGameObject.AddComponent<MeshFilter>();
        terrainGameObject.AddComponent<MeshRenderer>();
        terrainGameObject.AddComponent<MeshCollider>();

        MeshData meshData = MeshGenerator.Generate(heightMap, terrainColourGradient);
        Mesh mesh = meshData.CreateMesh();

        terrainGameObject.GetComponent<MeshRenderer>().material = terrainMaterial;
        terrainGameObject.GetComponent<MeshFilter>().mesh = mesh;
        terrainGameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void CreateForest(float[,] heightMap) {
        forestGenerator = GetComponent<ForestGenerator>();

        forestGenerator.Clear();
        forestGenerator.Generate(heightMap, waterLevel);
    }

    float[,] CreateHeightMap() {
        float[,] noiseMap = Noise.GeneratePerlinNoiseMap(mapWidth, mapHeight, noiseScale, mapOffsetX, mapOffsetY, noiseOctaves, persistence, lacunarity);
        float[,] heightMap = new float[mapWidth, mapHeight];

        for (int z = 0; z < mapHeight; z++) {
            for (int x = 0; x < mapWidth; x++) {
                heightMap[x, z] = noiseMap[x, z] * mapDepth;
            }
        }

        return heightMap;
    }
}
