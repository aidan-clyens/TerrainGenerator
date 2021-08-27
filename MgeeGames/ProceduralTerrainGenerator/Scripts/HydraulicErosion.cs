using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HydraulicErosionSettings {
    public bool useHydraulicErosion;

    public int iterations = 50000;
    [Range (0, 1)]
    public float inertia = 0.1f;
    public float gravity = 4f;
    public float minSlope = 0.01f;
    public float capacityFactor = 8f;
    [Range(0, 1)]
    public float depositionFactor = 0.1f;
    [Range(0, 1)]
    public float erosionFactor = 0.1f;
    [Range(0, 1)]
    public float evaporationFactor = 0.05f;
    public float erosionRadius = 5f;
    public float depositionRadius = 5f;
    public int dropletLifetime = 30;
}

public class HydraulicErosion : MonoBehaviour {
    [HideInInspector]
    public HydraulicErosionSettings settings;

    ErosionInfo erosionInfo;
    ErosionBrush erosionBrush;
    DepositionBrush depositionBrush;

    System.Random rng;

    public float[,] ErodeTerrain(float[,] heightMap, int seed) {
        rng = new System.Random(seed);

        erosionInfo = InitializeErosionInfo();

        erosionBrush = InitializeErosionBrush(heightMap);
        depositionBrush = InitializeDepositionBrush(heightMap);

        for (int i = 0; i < settings.iterations; i++) {
            Droplet droplet = new Droplet(heightMap, erosionInfo, erosionBrush, depositionBrush);       
            droplet.Update();
        }

        return heightMap;
    }

    ErosionBrush InitializeErosionBrush(float[,] heightMap) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        ErosionBrush erosionBrush = new ErosionBrush(width, height);

        Vector2 position = new Vector2();
        Vector2 v = new Vector2();

        float[] xOffsets = new float[(int)settings.erosionRadius * (int)settings.erosionRadius * 4];
        float[] yOffsets = new float[(int)settings.erosionRadius * (int)settings.erosionRadius * 4];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (x < 0 || x >= width || y < 0 || y >= height) continue;

                int index = 0;

                position.x = x;
                position.y = y;

                float weightSum = 0f;
                int numVertices = 0;
                for (int i = -(int)settings.erosionRadius; i <= settings.erosionRadius; i++) {
                    int xCoord = x + i;
                    for (int j = -(int)settings.erosionRadius; j <= settings.erosionRadius; j++) {
                        int yCoord = y + j;

                        v.x = xCoord;
                        v.y = yCoord;

                        if (xCoord < 0 || xCoord >= width || yCoord < 0 || yCoord >= height) continue;
                        
                        if ((v - position).magnitude <= settings.erosionRadius) {
                            weightSum += Mathf.Max(0f, settings.erosionRadius - (v - position).magnitude);
                            numVertices++;

                            xOffsets[index] = i;
                            yOffsets[index] = j;

                            index++;
                        }
                    }
                }

                erosionBrush.erosionBrushWeights[y * width + x] = new float[numVertices];
                erosionBrush.erosionBrushVertices[y * width + x] = new float[numVertices];

