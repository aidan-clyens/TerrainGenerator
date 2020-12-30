using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class TerrainData {
    public int seed;

    public Vector2 position;

    // Height Map Data
    public int mapWidth;
    public int mapDepth;
    public float noiseScale;
    public int noiseOctaves;
    public float persistence;
    public float lacunarity;
    public float noiseRedistributionFactor;
    public bool normalizeLocal;

    // Terrain Settings
    public float waterLevel;
    public bool useFalloff;
    public bool useHydraulicErosion;
    public bool createWater;
    public bool createForest;
    public Gradient terrainColourGradient;
    public Material terrainMaterial;
    public Material waterMaterial;

    // Hydraulic Erosion Data
    public int iterations;
    public float gravity;
    public float inertia;
    public float capacityFactor;
    public float minSlope;
    public float depositionRadius;
    public float depositionFactor;
    public float erosionRadius;
    public float erosionFactor;
    public float evaporationFactor;
    public int dropletLifetime;

    // Forest Generator Settings
    public List<GameObject> treePrefabs = new List<GameObject>();
    public float density;
    public float slopeThreshold;
    public float verticalOffset;
}
