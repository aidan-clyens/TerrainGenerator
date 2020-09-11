using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

    int meshWidth;
    int meshHeight;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public void Generate(int width, int height) {
        meshWidth = width;
        meshHeight = height;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    
        CreateVertices();
        CreateTriangles();

        UpdateMesh();
    }

    public void SetHeights(float[,] heightMap) {
        int index = 0;
        for (int x = 0; x <= meshWidth; x++) {
            for (int z = 0; z <= meshHeight; z++) {
                vertices[index].y = heightMap[x, z];
                index++;
            }
        }

        UpdateMesh();
    }

    void CreateVertices() {
        vertices = new Vector3[(meshWidth + 1) * (meshHeight + 1)];

        int index = 0;
        for (int z = 0; z <= meshHeight; z++) {
            for (int x = 0; x <= meshWidth; x++) {
                vertices[index] = new Vector3(x, 0, z);
                index++;
            }
        }
    }

    void CreateTriangles() {
        triangles = new int[meshWidth * meshHeight * 6];

        int index = 0;
        int vertex = 0;
        for (int z = 0; z < meshHeight; z++) {
            for (int x = 0; x < meshWidth; x++) {
                triangles[index] = vertex;
                triangles[index + 1] = vertex + meshWidth + 1;
                triangles[index + 2] = vertex + 1;

                triangles[index + 3] = vertex + 1;
                triangles[index + 4] = vertex + meshWidth + 1;
                triangles[index + 5] = vertex + meshWidth + 2;

                index += 6;
                vertex++;
            }

            vertex++;
        }
    }

    void UpdateMesh() {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