                for (int n = 0; n < numVertices; n++) {
                    v.x = x + xOffsets[n];
                    v.y = y + yOffsets[n];
                
                    float weight = Mathf.Max(0, settings.erosionRadius - (v - position).magnitude) / weightSum;
                    erosionBrush.erosionBrushVertices[y * width + x][n] = v.y * width + v.x;
                    erosionBrush.erosionBrushWeights[y * width + x][n] = weight;
                }
            }
        }

        return erosionBrush;
    }

    DepositionBrush InitializeDepositionBrush(float[,] heightMap) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        DepositionBrush depositionBrush = new DepositionBrush(width, height);

        Vector2 position = new Vector2();
        Vector2 v = new Vector2();

        float[] xOffsets = new float[(int)settings.depositionRadius * (int)settings.depositionRadius * 4];
        float[] yOffsets = new float[(int)settings.depositionRadius * (int)settings.depositionRadius * 4];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (x < 0 || x >= width || y < 0 || y >= height) continue;

                int index = 0;

                position.x = x;
                position.y = y;

                float weightSum = 0f;
                int numVertices = 0;
                for (int i = -(int)settings.depositionRadius; i <= settings.depositionRadius; i++) {
                    int xCoord = x + i;
                    for (int j = -(int)settings.depositionRadius; j <= settings.depositionRadius; j++) {
                        int yCoord = y + j;

                        v.x = xCoord;
                        v.y = yCoord;

                        if (xCoord < 0 || xCoord >= width || yCoord < 0 || yCoord >= height) continue;
                        
                        if ((v - position).magnitude <= settings.depositionRadius) {
                            weightSum += Mathf.Max(0f, settings.depositionRadius - (v - position).magnitude);
                            numVertices++;

                            xOffsets[index] = i;
                            yOffsets[index] = j;

                            index++;
                        }
                    }
                }

                depositionBrush.depositionBrushWeights[y * width + x] = new float[numVertices];
                depositionBrush.depositionBrushVertices[y * width + x] = new float[numVertices];

                for (int n = 0; n < numVertices; n++) {
                    v.x = x + xOffsets[n];
                    v.y = y + yOffsets[n];
                
                    float weight = Mathf.Max(0, settings.depositionRadius - (v - position).magnitude) / weightSum;
                    depositionBrush.depositionBrushVertices[y * width + x][n] = v.y * width + v.x;
                    depositionBrush.depositionBrushWeights[y * width + x][n] = weight;
                }
            }
        }

        return depositionBrush;
    }

    ErosionInfo InitializeErosionInfo() {
        ErosionInfo erosionInfo;

        erosionInfo.inertia = settings.inertia;
        erosionInfo.gravity = settings.gravity;
        erosionInfo.minSlope = settings.minSlope;
        erosionInfo.capacityFactor = settings.capacityFactor;
        erosionInfo.depositionFactor = settings.depositionFactor;
        erosionInfo.erosionFactor = settings.erosionFactor;
        erosionInfo.evaporationFactor = settings.evaporationFactor;
        erosionInfo.erosionRadius = settings.erosionRadius;
        erosionInfo.dropletLifetime = settings.dropletLifetime;
        erosionInfo.rng = rng;

        return erosionInfo;
    }

    class Droplet {
        Vector2 position;
        Vector2 direction;
        float height;
        float speed = 1f;
        float water = 1f;
        float sediment = 0f;


        float[,] heightMap;
        int mapWidth;
        int mapHeight;

        ErosionInfo erosionInfo;
        ErosionBrush erosionBrush;
        DepositionBrush depositionBrush;

        public Droplet(float[,] map, ErosionInfo info, ErosionBrush eBrush, DepositionBrush dBrush) {
            heightMap = map;
            erosionInfo = info;
            erosionBrush = eBrush;
            depositionBrush = dBrush;

            mapWidth = heightMap.GetLength(0);
            mapHeight = heightMap.GetLength(1);

            position = new Vector2(
                erosionInfo.rng.Next(0, mapWidth - 1),
                erosionInfo.rng.Next(0, mapHeight - 1)
            );

            direction = new Vector2(0f, 0f).normalized;
        }

        public void Update() {
            for (int i = 0; i < erosionInfo.dropletLifetime; i++) {
                // Update direction and get current height
                direction = GetNewDirection();
                height = GetNewHeight();

                position += direction;

                // Check whether droplet has stopped moving or flowed over the edge of the map
                if ((direction.x == 0 && direction.y == 0) || position.x < 0 || position.x >= mapWidth - 1 || position.y < 0 || position.y >= mapHeight - 1) {
                    break;
                }

                // Calculate new height using bilinear interpolation
                float heightDiff = GetNewHeight() - height;

                // Update sediment carrying capacity
                float sedimentCapacity = Mathf.Max(-heightDiff, erosionInfo.minSlope) * speed * water * erosionInfo.capacityFactor;

                // If height diff > 0, the new position is uphill
                // If travelling uphill, or carrying more sediment than capacity, deposit sediment
                if (sediment > sedimentCapacity || heightDiff > 0) {
                    float sedimentDeposited = (sediment - sedimentCapacity) * erosionInfo.depositionFactor;
                    sediment -= sedimentDeposited;

                    // Deposit sediment to surrounding vertices
                    DepositSediment(sedimentDeposited);
                }
                // If height diff < 0, the new position is downhill
                else {
                    // Erode a fraction of the sediment carrying capacity
                    float sedimentEroded = Mathf.Min((sedimentCapacity - sediment) * erosionInfo.erosionFactor, -heightDiff);
                    sediment += sedimentEroded;

                    // Erode sediment from surrounding vertices
                    ErodeSediment(sedimentEroded);
                }

                // Update speed
                speed = Mathf.Max(Mathf.Sqrt(speed * speed + heightDiff * erosionInfo.gravity), 0f);

                // Evaporate water
                water = water * (1 - erosionInfo.evaporationFactor);
            }
        }

        void DepositSediment(float sedimentDeposited) {
            int coordX = (int)position.x;
            int coordY = (int)position.y;

            int brushIndex = coordY * mapWidth + coordX;

            for (int i = 0; i < depositionBrush.depositionBrushVertices[brushIndex].Length; i++) {
                int nodeIndex = (int)depositionBrush.depositionBrushVertices[brushIndex][i];
                int x = nodeIndex % mapWidth;
                int y = nodeIndex / mapWidth;

                float weight = depositionBrush.depositionBrushWeights[brushIndex][i];

                heightMap[x, y] += sedimentDeposited * weight;
            }
        }

        void ErodeSediment(float sedimentEroded) {
            int coordX = (int)position.x;
            int coordY = (int)position.y;

            int brushIndex = coordY * mapWidth + coordX;

            for (int i = 0; i < erosionBrush.erosionBrushVertices[brushIndex].Length; i++) {
                int nodeIndex = (int)erosionBrush.erosionBrushVertices[brushIndex][i];
                int x = nodeIndex % mapWidth;
                int y = nodeIndex / mapWidth;

                float weight = erosionBrush.erosionBrushWeights[brushIndex][i];

                heightMap[x, y] -= sedimentEroded * weight;
            }
        }

        float GetNewHeight() {
            int coordX = (int)position.x;
            int coordY = (int)position.y;

            float offsetX = position.x - coordX;
            float offsetY = position.y - coordY;

            float heightNW = heightMap[coordX, coordY];
            float heightNE = heightMap[coordX + 1, coordY];
            float heightSW = heightMap[coordX, coordY + 1];
            float heightSE = heightMap[coordX + 1, coordY + 1];

            return heightNW * (1 - offsetX) * (1 - offsetY) + heightNE * offsetX * (1 - offsetY) + heightSW * (1 - offsetX) * offsetY + heightSE * offsetX * offsetY;
        }

        Vector2 GetNewDirection() {
            Vector2 gradient = GetGradient();

            return (direction * erosionInfo.inertia - gradient * (1 - erosionInfo.inertia)).normalized;
        }

        Vector2 GetGradient() {
            int coordX = (int)position.x;
            int coordY = (int)position.y;

            float offsetX = position.x - coordX;
            float offsetY = position.y - coordY;
        
            return new Vector2(
                (heightMap[coordX + 1, coordY] - heightMap[coordX, coordY]) * (1 - offsetX) - (heightMap[coordX + 1, coordY + 1] - heightMap[coordX, coordY + 1]) * offsetX,
                (heightMap[coordX, coordY + 1] - heightMap[coordX, coordY]) * (1 - offsetY) - (heightMap[coordX + 1, coordY + 1] - heightMap[coordX + 1, coordY]) * offsetY
            );
        }
    }

    
    class ErosionBrush {
        public float[][] erosionBrushWeights;
        public float[][] erosionBrushVertices;

        public ErosionBrush(int width, int height) {
            erosionBrushWeights = new float[width * height][];
            erosionBrushVertices = new float[width * height][];
        }
    }

    class DepositionBrush {
        public float[][] depositionBrushWeights;
        public float[][] depositionBrushVertices;

        public DepositionBrush(int width, int height) {
            depositionBrushWeights = new float[width * height][];
            depositionBrushVertices = new float[width * height][];
        }
    }

    struct ErosionInfo {
        public float inertia;
        public float gravity;
        public float minSlope;
        public float capacityFactor;
        public float depositionFactor;
        public float erosionFactor;
        public float evaporationFactor;
        public float erosionRadius;
        public int dropletLifetime;

        public System.Random rng;
    }
}
