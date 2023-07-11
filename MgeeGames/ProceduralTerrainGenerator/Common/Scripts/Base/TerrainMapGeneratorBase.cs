using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[RequireComponent(typeof(HeightMapGenerator))]
[RequireComponent(typeof(BiomeGenerator))]
public abstract class TerrainMapGeneratorBase : MonoBehaviour
{
    public const string VERSION = "1.4";

    [Header("Generator Settings")]
    [CustomAttributes.HorizontalLine()]
    [Space(10)]
    public int seed;
    public MapData mapData;

    [Space(10)]
    [Header("Height Map Settings")]
    [CustomAttributes.HorizontalLine()]
    [Space(10)]
    public float averageMapDepth;
    public List<HeightMapSettings> heightMapSettingsList;

    [Space(10)]
    [Header("Biome Settings")]
    [CustomAttributes.HorizontalLine()]
    [Space(10)]
    public NoiseSettings temperatureNoiseSettings;
    public NoiseSettings moistureNoiseSettings;

    // Components
    protected HeightMapGenerator heightMapGenerator;
    protected BiomeGenerator biomeGenerator;

    protected Queue<HeightMapThreadInfo> heightMapDataThreadInfoQueue = new Queue<HeightMapThreadInfo>();
    protected Queue<BiomeThreadInfo> biomeDataThreadInfoQueue = new Queue<BiomeThreadInfo>();

    public virtual void Start() {

    }

    public virtual void Update() {
        // Process height map data
        if (heightMapDataThreadInfoQueue.Count > 0) {
            for (int i = 0; i < heightMapDataThreadInfoQueue.Count; i++) {
                HeightMapThreadInfo info = heightMapDataThreadInfoQueue.Dequeue();
                ProcessHeightMapData(info);
            }
        }

        // Process biome data
        if (biomeDataThreadInfoQueue.Count > 0) {
            for (int i = 0; i < biomeDataThreadInfoQueue.Count; i++) {
                BiomeThreadInfo info = biomeDataThreadInfoQueue.Dequeue();
                ProcessBiomeData(info);
            }
        }
    }

    public virtual void OnValidate() {
        // Get components
        if (heightMapGenerator == null) {
            heightMapGenerator = GetComponent<HeightMapGenerator>();
        }

        if (biomeGenerator == null) {
            biomeGenerator = GetComponent<BiomeGenerator>();
        }

        // Update component settings
        heightMapGenerator.averageMapDepth = averageMapDepth;
        heightMapGenerator.heightMapSettingsList = heightMapSettingsList;
    }

    public virtual void Generate() {
        
    }

    public virtual void Clear() {

    }

    public virtual void ProcessHeightMapData(HeightMapThreadInfo info) {

    }

    public virtual void ProcessBiomeData(BiomeThreadInfo info) {

    }

    public virtual void Randomize() {
        seed = UnityEngine.Random.Range(0, 1000);

        heightMapSettingsList.Clear();

        int numLayers = UnityEngine.Random.Range(1, 4);
        for (int i = 0; i < numLayers; i++) {
            HeightMapSettings settings = new HeightMapSettings();
            settings.Randomize();

            heightMapSettingsList.Add(settings);
        }

        temperatureNoiseSettings.Randomize();
        moistureNoiseSettings.Randomize();
    }

    protected void OnHeightMapDataReceived(Vector2 position, float[,] heightMap) {
        lock (heightMapDataThreadInfoQueue) {
            heightMapDataThreadInfoQueue.Enqueue(new HeightMapThreadInfo(position, heightMap));
        }
    }

    protected void OnBiomeDataReceived(float[,] temperatureMap, float[,] moistureMap) {
        lock (biomeDataThreadInfoQueue) {
            biomeDataThreadInfoQueue.Enqueue(new BiomeThreadInfo(temperatureMap, moistureMap));
        }
    }

#if UNITY_EDITOR
    private void OnEnable() {
        EditorApplication.update += Update;
    }

    private void OnDisable() {
        EditorApplication.update -= Update;
    }
#endif
}