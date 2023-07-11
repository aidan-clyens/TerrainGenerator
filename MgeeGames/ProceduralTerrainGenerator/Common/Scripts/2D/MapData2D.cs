using UnityEngine;
using System;

[Serializable]
public struct TileData {
    public Vector2Int position;
    public string biome;
    public GroundTile2D groundTile;
}

[CreateAssetMenu(menuName = "Map/MapData2D")]
public class MapData2D : MapData {
    [SerializeField]
    public TileData[] tileData;
}
