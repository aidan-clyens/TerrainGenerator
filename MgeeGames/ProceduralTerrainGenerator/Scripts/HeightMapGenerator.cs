using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMapGenerator : MonoBehaviour {
    [Header("Noise Map Settings")]
    public NoiseType noiseType;
    public float noiseScale;
    public int noiseOctaves;
    public float persistence;
    public float lacunarity;
    public float noiseRedistributionFactor;

    [Header("Height Map Settings")]
    public int mapDepth;
    public bool useFalloff;

    [Header("Biome Height Map Settings")]
    public float biomeNoiseScaleFactor = 10f;
    public float biomeDepthFactor = 5f; 

    const int maxWidth = 256;

    float biomeNoiseScale;
    float biomeDepth;


    public float[,] CreateHeightMap(int seed, int mapWidth, int offsetX, int offsetY) {
        float widthFactor = (float)mapWidth / (float)maxWidth;
        float[,] noiseMap = Noise.GenerateNoiseMap(
            noiseType,
            mapWidth,
            mapWidth,
            noiseScale * widthFactor,
            offsetX,
            offsetY,
            noiseOctaves,
            persistence,
            lacunarity,
            noiseRedistributionFactor
        );

        biomeNoiseScale = noiseScale / biomeNoiseScaleFactor;
        biomeDepth = mapDepth * biomeDepthFactor;
        float[,] biomeMap = Noise.GenerateNoiseMap(
            noiseType,
            mapWidth,
            mapWidth,
            (biomeNoiseScale * widthFactor),
            offsetX,
            offsetY,
            noiseOctaves,
            persistence,
            lacunarity,
            noiseRedistributionFactor
        );

        float[,] falloffMap = Falloff.GenerateFalloffMap(mapWidth, mapWidth);

        bool useHydraulicErosion = GetComponent<HydraulicErosion>().useHydraulicErosion;
        if (useHydraulicErosion && mapDepth > 0) {
            HydraulicErosion hydraulicErosion = GetComponent<HydraulicErosion>();
            noiseMap = hydraulicErosion.ErodeTerrain(noiseMap, seed);
        }

        float[,] heightMap = new float[mapWidth, mapWidth];

        // Determine map depth
        for (int z = 0; z < mapWidth; z++) {
            for (int x = 0; x < mapWidth; x++) {
                if (useFalloff && mapDepth > 0) {
                    noiseMap[x, z] = Mathf.Clamp01(noiseMap[x, z] - falloffMap[x, z]);
                }

                if (mapDepth == 0) {
                    heightMap[x, z] = 1f;
                }
                else {
                    heightMap[x, z] = 2 * (noiseMap[x, z] * mapDepth);
                    heightMap[x, z] += (biomeMap[x, z] * biomeDepth);
                    heightMap[x, z] -= (biomeDepth / 2f);
                }
            }
        }

        return heightMap;
    }
}
