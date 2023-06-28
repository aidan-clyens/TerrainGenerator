using UnityEngine;

[System.Serializable]
public class NoiseSettings {
    public NoiseType noiseType;
    [MyBox.ConditionalField(nameof(noiseType), false, NoiseType.Voronoi)]
    public int numberOfPoints;
    [MyBox.ConditionalField(nameof(noiseType), true, NoiseType.Voronoi)]
    public float scale;
    [MyBox.ConditionalField(nameof(noiseType), true, NoiseType.Voronoi)]
    public int octaves;
    [MyBox.ConditionalField(nameof(noiseType), true, NoiseType.Voronoi)]
    public float persistence;
    [MyBox.ConditionalField(nameof(noiseType), true, NoiseType.Voronoi)]
    public float lacunarity;
    [MyBox.ConditionalField(nameof(noiseType), true, NoiseType.Voronoi)]
    public float noiseRedistributionFactor;
    [MyBox.ConditionalField(nameof(noiseType), true, NoiseType.Voronoi)]
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
    Simplex,
    Voronoi
};

public class Noise {

    static SimplexNoiseGenerator simplexNoiseGenerator = new SimplexNoiseGenerator();
    static System.Random rng;

    public static void CreateNewSimplexNoiseGenerator(int seed) {
        int[] seedArray = new int[8];

        if (rng == null)
            rng = new System.Random(seed);

        for (int i = 0; i < 8; i++) {
            seedArray[i] = rng.Next();
        }

        simplexNoiseGenerator = new SimplexNoiseGenerator(seedArray);
    }

    public static void CreateNewNoiseGenerator(int seed) {
        rng = new System.Random(seed);
    }

    public static float[,] GenerateNoiseMap(NoiseSettings settings, int width, int height, int offsetX, int offsetY) {
        switch (settings.noiseType) {
            case NoiseType.Perlin:
            case NoiseType.Simplex:
                return GeneratePerlinNoiseMap(settings, width, height, offsetX, offsetY);
            case NoiseType.Voronoi:
                return GenerateVoronoiNoiseMap(width, height, settings.numberOfPoints);
            default:
                return GeneratePerlinNoiseMap(settings, width, height, offsetX, offsetY);
        }
    }

    public static float[,] NormalizeMap(float[,] noiseMap) {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        float minDepth = float.MaxValue;
        float maxDepth = float.MinValue;

        // Find max and min values
        for (int z = 0; z < height; z++) {
            for (int x = 0; x < width; x++) {
                if (noiseMap[x, z] < minDepth) {
                    minDepth = noiseMap[x, z];
                }
                
                if (noiseMap[x, z] > maxDepth) {
                    maxDepth = noiseMap[x, z];
                }
            }
        }

        // Normalize each value
        for (int z = 0; z < height; z++) {
            for (int x = 0; x < width; x++) {
                noiseMap[x, z] = (noiseMap[x, z] - minDepth) / (maxDepth - minDepth);
            }
        }

        return noiseMap;
    }

    private static float[,] GeneratePerlinNoiseMap(NoiseSettings settings, int width, int height, int offsetX, int offsetY) {
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

        return Noise.NormalizeMap(noiseMap);
    }

    private static float[,] GenerateVoronoiNoiseMap(int width, int height, int numberOfPoints) {
        float[,] noiseMap = new float[width, height];
        Vector2Int[] points = new Vector2Int[numberOfPoints];

        if (rng == null)
            return noiseMap;

        for (int i = 0; i < numberOfPoints; i++) {
            int x = rng.Next(0, width - 1);
            int y = rng.Next(0, height - 1);

            points[i] = new Vector2Int(x, y);
        }

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                noiseMap[x, y] = Noise.FindClosestPoint(points, new Vector2Int(x, y));
            }
        }

        return Noise.NormalizeMap(noiseMap);
    }

    private static int FindClosestPoint(Vector2Int[] points, Vector2Int point) {
        float minDistance = float.MaxValue;
        int closestPoint = 0;

        for (int i = 0; i < points.Length; i++) {
            float distance = Vector2Int.Distance(point, points[i]);
            if (distance < minDistance) {
                minDistance = distance;
                closestPoint = i;
            }
        }

        return closestPoint;
    }
}
