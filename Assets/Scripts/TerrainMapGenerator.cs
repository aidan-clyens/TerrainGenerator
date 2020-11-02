using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMapGenerator : MonoBehaviour {
    [Header("Generator Settings")]
    public int seed = 1;
    public int mapWidth;
    public Vector2 position = new Vector2(0, 0);
    public GameObject viewer;
    public int chunkViewRange;
    public float objectViewRange;
    public bool infiniteTerrain;

    [Header("Terrain Settings")]
    public Gradient terrainColourGradient;
    public Material terrainMaterial;
    public bool createForest;
    public bool createWater;

    [Header("Water Settings")]
    public Material waterMaterial;
    public float waterLevel;

    ForestGenerator forestGenerator;



    // List<GameObject> terrainChunks = new List<GameObject>();
    Dictionary<Vector2, TerrainChunk> terrainChunks = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();


    public void Start() {
        Clear();
        Generate();
    }

    public void Update() {
        if (!infiniteTerrain) return;

        Vector2 viewerPosition = new Vector2(viewer.transform.position.x, viewer.transform.position.z);

        Vector2 viewPositionCoords = new Vector2(
            Mathf.RoundToInt(viewerPosition.x / mapWidth),
            Mathf.RoundToInt(viewerPosition.y / mapWidth)
        );

        foreach (TerrainChunk chunk in terrainChunksVisibleLastUpdate) {
            chunk.Update(viewerPosition);
        }
        terrainChunksVisibleLastUpdate.Clear();

        for (int yOffset = -chunkViewRange; yOffset <= chunkViewRange; yOffset++) {
            for (int xOffset = -chunkViewRange; xOffset <= chunkViewRange; xOffset++) {
                Vector2 viewedChunkCoords = viewPositionCoords + new Vector2(xOffset, yOffset);

                if (terrainChunks.ContainsKey(viewedChunkCoords)) {
                    terrainChunks[viewedChunkCoords].Update(viewerPosition);
                    if (terrainChunks[viewedChunkCoords].IsVisible()) {
                        terrainChunksVisibleLastUpdate.Add(terrainChunks[viewedChunkCoords]);
                    }
                }
                else {
                    terrainChunks.Add(viewedChunkCoords, new TerrainChunk(viewedChunkCoords, mapWidth, chunkViewRange, transform));
                }
            }
        }
    }

    public GameObject Generate(bool loadAllObjects=false) {
        if (infiniteTerrain) {
            GetComponent<HeightMapGenerator>().useHydraulicErosion = false;
            GetComponent<HeightMapGenerator>().normalizeLocal = false;
        }

        GameObject chunk = CreateTerrainChunk(position, loadAllObjects);
        terrainChunks.Add(position, chunk);
    
        return chunk;
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
        chunkGameObject.transform.position = new Vector3(position.x * (mapWidth - 1), 0f, position.y * (mapWidth - 1));
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
        forestGenerator.Init(viewer, objectViewRange);

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


    public class TerrainChunk {

        GameObject meshObject;
        Vector3 positionV3;
        Vector2 positionV2;
        int size;

        Vector2 viewerPosition;
        int chunkViewRange;


        public TerrainChunk(Vector2 position, int size, int chunkViewRange, Transform parent) {
            this.size = size;
            this.chunkViewRange = chunkViewRange;

            positionV3 = new Vector3(position.x * (size - 1), 0f, position.y * (size - 1));
            positionV2 = new Vector2(positionV3.x, positionV3.z);

            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = positionV3;
            meshObject.transform.localScale = Vector3.one * size / 10f;
            meshObject.transform.parent = parent;

            meshObject.SetActive(false);
        }

        public void Update(Vector2 viewerPosition) {
            bool visible = ((viewerPosition - positionV2).magnitude < chunkViewRange * size);
            meshObject.SetActive(visible);
        }

        public bool IsVisible() {
            return meshObject.activeSelf;
        }
    }
}