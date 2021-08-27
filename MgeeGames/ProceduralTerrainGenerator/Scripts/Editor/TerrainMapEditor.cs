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
        if (GUILayout.Button("Randomize")) {
            Randomize();
            terrainMapGenerator.Clear();
            terrainMapGenerator.Generate(loadAllObjects: true);
        }

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


        string[] saveFiles = GetSaveFiles();

        if (saveFiles.Length > 0) {
            GUILayout.Label("Load Level");
            EditorGUILayout.BeginHorizontal();

            levelNameIndex = EditorGUILayout.Popup(levelNameIndex, saveFiles);
            levelNameLoad = saveFiles[levelNameIndex];

            if (GUILayout.Button("Load")) {
                LoadTerrainData(levelNameLoad);
                terrainMapGenerator.Generate(loadAllObjects: true);
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    void Randomize() {
        terrainMapGenerator.seed = Random.Range(0, 1000);
        terrainMapGenerator.waterLevel = Random.Range(0, 30);

        heightMapGenerator.Randomize();

        EditorUtility.SetDirty(terrainMapGenerator);
        EditorUtility.SetDirty(heightMapGenerator);
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
        terrainData.centerPosition = terrainMapGenerator.centerPosition;
        terrainData.chunkGridWidth = terrainMapGenerator.chunkGridWidth;
        terrainData.chunkWidth = terrainMapGenerator.chunkWidth;
        terrainData.averageMapDepth = heightMapGenerator.averageMapDepth;
        terrainData.heightMapSettingsList = heightMapGenerator.heightMapSettingsList;
        terrainData.waterLevel = terrainMapGenerator.waterLevel;
        terrainData.createWater = terrainMapGenerator.createWater;
        terrainData.createForest = terrainMapGenerator.createForest;
        terrainData.terrainColourGradient = terrainMapGenerator.terrainColourGradient;
        terrainData.terrainMaterial = terrainMapGenerator.terrainMaterial;
        terrainData.waterMaterial = terrainMapGenerator.waterMaterial;
        terrainData.useHydraulicErosion = hydraulicErosion.useHydraulicErosion;
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
        terrainData.forestGeneratorSettings = forestGenerator.settings;

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
        terrainMapGenerator.centerPosition = terrainData.centerPosition;
        terrainMapGenerator.chunkGridWidth = terrainData.chunkGridWidth;
        terrainMapGenerator.chunkWidth = terrainData.chunkWidth;
        heightMapGenerator.averageMapDepth = terrainData.averageMapDepth;
        heightMapGenerator.heightMapSettingsList = terrainData.heightMapSettingsList;
        terrainMapGenerator.waterLevel = terrainData.waterLevel;
        terrainMapGenerator.createWater = terrainData.createWater;
        terrainMapGenerator.createForest = terrainData.createForest;
        terrainMapGenerator.terrainColourGradient = terrainData.terrainColourGradient;
        terrainMapGenerator.terrainMaterial = terrainData.terrainMaterial;
        terrainMapGenerator.waterMaterial = terrainData.waterMaterial;
        hydraulicErosion.useHydraulicErosion = terrainData.useHydraulicErosion;
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
        forestGenerator.settings = terrainData.forestGeneratorSettings;

        EditorUtility.SetDirty(terrainMapGenerator);
        EditorUtility.SetDirty(heightMapGenerator);
        EditorUtility.SetDirty(hydraulicErosion);
        EditorUtility.SetDirty(forestGenerator);
    }
}
