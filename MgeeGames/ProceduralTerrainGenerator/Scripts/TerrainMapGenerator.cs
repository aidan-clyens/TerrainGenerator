using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMapGenerator : MonoBehaviour {
    [Header("Generator Settings")]
    public int seed;
    public int mapWidth;
    public Vector2 position = new Vector2(0, 0);
    public GameObject viewer;
    public float objectViewRange;

    [Header("Terrain Settings")]
    public Gradient terrainColourGradient;
    public Material terrainMaterial;
    public bool createForest;
    public bool createWater;

    [Header("Water Settings")]
    public Material waterMaterial;
    public float waterLevel;

    ForestGenerator forestGenerator;



    List<GameObject> terrainChunks = new List<GameObject>();


    public void Start() {
        Clear();
        Generate();
    }

    public void Generate(bool loadAllObjects=false) {
        GameObject chunk = CreateTerrainChunk(position, loadAllObjects);
        terrainChunks.Add(chunk);
    }

    public void Clear() {
        Transform[] chunkTransforms = GetComponentsInChildren<Transform>();
        GameObject[] chunks = new GameObject[chunkTransforms.Length - 1];

        int index = 0;
        foreach (Transform chunk in chunkTransforms) {
            if (chunk != transform) {
                chunks[index] = chunk.gameObject;
                index++;
            }
        }

        foreach (GameObject chunk in chunks) {
            DestroyImmediate(chunk, true);
        }

        if (forestGenerator != null) {
            forestGenerator.Clear();
        }
    }

    GameObject CreateTerrainChunk(Vector2 position, bool loadAllObjects) {
        int mapOffsetX = (int)(position.x * (mapWidth - 1)) + seed;
        int mapOffsetY = (int)(position.y * (mapWidth - 1)) + seed;

        HeightMapGenerator heightMapGenerator = GetComponent<HeightMapGenerator>();
        float[,] heightMap = heightMapGenerator.CreateHeightMap(seed, mapWidth, mapOffsetX, mapOffsetY);

        GameObject chunkGameObject = new GameObject("TerrainChunk");
        GameObject terrainGameObject = CreateTerrain(heightMap);
        terrainGameObject.transform.parent = chunkGameObject.transform;

        if (createWater) {
            GameObject waterGameObject = CreateWater();
            waterGameObject.transform.parent = chunkGameObject.transform;
        }

        if (createForest) {
            Vector3[] normals = terrainGameObject.GetComponent<MeshFilter>().sharedMesh.normals;
            GameObject forestGameObject = CreateForest(heightMap, normals, loadAllObjects);
            forestGameObject.transform.parent = chunkGameObject.transform;
        }

        chunkGameObject.isStatic = true;
        chunkGameObject.transform.position = new Vector3(position.x * (mapWidth - 1), 0f, -position.y * (mapWidth - 1));
        chunkGameObject.transform.parent = transform;

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
        terrainGameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        terrainGameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    
        terrainGameObject.isStatic = true;

        return terrainGameObject;
    }

    GameObject CreateForest(float[,] heightMap, Vector3[] terrainNormals, bool loadAllObjects) {
        forestGenerator = GetComponent<ForestGenerator>();
        if (viewer != null) {
            forestGenerator.Init(viewer, objectViewRange);
        }
        else {
            loadAllObjects = true;
        }

        forestGenerator.Clear();
        GameObject forestGameObject = forestGenerator.Generate(heightMap, terrainNormals, waterLevel, seed, loadAllObjects);

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

        MeshData meshData = MeshGenerator.Generate(heightMap);
        Mesh mesh = meshData.CreateMesh();

        waterGameObject.GetComponent<MeshRenderer>().material = waterMaterial;
        waterGameObject.GetComponent<MeshFilter>().mesh = mesh;

        return waterGameObject;
    }
}
