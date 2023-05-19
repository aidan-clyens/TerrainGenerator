using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct TileData {
    public BiomeTypeEnum biome;
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

public enum BiomeTypeEnum {
    BiomeGrass,
    BiomeSnow
}

[RequireComponent(typeof(ProceduralTileGenerator2D))]
public class TerrainMapGenerator2D : TerrainMapGeneratorBase {
    [Header("2D Settings")]
    [Space(10)]
    public int tilemapWidth;
    public Grid grid;
    public bool smoothEdges = false;
    public List<TileData> tiles;

    [Header("Biome Settings")]
    [Space(10)]
    public NoiseSettings biomeNoiseSettings;

    [Header("Procedural Tile Settings")]
    [Space(10)]
    public List<ProceduralTileData> tileData;
    public NoiseSettings noiseSettings;

    private float maxHeight;
    private float[,] heightMap;

    // Tilemaps
    private List<GameObject> layers = new List<GameObject>();
    private GameObject objectLayer;
    private GameObject objectCollisionLayer;

    // Components
    private BiomeGenerator2D biomeGenerator2D;
    private ProceduralTileGenerator2D proceduralTileGenerator2D;

    // Biomes
    private float[,] temperatureMap;
    private Dictionary<BiomeTypeEnum, List<TileData>> biomeTiles = new Dictionary<BiomeTypeEnum, List<TileData>>();

    private Queue<BiomeThreadInfo> biomeDataThreadInfoQueue = new Queue<BiomeThreadInfo>();

    public override void Start() {
        base.Start();

        if (grid == null)
            return;

        // Get all layers
        foreach (Transform child in grid.transform) {
            if (!layers.Contains(child.gameObject)) {
                layers.Add(child.gameObject);
            }
        }
    }

    public override void Update() {
        base.Update();

        // Process biome data
        if (biomeDataThreadInfoQueue.Count > 0) {
            for (int i = 0; i < biomeDataThreadInfoQueue.Count; i++) {
                BiomeThreadInfo info = biomeDataThreadInfoQueue.Dequeue();
                ProcessBiomeData(info);
            }
        }
    }

    public override void OnValidate() {
        base.OnValidate();

        if (biomeGenerator2D == null) {
            biomeGenerator2D = GetComponent<BiomeGenerator2D>();
        }

        if (proceduralTileGenerator2D == null) {
            proceduralTileGenerator2D = GetComponent<ProceduralTileGenerator2D>();
        }

        // Height Map Generator settings
        heightMapGenerator.normalize = true;

        // Procedural Tile Generator 2D settings
        proceduralTileGenerator2D.tileData = tileData;
        proceduralTileGenerator2D.noiseSettings = noiseSettings;
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

        if (objectLayer != null) {
            Tilemap tilemap = objectLayer.GetComponent<Tilemap>();
            tilemap.ClearAllTiles();
        }

        if (objectCollisionLayer != null) {
            Tilemap tilemap = objectCollisionLayer.GetComponent<Tilemap>();
            tilemap.ClearAllTiles();
        }

        biomeTiles.Clear();
    }

    public override void Randomize() {
        base.Randomize();

        proceduralTileGenerator2D.Randomize();
    }

    public override void ProcessHeightMapData(HeightMapThreadInfo info) {
        heightMap = info.heightMap;

        biomeGenerator2D.RequestBiomeData(biomeNoiseSettings, tilemapWidth, OnBiomeDataReceived);
    }

    public void ProcessBiomeData(BiomeThreadInfo info) {
        temperatureMap = info.temperatureMap;
        GenerateTilemap(heightMap);
    }

    private void OnBiomeDataReceived(float[,] temperatureMap, float[,] moistureMap) {
        lock (biomeDataThreadInfoQueue) {
            biomeDataThreadInfoQueue.Enqueue(new BiomeThreadInfo(temperatureMap, moistureMap));
        }
    }

    private void RequestTilemap() {
        if (grid != null)
            CreateLayers();

        foreach (TileData tile in tiles) {
            if (!biomeTiles.ContainsKey(tile.biome)) {
                biomeTiles[tile.biome] = new List<TileData>();
            }

            biomeTiles[tile.biome].Add(tile);
        }

        maxHeight = biomeTiles[BiomeTypeEnum.BiomeGrass].Count;

        heightMapGenerator.RequestHeightMapData(seed, tilemapWidth, Vector2.zero, OnHeightMapDataReceived);
    }

    private void CreateLayers() {
        for (int i = 0; i < maxHeight; i++) {
            GameObject layer = new GameObject("Layer " + i);
            layer.AddComponent<Tilemap>();
            layer.AddComponent<TilemapRenderer>();

            TilemapRenderer renderer = layer.GetComponent<TilemapRenderer>();
            renderer.sortingOrder = i;

            layer.transform.parent = grid.transform;

            layers.Add(layer);
        }

        if (objectLayer == null) {
            objectLayer = new GameObject("Object Layer");
            objectLayer.AddComponent<Tilemap>();
            objectLayer.AddComponent<TilemapRenderer>();

            objectLayer.transform.parent = grid.transform;
        }

        if (objectCollisionLayer == null) {
            objectCollisionLayer = new GameObject("Object Collision Layer");
            objectCollisionLayer.AddComponent<Tilemap>();
            objectCollisionLayer.AddComponent<TilemapRenderer>();
            objectCollisionLayer.AddComponent<TilemapCollider2D>();

            objectCollisionLayer.transform.parent = grid.transform;
        }

        TilemapRenderer objectLayerRenderer = objectLayer.GetComponent<TilemapRenderer>();
        objectLayerRenderer.sortingOrder = layers.Count;

        objectLayerRenderer = objectCollisionLayer.GetComponent<TilemapRenderer>();
        objectLayerRenderer.sortingOrder = layers.Count;
    }

    private void GenerateTilemap(float[,] heightMap) {
        int mapWidth = heightMap.GetLength(0);
        int mapHeight = heightMap.GetLength(1);

        int[,] heightMapInt = new int[mapWidth, mapHeight];

        // Scale the normalized height map to an integer between 0 and the max height (determined by the number of tiles)
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                int height = (int)Mathf.Round(heightMap[x, y] * (maxHeight - 1));
                heightMapInt[x, y] = height;
            }
        }

        // Determine the tile type given the height
        for (int y = 1; y < mapHeight - 1; y++) {
            for (int x = 1; x < mapWidth - 1; x++) {
                int height = heightMapInt[x, y];
                Tile tile;

                TileTypeEnum type;
                BiomeTypeEnum biomeType = GetBiomeType(new Vector2Int(x, y));

                if (smoothEdges) {
                    type = GetTileType(heightMapInt, new Vector2Int(x, y));
                }
                else {
                    type = TileTypeEnum.TileCenter;
                }

                switch (type) {
                    case TileTypeEnum.TileCenter:
                        tile = biomeTiles[biomeType][height].center;
                        break;
                    case TileTypeEnum.TileTopLeft:
                        tile = biomeTiles[biomeType][height].topLeft;
                        break;
                    case TileTypeEnum.TileTopRight:
                        tile = biomeTiles[biomeType][height].topRight;
                        break;
                    case TileTypeEnum.TileBottomLeft:
                        tile = biomeTiles[biomeType][height].bottomLeft;
                        break;
                    case TileTypeEnum.TileBottomRight:
                        tile = biomeTiles[biomeType][height].bottomRight;
                        break;
                    default:
                        tile = biomeTiles[biomeType][height].center;
                        break;
                }

                if (grid != null) {
                    Tilemap tileMap = layers[height].GetComponent<Tilemap>();
                    tileMap.SetTile(new Vector3Int(x, y, 0), tile);

                    // Set a solid tile on the layer below
                    if (type != TileTypeEnum.TileCenter) {
                        if (height > 0) {
                            Tilemap lowerTileMap = layers[height - 1].GetComponent<Tilemap>();
                            // TODO - Need to check lower layers biome type
                            lowerTileMap.SetTile(new Vector3Int(x, y, 0), biomeTiles[biomeType][height - 1].center);
                        }
                    }
                }
            }
        }

        // Procedural tile settings
        Tilemap objectTileMap = (Tilemap)objectLayer.GetComponent<Tilemap>();
        Tilemap objectCollisionTileMap = (Tilemap)objectCollisionLayer.GetComponent<Tilemap>();
        proceduralTileGenerator2D.Generate(objectTileMap, objectCollisionTileMap, heightMapInt, seed);
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

    private BiomeTypeEnum GetBiomeType(Vector2Int position) {
        if (temperatureMap[position.x, position.y] < 0.2f) {
            return BiomeTypeEnum.BiomeSnow;
        }
        else {
            return BiomeTypeEnum.BiomeGrass;
        }
    }
}
