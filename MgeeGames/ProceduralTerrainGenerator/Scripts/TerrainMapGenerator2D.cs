using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainMapGenerator2D : TerrainMapGeneratorBase
{
    [Header("2D Settings")]
    [Space(10)]
    public int tilemapWidth;
    public Tilemap groundTilemap;
    public List<Tile> tiles;

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

        for (int y = 0; y < tilemapHeight; y++) {
            for (int x = 0; x < tilemapWidth; x++) {
                int height = (int)Mathf.Round(heightMap[x, y] * (tiles.Count - 1));
                if (groundTilemap != null) {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), tiles[height]);
                }
            }
        }
    }
}