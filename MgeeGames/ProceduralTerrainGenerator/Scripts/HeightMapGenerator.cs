using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMapGenerator : MonoBehaviour {
    public float noiseScale;
    public int noiseOctaves;
    public float persistence;
    public float lacunarity;
    public int mapDepth;
    public float noiseRedistributionFactor;
    public bool normalizeLocal;
    public bool useFalloff;
    public bool useHydraulicErosion;

    int maxWidth = 256;


    public float[,] CreateHeightMap(int seed, int mapWidth, int offsetX, int offsetY) {
        float widthFactor = (float)mapWidth / (float)maxWidth;
        float[,] noiseMap = Noise.GeneratePerlinNoiseMap(
            mapWidth,
            mapWidth,
            noiseScale * widthFactor,
            offsetX,
            offsetY,
            noiseOctaves,
            persistence,
            lacunarity,
            noiseRedistributionFactor,
            normalizeLocal
        );

        float[,] falloffMap = Falloff.GenerateFalloffMap(mapWidth, mapWidth);

        if (useHydraulicErosion && normalizeLocal && mapDepth > 0) {
            HydraulicErosion hydraulicErosion = GetComponent<HydraulicErosion>();
            noiseMap = hydraulicErosion.ErodeTerrain(noiseMap, seed);
        }
        else if (useHydraulicErosion && !normalizeLocal) {
            Debug.LogWarning("Warning: Cannot use hydraulic erosion in normalize global mode.");
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
                    heightMap[x, z] = noiseMap[x, z] * mapDepth * widthFactor;
                
                    // Multiply map depth by 2 is normalizing globally to compensate for lost depth
                    if (!normalizeLocal) {
                        heightMap[x, z] *= 2;
                    }
                }
            }
        }

        return heightMap;
    }
}
