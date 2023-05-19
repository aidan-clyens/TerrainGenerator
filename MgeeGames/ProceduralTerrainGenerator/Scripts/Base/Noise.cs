using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings {
    public NoiseType noiseType;
    public float scale;
    public int octaves;
    public float persistence;
    public float lacunarity;
    public float noiseRedistributionFactor;
    public bool useFalloff;

    public NoiseSettings() {
        noiseType = NoiseType.Perlin;
    }

    public void Randomize() {
        scale = UnityEngine.Random.Range(1f, 5f);
        octaves = UnityEngine.Random.Range(1, 6);
        persistence = UnityEngine.Random.Range(0.1f, 0.5f);
        lacunarity = UnityEngine.Random.Range(1f, 2f);
        noiseRedistributionFactor = UnityEngine.Random.Range(1f, 3f);
    }
}

public enum NoiseType {
    Perlin,
    Simplex
};

public class Noise {

    static SimplexNoiseGenerator simplexNoiseGenerator = new SimplexNoiseGenerator();

    public static void CreateNewSimplexNoiseGenerator(int seed) {
        int[] seedArray = new int[8];
        System.Random rand = new System.Random(seed);

        for (int i = 0; i < 8; i++) {
            seedArray[i] = rand.Next();
        }

        simplexNoiseGenerator = new SimplexNoiseGenerator(seedArray);
    }

    public static float[,] GenerateNoiseMap(NoiseSettings settings, int width, int height, int offsetX, int offsetY) {
        float[,] noiseMap = new float[width, height];

        float maxPossibleHeight = 0f;
        float amplitude = 1;
        float frequency = 1;

        // Find max possible height to normalize heightmap globally
        for (int i = 0; i < settings.octaves; i++) {
            maxPossibleHeight += amplitude;
            amplitude *= settings.persistence;
        }

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float noiseHeight = 0;
                amplitude = 1;
                frequency = 1;

                for (int i = 0; i < settings.octaves; i++) {
                    float sampleX = (float)(x + offsetX) / (float)width * settings.scale * frequency;
                    float sampleY = (float)(y + offsetY) / (float)height * settings.scale * frequency;

                    float noiseValue;
                    if (settings.noiseType == NoiseType.Perlin) {
                        noiseValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    }
                    else if (settings.noiseType == NoiseType.Simplex) {
                        noiseValue = simplexNoiseGenerator.noise(sampleX, sampleY, 0) * 2;
                    }
                    else {
                        noiseValue = 0f;
                    }

                    noiseHeight += noiseValue * amplitude;

                    amplitude *= settings.persistence;
                    frequency *= settings.lacunarity;
                }

                noiseMap[x, y] = noiseHeight;
            }   
        }

        // Normalize map and apply noise redistribution
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                noiseMap[x, y] = Mathf.InverseLerp(-maxPossibleHeight, maxPossibleHeight, noiseMap[x, y]);
                noiseMap[x, y] = Mathf.Pow(noiseMap[x, y], settings.noiseRedistributionFactor);
            }
        }

        return noiseMap;
    }
}
