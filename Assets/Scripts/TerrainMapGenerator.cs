using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMapGenerator : MonoBehaviour {

    public GameObject forest;

    public int mapWidth;
    public int mapHeight;
    public int mapDepth;
    public float noiseScale;
    public int noiseOctaves;
    public float persistence;
    public float lacunarity;
    public int mapOffsetX;
    public int mapOffsetY;

    void Update() {
        float[,] noiseMap = Noise.GeneratePerlinNoiseMap(mapWidth+1, mapHeight+1, noiseScale, mapOffsetX, mapOffsetY, noiseOctaves, persistence, lacunarity);
    
        CreateMesh(noiseMap);

        if (forest != null) {
            CreateForest(noiseMap);
        }
    }

    void CreateMesh(float[,] noiseMap) {
        MeshGenerator meshGenerator = GetComponent<MeshGenerator>();

        meshGenerator.Generate(mapWidth, mapHeight);
        meshGenerator.SetHeights(noiseMap, mapDepth);
    }

    void CreateForest(float[,] noiseMap) {
        ForestGenerator forestGenerator = forest.GetComponent<ForestGenerator>();
        forestGenerator.Generate(noiseMap);
    }
}
