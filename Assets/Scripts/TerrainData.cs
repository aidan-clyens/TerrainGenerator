using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class TerrainData {
    public int seed;

    public Vector2 position;

    public int mapWidth;
    public int mapDepth;
    public float noiseScale;
    public int noiseOctaves;
    public float persistence;
    public float lacunarity;
    public float noiseRedistributionFactor;
    public bool normalizeLocal;
    public float waterLevel;

    public bool useFalloff;
    public bool useHydraulicErosion;
    public bool createWater;
    public bool createForest;
    public Gradient terrainColourGradient;
    public Material terrainMaterial;
    public Material waterMaterial;
}
