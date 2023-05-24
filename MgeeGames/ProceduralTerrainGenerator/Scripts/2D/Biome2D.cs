using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Biome2D : ScriptableObject {
	// public BiomeTypeEnum biome;
    // public string biome;
	[Range(0.0f, 1.0f)]
	public float minTemperature;
    [Range(0.0f, 1.0f)]
    public float maxTemperature;
    [Range(0.0f, 1.0f)]
    public float minMoisture;
    [Range(0.0f, 1.0f)]
    public float maxMoisture;
	public List<GroundTile2D> tiles;
}