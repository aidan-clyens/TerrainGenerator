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

        DrawDefaultInspector();

        if (GUILayout.Button("Generate")) {
            terrainMapGenerator.Generate();
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
