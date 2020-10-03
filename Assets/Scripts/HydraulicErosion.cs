using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HydraulicErosion {
    public static float[,] ErodeTerrain(float[,] heightMap, int lifetime) {
        for (int i = 0; i < 10000; i++) {
            Droplet droplet = new Droplet(heightMap, lifetime);       
            droplet.Update();
        }

        return heightMap;
    }

    public class Droplet {
        public Vector2 position;
        public Vector2 direction;
        public float height;
        public float speed = 1f;
        public float water = 1f;
        public float sediment = 0f;

        // Parameters
        public float inertia = 0.1f;
        public float gravity = 4f;
        public float minSlope = 0.01f;
        public float capacityFactor = 8f;
        public float depositionFactor = 0.1f;
        public float erosionFactor = 0.1f;
        public float evaporationFactor = 0.05f;

        float[,] heightMap;
        int mapWidth;
        int mapHeight;
        int lifetime;

        public Droplet(float[,] map, int lt) {
            heightMap = map;
            lifetime = lt;

            mapWidth = heightMap.GetLength(0);
            mapHeight = heightMap.GetLength(1);

            position = new Vector2(
                Random.Range(0, mapWidth - 1),
                Random.Range(0, mapHeight - 1)
            );

            direction = new Vector2(0f, 0f).normalized;
        }

        public void Update() {
            for (int i = 0; i < lifetime; i++) {
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
                float sedimentCapacity = Mathf.Max(-heightDiff, minSlope) * speed * water * capacityFactor;

                // If height diff > 0, the new position is uphill
                // If travelling uphill, or carrying more sediment than capacity, deposit sediment
                if (sediment > sedimentCapacity || heightDiff > 0) {
                    float sedimentDeposited = (sediment - sedimentCapacity) * depositionFactor;
                    sediment -= sedimentDeposited;

                    // Deposit sediment to surrounding vertices
                    DepositSediment(sedimentDeposited);
                }
                // If height diff < 0, the new position is downhill
                else {
                    // Erode a fraction of the sediment carrying capacity
                    float sedimentEroded = Mathf.Min((sedimentCapacity - sediment) * erosionFactor, -heightDiff);
                    sediment += sedimentEroded;

                    // Erode sediment from surrounding vertices
                    ErodeSediment(sedimentEroded);
                }

                // Update speed
                speed = Mathf.Max(Mathf.Sqrt(speed * speed + heightDiff * gravity), 0f);

                // Evaporate water
                water = water * (1 - evaporationFactor);
            }
        }

        void DepositSediment(float sedimentDeposited) {
            int coordX = (int)position.x;
            int coordY = (int)position.y;

            float offsetX = position.x - coordX;
            float offsetY = position.y - coordY;

            heightMap[coordX, coordY] += sedimentDeposited * (1 - offsetX) * (1 - offsetY);
            heightMap[coordX + 1, coordY] += sedimentDeposited * offsetX * (1 - offsetY);
            heightMap[coordX, coordY + 1] += sedimentDeposited * (1 - offsetX) * offsetY;
            heightMap[coordX + 1, coordY + 1] += sedimentDeposited * offsetX * offsetY;
        }

        void ErodeSediment(float sedimentEroded) {
            int coordX = (int)position.x;
            int coordY = (int)position.y;

            float offsetX = position.x - coordX;
            float offsetY = position.y - coordY;

            heightMap[coordX, coordY] -= sedimentEroded * (1 - offsetX) * (1 - offsetY);
            heightMap[coordX + 1, coordY] -= sedimentEroded * offsetX * (1 - offsetY);
            heightMap[coordX, coordY + 1] -= sedimentEroded * (1 - offsetX) * offsetY;
            heightMap[coordX + 1, coordY + 1] -= sedimentEroded * offsetX * offsetY;
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

            return (direction * inertia - gradient * (1 - inertia)).normalized;
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
}
