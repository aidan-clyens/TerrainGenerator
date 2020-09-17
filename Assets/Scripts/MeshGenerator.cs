using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

    public Gradient gradient;

    int meshWidth;
    int meshHeight;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Color[] colours;

    float[,] heightMap;

    float minDepth = float.MaxValue;
    float maxDepth = float.MinValue;

    public void Generate(float[,] heights) {
        heightMap = heights;

        meshWidth = heightMap.GetLength(0);
        meshHeight = heightMap.GetLength(1);

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    
        CreateVertices();
        CreateTriangles();
        CreateColours();

        UpdateMesh();
    }

    void CreateVertices() {
        vertices = new Vector3[meshWidth * meshHeight];

        minDepth = float.MaxValue;
        maxDepth = float.MinValue;

        float topLeftX = (meshWidth - 1) / -2f;
        float topLeftZ = (meshHeight - 1) / 2f;

        int index = 0;
        for (int z = 0; z < meshHeight; z++) {
            for (int x = 0; x < meshWidth; x++) {
                vertices[index] = new Vector3(topLeftX + x, heightMap[x, z], topLeftZ - z);
                index++;

                if (heightMap[x, z] < minDepth) {
                    minDepth = heightMap[x, z];
                }

                
                if (heightMap[x, z] > maxDepth) {
                    maxDepth = heightMap[x, z];
                }
            }
        }
    }

    void CreateTriangles() {
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];

        int triangleIndex = 0;
        int vertex = 0;
        for (int z = 0; z < meshHeight - 1; z++) {
            for (int x = 0; x < meshWidth - 1; x++) {
                triangles[triangleIndex] = vertex;
                triangles[triangleIndex + 1] = vertex + meshWidth + 1;
                triangles[triangleIndex + 2] = vertex + meshWidth;

                triangles[triangleIndex + 3] = vertex + meshWidth + 1;
                triangles[triangleIndex + 4] = vertex;
                triangles[triangleIndex + 5] = vertex + 1;

                triangleIndex += 6;
                vertex++;
            }

            vertex++;
        }
    }

    void CreateColours() {
        colours = new Color[vertices.Length];

        int index = 0;
        for (int z = 0; z < meshHeight; z++) {
            for (int x = 0; x < meshWidth; x++) {
                float y = Mathf.InverseLerp(minDepth, maxDepth, vertices[index].y);
                colours[index] = gradient.Evaluate(y);
                index++;
            }
        }    
    }

    void UpdateMesh() {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colours;

        mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
