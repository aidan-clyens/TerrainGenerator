using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;


public enum MeshType {
    Terrain,
    Water
}

public static class MeshGenerator {

    public static void RequestMeshData(Vector2 position, float[,] heightMap, int levelOfDetail, Action<Vector2, float[,], MeshData> callback) {
        ThreadStart threadStart = delegate {
            MeshDataThread(position, heightMap, levelOfDetail, callback);
        };

        new Thread(threadStart).Start();
    }

    static void MeshDataThread(Vector2 position, float[,] heightMap, int levelOfDetail, Action<Vector2, float[,], MeshData> callback) {
        MeshData meshData = Generate(heightMap, levelOfDetail);

        callback(position, heightMap, meshData);
    }

    static MeshData Generate(float[,] heightMap, int levelOfDetail) {
        int meshWidth = heightMap.GetLength(0);
        int meshHeight = heightMap.GetLength(1);

        int lod = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
        int verticesPerLine = (meshWidth - 1) / lod + 1;

        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);

        float minDepth = float.MaxValue;
        float maxDepth = float.MinValue;

        float topLeftX = (meshWidth - 1) / -2f;
        float topLeftZ = (meshHeight - 1) / 2f;

        int index = 0;
        for (int z = 0; z < meshHeight; z += lod) {
            for (int x = 0; x < meshWidth; x += lod) {
                // Create vertex
                meshData.vertices[index] = new Vector3(topLeftX + x, heightMap[x, z], topLeftZ - z);

                // Create triangles
                if (x < (meshWidth - 1) && z < (meshHeight - 1)) {
                    meshData.CreateTriangle(index, index + verticesPerLine + 1, index + verticesPerLine);
                    meshData.CreateTriangle(index + verticesPerLine + 1, index, index + 1);
                }

                if (heightMap[x, z] < minDepth) {
                    minDepth = heightMap[x, z];
                }
                
                if (heightMap[x, z] > maxDepth) {
                    maxDepth = heightMap[x, z];
                }

                // Set UVs
                meshData.uvs[index] = new Vector2(x / (float)meshWidth, z / (float)meshHeight);

                index++;
            }
        }

        return meshData;
    }
}


public class MeshDataThreadInfo {
    public Vector2 position;
    public float[,] heightMap;
    public MeshData meshData;
    public MeshType type;

    public MeshDataThreadInfo(Vector2 position, float[,] heightMap, MeshData meshData, MeshType type) {
        this.position = position;
        this.heightMap = heightMap;
        this.meshData = meshData;
        this.type = type;
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

        vertices = new Vector3[width * height];
        triangles = new int[(width - 1) * (height - 1) * 6];
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
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colours;
        mesh.uv = uvs;

        mesh.RecalculateNormals();

        return mesh;
    }
}