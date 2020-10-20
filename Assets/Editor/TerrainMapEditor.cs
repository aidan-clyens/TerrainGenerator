using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (TerrainMapGenerator))]
public class TerrainMapEditor : Editor {
    string levelNameSave = "Terrain";
    string levelNameLoad = "Terrain";

    int levelNameIndex = 0;

    public override void OnInspectorGUI() {
        TerrainMapGenerator terrainMapGenerator = (TerrainMapGenerator) target;

        // Generator Settings
        EditorGUILayout.LabelField("Generator Settings", EditorStyles.boldLabel);
        terrainMapGenerator.seed = EditorGUILayout.IntField("Seed", terrainMapGenerator.seed);
        terrainMapGenerator.mapWidth = EditorGUILayout.IntField("Map Width", terrainMapGenerator.mapWidth);

        // Terrain Settings
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Terrain Settings", EditorStyles.boldLabel);
        terrainMapGenerator.terrainColourGradient = EditorGUILayout.GradientField("Terrain Colour Gradient", terrainMapGenerator.terrainColourGradient);
        terrainMapGenerator.terrainMaterial = (Material)EditorGUILayout.ObjectField("Terrain Material", terrainMapGenerator.terrainMaterial, typeof(Material));
        terrainMapGenerator.useFalloff = EditorGUILayout.Toggle("Use Falloff", terrainMapGenerator.useFalloff);
        terrainMapGenerator.useHydraulicErosion = EditorGUILayout.Toggle("Use Hydraulic Erosion", terrainMapGenerator.useHydraulicErosion);
        terrainMapGenerator.createForest = EditorGUILayout.Toggle("Create Forest", terrainMapGenerator.createForest);

        // Perlin Noise Height Map Settings
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Perlin Noise Settings", EditorStyles.boldLabel);
        terrainMapGenerator.mapDepth = EditorGUILayout.IntField("Map Depth", terrainMapGenerator.mapDepth);
        terrainMapGenerator.noiseScale = EditorGUILayout.FloatField("Noise Scale", terrainMapGenerator.noiseScale);
        terrainMapGenerator.noiseOctaves = EditorGUILayout.IntField("Noise Octaves", terrainMapGenerator.noiseOctaves);
        terrainMapGenerator.persistence = EditorGUILayout.FloatField("Persistence", terrainMapGenerator.persistence);
        terrainMapGenerator.lacunarity = EditorGUILayout.FloatField("Lacunarity", terrainMapGenerator.lacunarity);
        terrainMapGenerator.noiseRedistributionFactor = EditorGUILayout.FloatField("Noise Redistribution Factor", terrainMapGenerator.noiseRedistributionFactor);
        terrainMapGenerator.normalizeLocal = EditorGUILayout.Toggle("Normalize Local", terrainMapGenerator.normalizeLocal);

        // Water Settings
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Water Settings", EditorStyles.boldLabel);
        terrainMapGenerator.createWater = EditorGUILayout.Toggle("Create Water", terrainMapGenerator.createWater);
        terrainMapGenerator.waterMaterial = (Material)EditorGUILayout.ObjectField("Water Material", terrainMapGenerator.waterMaterial, typeof(Material));
        terrainMapGenerator.waterLevel = EditorGUILayout.FloatField("Water Level", terrainMapGenerator.waterLevel);

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
            SaveTerrainData(terrainMapGenerator, levelNameSave);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Load Level");
        EditorGUILayout.BeginHorizontal();

        string[] saveFiles = GetSaveFiles();

        levelNameIndex = EditorGUILayout.Popup(levelNameIndex, saveFiles);
        levelNameLoad = saveFiles[levelNameIndex];
        if (GUILayout.Button("Load")) {
            LoadTerrainData(terrainMapGenerator, levelNameLoad);
            terrainMapGenerator.Generate(loadAllObjects: true);
        }
        EditorGUILayout.EndHorizontal();
    }

