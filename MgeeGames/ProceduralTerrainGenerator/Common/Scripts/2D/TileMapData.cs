using UnityEngine;
using System;

[Serializable]
public struct TileData {
    public Vector2Int position;
    public string biome;
    public GroundTile2D groundTile;
}

public class TileMapData : ScriptableObject {
    [SerializeField]
    public TileData[] tileData;
}
