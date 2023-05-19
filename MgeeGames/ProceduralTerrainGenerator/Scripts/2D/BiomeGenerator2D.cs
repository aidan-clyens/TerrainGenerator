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

public class BiomeGenerator2D : MonoBehaviour {
    public NoiseSettings noiseSettings;

    private Queue<BiomeThreadInfo> biomeDataThreadInfoQueue = new Queue<BiomeThreadInfo>();

    public void Start() {

    }

    public void RequestBiomeData(NoiseSettings noiseSettings, int mapWidth, Action<float[,], float[,]> callback) {
        ThreadStart threadStart = delegate {
            BiomeDataThread(noiseSettings, mapWidth, callback);
        };

        new Thread(threadStart).Start();
    }

    private void BiomeDataThread(NoiseSettings noiseSettings, int mapWidth, Action<float[,], float[,]> callback) {
        float[,] temperatureMap = Noise.GenerateNoiseMap(noiseSettings, mapWidth, mapWidth, 0, 0);
        float[,] moistureMap = Noise.GenerateNoiseMap(noiseSettings, mapWidth, mapWidth, 0, 0);

        callback(temperatureMap, moistureMap);
    }
}