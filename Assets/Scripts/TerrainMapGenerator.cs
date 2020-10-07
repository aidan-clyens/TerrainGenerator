using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMapGenerator : MonoBehaviour {
    public int seed = 1;

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

    public bool useHydraulicErosion;
    public bool createWater;
    public bool createForest;

    public Gradient terrainColourGradient;
    public Gradient waterColourGradient;
    public Material terrainMaterial;
    public Material waterMaterial;

    ForestGenerator forestGenerator;

    GameObject terrainGameObject;
    GameObject waterGameObject;

    void Start() {
        Generate();
    }

    void OnDestroy() {
        Clear();    
    }

    public void Generate() {
        float[,] heightMap = CreateHeightMap();

        CreateTerrain(heightMap);
        if (createWater) {
            CreateWater();
        }

        if (createForest) {
            CreateForest(heightMap);
        }
    }

    public void Clear() {
        if (terrainGameObject != null) {
            DestroyImmediate(terrainGameObject, true);
        }

        if (waterGameObject != null) {
            DestroyImmediate(waterGameObject, true);
        }

        if (forestGenerator != null) {
            forestGenerator.Clear();
        }
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
        forestGenerator.Generate(heightMap, waterLevel, seed);
    }

    void CreateWater() {
        float[,] heightMap = new float[mapWidth, mapHeight];

        for (int z = 0; z < mapHeight; z++) {
            for (int x = 0; x < mapWidth; x++) {
                heightMap[x, z] = waterLevel;
            }
        }

        waterGameObject = new GameObject("Water");
        waterGameObject.AddComponent<MeshFilter>();
        waterGameObject.AddComponent<MeshRenderer>();

        MeshData meshData = MeshGenerator.Generate(heightMap, waterColourGradient);
        Mesh mesh = meshData.CreateMesh();

        waterGameObject.GetComponent<MeshRenderer>().material = waterMaterial;
        waterGameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    float[,] CreateHeightMap() {
        float[,] noiseMap = Noise.GeneratePerlinNoiseMap(mapWidth, mapHeight, noiseScale, mapOffsetX, mapOffsetY, noiseOctaves, persistence, lacunarity);
        
        if (useHydraulicErosion) {
            HydraulicErosion hydraulicErosion = GetComponent<HydraulicErosion>();
            noiseMap = hydraulicErosion.ErodeTerrain(noiseMap, seed);
        }

        float[,] heightMap = new float[mapWidth, mapHeight];

        for (int z = 0; z < mapHeight; z++) {
            for (int x = 0; x < mapWidth; x++) {
                heightMap[x, z] = noiseMap[x, z] * mapDepth;
            }
        }

        return heightMap;
    }
}
