using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public enum TileTypeEnum {
    TileCenter,
    TileTopLeft,
    TileTopRight,
    TileBottomLeft,
    TileBottomRight
}

[RequireComponent(typeof(ProceduralTileGenerator2D))]
public class TerrainMapGenerator2D : TerrainMapGeneratorBase {
    [Space(10)]
    [Header("2D Settings")]
    [CustomAttributes.HorizontalLine()]
    [Space(10)]
    public int tilemapWidth;
    public Grid grid;
    public bool smoothEdges = false;
    public int numLayers;
    public string terrainSortingLayer;
    public string collisionSortingLayer;
    public List<Biome2D> biomeTiles;

    [Space(10)]
    [Header("Procedural Tile Settings")]
    [CustomAttributes.HorizontalLine()]
    [Space(10)]
    public List<ProceduralTileData> tileData;
    public NoiseSettings noiseSettings;

    [Space(10)]
    [Header("Water Settings")]
    [CustomAttributes.HorizontalLine()]
    [Space(10)]
    public bool useWater = false;
    public int waterLevel;
    public GroundTile2D waterTile;
 
    private float[,] heightMap;

    // Tilemaps
    private List<GameObject> layers = new List<GameObject>();
    private GameObject objectLayer;
    private GameObject objectCollisionLayer;
    private GameObject waterLayer;

    // Components
    private ProceduralTileGenerator2D proceduralTileGenerator2D;

    // Biomes
    private float[,] temperatureMap;
    private float[,] moistureMap;
    private Dictionary<string, List<GroundTile2D>> biomeTileMap = new Dictionary<string, List<GroundTile2D>>();

    private MapData2D mapData2D;

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

        if (mapData is MapData2D) {
            mapData2D = (MapData2D)mapData;
        }

        // Height Map Generator settings
        heightMapGenerator.normalize = true;

