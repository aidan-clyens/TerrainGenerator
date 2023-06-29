using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainMapGeneratorBase))]
public class TerrainMapGeneratorBaseEditor : Editor {
    protected string levelNameSave = "Terrain";
    protected string levelNameLoad = "Terrain";

    protected int levelNameIndex = 0;

    protected TerrainMapGeneratorBase terrainMapGenerator;
    protected HeightMapGenerator heightMapGenerator;

    public override void OnInspectorGUI() {
        terrainMapGenerator = (TerrainMapGeneratorBase) target;
        heightMapGenerator = terrainMapGenerator.GetComponent<HeightMapGenerator>();

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
