using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;

public class BiomeThreadInfo {
    public float[,] temperatureMap;
    public float[,] moistureMap;

    public BiomeThreadInfo(float[,] temperatureMap, float[,] moistureMap) {
        this.temperatureMap = temperatureMap;
        this.moistureMap = moistureMap;
    }
}

public class BiomeGenerator : MonoBehaviour {
    [HideInInspector]
    public NoiseSettings noiseSettings;

    public const int MAX_TEMPERATURE = 30;
    public const int MIN_TEMPERATURE = -10;
    public const int MIN_MOISTURE = 0;
    public const int MAX_MOISTURE = 100;

    private Queue<BiomeThreadInfo> biomeDataThreadInfoQueue = new Queue<BiomeThreadInfo>();

    public void Start() {

    }

    public void RequestBiomeData(NoiseSettings temperatureNoiseSettings, NoiseSettings moistureNoiseSettings, int mapWidth, Action<float[,], float[,]> callback) {
        ThreadStart threadStart = delegate {
            BiomeDataThread(temperatureNoiseSettings, moistureNoiseSettings, mapWidth, callback);
        };

        new Thread(threadStart).Start();
    }

    private void BiomeDataThread(NoiseSettings temperatureNoiseSettings, NoiseSettings moistureNoiseSettings, int mapWidth, Action<float[,], float[,]> callback) {
        float[,] temperatureMap = Noise.GenerateNoiseMap(temperatureNoiseSettings, mapWidth, mapWidth, 0, 0);
        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapWidth; y++) {
                temperatureMap[x, y] = temperatureMap[x, y] * Mathf.Abs(MAX_TEMPERATURE - MIN_TEMPERATURE) + MIN_TEMPERATURE;
            }
        }

        float[,] moistureMap = Noise.GenerateNoiseMap(moistureNoiseSettings, mapWidth, mapWidth, 0, 0);
        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapWidth; y++) {
                moistureMap[x, y] = moistureMap[x, y] * Mathf.Abs(MAX_MOISTURE - MIN_MOISTURE) + MIN_MOISTURE;
            }
        }

        callback(temperatureMap, moistureMap);
    }
}