        // Procedural Tile Generator 2D settings
        proceduralTileGenerator2D.tileData = tileData;
        proceduralTileGenerator2D.noiseSettings = noiseSettings;
    }

    public override void Generate() {
        if (mapData2D == null) {
            Debug.LogWarning("Error: Missing MapData scriptable object. Failed to Generate.");
            return;
        }

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

        if (waterLayer != null) {
            Tilemap tilemap = waterLayer.GetComponent<Tilemap>();
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

    public MapData2D GetMapData2D() {
        return mapData2D;
    }

    public TileData GetTileData(Vector2Int position) {
        MapData2D mapData2D = GetMapData2D();

        int index = position.y * tilemapWidth + position.x;
        
        if (index < mapData2D.tileData.Length) {
            return mapData2D.tileData[index];
        }

        return new TileData();
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
            if (terrainSortingLayer.Length > 0) {
                renderer.sortingLayerName = terrainSortingLayer;
            }
            renderer.sortingOrder = i;

            layer.transform.position = new Vector3(0, 0, 0);
            layer.transform.parent = grid.transform;

            layers.Add(layer);
        }

        if (objectLayer == null) {
            objectLayer = new GameObject("Object Layer");
            objectLayer.AddComponent<Tilemap>();
            objectLayer.AddComponent<TilemapRenderer>();

            objectLayer.transform.position = new Vector3(0, 0, 0);
            objectLayer.transform.parent = grid.transform;
        }

        if (objectCollisionLayer == null) {
            objectCollisionLayer = new GameObject("Object Collision Layer");
            objectCollisionLayer.AddComponent<Tilemap>();
            objectCollisionLayer.AddComponent<TilemapRenderer>();
            objectCollisionLayer.AddComponent<TilemapCollider2D>();

            objectCollisionLayer.transform.position = new Vector3(0, 0, 0);
            objectCollisionLayer.transform.parent = grid.transform;
        }

        if (waterLayer == null) {
            waterLayer = new GameObject("Water Layer");
            waterLayer.AddComponent<Tilemap>();
            waterLayer.AddComponent<TilemapRenderer>();
            waterLayer.AddComponent<TilemapCollider2D>();

            waterLayer.transform.position = new Vector3(0, 0, 0);
            waterLayer.transform.parent = grid.transform;
        }

        TilemapRenderer objectLayerRenderer = objectLayer.GetComponent<TilemapRenderer>();
        if (terrainSortingLayer.Length > 0) {
            objectLayerRenderer.sortingLayerName = terrainSortingLayer;
        }
        objectLayerRenderer.sortingOrder = layers.Count;

        objectLayerRenderer = objectCollisionLayer.GetComponent<TilemapRenderer>();
        if (collisionSortingLayer.Length > 0) {
            objectLayerRenderer.sortingLayerName = collisionSortingLayer;
        }
        objectLayerRenderer.sortingOrder = layers.Count + 1;

        objectLayerRenderer = waterLayer.GetComponent<TilemapRenderer>();
        if (collisionSortingLayer.Length > 0) {
            objectLayerRenderer.sortingLayerName = collisionSortingLayer;
        }
        objectLayerRenderer.sortingOrder = layers.Count;
    }

    private void GenerateTilemap(float[,] heightMap) {
        int mapWidth = heightMap.GetLength(0);
        int mapHeight = heightMap.GetLength(1);

        mapData2D.tileData = new TileData[mapWidth * mapHeight];

        int[,] heightMapInt = new int[mapWidth, mapHeight];

        // Scale the normalized height map to an integer between 0 and the max height (determined by the number of tiles)
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                int height = (int)Mathf.Round(heightMap[x, y] * (numLayers - 1));
                heightMapInt[x, y] = height;

                int index = y * mapWidth + x;
                mapData2D.tileData[index] = new TileData();
                mapData2D.tileData[index].position = new Vector2Int(x, y);
            }
        }

        // Determine the tile type given the height
        for (int y = 1; y < mapHeight - 1; y++) {
            for (int x = 1; x < mapWidth - 1; x++) {
                int height = heightMapInt[x, y];
                Vector2Int position = new Vector2Int(x, y);
                GroundTile2D tile;

                string biomeType = GetBiomeType(position);
                if (!biomeTileMap.ContainsKey(biomeType)) {
                    continue;
                }

                int index = y * mapWidth + x;
                mapData2D.tileData[index].biome = biomeType;

                tile = biomeTileMap[biomeType][height];
                tile.Height = height;
                tile.Temperature = temperatureMap[x, y];
                tile.Moisture = moistureMap[x, y];

                if (useWater) {
                    if (height <= waterLevel) {
                        tile = waterTile;
                    }
                }

                mapData2D.tileData[index].groundTile = tile;

                if (grid != null) {
                    Tilemap tileMap = layers[height].GetComponent<Tilemap>();
                    if (useWater) {
                        if (height <= waterLevel) {
                            tileMap = waterLayer.GetComponent<Tilemap>();
                        }
                    }

                    tileMap.SetTile(new Vector3Int(x, y, 0), tile);

                    // Set a solid tile on the layer below
                    if (!IsCenterTile(heightMapInt, position)) {
                        if (height > 0) {
                            Tilemap lowerTileMap = layers[height - 1].GetComponent<Tilemap>();
                            // TODO - Need to check lower layers biome type
                            GroundTile2D lowerTile = biomeTileMap[biomeType][height - 1];
                            if (useWater && height - 1 <= waterLevel) {
                                lowerTile = waterTile;
                            }

                            lowerTileMap.SetTile(new Vector3Int(x, y, 0), lowerTile);
                        }
                    }
                }
            }
        }

        // Procedural tile settings
        Tilemap objectTileMap = (Tilemap)objectLayer.GetComponent<Tilemap>();
        Tilemap objectCollisionTileMap = (Tilemap)objectCollisionLayer.GetComponent<Tilemap>();

        if (useWater) {
            proceduralTileGenerator2D.Generate(objectTileMap, objectCollisionTileMap, heightMapInt, seed, waterLevel);
        }
        else {
            proceduralTileGenerator2D.Generate(objectTileMap, objectCollisionTileMap, heightMapInt, seed);
        }
    }

    private bool IsCenterTile(int[,] heightMapInt, Vector2Int position) {
        int heightTop = heightMapInt[position.x, position.y + 1];
        int heightBottom = heightMapInt[position.x, position.y - 1];
        int heightLeft = heightMapInt[position.x - 1, position.y];
        int heightRight = heightMapInt[position.x + 1, position.y];

        return ((heightTop == heightBottom) || (heightLeft == heightRight));
    }

    private string GetBiomeType(Vector2Int position) {
        float temp = temperatureMap[position.x, position.y];
        float moisture = moistureMap[position.x, position.y];

        foreach (Biome2D biome in biomeTiles) {
            if ((temp >= biome.minTemperature && temp <= biome.maxTemperature)
                && (moisture >= biome.minMoisture && moisture <= biome.maxMoisture)) {
                return biome.name;
            }
        }

        return "";
    }
}
