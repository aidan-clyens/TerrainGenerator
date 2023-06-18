using UnityEngine;
using System;

[Serializable]
public struct TileData {
    public Vector2Int position;
    public int height;
    public string biome;
    public float temperature;
    public float moisture;
    public GroundTile2D groundTile;
}

public class TileMapData : ScriptableObject {
    [SerializeField]
    public TileData[] tileData;
}
