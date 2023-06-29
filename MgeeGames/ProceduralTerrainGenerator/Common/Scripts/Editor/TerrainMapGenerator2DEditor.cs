using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainMapGenerator2D))]
public class TerrainMap2DEditor : TerrainMapGeneratorBaseEditor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
    }
}