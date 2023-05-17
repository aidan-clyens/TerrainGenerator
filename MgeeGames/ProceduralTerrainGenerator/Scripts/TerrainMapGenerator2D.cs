using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct TileData {
    public Tile center;
    public Tile topLeft;
    public Tile topRight;
    public Tile bottomLeft;
    public Tile bottomRight;
}

public enum TileTypeEnum {
    TileCenter,
    TileTopLeft,
    TileTopRight,
    TileBottomLeft,
    TileBottomRight
}

public class TerrainMapGenerator2D : TerrainMapGeneratorBase {
    [Header("2D Settings")]
    [Space(10)]
    public int tilemapWidth;
    public Grid grid;
    public bool smoothEdges = false;
    public List<TileData> tiles;

    private List<GameObject> layers;

    public override void Update() {
        base.Update();
    }

    public override void OnValidate() {
        base.OnValidate();

        heightMapGenerator.normalize = true;
    }

    public override void Generate() {
        Clear();
        RequestTilemap();
    }

    public override void Clear() {
        foreach (GameObject layer in layers) {
            DestroyImmediate(layer, true);
        }

        layers.Clear();
    }

    public override void ProcessHeightMapData(HeightMapThreadInfo info) {
        GenerateTilemap(info.heightMap);
    }

    private void RequestTilemap() {
        if (grid != null)
            CreateLayers();

        heightMapGenerator.RequestHeightMapData(seed, tilemapWidth, Vector2.zero, OnHeightMapDataReceived);
    }

    private void CreateLayers() {
        for (int i = 0; i < tiles.Count; i++) {
            GameObject layer = new GameObject("Layer " + i);
            layer.AddComponent<Tilemap>();
            layer.AddComponent<TilemapRenderer>();

            layer.transform.parent = grid.transform;

            layers.Add(layer);
        }
    }

    private void GenerateTilemap(float[,] heightMap) {
        int mapWidth = heightMap.GetLength(0);
        int mapHeight = heightMap.GetLength(1);

        int[,] heightMapInt = new int[mapWidth, mapHeight];

        // Scale the normalized height map to an integer between 0 and the max height (determined by the number of tiles)
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                int height = (int)Mathf.Round(heightMap[x, y] * (tiles.Count - 1));
                heightMapInt[x, y] = height;
            }
        }

        // Determine the tile type given the height
        for (int y = 1; y < mapHeight - 1; y++) {
            for (int x = 1; x < mapWidth - 1; x++) {
                int height = heightMapInt[x, y];
                Tile tile;

                TileTypeEnum type;

                if (smoothEdges) {
                    type = GetTileType(heightMapInt, new Vector2Int(x, y));
                }
                else {
                    type = TileTypeEnum.TileCenter;
                }

                switch (type) {
                    case TileTypeEnum.TileCenter:
                        tile = tiles[height].center;
                        break;
                    case TileTypeEnum.TileTopLeft:
                        tile = tiles[height].topLeft;
                        break;
                    case TileTypeEnum.TileTopRight:
                        tile = tiles[height].topRight;
                        break;
                    case TileTypeEnum.TileBottomLeft:
                        tile = tiles[height].bottomLeft;
                        break;
                    case TileTypeEnum.TileBottomRight:
                        tile = tiles[height].bottomRight;
                        break;
                    default:
                        tile = tiles[height].center;
                        break;
                }

                if (grid != null) {
                    Tilemap tileMap = layers[height].GetComponent<Tilemap>();
                    tileMap.SetTile(new Vector3Int(x, y, 0), tile);

                    // Set a solid tile on the layer below
                    if (type != TileTypeEnum.TileCenter) {
                        if (height > 0) {
                            Tilemap lowerTileMap = layers[height - 1].GetComponent<Tilemap>();
                            lowerTileMap.SetTile(new Vector3Int(x, y, 0), tiles[height - 1].center);
                        }
                    }
                }
            }
        }
    }

    private TileTypeEnum GetTileType(int[,] heightMapInt, Vector2Int position) {
        int height = heightMapInt[position.x, position.y];
        int heightTop = heightMapInt[position.x, position.y + 1];
        int heightBottom = heightMapInt[position.x, position.y - 1];
        int heightLeft = heightMapInt[position.x - 1, position.y];
        int heightRight = heightMapInt[position.x + 1, position.y];

        if ((heightTop == heightBottom) || (heightLeft == heightRight)) {
            return TileTypeEnum.TileCenter;
        }

        if ((height != heightRight) && (height != heightBottom)) {
            if (height > heightRight) {
                return TileTypeEnum.TileTopLeft;
            }
        }

        if ((height != heightLeft) && (height != heightBottom)) {
            if (height > heightLeft) {
               return TileTypeEnum.TileTopRight;
            }
        }

        if ((height != heightRight) && (height != heightTop)) {
            if (height > heightRight) {
                return TileTypeEnum.TileBottomLeft;
            }
        }

        if ((height != heightLeft) && (height != heightTop)) {
            if (height > heightLeft) {
                return TileTypeEnum.TileBottomRight;
            }
        }

        return TileTypeEnum.TileCenter;
    }
}
