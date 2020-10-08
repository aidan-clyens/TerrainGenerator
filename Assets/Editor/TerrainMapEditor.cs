using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (TerrainMapGenerator))]
public class TerrainMapEditor : Editor {
    string levelNameSave = "Terrain";
    string levelNameLoad = "Terrain";


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
        levelNameLoad = EditorGUILayout.TextField(levelNameLoad);
        if (GUILayout.Button("Load")) {
            terrainMapGenerator.LoadTerrainData(levelNameLoad);
        }
        EditorGUILayout.EndHorizontal();
    }
}
