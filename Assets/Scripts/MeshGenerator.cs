using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator {

    public static MeshData Generate(float[,] heightMap, Gradient gradient) {
        int meshWidth = heightMap.GetLength(0);
        int meshHeight = heightMap.GetLength(1);

        MeshData meshData = new MeshData(meshWidth, meshHeight);

        float minDepth = float.MaxValue;
        float maxDepth = float.MinValue;

        float topLeftX = (meshWidth - 1) / -2f;
        float topLeftZ = (meshHeight - 1) / 2f;

        int index = 0;
        for (int z = 0; z < meshHeight; z++) {
            for (int x = 0; x < meshWidth; x++) {
                // Create vertex
                meshData.vertices[index] = new Vector3(topLeftX + x, heightMap[x, z], topLeftZ - z);

                // Create triangles
                if (x < (meshWidth - 1) && z < (meshHeight - 1)) {
                    meshData.CreateTriangle(index, index + meshWidth + 1, index + meshWidth);
                    meshData.CreateTriangle(index + meshWidth + 1, index, index + 1);
                }

                if (heightMap[x, z] < minDepth) {
                    minDepth = heightMap[x, z];
                }
                
                if (heightMap[x, z] > maxDepth) {
                    maxDepth = heightMap[x, z];
                }

                // Set vertex colour
                float y = Mathf.InverseLerp(minDepth, maxDepth, meshData.vertices[index].y);
                meshData.colours[index] = gradient.Evaluate(y);

                // Set UVs
                meshData.uvs[index] = new Vector2(x / (float)meshWidth, z / (float)meshHeight);

                index++;
            }
        }

        return meshData;
    }
}


public class MeshData {

    public Vector3[] vertices;
    public int[] triangles;
    public Color[] colours;
    public Vector2[] uvs;

    int meshWidth;
    int meshHeight;

    int triangleIndex = 0;

    public MeshData(int width, int height) {
        meshWidth = width;
        meshHeight = height;

        vertices = new Vector3[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        colours = new Color[vertices.Length];
        uvs = new Vector2[vertices.Length];
    }

    public void CreateTriangle(int a, int b, int c) {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;

        triangleIndex += 3;
    }

    public Mesh CreateMesh() {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colours;
        mesh.uv = uvs;

        mesh.RecalculateNormals();

        return mesh;
    }
}