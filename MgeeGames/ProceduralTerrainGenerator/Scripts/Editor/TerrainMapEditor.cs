using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainMapGeneratorBase))]
public class TerrainMapBaseEditor : Editor {
    protected string levelNameSave = "Terrain";
    protected string levelNameLoad = "Terrain";

    protected int levelNameIndex = 0;

    protected TerrainMapGeneratorBase terrainMapGenerator;
    protected HeightMapGenerator heightMapGenerator;
    protected HydraulicErosion hydraulicErosion;
    protected ProceduralObjectGenerator proceduralObjectGenerator;


    public override void OnInspectorGUI() {
        terrainMapGenerator = (TerrainMapGeneratorBase) target;
        heightMapGenerator = terrainMapGenerator.GetComponent<HeightMapGenerator>();
        hydraulicErosion = terrainMapGenerator.GetComponent<HydraulicErosion>();
        proceduralObjectGenerator = terrainMapGenerator.GetComponent<ProceduralObjectGenerator>();

        DrawDefaultInspector();

        // Buttons
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        ShowRandomizeButton();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        ShowGenerateButton();
        ShowClearButton();
    }

    protected void ShowRandomizeButton() {
        if (GUILayout.Button("Randomize")) {
            Randomize();
            terrainMapGenerator.Clear();
            terrainMapGenerator.Generate();
        }
    }

    protected void ShowGenerateButton() {
        if (GUILayout.Button("Generate")) {
            terrainMapGenerator.Generate();
        }
    }

    protected void ShowClearButton() {
        if (GUILayout.Button("Clear")) {
            terrainMapGenerator.Clear();
        }
    }

    protected void Randomize() {
        terrainMapGenerator.Randomize();

        EditorUtility.SetDirty(terrainMapGenerator);
        EditorUtility.SetDirty(heightMapGenerator);
    }

    protected string[] GetSaveFiles() {
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
}

[CustomEditor(typeof(TerrainMapGenerator))]
public class TerrainMapEditor : TerrainMapBaseEditor {
    private TerrainMapGenerator terrainMapGenerator3D;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        terrainMapGenerator3D = (TerrainMapGenerator)target;

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
                terrainMapGenerator.Generate();
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    void SaveTerrainData(string levelName) {
        TerrainData terrainData = new TerrainData();
        terrainData.version = TerrainMapGenerator.VERSION;

        terrainData.seed = terrainMapGenerator3D.seed;
        terrainData.centerPosition = terrainMapGenerator3D.centerPosition;
        terrainData.chunkGridWidth = terrainMapGenerator3D.chunkGridWidth;
        terrainData.levelOfDetail = terrainMapGenerator3D.levelOfDetail;
        terrainData.averageMapDepth = terrainMapGenerator3D.averageMapDepth;
        terrainData.heightMapSettingsList = terrainMapGenerator3D.heightMapSettingsList;
        terrainData.waterLevel = terrainMapGenerator3D.waterLevel;
        terrainData.createWater = terrainMapGenerator3D.createWater;
        terrainData.createProceduralObjects = terrainMapGenerator3D.createProceduralObjects;
        terrainData.terrainColourGradient = terrainMapGenerator3D.terrainColourGradient;
        terrainData.terrainMaterial = terrainMapGenerator3D.terrainMaterial;
        terrainData.waterMaterial = terrainMapGenerator3D.waterMaterial;
        terrainData.hydraulicErosionSettings = terrainMapGenerator3D.hydraulicErosionSettings;
        terrainData.proceduralObjectGeneratorSettings = terrainMapGenerator3D.proceduralObjectGeneratorSettings;

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

        if (!TerrainMapGenerator.VERSION.Equals(terrainData.version)) {
            Debug.LogWarning("Attempting to load Terrain Data from a different version. Data may not load correctly.");
        }

        terrainMapGenerator3D.seed = terrainData.seed;
        terrainMapGenerator3D.centerPosition = terrainData.centerPosition;
        terrainMapGenerator3D.chunkGridWidth = terrainData.chunkGridWidth;
        terrainMapGenerator3D.levelOfDetail = terrainData.levelOfDetail;
        terrainMapGenerator3D.averageMapDepth = terrainData.averageMapDepth;
        terrainMapGenerator3D.heightMapSettingsList = terrainData.heightMapSettingsList;
        terrainMapGenerator3D.waterLevel = terrainData.waterLevel;
        terrainMapGenerator3D.createWater = terrainData.createWater;
        terrainMapGenerator3D.createProceduralObjects = terrainData.createProceduralObjects;
        terrainMapGenerator3D.terrainColourGradient = terrainData.terrainColourGradient;
        terrainMapGenerator3D.terrainMaterial = terrainData.terrainMaterial;
        terrainMapGenerator3D.waterMaterial = terrainData.waterMaterial;
        terrainMapGenerator3D.hydraulicErosionSettings = terrainData.hydraulicErosionSettings;
        terrainMapGenerator3D.proceduralObjectGeneratorSettings = terrainData.proceduralObjectGeneratorSettings;

        EditorUtility.SetDirty(terrainMapGenerator);
        EditorUtility.SetDirty(heightMapGenerator);
        EditorUtility.SetDirty(hydraulicErosion);
        EditorUtility.SetDirty(proceduralObjectGenerator);
    }
}

[CustomEditor(typeof(TerrainMapGenerator2D))]
public class TerrainMap2DEditor : TerrainMapBaseEditor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
    }
}