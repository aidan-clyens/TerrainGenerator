using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise {
    public static float[,] GeneratePerlinNoiseMap(int width, int height, float scale, int offsetX, int offsetY, int octaves, float persistence, float lacunarity, float noiseRedistributionFactor) {
        float[,] noiseMap = new float[width, height];

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) {
                    float sampleX = (float)x / (float)width * scale * frequency + offsetX;
                    float sampleY = (float)y / (float)height * scale * frequency + offsetY;

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
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                noiseMap[x, y] = Mathf.Pow(noiseMap[x, y], noiseRedistributionFactor);
            }
        }

        return noiseMap;
    }
}
