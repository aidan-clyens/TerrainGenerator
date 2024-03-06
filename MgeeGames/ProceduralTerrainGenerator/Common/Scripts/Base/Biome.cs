using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Biome : ScriptableObject
{
    public int minLatitude;
    public int maxLatitude;
    public bool inverse;
    [Range(BiomeGenerator.MIN_TEMPERATURE, BiomeGenerator.MAX_TEMPERATURE)]
    public float minTemperature;
    [Range(BiomeGenerator.MIN_TEMPERATURE, BiomeGenerator.MAX_TEMPERATURE)]
    public float maxTemperature;
    [Range(BiomeGenerator.MIN_MOISTURE, BiomeGenerator.MAX_MOISTURE)]
    public float minMoisture;
    [Range(BiomeGenerator.MIN_MOISTURE, BiomeGenerator.MAX_MOISTURE)]
    public float maxMoisture;
}