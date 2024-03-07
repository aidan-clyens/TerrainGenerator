using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

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
    public string objectSortingLayer;
    public string collisionSortingLayer;

    [Space(10)]
    [Header("Procedural Tile Settings")]
    [CustomAttributes.HorizontalLine()]
    [Space(10)]
    public bool useProceduralTiles;
    [MyBox.ConditionalField(nameof(useProceduralTiles), false, true)]
    public ProceduralTileDataList tileData;
    [MyBox.ConditionalField(nameof(useProceduralTiles), false, true)]
    public NoiseSettings noiseSettings;

    [Space(10)]
    [Header("Water Settings")]
    [CustomAttributes.HorizontalLine()]
    [Space(10)]
    public bool useWater = false;
    public int waterLevel;
    public GroundTile2D waterTile;
 
    private int[,] heightMap;

    // Tilemaps
    private List<GameObject> layers = new List<GameObject>();
    private Dictionary<string, List<GameObject>> biomeLayers = new Dictionary<string, List<GameObject>>(); 
    private GameObject objectLayer;
    private GameObject objectCollisionLayer;
    private GameObject waterLayer;

    // Components
    private ProceduralTileGenerator2D proceduralTileGenerator2D;

    // Biomes
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
        biomeLayers.Clear();

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
        int mapWidth = info.heightMap.GetLength(0);
        int mapHeight = info.heightMap.GetLength(1);

        heightMap = new int[mapWidth, mapHeight];

        // Scale the normalized height map to an integer between 0 and the max height (determined by the number of tiles)
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                heightMap[x, y] = (int)Mathf.Round(info.heightMap[x, y] * (numLayers - 1));
            }
        }

        biomeGenerator.RequestBiomeData(temperatureNoiseSettings, moistureNoiseSettings, tilemapWidth, OnBiomeDataReceived);
    }

    public override void ProcessBiomeData(BiomeThreadInfo info) {
        GenerateTilemap();
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

        foreach (Biome biome in biomes.biomes) {
            if (biome is Biome2D) {
                Biome2D biome2D = biome as Biome2D;
                if (biome2D.tiles.Count > numLayers) {
                    Debug.LogWarning("Each Biome must have a number of Tiles less than or equal the number of layers.");
                    return;
                }

                if (!biomeTileMap.ContainsKey(biome2D.name)) {
                    biomeTileMap[biome2D.name] = biome2D.tiles;
                }
            }
        }

        heightMapGenerator.RequestHeightMapData(seed, tilemapWidth, Vector2.zero, OnHeightMapDataReceived);
    }

    private void CreateLayers() {
        List<Biome> allBiomeTiles = new List<Biome>();
        allBiomeTiles.AddRange(biomes.biomes);
        allBiomeTiles.Add(defaultBiome);

        for (int n = 0; n < allBiomeTiles.Count; n++) {
            GameObject biomeLayer = new GameObject("Biome " + allBiomeTiles[n].name);
            biomeLayer.transform.position = new Vector3(0, 0, 0);
            biomeLayer.transform.parent = grid.transform;

            layers.Add(biomeLayer);
            biomeLayers[allBiomeTiles[n].name] = new List<GameObject>();

            for (int i = 0; i < numLayers; i++) {
                GameObject layer = CreateLayerObject(allBiomeTiles[n].name + " Layer " + i, terrainSortingLayer, i, biomeLayer.transform, collision: true, trigger: true);
                biomeLayers[allBiomeTiles[n].name].Add(layer);
            }
        }

        if (objectLayer == null) {
	        objectLayer = CreateLayerObject("Object Layer", objectSortingLayer, biomeLayers.Count, grid.transform, collision: false, renderer_mode: TilemapRenderer.Mode.Individual);
        }

        if (objectCollisionLayer == null) {
	        objectCollisionLayer = CreateLayerObject("Object Collision Layer", collisionSortingLayer, biomeLayers.Count + 1, grid.transform, collision: true, renderer_mode: TilemapRenderer.Mode.Individual);
        }

        if (waterLayer == null) {
            waterLayer = CreateLayerObject("Water Layer", objectSortingLayer, biomeLayers.Count, grid.transform, collision: true);
            layers.Add(waterLayer);
        }
    }

	private GameObject CreateLayerObject(string name, string sortingLayerName, int sortingOrder, Transform parent, bool collision = false, bool trigger = false, TilemapRenderer.Mode renderer_mode = TilemapRenderer.Mode.Chunk) {
        GameObject layer = new GameObject(name);
        layer.AddComponent<Tilemap>();
        layer.AddComponent<TilemapRenderer>();

        if (collision) {
            layer.AddComponent<TilemapCollider2D>();
            layer.AddComponent<CompositeCollider2D>();

            layer.GetComponent<TilemapCollider2D>().usedByComposite = true;
            layer.GetComponent<CompositeCollider2D>().isTrigger = trigger;
            layer.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }

        TilemapRenderer renderer = layer.GetComponent<TilemapRenderer>();
        if (sortingLayerName.Length > 0) {
            renderer.sortingLayerName = sortingLayerName;
        }

		renderer.mode = renderer_mode;

        layer.transform.position = new Vector3(0, 0, 0);
        layer.transform.parent = parent;

        return layer;
    }

    private void GenerateTilemap() {
        int mapWidth = heightMap.GetLength(0);
        int mapHeight = heightMap.GetLength(1);
        string[,] biomeMap = new string[mapWidth, mapHeight];

        mapData2D.tileData = new TileData[mapWidth * mapHeight];

        // Determine the tile type given the height
        for (int y = 1; y < mapHeight - 1; y++) {
            for (int x = 1; x < mapWidth - 1; x++) {
                int index = y * mapWidth + x;
                int height = heightMap[x, y];
                Vector2Int position = new Vector2Int(x, y);

                string biomeType = biomeGenerator.GetBiomeType(position);
                if (!biomeTileMap.ContainsKey(biomeType)) {
                    biomeType = defaultBiome.name;
                }

                mapData2D.tileData[index] = new TileData();
                mapData2D.tileData[index].position = position;
                mapData2D.tileData[index].biome = biomeType;
                mapData2D.tileData[index].groundTile = GetTile(position, biomeType, height);
            }
        }

        // Render tilemaps
        if (grid != null) {
            for (int y = 1; y < mapHeight - 1; y++) {
                for (int x = 1; x < mapWidth - 1; x++) {
                    int index = y * mapWidth + x;
                    int height = heightMap[x, y];
                    Vector2Int position = new Vector2Int(x, y);

                    GroundTile2D tile = mapData2D.tileData[index].groundTile;
                    string biomeType = mapData2D.tileData[index].biome;

                    if (biomeType == null)
                        continue;

                    biomeMap[x, y] = biomeType;

                    Tilemap tileMap = biomeLayers[biomeType][height].GetComponent<Tilemap>();
                    if (useWater) {
                        if (height <= waterLevel) {
                            tileMap = waterLayer.GetComponent<Tilemap>();
                        }
                    }

                    tileMap.SetTile(new Vector3Int(x, y, 0), tile);

                    if (smoothEdges) {
                        // Set a solid tile on the layer below
                        if (!IsCenterTile(position)) {
                            if (height > 0) {
                                Tilemap lowerTileMap = biomeLayers[biomeType][height - 1].GetComponent<Tilemap>();
                                GroundTile2D lowerTile = GetTile(position, biomeType, height - 1);
                                if (useWater && height - 1 <= waterLevel) {
                                    lowerTile = waterTile;
                                }

                                lowerTileMap.SetTile(new Vector3Int(x, y, 0), lowerTile);
                            }
                        }
                    }
                }
            }
        }

        // Procedural tile settings
        Tilemap objectTileMap = (Tilemap)objectLayer.GetComponent<Tilemap>();
        Tilemap objectCollisionTileMap = (Tilemap)objectCollisionLayer.GetComponent<Tilemap>();

        if (useWater) {
            proceduralTileGenerator2D.Generate(objectTileMap, objectCollisionTileMap, heightMap, biomeMap, seed, waterLevel);
        }
        else {
            proceduralTileGenerator2D.Generate(objectTileMap, objectCollisionTileMap, heightMap, biomeMap, seed);
        }

        EditorUtility.SetDirty(mapData2D);
    }

    private GroundTile2D GetTile(Vector2Int position, string biome, int height) {
        List<GroundTile2D> tiles = (defaultBiome as Biome2D).tiles;
        if (biomeTileMap.ContainsKey(biome)) {
            tiles = biomeTileMap[biome];
        }

        if (height < tiles.Count) {
            GroundTile2D tile = tiles[height];
            tile.Height = height;
            tile.Temperature = biomeGenerator.GetTemperature(position);
            tile.Moisture = biomeGenerator.GetMoisture(position);

            tile.SetSmoothEdges(smoothEdges);

            if (useWater) {
                if (height <= waterLevel) {
                    tile = waterTile;
                }
            }

            return tile;
        }

        return new GroundTile2D();
    }

    private bool IsCenterTile(Vector2Int position) {
        int heightTop = heightMap[position.x, position.y + 1];
        int heightBottom = heightMap[position.x, position.y - 1];
        int heightLeft = heightMap[position.x - 1, position.y];
        int heightRight = heightMap[position.x + 1, position.y];

        return ((heightTop == heightBottom) || (heightLeft == heightRight));
    }
}
