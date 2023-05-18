using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static float[,] GenerateNoiseMap(NoiseType noiseType, int width, int height, float scale, int offsetX, int offsetY, int octaves, float persistence, float lacunarity, float noiseRedistributionFactor) {
        float[,] noiseMap = new float[width, height];

        float maxPossibleHeight = 0f;
        float amplitude = 1;
        float frequency = 1;

        // Find max possible height to normalize heightmap globally
        for (int i = 0; i < octaves; i++) {
            maxPossibleHeight += amplitude;
            amplitude *= persistence;
        }

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float noiseHeight = 0;
                amplitude = 1;
                frequency = 1;

                for (int i = 0; i < octaves; i++) {
                    float sampleX = (float)(x + offsetX) / (float)width * scale * frequency;
                    float sampleY = (float)(y + offsetY) / (float)height * scale * frequency;

                    float noiseValue;
                    if (noiseType == NoiseType.Perlin) {
                        noiseValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    }
                    else if (noiseType == NoiseType.Simplex) {
                        noiseValue = simplexNoiseGenerator.noise(sampleX, sampleY, 0) * 2;
                    }
                    else {
                        noiseValue = 0f;
                    }

                    noiseHeight += noiseValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                noiseMap[x, y] = noiseHeight;
            }   
        }

        // Normalize map and apply noise redistribution
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                noiseMap[x, y] = Mathf.InverseLerp(-maxPossibleHeight, maxPossibleHeight, noiseMap[x, y]);
                noiseMap[x, y] = Mathf.Pow(noiseMap[x, y], noiseRedistributionFactor);
            }
        }

        return noiseMap;
    }
}
