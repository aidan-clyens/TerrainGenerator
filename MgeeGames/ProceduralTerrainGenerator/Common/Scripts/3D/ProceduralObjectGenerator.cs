using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProceduralObjectGeneratorSettings {
    public List<GameObject> prefabs = new List<GameObject>();
    public float density;
    public float slopeThreshold;
    public float verticalOffset;
}

public class ProceduralObjectGenerator : MonoBehaviour {
    [HideInInspector]
    public ProceduralObjectGeneratorSettings settings;

    List<GameObject> objects = new List<GameObject>();

    System.Random rng;

    GameObject viewer;
    float viewRange;


    public void Init(GameObject view, float range) {
        viewer = view;
        viewRange = range;
    }

    public void Init(List<GameObject> currentobjects, GameObject view, float range) {
        objects = currentobjects;
        viewer = view;
        viewRange = range;
    }

    public void Update() {
        if (viewer == null) return;

        Vector2 position = new Vector2(viewer.transform.position.x, viewer.transform.position.z);

        foreach (GameObject obj in objects) {
            Vector2 objPosition = new Vector2(obj.transform.position.x, obj.transform.position.z);

            if ((position - objPosition).magnitude < viewRange) {
                obj.SetActive(true);
            }
            else {
                obj.SetActive(false);
            }
        }
    }

    public GameObject Generate(float[,] heightMap, Vector3[] normals, bool useWater, float waterLevel, int seed) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        if (!useWater)
            waterLevel = float.MinValue;

        // Get area of land above water level (number of vertices)
        int areaAboveWater = 0;
        for (int z = 0; z < height; z++) {
            for (int x = 0; x < width; x++) {
                if (heightMap[z, x] > waterLevel) {
                    areaAboveWater++;
                }
            }
        }
        
        // Calculate number of objects based on area and density
        int numobjects = (int)((areaAboveWater / 100f) * settings.density);

        rng = new System.Random(seed);

        GameObject parentGameObject = new GameObject("ProceduralObjects");

        if (settings.prefabs.Count == 0) {
            return parentGameObject; 
        }

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        while (objects.Count < numobjects) {
            int x = rng.Next(0, width - 1);
            int z = rng.Next(0, height - 1);
            float y = heightMap[x, z] + settings.verticalOffset;

            Vector3 normal = normals[(int)(z*width + x)];
            float angle = Vector3.Angle(normal, new Vector3(0, 1, 0));

            x = (int)topLeftX + x;
            z = (int)topLeftZ - z;

            if (y > waterLevel + 5 && angle < settings.slopeThreshold) {
                Vector3 position = new Vector3(x, y, z);
                GameObject prefab = settings.prefabs[rng.Next(0, settings.prefabs.Count)];
                GameObject obj = Instantiate(prefab, position, Quaternion.identity, parentGameObject.transform);

                float scale = (float)rng.NextDouble() + 1f;
                obj.transform.localScale = new Vector3(scale, scale, scale);

                obj.isStatic = true;

                objects.Add(obj);
            }
        }

        return parentGameObject;
    }

    public void Clear() {
        objects.Clear();
    }
}
