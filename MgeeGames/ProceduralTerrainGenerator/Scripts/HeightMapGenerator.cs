using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeightMapSettings {
    [HideInInspector]
    public int chunkWidth;
    public int mapDepth;
    public NoiseType noiseType;
    public float noiseScale;
    public int noiseOctaves;
    public float persistence;
    public float lacunarity;
    public float noiseRedistributionFactor;
    public bool useFalloff;

    public void Randomize() {
        noiseScale = Random.Range(1f, 5f);
        noiseOctaves = Random.Range(1, 6);
        persistence = Random.Range(0.1f, 0.5f);
        lacunarity = Random.Range(1f, 2f);
        noiseRedistributionFactor = Random.Range(1f, 3f);
        mapDepth = Random.Range(30, 100);
    }
}

public class HeightMapGenerator : MonoBehaviour {
    public HeightMapSettings heightMapSettings;

    const int maxWidth = 256;

    float biomeNoiseScale;
    float biomeDepth;


    public float[,] CreateHeightMap(int seed, int mapWidth, int offsetX, int offsetY) {
        heightMapSettings.chunkWidth = mapWidth;

        float widthFactor = (float)mapWidth / (float)maxWidth;
        float[,] noiseMap = Noise.GenerateNoiseMap(
            heightMapSettings.noiseType,
            mapWidth,
            mapWidth,
            heightMapSettings.noiseScale * widthFactor,
            offsetX,
            offsetY,
            heightMapSettings.noiseOctaves,
            heightMapSettings.persistence,
            heightMapSettings.lacunarity,
            heightMapSettings.noiseRedistributionFactor
        );

        float[,] falloffMap = Falloff.GenerateFalloffMap(mapWidth, mapWidth);

        bool useHydraulicErosion = GetComponent<HydraulicErosion>().useHydraulicErosion;
        if (useHydraulicErosion && heightMapSettings.mapDepth > 0) {
            HydraulicErosion hydraulicErosion = GetComponent<HydraulicErosion>();
            noiseMap = hydraulicErosion.ErodeTerrain(noiseMap, seed);
        }

        float[,] heightMap = new float[mapWidth, mapWidth];

        // Determine map depth
        for (int z = 0; z < mapWidth; z++) {
            for (int x = 0; x < mapWidth; x++) {
                if (heightMapSettings.useFalloff && heightMapSettings.mapDepth > 0) {
                    noiseMap[x, z] = Mathf.Clamp01(noiseMap[x, z] - falloffMap[x, z]);
                }

                if (heightMapSettings.mapDepth == 0) {
                    heightMap[x, z] = 1f;
                }
                else {
                    heightMap[x, z] = 2 * (noiseMap[x, z] * heightMapSettings.mapDepth);
                }
            }
        }

        return heightMap;
    }
}
