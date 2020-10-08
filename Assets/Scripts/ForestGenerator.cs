using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGenerator : MonoBehaviour {

    public GameObject treePrefab;

    public int numTrees;

    ArrayList trees = new ArrayList();
    GameObject forestGameObject;

    System.Random rng;

    public void Generate(float[,] heightMap, float waterLevel, int seed) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        rng = new System.Random(seed);

        forestGameObject = new GameObject("Forest");

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        for (int i = 0; i < numTrees; i++) {
            int x = rng.Next(0, width);
            int z = rng.Next(0, height);
            float y = heightMap[x, z] - 1;

            x = (int)topLeftX + x;
            z = (int)topLeftZ - z;

            if (y > waterLevel) {
                Vector3 position = new Vector3(x, y, z);
                GameObject tree = Instantiate(treePrefab, position, Quaternion.identity, forestGameObject.transform);

                float scale = (float)rng.NextDouble() + 1f;
                tree.transform.localScale = new Vector3(scale, scale, scale);
            
                trees.Add(tree);
            }
        }
    }

    public void Clear() {
        // foreach (GameObject tree in trees) {
        //     DestroyImmediate(tree, true);
        // }
        DestroyImmediate(forestGameObject, true);

        trees.Clear();
    }
}
