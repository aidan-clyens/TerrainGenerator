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
            terrainMapGenerator.SaveTerrainData(levelNameSave);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Load Level");
        EditorGUILayout.BeginHorizontal();

        string[] saveFiles = GetSaveFiles();

        levelNameIndex = EditorGUILayout.Popup(levelNameIndex, saveFiles);
        levelNameLoad = saveFiles[levelNameIndex];
        if (GUILayout.Button("Load")) {
            terrainMapGenerator.LoadTerrainData(levelNameLoad);
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
}
