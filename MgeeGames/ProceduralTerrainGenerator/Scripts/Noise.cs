using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise {
    public static float[,] GeneratePerlinNoiseMap(int width, int height, float scale, int offsetX, int offsetY, int octaves, float persistence, float lacunarity, float noiseRedistributionFactor, bool normalizeLocal) {
        float[,] noiseMap = new float[width, height];

        // Use local min and max to normalize locally, does not work well with other chunks
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

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

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }   
        }

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (normalizeLocal) {
                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                }
                else {
                    noiseMap[x, y] = Mathf.InverseLerp(-maxPossibleHeight, maxPossibleHeight, noiseMap[x, y]);
                }

                noiseMap[x, y] = Mathf.Pow(noiseMap[x, y], noiseRedistributionFactor);
            }
        }

        return noiseMap;
    }
}
