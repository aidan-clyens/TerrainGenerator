using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct ProceduralTileData {
    public Tile tile;
    public float density;
    public int minLayer;
    public int maxLayer;
    [Range(0.0f, 1.0f)]
    public float threshold;
    public bool collide;
}

public class ProceduralTileGenerator2D : MonoBehaviour {
    [HideInInspector]
    public List<ProceduralTileData> tileData;
    [HideInInspector]
    public NoiseSettings noiseSettings;

    System.Random rng;

    public void Generate(Tilemap tilemap, Tilemap collisionTilemap, int[,] heightMap, int seed) {
        int tilemapWidth = heightMap.GetLength(0);
        
        rng = new System.Random(seed);
        float[,] noiseMap = GeneratePerlinNoiseMap(tilemapWidth);

        foreach (ProceduralTileData data in tileData) {
            int numTiles = (int)Mathf.Round(data.density);

            for (int i = 0; i < numTiles; i++) {
                int x = rng.Next(0, tilemapWidth - 1);
                int y = rng.Next(0, tilemapWidth - 1);

                float value = noiseMap[x, y];

                if (value > data.threshold) {
                    // Only draw tile if height is in range
                    int height = heightMap[x, y];
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

    public void Randomize() {
        noiseSettings.Randomize();
    }

    private float[,] GeneratePerlinNoiseMap(int tilemapWidth) {
        return Noise.GenerateNoiseMap(
            noiseSettings,
            tilemapWidth,
            tilemapWidth,
            0,
            0
        );
    }
}
