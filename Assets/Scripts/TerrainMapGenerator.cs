using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainMapGenerator : MonoBehaviour {
    [Header("Generator Settings")]
    public int seed = 1;
    public int mapWidth;
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
    static HeightMapGenerator heightMapGenerator;


    static Queue<ThreadInfo<HeightMapData>> heightMapThreadInfoQueue = new Queue<ThreadInfo<HeightMapData>>();
    static Queue<ThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<ThreadInfo<MeshData>>();


    Dictionary<Vector2, TerrainChunk> terrainChunks = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();


    public void Start() {
        heightMapGenerator = GetComponent<HeightMapGenerator>();

        if (infiniteTerrain) {
            GetComponent<HeightMapGenerator>().useHydraulicErosion = false;
            GetComponent<HeightMapGenerator>().normalizeLocal = false;
        }

        Clear();
    }

    public void Update() {
        if (!infiniteTerrain) return;

        if (heightMapThreadInfoQueue.Count > 0) {
            for (int i = 0; i < heightMapThreadInfoQueue.Count; i++) {
                ThreadInfo<HeightMapData> info = heightMapThreadInfoQueue.Dequeue();
                info.callback(info.parameter);
            }
        }

        if (meshDataThreadInfoQueue.Count > 0) {
            for (int i = 0; i < meshDataThreadInfoQueue.Count; i++) {
                ThreadInfo<MeshData> info = meshDataThreadInfoQueue.Dequeue();
                info.callback(info.parameter);
            }
        }

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
                    terrainChunks.Add(viewedChunkCoords, new TerrainChunk(seed, viewedChunkCoords, mapWidth, chunkViewRange, transform, terrainColourGradient, terrainMaterial));
                }
            }
        }
    }

    public void GenerateEditor(bool loadAllObjects=false) {
        if (infiniteTerrain) {
            GetComponent<HeightMapGenerator>().useHydraulicErosion = false;
            GetComponent<HeightMapGenerator>().normalizeLocal = false;
        }

        TerrainChunk chunk = new TerrainChunk(seed, new Vector2(0, 0), mapWidth, 1, transform, terrainColourGradient, terrainMaterial, true);
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

    GameObject CreateTerrainChunk(HeightMapData heightMapData, bool loadAllObjects) {
        GameObject chunkGameObject = new GameObject("TerrainChunk");
        GameObject terrainGameObject = CreateTerrain(heightMapData.heightMap);
        terrainGameObject.transform.parent = chunkGameObject.transform;

        if (createWater) {
            GameObject waterGameObject = CreateWater();
            waterGameObject.transform.parent = chunkGameObject.transform;
        }

        if (createForest) {
            Vector3[] normals = terrainGameObject.GetComponent<MeshFilter>().sharedMesh.normals;
            GameObject forestGameObject = CreateForest(heightMapData.heightMap, normals, loadAllObjects);
            forestGameObject.transform.parent = chunkGameObject.transform;
        }

        chunkGameObject.isStatic = true;
        chunkGameObject.transform.position = new Vector3(heightMapData.position.x * (mapWidth - 1), 0f, heightMapData.position.y * (mapWidth - 1));
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


    public struct HeightMapData {
        public readonly float[,] heightMap;
        public readonly Vector2 position;

        public HeightMapData(float[,] heightMap, Vector2 position) {
            this.heightMap = heightMap;
            this.position = position;
        }
    }


    struct ThreadInfo<T> {
        public readonly Action<T> callback;
        public readonly T parameter;

        public ThreadInfo(Action<T> callback, T parameter) {
            this.callback = callback;
            this.parameter = parameter;
        }
    }


    public class TerrainChunk {

        GameObject terrainGameObject;
        Transform parent;
        Vector3 positionV3;
        Vector2 positionV2;
        int size;
        int seed;
        Gradient terrainColourGradient;
        Material terrainMaterial;

        Vector2 viewerPosition;
        int chunkViewRange;


        public TerrainChunk(int seed, Vector2 position, int size, int chunkViewRange, Transform parent, Gradient terrainColourGradient, Material terrainMaterial, bool editor=false) {
            this.seed = seed;
            this.size = size;
            this.chunkViewRange = chunkViewRange;
            this.parent = parent;
            this.terrainColourGradient = terrainColourGradient;
            this.terrainMaterial = terrainMaterial;

            positionV3 = new Vector3(position.x * (size - 1), 0f, position.y * (size - 1));
            positionV2 = new Vector2(positionV3.x, positionV3.z);

            terrainGameObject = new GameObject("Terrain");
            terrainGameObject.AddComponent<MeshFilter>();
            terrainGameObject.AddComponent<MeshRenderer>();
            terrainGameObject.AddComponent<MeshCollider>();

            terrainGameObject.transform.position = positionV3;
            terrainGameObject.transform.parent = parent;
            terrainGameObject.isStatic = true;

            terrainGameObject.SetActive(editor);

            RequestHeightMapData(OnHeightMapDataReceived);
        }

        public void Update(Vector2 viewerPosition) {
            bool visible = ((viewerPosition - positionV2).magnitude < chunkViewRange * size);
            terrainGameObject.SetActive(visible);
        }

        public bool IsVisible() {
            return terrainGameObject.activeSelf;
        }

        public void RequestHeightMapData(Action<HeightMapData> callback) {
            ThreadStart threadStart = delegate {
                CreateHeightMapDataThread(callback);
            };
            new Thread(threadStart).Start();
        }

        public void CreateHeightMapDataThread(Action<HeightMapData> callback) {
            int mapOffsetX = seed;
            int mapOffsetY = seed;

            float[,] heightMap = heightMapGenerator.CreateHeightMap(seed, size, mapOffsetX, mapOffsetY);

            HeightMapData heightMapData = new HeightMapData(heightMap, positionV2);

            lock (heightMapThreadInfoQueue) {
                heightMapThreadInfoQueue.Enqueue(new ThreadInfo<HeightMapData>(callback, heightMapData));
            }
        }

        public void OnHeightMapDataReceived(HeightMapData heightMapData) {
            RequestMeshData(heightMapData, OnMeshDataReceived);
        }

        public void RequestMeshData(HeightMapData heightMapData, Action<MeshData> callback) {
            ThreadStart threadStart = delegate {
                CreateMeshDataThread(heightMapData, callback);
            };

            new Thread(threadStart).Start();
        }

        public void CreateMeshDataThread(HeightMapData heightMapData, Action<MeshData> callback) {
            MeshData meshData = MeshGenerator.Generate(heightMapData.heightMap, terrainColourGradient);

            lock (meshDataThreadInfoQueue) {
                meshDataThreadInfoQueue.Enqueue(new ThreadInfo<MeshData>(callback, meshData));
            }
        }

        public void OnMeshDataReceived(MeshData meshData) {
            Mesh mesh = meshData.CreateMesh();

            terrainGameObject.GetComponent<MeshRenderer>().material = terrainMaterial;
            terrainGameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
            terrainGameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        }
    }
}