using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeightMapSettings {
    public int mapDepth;
    public NoiseType noiseType;
    public float noiseScale;
    public int noiseOctaves;
    public float persistence;
    public float lacunarity;
    public float noiseRedistributionFactor;
    public bool useFalloff;

    public HeightMapSettings() {
        noiseType = NoiseType.Simplex;
    }

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
    public float averageMapDepth;
    public List<HeightMapSettings> heightMapSettingsList;

    const int maxWidth = 256;

    float biomeNoiseScale;
    float biomeDepth;

    void Start() {
        heightMapSettingsList = new List<HeightMapSettings>();
        heightMapSettingsList.Add(new HeightMapSettings());
    }

    public float[,] CreateHeightMap(int seed, int mapWidth, int offsetX, int offsetY) {
        Noise.CreateNewSimplexNoiseGenerator(seed);

        float[,] heightMap = new float[mapWidth, mapWidth];
        // Initialize height map values
        for (int z = 0; z < mapWidth; z++) {
            for (int x = 0; x < mapWidth; x++) {
                heightMap[x, z] = 0f;
            }
        }

        // Add each height map layer together
        float totalMapDepth = 0f;
        foreach (HeightMapSettings settings in heightMapSettingsList) {
            float[,] layer = CreateHeightMapLayer(settings, seed, mapWidth, offsetX, offsetY);
            totalMapDepth += settings.mapDepth;

            // Determine map depth
            for (int z = 0; z < mapWidth; z++) {
                for (int x = 0; x < mapWidth; x++) {
                    heightMap[x, z] += layer[x, z];
                }
            }
        }

        float actualAverageMapDepth = totalMapDepth / heightMapSettingsList.Count;
        float averageMapDepthDifference = Mathf.Abs(actualAverageMapDepth - averageMapDepth);

        // Adjust map depth to obtain targeted average map depth
        for (int z = 0; z < mapWidth; z++) {
            for (int x = 0; x < mapWidth; x++) {
                heightMap[x, z] -= averageMapDepthDifference;
            }
        }

        return heightMap;
    }

    public void Randomize() {
        heightMapSettingsList.Clear();

        int numLayers = Random.Range(1, 4);
        for (int i = 0; i < numLayers; i++) {
            HeightMapSettings settings = new HeightMapSettings();
            settings.Randomize();

            heightMapSettingsList.Add(settings);
        }
    }

    float[,] CreateHeightMapLayer(HeightMapSettings settings, int seed, int mapWidth, int offsetX, int offsetY) {
        float widthFactor = (float)mapWidth / (float)maxWidth;
        float[,] noiseMap = Noise.GenerateNoiseMap(
            settings.noiseType,
            mapWidth,
            mapWidth,
            settings.noiseScale * widthFactor,
            offsetX,
            offsetY,
            settings.noiseOctaves,
            settings.persistence,
            settings.lacunarity,
            settings.noiseRedistributionFactor
        );

        float[,] falloffMap = null;
        if (settings.useFalloff) {
            falloffMap = Falloff.GenerateFalloffMap(mapWidth, mapWidth);
        }

        bool useHydraulicErosion = GetComponent<HydraulicErosion>().useHydraulicErosion;
        if (useHydraulicErosion && settings.mapDepth > 0) {
            HydraulicErosion hydraulicErosion = GetComponent<HydraulicErosion>();
            noiseMap = hydraulicErosion.ErodeTerrain(noiseMap, seed);
        }

        float[,] heightMap = new float[mapWidth, mapWidth];

        // Determine map depth
        for (int z = 0; z < mapWidth; z++) {
            for (int x = 0; x < mapWidth; x++) {
                if (settings.useFalloff && settings.mapDepth > 0) {
                    noiseMap[x, z] = Mathf.Clamp01(noiseMap[x, z] - falloffMap[x, z]);
                }

                if (settings.mapDepth == 0) {
                    heightMap[x, z] = 1f;
                }
                else {
                    heightMap[x, z] = 2 * (noiseMap[x, z] * settings.mapDepth);
                }
            }
        }

        return heightMap;
    }
}
