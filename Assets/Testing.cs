using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Testing : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh currentMesh = new Mesh();

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        GenerateMesh();
    }

    // Update is called once per frame
    private void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[3];
        int[] tris = new int[3];

        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(2, 0, 0);
        vertices[2] = new Vector3(1, 0, 2);

        tris[0] = 0;
        tris[1] = 1;
        tris[2] = 2;

        mesh.vertices = vertices;
        mesh.triangles = tris;
        //mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        currentMesh = mesh;
    }

    private void subDivManage()
    {
        List<Vector3> newPositions = new List<Vector3>();
        List<int> newTris = new List<int>();

        for (int i = 0; i<currentMesh.triangles.Length;i+=3)
        {
            bool doSubDiv = true;
            //Logic for if subdividing triangle 
            if(doSubDiv)
            {
                int[] currentTri = new int[3] { i + 0, i + 1, i + 2 };
                Vector3[] newPos = patchSubDiv(currentTri);
            }
            else
            {
            }
        }
    }



    private Vector3[] patchSubDiv(int[] patch)
    {
        Vector3[] vertices = meshFilter.mesh.vertices;

        int idx0 = patch[0];
        int idx1 = patch[1];
        int idx2 = patch[2];

        Vector3 v0 = vertices[idx0];
        Vector3 v1 = vertices[idx1];
        Vector3 v2 = vertices[idx2];

        Vector3 m0 = (v0 + v1) * 0.5f;
        Vector3 m1 = (v1 + v2) * 0.5f;
        Vector3 m2 = (v2 + v0) * 0.5f;

        Vector3[] vertices2 = new Vector3[6] { v0, m0, m1, v1, m2, v2 };
        return vertices2;
    }
}
