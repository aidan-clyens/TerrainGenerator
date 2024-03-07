using UnityEngine;

public class ProceduralObjectGeneratorBase : MonoBehaviour {
    protected System.Random rng;

    protected float CalculateMapArea(float[,] heightMap, float waterLevel) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        int areaAboveWater = 0;
        for (int z = 0; z < height; z++) {
            for (int x = 0; x < width; x++) {
                if (heightMap[z, x] > waterLevel) {
                    areaAboveWater++;
                }
            }
        }

        return areaAboveWater;
    }

    protected int CalculateMapArea(int[,] heightMap, int waterLevel) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        int areaAboveWater = 0;
        for (int z = 0; z < height; z++) {
            for (int x = 0; x < width; x++) {
                if (heightMap[z, x] > waterLevel) {
                    areaAboveWater++;
                }
            }
        }

        return areaAboveWater;
    }
}