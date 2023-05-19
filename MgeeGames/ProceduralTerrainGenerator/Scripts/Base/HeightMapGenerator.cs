using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;

[System.Serializable]
public class HeightMapSettings {
    public int mapDepth;
    public NoiseSettings noiseSettings;

    public HeightMapSettings() {
        noiseSettings = new NoiseSettings();
    }

    public void Randomize() {
        noiseSettings.Randomize();
        mapDepth = UnityEngine.Random.Range(30, 100);
    }
}

public class HeightMapGenerator : MonoBehaviour {
    [HideInInspector]
    public float averageMapDepth;
    [HideInInspector]
    public bool normalize = false;
    [HideInInspector]
    public List<HeightMapSettings> heightMapSettingsList;

    HydraulicErosion hydraulicErosion;

    const int maxWidth = 256;

    void Start() {
        heightMapSettingsList = new List<HeightMapSettings>();
        heightMapSettingsList.Add(new HeightMapSettings());
    }

    void OnValidate() {
        if (hydraulicErosion == null) {
            hydraulicErosion = GetComponent<HydraulicErosion>();
        }
    }

    public void RequestHeightMapData(int seed, int mapWidth, Vector2 position, Action<Vector2, float[,]> callback) {
        ThreadStart threadStart = delegate {
            HeightMapDataThread(seed, mapWidth, position, callback);
        };

        new Thread(threadStart).Start();
    }

    private void HeightMapDataThread(int seed, int mapWidth, Vector2 position, Action<Vector2, float[,]> callback) {
        int mapOffsetX = (int)(position.x * (mapWidth - 1)) + seed;
        int mapOffsetY = (int)(position.y * (mapWidth - 1)) + seed;

        float[,] heightMap = CreateHeightMap(seed, mapWidth, mapOffsetX, mapOffsetY);

        callback(position, heightMap);
    }

    private float[,] CreateHeightMap(int seed, int mapWidth, int offsetX, int offsetY) {
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

        if (normalize) {
            heightMap = NormalizeHeightMap(heightMap);
        }

        return heightMap;
    }

    private float[,] CreateHeightMapLayer(HeightMapSettings settings, int seed, int mapWidth, int offsetX, int offsetY) {
        float widthFactor = (float)mapWidth / (float)maxWidth;
        float[,] noiseMap = Noise.GenerateNoiseMap(
            settings.noiseSettings,
            mapWidth,
            mapWidth,
            offsetX,
            offsetY
        );

        float[,] falloffMap = null;
        if (settings.noiseSettings.useFalloff) {
            falloffMap = Falloff.GenerateFalloffMap(mapWidth, mapWidth);
        }

        if (hydraulicErosion) {
            bool useHydraulicErosion = hydraulicErosion.settings.useHydraulicErosion;
            if (useHydraulicErosion && settings.mapDepth > 0) {
                noiseMap = hydraulicErosion.ErodeTerrain(noiseMap, seed);
            }
        }

        float[,] heightMap = new float[mapWidth, mapWidth];

        // Determine map depth
        for (int z = 0; z < mapWidth; z++) {
            for (int x = 0; x < mapWidth; x++) {
                if (settings.noiseSettings.useFalloff && settings.mapDepth > 0) {
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

    private float[,] NormalizeHeightMap(float[,] heightMap) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        float minDepth = float.MaxValue;
        float maxDepth = float.MinValue;

        // Find max and min values
        for (int z = 0; z < height; z++) {
            for (int x = 0; x < width; x++) {
                if (heightMap[x, z] < minDepth) {
                    minDepth = heightMap[x, z];
                }
                
                if (heightMap[x, z] > maxDepth) {
                    maxDepth = heightMap[x, z];
                }
            }
        }

        // Normalize each value
        for (int z = 0; z < height; z++) {
            for (int x = 0; x < width; x++) {
                heightMap[x, z] = (heightMap[x, z] - minDepth) / (maxDepth - minDepth);
            }
        }

        return heightMap;
    }
}


public class HeightMapThreadInfo {
    public Vector2 position;
    public float[,] heightMap;

    public HeightMapThreadInfo(Vector2 position, float[,] heightMap) {
        this.position = position;
        this.heightMap = heightMap;
    }
}
