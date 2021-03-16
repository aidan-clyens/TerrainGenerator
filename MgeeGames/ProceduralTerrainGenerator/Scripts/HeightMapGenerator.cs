using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMapGenerator : MonoBehaviour {
    public NoiseType noiseType;
    public float noiseScale;
    public int noiseOctaves;
    public float persistence;
    public float lacunarity;
    public int mapDepth;
    public float noiseRedistributionFactor;
    public bool useFalloff;
    public bool useHydraulicErosion;

    int maxWidth = 256;


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

        float[,] falloffMap = Falloff.GenerateFalloffMap(mapWidth, mapWidth);

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
                    heightMap[x, z] = 2 * noiseMap[x, z] * mapDepth;
                }
            }
        }

        return heightMap;
    }
}
