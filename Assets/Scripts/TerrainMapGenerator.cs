using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMapGenerator : MonoBehaviour {

    public GameObject forest;

    public int mapDepth;
    public float noiseScale;
    public int noiseOctaves;
    public float persistence;
    public float lacunarity;
    public int mapOffsetX;
    public int mapOffsetY;
    
    int mapWidth;
    int mapHeight;

    void Update() {
        MeshGenerator meshGenerator = GetComponent<MeshGenerator>();
        mapWidth = meshGenerator.meshWidth;
        mapHeight = meshGenerator.meshHeight;

        ForestGenerator forestGenerator = forest.GetComponent<ForestGenerator>();

        float[,] noiseMap = Noise.GeneratePerlinNoiseMap(mapWidth+1, mapHeight+1, noiseScale, mapOffsetX, mapOffsetY, noiseOctaves, persistence, lacunarity);
    
        meshGenerator.SetHeights(noiseMap, mapDepth);

        forestGenerator.Generate(noiseMap);
    }
}
