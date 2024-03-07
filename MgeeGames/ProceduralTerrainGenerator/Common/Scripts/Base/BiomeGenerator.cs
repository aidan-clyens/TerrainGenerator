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

[Serializable]
public struct BiomeList {
    public List<Biome> biomes;
}

public class BiomeGenerator : MonoBehaviour {
    [HideInInspector]
    public BiomeList biomes;

    public const int MAX_TEMPERATURE = 30;
    public const int MIN_TEMPERATURE = -10;
    public const int MIN_MOISTURE = 0;
    public const int MAX_MOISTURE = 100;

    private float[,] temperatureMap;
    private float[,] moistureMap;

    private Queue<BiomeThreadInfo> biomeDataThreadInfoQueue = new Queue<BiomeThreadInfo>();

    private void Start() {

    }

    public void RequestBiomeData(NoiseSettings temperatureNoiseSettings, NoiseSettings moistureNoiseSettings, int mapWidth, Action<float[,], float[,]> callback) {
        temperatureMap = new float[mapWidth, mapWidth];
        moistureMap = new float[mapWidth, mapWidth];

        ThreadStart threadStart = delegate {
            BiomeDataThread(temperatureNoiseSettings, moistureNoiseSettings, mapWidth, callback);
        };

        new Thread(threadStart).Start();
    }

    public string GetBiomeType(Vector2 position) {
        float temperature = GetTemperature(position);
        float moisture = GetMoisture(position);
        float latitude = position.y;

        foreach (Biome biome in biomes.biomes) {
            bool ret = true;

            if (!(temperature >= biome.minTemperature && temperature <= biome.maxTemperature)) {
                ret = false;
            }

            if (!(moisture >= biome.minMoisture && moisture <= biome.maxMoisture)) {
                ret = false;
            }

            if (biome.inverse) {
                if (latitude >= biome.minLatitude && latitude <= biome.maxLatitude) {
                    ret = false;
                }
            }
            else {
                if (!(latitude >= biome.minLatitude && latitude <= biome.maxLatitude)) {
                    ret = false;
                }
            }

            if (ret)
                return biome.name;
        }

        return "";
    }

    public float GetTemperature(Vector2 position) {
        return temperatureMap[(int)position.x, (int)position.y];
    }

    public float GetMoisture(Vector2 position) {
        return moistureMap[(int)position.x, (int)position.y];
    }

    private void BiomeDataThread(NoiseSettings temperatureNoiseSettings, NoiseSettings moistureNoiseSettings, int mapWidth, Action<float[,], float[,]> callback) {
        temperatureMap = Noise.GenerateNoiseMap(temperatureNoiseSettings, mapWidth, mapWidth, 0, 0);
        temperatureMap = Noise.NormalizeMap(temperatureMap);
        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapWidth; y++) {
                temperatureMap[x, y] = temperatureMap[x, y] * Mathf.Abs(MAX_TEMPERATURE - MIN_TEMPERATURE) + MIN_TEMPERATURE;
            }
        }

        moistureMap = Noise.GenerateNoiseMap(moistureNoiseSettings, mapWidth, mapWidth, 0, 0);
        moistureMap = Noise.NormalizeMap(moistureMap);
        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapWidth; y++) {
                moistureMap[x, y] = moistureMap[x, y] * Mathf.Abs(MAX_MOISTURE - MIN_MOISTURE) + MIN_MOISTURE;
            }
        }

        callback(temperatureMap, moistureMap);
    }
}