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

    public bool useFalloff;
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

        Clear();

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

    public void SaveTerrainData(string levelName) {
        TerrainData terrainData = new TerrainData();
        terrainData.seed = seed;
        terrainData.mapWidth = mapWidth;
        terrainData.mapHeight = mapHeight;
        terrainData.mapDepth = mapDepth;
        terrainData.noiseScale = noiseScale;
        terrainData.noiseOctaves = noiseOctaves;
        terrainData.persistence = persistence;
        terrainData.lacunarity = lacunarity;
        terrainData.mapOffsetX = mapOffsetX;
        terrainData.mapOffsetY = mapOffsetY;
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
        string filePath = Application.persistentDataPath + "/" + levelName + ".json";

        Debug.Log("Saved terrain to: " + filePath);
        System.IO.File.WriteAllText(filePath, terrainDataJson);
    }

    public void LoadTerrainData(string levelName) {
        string filePath = Application.persistentDataPath + "/" + levelName + ".json";
        string terrainDataJson = System.IO.File.ReadAllText(filePath);

        TerrainData terrainData = JsonUtility.FromJson<TerrainData>(terrainDataJson);
        Debug.Log("Loaded terrain from: " + filePath);

        seed = terrainData.seed;
        mapWidth = terrainData.mapWidth;
        mapHeight = terrainData.mapHeight;
        mapDepth = terrainData.mapDepth;
        noiseScale = terrainData.noiseScale;
        noiseOctaves = terrainData.noiseOctaves;
        persistence = terrainData.persistence;
        lacunarity = terrainData.lacunarity;
        mapOffsetX = terrainData.mapOffsetX;
        mapOffsetY = terrainData.mapOffsetY;
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
        float[,] falloffMap = Falloff.GenerateFalloffMap(mapWidth, mapHeight);

        if (useHydraulicErosion) {
            HydraulicErosion hydraulicErosion = GetComponent<HydraulicErosion>();
            noiseMap = hydraulicErosion.ErodeTerrain(noiseMap, seed);
        }

        float[,] heightMap = new float[mapWidth, mapHeight];

        for (int z = 0; z < mapHeight; z++) {
            for (int x = 0; x < mapWidth; x++) {
                if (useFalloff) {
                    noiseMap[x, z] = Mathf.Clamp01(noiseMap[x, z] - falloffMap[x, z]);
                }

                heightMap[x, z] = noiseMap[x, z] * mapDepth;
            }
        }

        return heightMap;
    }


    [System.Serializable]
    class TerrainData {
        public int seed;

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

        public bool useFalloff;
        public bool useHydraulicErosion;
        public bool createWater;
        public bool createForest;
        public Gradient terrainColourGradient;
        public Gradient waterColourGradient;
        public Material terrainMaterial;
        public Material waterMaterial;
    }
}
