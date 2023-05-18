using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProceduralObjectGenerator))]
[RequireComponent(typeof(HydraulicErosion))]
public class TerrainMapGenerator : TerrainMapGeneratorBase {
    [Header("Terrain Settings")]
    [Space(10)]
    public Gradient terrainColourGradient;
    public Material terrainMaterial;
    public bool createProceduralObjects;
    public bool createWater;

    [Header("Hydraulic Erosion Settings")]
    [Space(10)]
    public HydraulicErosionSettings hydraulicErosionSettings;

    [Header("Procedural Object Settings")]
    [Space(10)]
    public ProceduralObjectGeneratorSettings proceduralObjectGeneratorSettings;

    [Header("Water Settings")]
    [Space(10)]
    public Material waterMaterial;
    public float waterLevel;
    public float waveSpeed;
    public float waveStrength;

    private int chunkWidth = 241;

    private ProceduralObjectGenerator proceduralObjectGenerator;
    private HydraulicErosion hydraulicErosion;

    private Dictionary<Vector2, GameObject> terrainChunks = new Dictionary<Vector2, GameObject>();

    private Queue<MeshDataThreadInfo> meshDataThreadInfoQueue = new Queue<MeshDataThreadInfo>();

    public override void Start() {
        base.Start();

        // Get all Terrain Chunks
        foreach (Transform child in transform) {
            Vector2 position = new Vector2(child.position.x / chunkWidth, child.position.z / chunkWidth);
            if (!terrainChunks.ContainsKey(position)) {
                terrainChunks.Add(position, child.gameObject);
            }
        }

        // Initialize Procedural Object Generator
        List<GameObject> trees = new List<GameObject>();
        foreach (KeyValuePair<Vector2, GameObject> chunkEntry in terrainChunks) {
            Transform proceduralObjectsTransform = chunkEntry.Value.transform.Find("ProceduralObjects");

            if (proceduralObjectsTransform != null) {
                foreach (Transform objTransform in proceduralObjectsTransform) {
                    trees.Add(objTransform.gameObject);
                }   
            }
        }

        proceduralObjectGenerator.Init(trees, viewer, objectViewRange);
    }

    public override void OnValidate() {
        base.OnValidate();

        if (proceduralObjectGenerator == null) {
            proceduralObjectGenerator = GetComponent<ProceduralObjectGenerator>();
        }

        if (hydraulicErosion == null) {
            hydraulicErosion = GetComponent<HydraulicErosion>();
        }

        heightMapGenerator.normalize = false;

        proceduralObjectGenerator.settings = proceduralObjectGeneratorSettings;

        hydraulicErosion.settings = hydraulicErosionSettings;

        if (hydraulicErosion.settings.useHydraulicErosion && chunkGridWidth > 1) {
            Debug.LogWarning("Can only use Hydraulic Erosion for single chunks");
            hydraulicErosion.settings.useHydraulicErosion = false;
        }
    }

    public override void Update() {
        base.Update();

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

                    if (createProceduralObjects) {
                        Vector3[] normals = chunk.GetComponent<MeshFilter>().sharedMesh.normals;
                        GameObject proceduralObjects = GenerateProceduralObjects(info.heightMap, normals);
                        proceduralObjects.transform.parent = terrainChunks[info.position].transform;
                        proceduralObjects.transform.localPosition = new Vector3(0f, 0f, proceduralObjects.transform.position.z);
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

    public override void Generate() {
        // Generate grid of chunks
        CreateChunkGrid();
    }

    public override void Clear() {
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

        if (proceduralObjectGenerator != null) {
            proceduralObjectGenerator.Clear();
        }
    }

    public override void Randomize() {
        base.Randomize();

        waterLevel = UnityEngine.Random.Range(0, 30);
    }

    private void CreateChunkGrid(bool loadAllObjects = true) {
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

    private void RequestTerrainChunk(Vector2 position, bool loadAllObjects) {
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

    private GameObject GenerateProceduralObjects(float[,] heightMap, Vector3[] terrainNormals) {
        proceduralObjectGenerator.Clear();
        GameObject proceduralObjects = proceduralObjectGenerator.Generate(heightMap, terrainNormals, waterLevel, seed);

        proceduralObjects.isStatic = true;
    
        return proceduralObjects;
    }

    public override void ProcessHeightMapData(HeightMapThreadInfo info) {
        MeshGenerator.RequestMeshData(info.position, info.heightMap, levelOfDetail, OnTerrainMeshDataReceived, terrainColourGradient);
    }

    private void OnTerrainMeshDataReceived(Vector2 position, float[,] heightMap, MeshData meshData) {
        lock (meshDataThreadInfoQueue) {
            meshDataThreadInfoQueue.Enqueue(new MeshDataThreadInfo(position, heightMap, meshData, MeshType.Terrain));
        }
    }

    private void OnWaterMeshDataReceived(Vector2 position, float[,] heightMap, MeshData meshData) {
        lock (meshDataThreadInfoQueue) {
            meshDataThreadInfoQueue.Enqueue(new MeshDataThreadInfo(position, heightMap, meshData, MeshType.Water));
        }
    }

    private GameObject CreateTerrainChunk(Vector2 position, MeshData meshData) {
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

    private GameObject CreateWater(Vector2 position, MeshData meshData) {
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
