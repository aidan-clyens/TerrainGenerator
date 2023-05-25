using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    [Space(10)]
    [CustomAttributes.HorizontalLine()]
    [Header("2D Settings")]
    [Space(10)]
    public int tilemapWidth;
    public Grid grid;
    public bool smoothEdges = false;
    public int numLayers;
    public List<Biome2D> biomeTiles;

    [Space(10)]
    [CustomAttributes.HorizontalLine()]
    [Header("Procedural Tile Settings")]
    [Space(10)]
    public List<ProceduralTileData> tileData;
    public NoiseSettings noiseSettings;

    private float[,] heightMap;

    // Tilemaps
    private List<GameObject> layers = new List<GameObject>();
    private GameObject objectLayer;
    private GameObject objectCollisionLayer;

    // Components
    private ProceduralTileGenerator2D proceduralTileGenerator2D;

    // Biomes
    private float[,] temperatureMap;
    private float[,] moistureMap;
    private Dictionary<string, List<GroundTile2D>> biomeTileMap = new Dictionary<string, List<GroundTile2D>>();

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

    public override void OnValidate() {
        base.OnValidate();

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

        biomeTileMap.Clear();
    }

    public override void Randomize() {
        base.Randomize();

        proceduralTileGenerator2D.Randomize();
    }

    public override void ProcessHeightMapData(HeightMapThreadInfo info) {
        heightMap = info.heightMap;

        biomeGenerator.RequestBiomeData(temperatureNoiseSettings, moistureNoiseSettings, tilemapWidth, OnBiomeDataReceived);
    }

    public override void ProcessBiomeData(BiomeThreadInfo info) {
        temperatureMap = info.temperatureMap;
        moistureMap = info.moistureMap;
        GenerateTilemap(heightMap);
    }

    private void RequestTilemap() {
        if (grid != null)
            CreateLayers();

        foreach (Biome2D biome in biomeTiles) {
            if (biome.tiles.Count > numLayers) {
                Debug.LogWarning("Each Biome must have a number of Tiles less than or equal the number of layers.");
                return;
            }

            if (!biomeTileMap.ContainsKey(biome.name)) {
                biomeTileMap[biome.name] = biome.tiles;
            }
        }

        heightMapGenerator.RequestHeightMapData(seed, tilemapWidth, Vector2.zero, OnHeightMapDataReceived);
    }

    private void CreateLayers() {
        for (int i = 0; i < numLayers; i++) {
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
                int height = (int)Mathf.Round(heightMap[x, y] * (numLayers - 1));
                heightMapInt[x, y] = height;
            }
        }

        // Determine the tile type given the height
        for (int y = 1; y < mapHeight - 1; y++) {
            for (int x = 1; x < mapWidth - 1; x++) {
                int height = heightMapInt[x, y];
                Tile tile;

                TileTypeEnum type;
                string biomeType = GetBiomeType(new Vector2Int(x, y));
                if (!biomeTileMap.ContainsKey(biomeType)) {
                    continue;
                }

                if (smoothEdges) {
                    type = GetTileType(heightMapInt, new Vector2Int(x, y));
                }
                else {
                    type = TileTypeEnum.TileCenter;
                }

                switch (type) {
                    case TileTypeEnum.TileCenter:
                        tile = biomeTileMap[biomeType][height].center;
                        break;
                    case TileTypeEnum.TileTopLeft:
                        tile = biomeTileMap[biomeType][height].topLeft;
                        break;
                    case TileTypeEnum.TileTopRight:
                        tile = biomeTileMap[biomeType][height].topRight;
                        break;
                    case TileTypeEnum.TileBottomLeft:
                        tile = biomeTileMap[biomeType][height].bottomLeft;
                        break;
                    case TileTypeEnum.TileBottomRight:
                        tile = biomeTileMap[biomeType][height].bottomRight;
                        break;
                    default:
                        tile = biomeTileMap[biomeType][height].center;
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
                            lowerTileMap.SetTile(new Vector3Int(x, y, 0), biomeTileMap[biomeType][height - 1].center);
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

    private string GetBiomeType(Vector2Int position) {
        float temp = temperatureMap[position.x, position.y];
        float moisture = moistureMap[position.x, position.y];

        foreach (Biome2D biome in biomeTiles) {
            if ((temp > biome.minTemperature && temp < biome.maxTemperature)
                && (moisture > biome.minMoisture && moisture < biome.maxMoisture)) {
                return biome.name;
            }
        }

        return "";
    }
}
