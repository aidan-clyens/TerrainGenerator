using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMapGenerator : MonoBehaviour {
    [Header("Generator Settings")]
    public int seed = 1;
    public int mapWidth;
    public Vector2 position;

    [Header("Terrain Settings")]
    public Gradient terrainColourGradient;
    public Material terrainMaterial;
    public bool useFalloff;
    public bool useHydraulicErosion;
    public bool createForest;

    [Header("Perlin Noise Settings")]
    public float noiseScale;
    public int noiseOctaves;
    public float persistence;
    public float lacunarity;
    public int mapDepth;
    public float noiseRedistributionFactor;

    [Header("Water Settings")]
    public bool createWater;
    public Gradient waterColourGradient;
    public Material waterMaterial;
    public float waterLevel;

    ForestGenerator forestGenerator;

    List<GameObject> terrainChunks = new List<GameObject>();

    public void Generate() {
        GameObject chunk = CreateTerrainChunk(position);
        terrainChunks.Add(chunk);
    }

    public void Clear() {
        foreach (GameObject chunk in terrainChunks) {
            DestroyImmediate(chunk, true);
        }

        if (forestGenerator != null) {
            forestGenerator.Clear();
        }
    }

    public void SaveTerrainData(string levelName) {
        TerrainData terrainData = new TerrainData();
        terrainData.seed = seed;
        terrainData.position = position;
        terrainData.mapWidth = mapWidth;
        terrainData.mapDepth = mapDepth;
        terrainData.noiseScale = noiseScale;
        terrainData.noiseOctaves = noiseOctaves;
        terrainData.persistence = persistence;
        terrainData.lacunarity = lacunarity;
        terrainData.waterLevel = waterLevel;
        terrainData.useHydraulicErosion = useHydraulicErosion;
        terrainData.useFalloff = useFalloff;
        terrainData.createWater = createWater;
        terrainData.createForest = createForest;
        terrainData.terrainColourGradient = terrainColourGradient;
        terrainData.waterColourGradient = waterColourGradient;
        terrainData.terrainMaterial = terrainMaterial;
        terrainData.waterMaterial = waterMaterial;

        string terrainDataJson = JsonUtility.ToJson(terrainData);
        string directory = Application.persistentDataPath + "/worlds"; 
        if (!System.IO.Directory.Exists(directory)) {
            System.IO.Directory.CreateDirectory(directory);
        }

        string filePath = directory + "/" + levelName + ".json";

        Debug.Log("Saved terrain to: " + filePath);
        System.IO.File.WriteAllText(filePath, terrainDataJson);
    }

    public void LoadTerrainData(string levelName) {
        string directory = Application.persistentDataPath + "/worlds"; 
        string filePath = directory + "/" + levelName + ".json";
        string terrainDataJson = System.IO.File.ReadAllText(filePath);

        TerrainData terrainData = JsonUtility.FromJson<TerrainData>(terrainDataJson);
        Debug.Log("Loaded terrain from: " + filePath);

        seed = terrainData.seed;
        position = terrainData.position;
        mapWidth = terrainData.mapWidth;
        mapDepth = terrainData.mapDepth;
        noiseScale = terrainData.noiseScale;
        noiseOctaves = terrainData.noiseOctaves;
        persistence = terrainData.persistence;
        lacunarity = terrainData.lacunarity;
        waterLevel = terrainData.waterLevel;
        useHydraulicErosion = terrainData.useHydraulicErosion;
        useFalloff = terrainData.useFalloff;
        createWater = terrainData.createWater;
        createForest = terrainData.createForest;
        terrainColourGradient = terrainData.terrainColourGradient;
        waterColourGradient = terrainData.waterColourGradient;
        terrainMaterial = terrainData.terrainMaterial;
        waterMaterial = terrainData.waterMaterial;

        Generate();
    }

    GameObject CreateTerrainChunk(Vector2 position) {
        int mapOffsetX = (int)(position.x * (mapWidth - 1)) + seed;
        int mapOffsetY = (int)(position.y * (mapWidth - 1)) + seed;

        float[,] heightMap = CreateHeightMap(mapOffsetX, mapOffsetY);

        GameObject chunkGameObject = new GameObject("TerrainChunk");
        GameObject terrainGameObject = CreateTerrain(heightMap);
        terrainGameObject.transform.parent = chunkGameObject.transform;

        if (createWater) {
            GameObject waterGameObject = CreateWater();
            waterGameObject.transform.parent = chunkGameObject.transform;
        }

        if (createForest) {
            GameObject forestGameObject = CreateForest(heightMap);
            forestGameObject.transform.parent = chunkGameObject.transform;
        }

        chunkGameObject.isStatic = true;
        chunkGameObject.transform.position = new Vector3(position.x * (mapWidth - 1), 0f, -position.y * (mapWidth - 1));

        return chunkGameObject;
    }

    GameObject CreateTerrain(float[,] heightMap) {
        GameObject terrainGameObject = new GameObject("Terrain");
        terrainGameObject.AddComponent<MeshFilter>();
        terrainGameObject.AddComponent<MeshRenderer>();
        terrainGameObject.AddComponent<MeshCollider>();

        MeshData meshData = MeshGenerator.Generate(heightMap, terrainColourGradient);
        Mesh mesh = meshData.CreateMesh();

        terrainGameObject.GetComponent<MeshRenderer>().material = terrainMaterial;
        terrainGameObject.GetComponent<MeshFilter>().mesh = mesh;
        terrainGameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    
        terrainGameObject.isStatic = true;

        return terrainGameObject;
    }

    GameObject CreateForest(float[,] heightMap) {
        forestGenerator = GetComponent<ForestGenerator>();

        forestGenerator.Clear();
        GameObject forestGameObject = forestGenerator.Generate(heightMap, waterLevel, seed);

        forestGameObject.isStatic = true;
    
        return forestGameObject;
    }

    GameObject CreateWater() {
        float[,] heightMap = new float[mapWidth, mapWidth];

        for (int z = 0; z < mapWidth; z++) {
            for (int x = 0; x < mapWidth; x++) {
                heightMap[x, z] = waterLevel;
            }
        }

        GameObject waterGameObject = new GameObject("Water");
        waterGameObject.AddComponent<MeshFilter>();
        waterGameObject.AddComponent<MeshRenderer>();

        MeshData meshData = MeshGenerator.Generate(heightMap, waterColourGradient);
        Mesh mesh = meshData.CreateMesh();

        waterGameObject.GetComponent<MeshRenderer>().material = waterMaterial;
        waterGameObject.GetComponent<MeshFilter>().mesh = mesh;

        return waterGameObject;
    }

    float[,] CreateHeightMap(int offsetX, int offsetY) {
        float[,] noiseMap = Noise.GeneratePerlinNoiseMap(mapWidth, mapWidth, noiseScale, offsetX, offsetY, noiseOctaves, persistence, lacunarity, noiseRedistributionFactor);
        float[,] falloffMap = Falloff.GenerateFalloffMap(mapWidth, mapWidth);

        if (useHydraulicErosion) {
            HydraulicErosion hydraulicErosion = GetComponent<HydraulicErosion>();
            noiseMap = hydraulicErosion.ErodeTerrain(noiseMap, seed);
        }

        float[,] heightMap = new float[mapWidth, mapWidth];

        for (int z = 0; z < mapWidth; z++) {
            for (int x = 0; x < mapWidth; x++) {
                if (useFalloff) {
                    noiseMap[x, z] = Mathf.Clamp01(noiseMap[x, z] - falloffMap[x, z]);
                }

                heightMap[x, z] = noiseMap[x, z] * mapDepth;
            }
        }

        return heightMap;
    }
}
