using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class GridMeshGenerator : MonoBehaviour
{
    public int xVertices = 10; // Number of vertices along the x-axis
    public int yVertices = 10; // Number of vertices along the y-axis
    public float heightScale = 1f; // Scale factor for height

    // Customize noise parameters here
    public float noiseScale = 0.1f;
    public float offsetX = 0f;
    public float offsetY = 0f;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        GenerateMesh();
    }

    void GenerateMesh()
    {
        Mesh mesh = new Mesh();

        // Vertices
        Vector3[] vertices = new Vector3[(xVertices + 1) * (yVertices + 1)];
        for (int y = 0, index = 0; y <= yVertices; y++)
        {
            for (int x = 0; x <= xVertices; x++)
            {
                // Generate height using Perlin noise
                float height = Mathf.PerlinNoise((x + offsetX) * noiseScale, (y + offsetY) * noiseScale) * heightScale;
                vertices[index++] = new Vector3(x, height, y);
            }
        }

        // Triangles
        int[] triangles = new int[xVertices * yVertices * 6];
        for (int ti = 0, vi = 0, y = 0; y < yVertices; y++, vi++)
        {
            for (int x = 0; x < xVertices; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 1] = vi + xVertices + 1;
                triangles[ti + 2] = vi + 1;
                triangles[ti + 3] = vi + 1;
                triangles[ti + 4] = vi + xVertices + 1;
                triangles[ti + 5] = vi + xVertices + 2;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        meshFilter.mesh = mesh;

    }
}
