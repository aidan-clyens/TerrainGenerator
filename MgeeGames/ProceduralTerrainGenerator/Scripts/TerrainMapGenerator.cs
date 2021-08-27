using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMapGenerator : MonoBehaviour {
    [Header("Generator Settings")]
    [Space(10)]
    public int seed;
    [Range (16, 1024)]
    public int chunkWidth;
    public Vector2 centerPosition = new Vector2(0, 0);
    [Range (1, 10)]
    public int chunkGridWidth = 1;
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

    HeightMapGenerator heightMapGenerator;
    ForestGenerator forestGenerator;
    HydraulicErosion hydraulicErosion;


    Dictionary<Vector2, GameObject> terrainChunks = new Dictionary<Vector2, GameObject>();


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

        // Round map width to nearest power of 2
        chunkWidth = (int)Mathf.Pow(2, Mathf.Round(Mathf.Log(chunkWidth) / Mathf.Log(2)));
        // Round chunk grid width to nearest odd number >= 1
        if (chunkGridWidth % 2 == 0) {
            chunkGridWidth = (int)Mathf.Round(chunkGridWidth / 2) * 2 + 1;
        }

        // Update component settings
        heightMapGenerator.averageMapDepth = averageMapDepth;
        heightMapGenerator.heightMapSettingsList = heightMapSettingsList;
    
        forestGenerator.settings = forestGeneratorSettings;

        hydraulicErosion.settings = hydraulicErosionSettings;
    }

    void Update() {
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
        seed = Random.Range(0, 1000);
        waterLevel = Random.Range(0, 30);

        heightMapSettingsList.Clear();

        int numLayers = Random.Range(1, 4);
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
                GameObject chunk = CreateTerrainChunk(pos, loadAllObjects);
                if (terrainChunks.ContainsKey(pos)) {
                    DestroyImmediate(terrainChunks[pos], true);
                    terrainChunks[pos] = chunk;
                }
                else {
                    terrainChunks.Add(pos, chunk);
                }
            }
        }
    }

    GameObject CreateTerrainChunk(Vector2 position, bool loadAllObjects) {
        int mapOffsetX = (int)(position.x * (chunkWidth - 1)) + seed;
        int mapOffsetY = (int)(position.y * (chunkWidth - 1)) + seed;

        float[,] heightMap = heightMapGenerator.CreateHeightMap(seed, chunkWidth, mapOffsetX, mapOffsetY);

        GameObject chunkGameObject = new GameObject("TerrainChunk");
        GameObject terrainGameObject = CreateTerrain(heightMap);
        terrainGameObject.transform.parent = chunkGameObject.transform;

        if (createWater) {
            // Waves are in the X-direction, so add X offset
            GameObject waterGameObject = CreateWater(position.x * (chunkWidth - 1));
            waterGameObject.transform.parent = chunkGameObject.transform;
        }

        if (createForest) {
            Vector3[] normals = terrainGameObject.GetComponent<MeshFilter>().sharedMesh.normals;
            GameObject forestGameObject = CreateForest(heightMap, normals, loadAllObjects);
            forestGameObject.transform.parent = chunkGameObject.transform;
        }

        chunkGameObject.isStatic = true;
        chunkGameObject.transform.position = new Vector3(position.x * (chunkWidth - 1), 0f, -position.y * (chunkWidth - 1));
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

    GameObject CreateWater(float offset) {
        float[,] heightMap = new float[chunkWidth, chunkWidth];

        for (int z = 0; z < chunkWidth; z++) {
            for (int x = 0; x < chunkWidth; x++) {
                heightMap[x, z] = waterLevel;
            }
        }

        GameObject waterGameObject = new GameObject("Water");
        waterGameObject.AddComponent<MeshFilter>();
        waterGameObject.AddComponent<MeshRenderer>();
        waterGameObject.AddComponent<WaterManager>();

        waterGameObject.GetComponent<WaterManager>().waterLevel = waterLevel;
        waterGameObject.GetComponent<WaterManager>().waveSpeed = waveSpeed;
        waterGameObject.GetComponent<WaterManager>().waveStrength = waveStrength;
        waterGameObject.GetComponent<WaterManager>().offset = offset;

        MeshData meshData = MeshGenerator.Generate(heightMap);
        Mesh mesh = meshData.CreateMesh();

        waterGameObject.GetComponent<MeshRenderer>().material = waterMaterial;
        waterGameObject.GetComponent<MeshFilter>().sharedMesh = mesh;

        return waterGameObject;
    }
}
