using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ForestGeneratorSettings {
    public List<GameObject> treePrefabs = new List<GameObject>();
    public float density;
    public float slopeThreshold;
    public float verticalOffset;
}

public class ForestGenerator : MonoBehaviour {
    [HideInInspector]
    public ForestGeneratorSettings settings;

    List<GameObject> trees = new List<GameObject>();

    System.Random rng;

    GameObject viewer;
    float viewRange;


    public void Init(GameObject view, float range) {
        viewer = view;
        viewRange = range;
    }

    public void Init(List<GameObject> currentTrees, GameObject view, float range) {
        trees = currentTrees;
        viewer = view;
        viewRange = range;
    }

    public void Update() {
        if (viewer == null) return;

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

    public GameObject Generate(float[,] heightMap, Vector3[] normals, float waterLevel, int seed) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        // Get area of land above water level (number of vertices)
        int areaAboveWater = 0;
        for (int z = 0; z < height; z++) {
            for (int x = 0; x < width; x++) {
                if (heightMap[z, x] > waterLevel) {
                    areaAboveWater++;
                }
            }
        }
        
        // Calculate number of trees based on area and density
        int numTrees = (int)((areaAboveWater / 100f) * settings.density);

        rng = new System.Random(seed);

        GameObject forestGameObject = new GameObject("Forest");

        if (settings.treePrefabs.Count == 0) {
            return forestGameObject; 
        }

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        while (trees.Count < numTrees) {
            int x = rng.Next(0, width - 1);
            int z = rng.Next(0, height - 1);
            float y = heightMap[x, z] + settings.verticalOffset;

            Vector3 normal = normals[(int)(z*width + x)];
            float angle = Vector3.Angle(normal, new Vector3(0, 1, 0));

            x = (int)topLeftX + x;
            z = (int)topLeftZ - z;

            if (y > waterLevel + 5 && angle < settings.slopeThreshold) {
                Vector3 position = new Vector3(x, y, z);
                GameObject treePrefab = settings.treePrefabs[rng.Next(0, settings.treePrefabs.Count)];
                GameObject tree = Instantiate(treePrefab, position, Quaternion.identity, forestGameObject.transform);

                float scale = (float)rng.NextDouble() + 1f;
                tree.transform.localScale = new Vector3(scale, scale, scale);

                tree.isStatic = true;

                trees.Add(tree);
            }
        }

        return forestGameObject;
    }

    public void Clear() {
        trees.Clear();
    }
}
