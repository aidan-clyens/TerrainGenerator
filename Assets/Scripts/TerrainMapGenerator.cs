using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMapGenerator : MonoBehaviour {

    public Renderer textureRenderer;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public int mapOffsetX;
    public int mapOffsetY;

    void Start() {
        float[,] noiseMap = GenerateNoiseMap(mapWidth, mapHeight, noiseScale, mapOffsetX, mapOffsetY);
        DrawHeightMap(noiseMap);
    }

    public float[,] GenerateNoiseMap(int width, int height, float scale, int offsetX, int offsetY) {
        float[,] noiseMap = new float[width, height];

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float sampleX = (float)x / scale + offsetX;
                float sampleY = (float)y / scale + offsetY;

                noiseMap[x, y] = Mathf.PerlinNoise(sampleX, sampleY);
            }   
        }

        return noiseMap;
    }

    
    public void DrawHeightMap(float[,] noiseMap) {
        Texture2D texture = new Texture2D(mapWidth, mapHeight);

        Color[] colorMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                colorMap[y*mapWidth + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }

        texture.SetPixels(colorMap);
        texture.Apply();
    
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(mapWidth, 1, mapHeight);
    }
}
