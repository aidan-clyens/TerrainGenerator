using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Biome2D : ScriptableObject {
	// public BiomeTypeEnum biome;
    // public string biome;
	[Range(BiomeGenerator.MIN_TEMPERATURE, BiomeGenerator.MAX_TEMPERATURE)]
	public float minTemperature;
    [Range(BiomeGenerator.MIN_TEMPERATURE, BiomeGenerator.MAX_TEMPERATURE)]
    public float maxTemperature;
    [Range(BiomeGenerator.MIN_MOISTURE, BiomeGenerator.MAX_MOISTURE)]
    public float minMoisture;
    [Range(BiomeGenerator.MIN_MOISTURE, BiomeGenerator.MAX_MOISTURE)]
    public float maxMoisture;
	public List<GroundTile2D> tiles;
}