    string[] GetSaveFiles() {
        string[] files = System.IO.Directory.GetFiles(Application.persistentDataPath + "/worlds");

        for (int i = 0; i < files.Length; i++) {
            files[i] = System.IO.Path.GetFileNameWithoutExtension(files[i]);
        }

        return files;
    }

    void SaveTerrainData(TerrainMapGenerator terrainMapGenerator, string levelName) {
        TerrainData terrainData = new TerrainData();
        terrainData.seed = terrainMapGenerator.seed;
        terrainData.position = terrainMapGenerator.position;
        terrainData.mapWidth = terrainMapGenerator.mapWidth;
        terrainData.mapDepth = terrainMapGenerator.mapDepth;
        terrainData.noiseScale = terrainMapGenerator.noiseScale;
        terrainData.noiseOctaves = terrainMapGenerator.noiseOctaves;
        terrainData.persistence = terrainMapGenerator.persistence;
        terrainData.lacunarity = terrainMapGenerator.lacunarity;
        terrainData.normalizeLocal = terrainMapGenerator.normalizeLocal;
        terrainData.noiseRedistributionFactor = terrainMapGenerator.noiseRedistributionFactor;
        terrainData.waterLevel = terrainMapGenerator.waterLevel;
        terrainData.useHydraulicErosion = terrainMapGenerator.useHydraulicErosion;
        terrainData.useFalloff = terrainMapGenerator.useFalloff;
        terrainData.createWater = terrainMapGenerator.createWater;
        terrainData.createForest = terrainMapGenerator.createForest;
        terrainData.terrainColourGradient = terrainMapGenerator.terrainColourGradient;
        terrainData.terrainMaterial = terrainMapGenerator.terrainMaterial;
        terrainData.waterMaterial = terrainMapGenerator.waterMaterial;

        string terrainDataJson = JsonUtility.ToJson(terrainData);
        string directory = Application.persistentDataPath + "/worlds"; 
        if (!System.IO.Directory.Exists(directory)) {
            System.IO.Directory.CreateDirectory(directory);
        }

        string filePath = directory + "/" + levelName + ".json";

        Debug.Log("Saved terrain to: " + filePath);
        System.IO.File.WriteAllText(filePath, terrainDataJson);
    }

    void LoadTerrainData(TerrainMapGenerator terrainMapGenerator, string levelName) {
        string directory = Application.persistentDataPath + "/worlds"; 
        string filePath = directory + "/" + levelName + ".json";
        string terrainDataJson = System.IO.File.ReadAllText(filePath);

        TerrainData terrainData = JsonUtility.FromJson<TerrainData>(terrainDataJson);
        Debug.Log("Loaded terrain from: " + filePath);

        terrainMapGenerator.seed = terrainData.seed;
        terrainMapGenerator.position = terrainData.position;
        terrainMapGenerator.mapWidth = terrainData.mapWidth;
        terrainMapGenerator.mapDepth = terrainData.mapDepth;
        terrainMapGenerator.noiseScale = terrainData.noiseScale;
        terrainMapGenerator.noiseOctaves = terrainData.noiseOctaves;
        terrainMapGenerator.persistence = terrainData.persistence;
        terrainMapGenerator.lacunarity = terrainData.lacunarity;
        terrainMapGenerator.noiseRedistributionFactor = terrainData.noiseRedistributionFactor;
        terrainMapGenerator.normalizeLocal = terrainData.normalizeLocal;
        terrainMapGenerator.waterLevel = terrainData.waterLevel;
        terrainMapGenerator.useHydraulicErosion = terrainData.useHydraulicErosion;
        terrainMapGenerator.useFalloff = terrainData.useFalloff;
        terrainMapGenerator.createWater = terrainData.createWater;
        terrainMapGenerator.createForest = terrainData.createForest;
        terrainMapGenerator.terrainColourGradient = terrainData.terrainColourGradient;
        terrainMapGenerator.terrainMaterial = terrainData.terrainMaterial;
        terrainMapGenerator.waterMaterial = terrainData.waterMaterial;
    }
}
