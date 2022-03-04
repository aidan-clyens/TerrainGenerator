using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TerrainMapGenerator : MonoBehaviour {
    public const string VERSION = "1.3";

    [Header("Generator Settings")]
    [Space(10)]
    public int seed;
    public Vector2 centerPosition = new Vector2(0, 0);
    [Range (1, 10)]
    public int chunkGridWidth = 1;
    [Range (0, 6)]
    public int levelOfDetail;
    public GameObject viewer;
    public float chunkViewRange;
    public float objectViewRange;

    [Header("Terrain Settings")]
    [Space(10)]
    public Gradient terrainColourGradient;
    public Material terrainMaterial;
    public bool createForest;
    public bool createWater;

    [Header("Height Map Settings")]
    [Space(10)]
    public float averageMapDepth;
    public List<HeightMapSettings> heightMapSettingsList;

    [Header("Hydraulic Erosion Settings")]
    [Space(10)]
    public HydraulicErosionSettings hydraulicErosionSettings;

    [Header("Forest Settings")]
    [Space(10)]
    public ForestGeneratorSettings forestGeneratorSettings;

    [Header("Water Settings")]
    [Space(10)]
    public Material waterMaterial;
    public float waterLevel;
    public float waveSpeed;
    public float waveStrength;

    const int chunkWidth = 241;

    HeightMapGenerator heightMapGenerator;
    ForestGenerator forestGenerator;
    HydraulicErosion hydraulicErosion;


    Dictionary<Vector2, GameObject> terrainChunks = new Dictionary<Vector2, GameObject>();

    Queue<HeightMapThreadInfo> heightMapDataThreadInfoQueue = new Queue<HeightMapThreadInfo>();
    Queue<MeshDataThreadInfo> meshDataThreadInfoQueue = new Queue<MeshDataThreadInfo>();

    void OnEnable() {
        EditorApplication.update += Update;
    }

    void OnDisable() {
        EditorApplication.update -= Update;
    }

    void Start() {
        // Get all Terrain Chunks
        foreach (Transform child in transform) {
            Vector2 position = new Vector2(child.position.x / chunkWidth, child.position.z / chunkWidth);
            if (!terrainChunks.ContainsKey(position)) {
                terrainChunks.Add(position, child.gameObject);
            }
        }

        // Initialize Forest Generator
        List<GameObject> trees = new List<GameObject>();
        foreach (KeyValuePair<Vector2, GameObject> chunkEntry in terrainChunks) {
            Transform forestTransform = chunkEntry.Value.transform.Find("Forest");

            if (forestTransform != null) {
                foreach (Transform treeTransform in forestTransform) {
                    trees.Add(treeTransform.gameObject);
                }   
            }
        }

        forestGenerator.Init(trees, viewer, objectViewRange);
    }

    void OnValidate() {
        // Get components
        if (heightMapGenerator == null) {
            heightMapGenerator = GetComponent<HeightMapGenerator>();
        }

        if (forestGenerator == null) {
            forestGenerator = GetComponent<ForestGenerator>();
        }

        if (hydraulicErosion == null) {
            hydraulicErosion = GetComponent<HydraulicErosion>();
        }

        // Round chunk grid width to nearest odd number >= 1
        if (chunkGridWidth % 2 == 0) {
            chunkGridWidth = (int)Mathf.Round(chunkGridWidth / 2) * 2 + 1;
        }

        // Update component settings
        heightMapGenerator.averageMapDepth = averageMapDepth;
        heightMapGenerator.heightMapSettingsList = heightMapSettingsList;
    
        forestGenerator.settings = forestGeneratorSettings;

        hydraulicErosion.settings = hydraulicErosionSettings;

        if (hydraulicErosion.settings.useHydraulicErosion && chunkGridWidth > 1) {
            Debug.LogWarning("Can only use Hydraulic Erosion for single chunks");
            hydraulicErosion.settings.useHydraulicErosion = false;
        }
    }

    void Update() {
        // Process height map data
        if (heightMapDataThreadInfoQueue.Count > 0) {
            for (int i = 0; i < heightMapDataThreadInfoQueue.Count; i++) {
                HeightMapThreadInfo info = heightMapDataThreadInfoQueue.Dequeue();
                MeshGenerator.RequestMeshData(info.position, info.heightMap, levelOfDetail, OnTerrainMeshDataReceived, terrainColourGradient);
            }
        }

        // Process mesh data
        if (meshDataThreadInfoQueue.Count > 0) {
            for (int i = 0; i < meshDataThreadInfoQueue.Count; i++) {
                MeshDataThreadInfo info = meshDataThreadInfoQueue.Dequeue();

                GameObject chunk;
                if (info.type == MeshType.Water) {
                    chunk = CreateWater(info.position, info.meshData);
                }
                else {
                    chunk = CreateTerrainChunk(info.position, info.meshData);

                    if (createForest) {
                        Vector3[] normals = chunk.GetComponent<MeshFilter>().sharedMesh.normals;
                        GameObject forestGameObject = CreateForest(info.heightMap, normals);
                        forestGameObject.transform.parent = terrainChunks[info.position].transform;
                        forestGameObject.transform.localPosition = new Vector3(0f, 0f, forestGameObject.transform.position.z);
                    }
                }

                chunk.transform.parent = terrainChunks[info.position].transform;
                chunk.transform.localPosition = new Vector3(0f, 0f, chunk.transform.position.z);
            }
        }

        if (viewer == null) return;

        Vector2 position = new Vector2(viewer.transform.position.x, viewer.transform.position.z);

        // Only show Terrain Chunks in range of viewer
        foreach (KeyValuePair<Vector2, GameObject> chunkEntry in terrainChunks) {
            GameObject chunk = chunkEntry.Value;
            Vector2 chunkPosition = new Vector2(chunk.transform.position.x, chunk.transform.position.z);

            if ((position - chunkPosition).magnitude < chunkViewRange) {
                chunk.SetActive(true);
            }
            else {
                chunk.SetActive(false);
            }
        }
    }

    public void Generate(bool loadAllObjects=false) {
        // Generate grid of chunks
        CreateChunkGrid(loadAllObjects);
    }

    public void Clear() {
        // Make all chunks visible before clearing
        foreach (KeyValuePair<Vector2, GameObject> chunkEntry in terrainChunks) {
            GameObject chunk = chunkEntry.Value;
            chunk.SetActive(true);
        }

        Transform[] chunkTransforms = GetComponentsInChildren<Transform>();
        GameObject[] chunks = new GameObject[chunkTransforms.Length - 1];
        terrainChunks.Clear();

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

    public void Randomize() {
        seed = UnityEngine.Random.Range(0, 1000);
        waterLevel = UnityEngine.Random.Range(0, 30);

        heightMapSettingsList.Clear();

        int numLayers = UnityEngine.Random.Range(1, 4);
        for (int i = 0; i < numLayers; i++) {
            HeightMapSettings settings = new HeightMapSettings();
            settings.Randomize();

            heightMapSettingsList.Add(settings);
        }
    }

    void CreateChunkGrid(bool loadAllObjects) {
        int w = (int)Mathf.Round(chunkGridWidth / 2);
        for (int x = -w; x <= w; x++) {
            for (int y = -w; y <= w; y++) {
                Vector2 pos = new Vector2(centerPosition.x + x, centerPosition.y + y);
                GameObject chunk = new GameObject("TerrainChunk");

                chunk.isStatic = true;
                chunk.transform.parent = transform;
                chunk.transform.position = new Vector3(pos.x * (chunkWidth - 1), 0f, -pos.y * (chunkWidth - 1));

                if (terrainChunks.ContainsKey(pos)) {
                    DestroyImmediate(terrainChunks[pos], true);
                    terrainChunks[pos] = chunk;
                }
                else {
                    terrainChunks.Add(pos, chunk);
                }

                RequestTerrainChunk(pos, loadAllObjects);
            }
        }
    }

    void RequestTerrainChunk(Vector2 position, bool loadAllObjects) {
        heightMapGenerator.RequestHeightMapData(seed, chunkWidth, position, OnHeightMapDataReceived);

        if (createWater) {
            float[,] heightMap = new float[chunkWidth, chunkWidth];

            for (int z = 0; z < chunkWidth; z++) {
                for (int x = 0; x < chunkWidth; x++) {
                    heightMap[x, z] = waterLevel;
                }
            }

            MeshGenerator.RequestMeshData(position, heightMap, levelOfDetail, OnWaterMeshDataReceived);
        }
    }

    GameObject CreateForest(float[,] heightMap, Vector3[] terrainNormals) {
        forestGenerator.Clear();
        GameObject forestGameObject = forestGenerator.Generate(heightMap, terrainNormals, waterLevel, seed);

        forestGameObject.isStatic = true;
    
        return forestGameObject;
    }

    void OnHeightMapDataReceived(Vector2 position, float[,] heightMap) {
        lock (heightMapDataThreadInfoQueue) {
            heightMapDataThreadInfoQueue.Enqueue(new HeightMapThreadInfo(position, heightMap));
        }
    }

    void OnTerrainMeshDataReceived(Vector2 position, float[,] heightMap, MeshData meshData) {
        lock (meshDataThreadInfoQueue) {
            meshDataThreadInfoQueue.Enqueue(new MeshDataThreadInfo(position, heightMap, meshData, MeshType.Terrain));
        }
    }

    void OnWaterMeshDataReceived(Vector2 position, float[,] heightMap, MeshData meshData) {
        lock (meshDataThreadInfoQueue) {
            meshDataThreadInfoQueue.Enqueue(new MeshDataThreadInfo(position, heightMap, meshData, MeshType.Water));
        }
    }

    GameObject CreateTerrainChunk(Vector2 position, MeshData meshData) {
        Mesh mesh = meshData.CreateMesh();

        GameObject terrainGameObject = new GameObject("Terrain");
        terrainGameObject.AddComponent<MeshFilter>();
        terrainGameObject.AddComponent<MeshRenderer>();
        terrainGameObject.AddComponent<MeshCollider>();

        terrainGameObject.GetComponent<MeshRenderer>().material = terrainMaterial;
        terrainGameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        terrainGameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    
        terrainGameObject.isStatic = true;

        return terrainGameObject;
    }

    GameObject CreateWater(Vector2 position, MeshData meshData) {
        float offset = position.x * (chunkWidth - 1);

        Mesh mesh = meshData.CreateMesh();

        GameObject waterGameObject = new GameObject("Water");
        waterGameObject.AddComponent<MeshFilter>();
        waterGameObject.AddComponent<MeshRenderer>();
        waterGameObject.AddComponent<WaterManager>();

        waterGameObject.GetComponent<WaterManager>().waterLevel = waterLevel;
        waterGameObject.GetComponent<WaterManager>().waveSpeed = waveSpeed;
        waterGameObject.GetComponent<WaterManager>().waveStrength = waveStrength;
        waterGameObject.GetComponent<WaterManager>().offset = offset;

        waterGameObject.GetComponent<MeshRenderer>().material = waterMaterial;
        waterGameObject.GetComponent<MeshFilter>().sharedMesh = mesh;

        return waterGameObject;
    }
}
