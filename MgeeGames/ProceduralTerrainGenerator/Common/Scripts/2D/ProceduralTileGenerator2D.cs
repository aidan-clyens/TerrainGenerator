using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class ProceduralTileData : ProceduralObjectDataBase {
    public Tile tile;
    public int minLayer;
    public int maxLayer;
    [Range(0.0f, 1.0f)]
    public float threshold;
    public bool collide;
}

public class ProceduralTileGenerator2D : ProceduralObjectGeneratorBase {
    [HideInInspector]
    public List<ProceduralTileData> tileData;
    [HideInInspector]
    public NoiseSettings noiseSettings;

    public void Generate(Tilemap tilemap, Tilemap collisionTilemap, int[,] heightMap, int seed, int waterLevel = -1) {
        int tilemapWidth = heightMap.GetLength(0);
        
        rng = new System.Random(seed);
        float[,] noiseMap = GenerateNoiseMap(tilemapWidth);

        if (waterLevel == -1)
            waterLevel = int.MinValue;

        int areaAboveWater = CalculateMapArea(heightMap, waterLevel);

        foreach (ProceduralTileData data in tileData) {
            int numTiles = (int)(areaAboveWater * data.density);

            for (int i = 0; i < numTiles; i++) {
                int x = rng.Next(0, tilemapWidth - 1);
                int y = rng.Next(0, tilemapWidth - 1);

                float value = noiseMap[x, y];

                if (value > data.threshold) {
                    // Only draw tile if height is in range
                    int height = heightMap[x, y];
                    if (height > waterLevel) {
                        if ((height >= data.minLayer) && (height < data.maxLayer)) {
                            if (data.collide) {
                                // Collision object layer
                                collisionTilemap.SetTile(new Vector3Int(x, y, 0), data.tile);
                                // If there is a tile on the non-collision object layer, overrite that tile
                                if (tilemap.HasTile(new Vector3Int(x, y, 0))) {
                                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                                }
                            }
                            else {
                                // Non-collision object layer
                                tilemap.SetTile(new Vector3Int(x, y, 0), data.tile);
                            }
                        }
                    }
                }
            }
        }
    }

    public void Randomize() {
        noiseSettings.Randomize();
    }

    private float[,] GenerateNoiseMap(int tilemapWidth) {
        return Noise.GenerateNoiseMap(
            noiseSettings,
            tilemapWidth,
            tilemapWidth,
            0,
            0
        );
    }
}
