using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class TerrainData {
    public string version = "";

    public int seed;

    public Vector2 centerPosition;
    public int chunkGridWidth;
    public int levelOfDetail;

    // Height Map Data
    public float averageMapDepth;
    public List<HeightMapSettings> heightMapSettingsList;

    // Terrain Settings
    public float waterLevel;
    public bool createWater;
    public bool createForest;
    public Gradient terrainColourGradient;
    public Material terrainMaterial;
    public Material waterMaterial;

    // Hydraulic Erosion Data
    public HydraulicErosionSettings hydraulicErosionSettings;

    // Forest Generator Settings
    public ForestGeneratorSettings forestGeneratorSettings;
}
