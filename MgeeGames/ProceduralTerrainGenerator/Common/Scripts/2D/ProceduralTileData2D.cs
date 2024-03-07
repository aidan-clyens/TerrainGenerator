using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class ProceduralTileData2D : ProceduralObjectDataBase {
	public Tile tile;
	public int minLayer;
	public int maxLayer;
	[Range(0.0f, 1.0f)]
	public float threshold;
	public bool collide;
	public string biome = "";
}
