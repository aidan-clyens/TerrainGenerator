using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class GroundTile2D : ScriptableObject {
    public Tile center;
    public Tile topLeft;
    public Tile topRight;
    public Tile bottomLeft;
    public Tile bottomRight;
}