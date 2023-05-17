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

public class TerrainMapGenerator2D : TerrainMapGeneratorBase
{
    [Header("2D Settings")]
    [Space(10)]
    public int tilemapWidth;
    public Tilemap groundTilemap;
    public bool smoothEdges = false;
    public List<TileData> tiles;

    public override void Update() {
        base.Update();
    }

    public override void OnValidate() {
        base.OnValidate();

        heightMapGenerator.normalize = true;
    }

    public override void Generate() {
        RequestTilemap();
    }

    public override void Clear() {
        if (groundTilemap != null)
            groundTilemap.ClearAllTiles();
    }

    public override void ProcessHeightMapData(HeightMapThreadInfo info) {
        GenerateTilemap(info.heightMap);
    }

    private void RequestTilemap() {
        heightMapGenerator.RequestHeightMapData(seed, tilemapWidth, Vector2.zero, OnHeightMapDataReceived);
    }

    private void GenerateTilemap(float[,] heightMap) {
        int tilemapWidth = heightMap.GetLength(0);
        int tilemapHeight = heightMap.GetLength(1);

        int[,] heightMapInt = new int[tilemapWidth, tilemapHeight];

        for (int y = 0; y < tilemapHeight; y++) {
            for (int x = 0; x < tilemapWidth; x++) {
                int height = (int)Mathf.Round(heightMap[x, y] * (tiles.Count - 1));
                heightMapInt[x, y] = height;
            }
        }

        for (int y = 1; y < tilemapHeight - 1; y++) {
            for (int x = 1; x < tilemapWidth - 1; x++) {
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

                if (groundTilemap != null) {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), tile);
                } 
            }
        }
    }

    private TileTypeEnum GetTileType(int[,] heightMapInt, Vector2Int position) {
        int tilemapWidth = heightMapInt.GetLength(0);
        int tilemapHeight = heightMapInt.GetLength(1);

        int height = heightMapInt[position.x, position.y];
        int heightTop = heightMapInt[position.x, position.y + 1];
        int heightBottom = heightMapInt[position.x, position.y - 1];
        int heightLeft = heightMapInt[position.x - 1, position.y];
        int heightRight = heightMapInt[position.x + 1, position.y];

        if ((heightTop == heightBottom) || (heightLeft == heightRight)) {
            return TileTypeEnum.TileCenter;
        }

        if ((height != heightRight) && (height != heightBottom)) {
            return TileTypeEnum.TileTopLeft;
        }

        if ((height != heightLeft) && (height != heightBottom)) {
            return TileTypeEnum.TileTopRight;
        }

        if ((height != heightRight) && (height != heightTop)) {
            return TileTypeEnum.TileBottomLeft;
        }

        if ((height != heightLeft) && (height != heightTop)) {
            return TileTypeEnum.TileBottomRight;
        }

        return TileTypeEnum.TileCenter;
    }
}
