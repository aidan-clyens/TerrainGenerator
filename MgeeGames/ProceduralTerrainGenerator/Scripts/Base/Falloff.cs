using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falloff {

    public static float[,] GenerateFalloffMap(int width, int height) {
        float[,] falloffMap = new float[width, height];

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                float x = i / (float)width * 2 - 1;
                float y = j / (float)height * 2 - 1;
            
                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                falloffMap[i, j] = EvaluateFalloff(value);
            }
        }

        return falloffMap;
    }

    static float EvaluateFalloff(float value) {
        float a = 3f;
        float b = 2.2f;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b*value, a));
    }
}
