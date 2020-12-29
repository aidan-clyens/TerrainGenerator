using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGenerator : MonoBehaviour {

    public List<GameObject> treePrefabs = new List<GameObject>();
    public int numTrees;
    public float slopeThreshold;
    public float verticalOffset;

    List<GameObject> trees = new List<GameObject>();

    System.Random rng;

    GameObject viewer;
    float viewRange;


    public void Init(GameObject view, float range) {
        viewer = view;
        viewRange = range;
    }

    public void Update() {
        Vector2 position = new Vector2(viewer.transform.position.x, viewer.transform.position.z);

        foreach (GameObject tree in trees) {
            Vector2 treePosition = new Vector2(tree.transform.position.x, tree.transform.position.z);

            if ((position - treePosition).magnitude < viewRange) {
                tree.SetActive(true);
            }
            else {
                tree.SetActive(false);
            }
        }
    }

    public GameObject Generate(float[,] heightMap, Vector3[] normals, float waterLevel, int seed, bool loadAllObjects) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        rng = new System.Random(seed);

        GameObject forestGameObject = new GameObject("Forest");

        if (treePrefabs.Count == 0) {
            return forestGameObject; 
        }

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        while (trees.Count < numTrees) {
            int x = rng.Next(0, width - 1);
            int z = rng.Next(0, height - 1);
            float y = heightMap[x, z] + verticalOffset;

            Vector3 normal = normals[(int)(z*width + x)];
            float angle = Vector3.Angle(normal, new Vector3(0, 1, 0));

            x = (int)topLeftX + x;
            z = (int)topLeftZ - z;

            if (y > waterLevel + 5 && angle < slopeThreshold) {
                Vector3 position = new Vector3(x, y, z);
                GameObject treePrefab = treePrefabs[rng.Next(0, treePrefabs.Count)];
                GameObject tree = Instantiate(treePrefab, position, Quaternion.identity, forestGameObject.transform);

                float scale = (float)rng.NextDouble() + 1f;
                tree.transform.localScale = new Vector3(scale, scale, scale);

                tree.isStatic = true;
                tree.SetActive(loadAllObjects);

                trees.Add(tree);
            }
        }

        return forestGameObject;
    }

    public void Clear() {
        trees.Clear();
    }
}
