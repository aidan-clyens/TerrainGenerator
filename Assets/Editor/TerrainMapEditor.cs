using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (TerrainMapGenerator))]
public class TerrainMapEditor : Editor {
    string levelNameSave = "Terrain";
    string levelNameLoad = "Terrain";

    int levelNameIndex = 0;

    TerrainMapGenerator terrainMapGenerator;
    HeightMapGenerator heightMapGenerator;
    HydraulicErosion hydraulicErosion;
    ForestGenerator forestGenerator;


    public override void OnInspectorGUI() {
        terrainMapGenerator = (TerrainMapGenerator) target;
        heightMapGenerator = terrainMapGenerator.GetComponent<HeightMapGenerator>();
        hydraulicErosion = terrainMapGenerator.GetComponent<HydraulicErosion>();
        forestGenerator = terrainMapGenerator.GetComponent<ForestGenerator>();

        DrawDefaultInspector();

        // Buttons
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Generate")) {
            terrainMapGenerator.Generate(loadAllObjects: true);
        }

        if (GUILayout.Button("Clear")) {
            terrainMapGenerator.Clear();
        }

        GUILayout.Label("Save Level");
        EditorGUILayout.BeginHorizontal();
        levelNameSave = EditorGUILayout.TextField(levelNameSave);
        if (GUILayout.Button("Save")) {
            SaveTerrainData(levelNameSave);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Load Level");
        EditorGUILayout.BeginHorizontal();

        string[] saveFiles = GetSaveFiles();

        levelNameIndex = EditorGUILayout.Popup(levelNameIndex, saveFiles);
        levelNameLoad = saveFiles[levelNameIndex];
        if (GUILayout.Button("Load")) {
            LoadTerrainData(levelNameLoad);
            terrainMapGenerator.Generate(loadAllObjects: true);
        }
        EditorGUILayout.EndHorizontal();
    }

    string[] GetSaveFiles() {
        string directory = Application.persistentDataPath + "/worlds";
        if (!System.IO.Directory.Exists(directory)) {
            System.IO.Directory.CreateDirectory(directory);
        }

        string[] files = System.IO.Directory.GetFiles(directory);

        for (int i = 0; i < files.Length; i++) {
            files[i] = System.IO.Path.GetFileNameWithoutExtension(files[i]);
        }

        return files;
    }

    void SaveTerrainData(string levelName) {
        TerrainData terrainData = new TerrainData();
        terrainData.seed = terrainMapGenerator.seed;
        terrainData.position = terrainMapGenerator.position;
        terrainData.mapWidth = terrainMapGenerator.mapWidth;
        terrainData.mapDepth = heightMapGenerator.mapDepth;
        terrainData.noiseScale = heightMapGenerator.noiseScale;
        terrainData.noiseOctaves = heightMapGenerator.noiseOctaves;
        terrainData.persistence = heightMapGenerator.persistence;
        terrainData.lacunarity = heightMapGenerator.lacunarity;
        terrainData.normalizeLocal = heightMapGenerator.normalizeLocal;
        terrainData.noiseRedistributionFactor = heightMapGenerator.noiseRedistributionFactor;
        terrainData.waterLevel = terrainMapGenerator.waterLevel;
        terrainData.useHydraulicErosion = heightMapGenerator.useHydraulicErosion;
        terrainData.useFalloff = heightMapGenerator.useFalloff;
        terrainData.createWater = terrainMapGenerator.createWater;
        terrainData.createForest = terrainMapGenerator.createForest;
        terrainData.terrainColourGradient = terrainMapGenerator.terrainColourGradient;
        terrainData.terrainMaterial = terrainMapGenerator.terrainMaterial;
        terrainData.waterMaterial = terrainMapGenerator.waterMaterial;
        terrainData.iterations = hydraulicErosion.iterations;
        terrainData.gravity = hydraulicErosion.gravity;
        terrainData.inertia = hydraulicErosion.inertia;
        terrainData.capacityFactor = hydraulicErosion.capacityFactor;
        terrainData.minSlope = hydraulicErosion.minSlope;
        terrainData.erosionFactor = hydraulicErosion.erosionFactor;
        terrainData.depositionRadius = hydraulicErosion.depositionRadius;
        terrainData.depositionFactor = hydraulicErosion.depositionFactor;
        terrainData.erosionRadius = hydraulicErosion.erosionRadius;
        terrainData.evaporationFactor = hydraulicErosion.evaporationFactor;
        terrainData.dropletLifetime = hydraulicErosion.dropletLifetime;
        terrainData.treePrefabs = forestGenerator.treePrefabs;
        terrainData.numTrees = forestGenerator.numTrees;
        terrainData.slopeThreshold = forestGenerator.slopeThreshold;
        terrainData.verticalOffset = forestGenerator.verticalOffset;

        string terrainDataJson = JsonUtility.ToJson(terrainData);
        string directory = Application.persistentDataPath + "/worlds"; 
        if (!System.IO.Directory.Exists(directory)) {
            System.IO.Directory.CreateDirectory(directory);
        }

        string filePath = directory + "/" + levelName + ".json";

        Debug.Log("Saved terrain to: " + filePath);
        System.IO.File.WriteAllText(filePath, terrainDataJson);
    }

