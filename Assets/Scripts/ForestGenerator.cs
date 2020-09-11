using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGenerator : MonoBehaviour {

    public GameObject treePrefab;

    public int mapWidth;
    public int mapHeight;
    public int numTrees;

    public void Generate(float[,] heightMap) {
        for (int i = 0; i < numTrees; i++) {
            float x = Random.Range(0f, mapWidth);
            float z = Random.Range(0f, mapHeight);
            float y = heightMap[(int)x, (int)z] - 1;
            Debug.Log(x + " " + y + " " + z);

            Vector3 position = new Vector3(x, y, z);
            GameObject tree = Instantiate(treePrefab, position, Quaternion.identity);

            float scale = Random.Range(1.5f, 2.5f);
            tree.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
