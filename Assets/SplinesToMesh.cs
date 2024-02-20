using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SplinesToMesh : MonoBehaviour
{

    public bool flippedNormals = false;
    public CreateBezier spline1;
    public CreateBezier spline2;

    MeshFilter meshFilter;
    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        GenerateMeshBetweenSplines();
    }
    private void Update()
    {
        GenerateMeshBetweenSplines();
    }

    // Update is called once per frame
    void GenerateMeshBetweenSplines()
    {
        // Get points from both splines
        List<Vector3> spline1Points = spline1.GetSplinePoints();
        List<Vector3> spline2Points = spline2.GetSplinePoints();

        // Ensure both splines have at least 2 points
        if (spline1Points.Count < 2 || spline2Points.Count < 2)
        {
            Debug.LogError("Both splines must have at least 2 points.");
            return;
        }
        List<Vector3> vertices = InterpolatePointsAlongSecondSpline(spline1Points, spline2Points);
        List<int> triangles = GenerateTriangles(spline1Points, spline2Points);


        // Generate mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

    List<Vector3> InterpolatePointsAlongSecondSpline(List<Vector3> spline1Points, List<Vector3> spline2Points)
    {
        List<Vector3> points = new List<Vector3>();

        List<Vector3> tangents = CalculateTangentLines(spline2Points);
        Vector3 originalTangent = tangents[0];
       // print(originalTangent);

        for (int i=0; i<spline2Points.Count;i++)
        {
            Vector3 currentTangent = tangents[i];

            foreach (Vector3 spline1Point in spline1Points)
            {
                // Calculate rotation angle difference between current and original tangent


                Quaternion rotation = Quaternion.FromToRotation(originalTangent, currentTangent);
                // Combine the rotated point with the current point on spline2
                Vector3 newPoint = spline1Point + spline2Points[i]; //in line with current point
                Vector3 newPoint2 = newPoint - spline2Points[i]; //in line with current point
                newPoint2 = rotation * newPoint2;
                Vector3 rotatedPoint = newPoint2 + spline2Points[i];

                points.Add(rotatedPoint);
            }
        }

        return points;
    }
   
    List<int> GenerateTriangles(List<Vector3> spline1Points, List<Vector3> spline2Points)
    {
        List<int> triangles = new List<int>();

        int numPointsSpline1 = spline1Points.Count;
        int numPointsSpline2 = spline2Points.Count;

        for (int x=0;x<numPointsSpline2-1;x++)
        {
            for (int y = 0; y < numPointsSpline1 - 1; y++)
            {
                int currentVertexPos = (x * numPointsSpline1) + y;
                int aboveVertexPos = (x * numPointsSpline1) + y+1;
                int adjacentVertexPos = ((x+1) * numPointsSpline1) + y;
                int diagonalVertexPos = ((x+1) * numPointsSpline1) + y+1;

                if (!flippedNormals)
                { //winding one way add 2 triangles
                    triangles.Add(currentVertexPos);
                    triangles.Add(aboveVertexPos);
                    triangles.Add(adjacentVertexPos);

                    triangles.Add(adjacentVertexPos);
                    triangles.Add(aboveVertexPos);
                    triangles.Add(diagonalVertexPos);
                }
                else
                { //winding the other way
                    triangles.Add(currentVertexPos);
                    triangles.Add(adjacentVertexPos);
                    triangles.Add(aboveVertexPos);

                    triangles.Add(adjacentVertexPos);
                    triangles.Add(diagonalVertexPos);
                    triangles.Add(aboveVertexPos);
                }
                   
            }
        }
        

        return triangles;
    }
    private List<Vector3> CalculateTangentLines(List<Vector3> line)
    {
        List<Vector3> tangentLines = new List<Vector3>();

        for (int i = 0; i < line.Count; i++)
        {
            Vector3 tangent;

            // Handle the first and last points
            if (i == 0)
            {
                tangent = (line[i + 1] - line[i]).normalized;
            }
            else if (i == line.Count - 1)
            {
                tangent = (line[i] - line[i - 1]).normalized;
            }
            else
            {
                // Use central difference approximation
                Vector3 prevPoint = line[i - 1];
                Vector3 nextPoint = line[i + 1];
                tangent = ((nextPoint - line[i]).normalized + (line[i] - prevPoint).normalized) / 2f;
            }

            tangentLines.Add(tangent);
        }
        return tangentLines;
    }
}