    void LoadTerrainData(string levelName) {
        string directory = Application.persistentDataPath + "/worlds"; 
        string filePath = directory + "/" + levelName + ".json";
        string terrainDataJson = System.IO.File.ReadAllText(filePath);

        TerrainData terrainData = JsonUtility.FromJson<TerrainData>(terrainDataJson);
        Debug.Log("Loaded terrain from: " + filePath);

        terrainMapGenerator.seed = terrainData.seed;
        terrainMapGenerator.position = terrainData.position;
        terrainMapGenerator.mapWidth = terrainData.mapWidth;
        heightMapGenerator.mapDepth = terrainData.mapDepth;
        heightMapGenerator.noiseScale = terrainData.noiseScale;
        heightMapGenerator.noiseOctaves = terrainData.noiseOctaves;
        heightMapGenerator.persistence = terrainData.persistence;
        heightMapGenerator.lacunarity = terrainData.lacunarity;
        heightMapGenerator.noiseRedistributionFactor = terrainData.noiseRedistributionFactor;
        heightMapGenerator.normalizeLocal = terrainData.normalizeLocal;
        terrainMapGenerator.waterLevel = terrainData.waterLevel;
        heightMapGenerator.useHydraulicErosion = terrainData.useHydraulicErosion;
        heightMapGenerator.useFalloff = terrainData.useFalloff;
        terrainMapGenerator.createWater = terrainData.createWater;
        terrainMapGenerator.createForest = terrainData.createForest;
        terrainMapGenerator.terrainColourGradient = terrainData.terrainColourGradient;
        terrainMapGenerator.terrainMaterial = terrainData.terrainMaterial;
        terrainMapGenerator.waterMaterial = terrainData.waterMaterial;
        hydraulicErosion.iterations = terrainData.iterations;
        hydraulicErosion.gravity = terrainData.gravity;
        hydraulicErosion.inertia = terrainData.inertia;
        hydraulicErosion.capacityFactor = terrainData.capacityFactor;
        hydraulicErosion.minSlope = terrainData.minSlope;
        hydraulicErosion.erosionFactor = terrainData.erosionFactor;
        hydraulicErosion.depositionRadius = terrainData.depositionRadius;
        hydraulicErosion.depositionFactor = terrainData.depositionFactor;
        hydraulicErosion.erosionRadius = terrainData.erosionRadius;
        hydraulicErosion.evaporationFactor = terrainData.evaporationFactor;
        hydraulicErosion.dropletLifetime = terrainData.dropletLifetime;
        forestGenerator.treePrefabs = terrainData.treePrefabs;
        forestGenerator.numTrees = terrainData.numTrees;
        forestGenerator.slopeThreshold = terrainData.slopeThreshold;
        forestGenerator.verticalOffset = terrainData.verticalOffset;
    }
}
