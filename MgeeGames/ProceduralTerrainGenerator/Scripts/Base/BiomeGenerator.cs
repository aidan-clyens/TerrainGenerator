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
        float[,] moistureMap = Noise.GenerateNoiseMap(moistureNoiseSettings, mapWidth, mapWidth, 0, 0);

        callback(temperatureMap, moistureMap);
    }
}