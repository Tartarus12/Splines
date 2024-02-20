using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CreateBezier : MonoBehaviour
{
    public GameObject controlPointPrefab; // Reference to the control point prefab
    public List<Vector3> controlPoints = new List<Vector3>();
    public int segments; // Number of segments to render

    private List<GameObject> controlPointObjects = new List<GameObject>();
    public LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        GenerateBezierSpline();
        CreateControlPointObjects();
    }

    //Generate the Bezier spline curve
    private void GenerateBezierSpline()
    {
        List<Vector3> splinePoints = new List<Vector3>();
        segments = controlPoints.Count * 2;

        for (int i = 0; i + 3 < controlPoints.Count; i += 3)
        {
            for (int j = 0; j <= segments; j++)
            {
                float t = (float)j / segments;
                Vector3 point = CubicBezier(controlPoints[i], controlPoints[i + 1], controlPoints[i + 2], controlPoints[i + 3], t);
                splinePoints.Add(point);
            }
        }

        // Set positions for the LineRenderer
        lineRenderer.positionCount = splinePoints.Count;
        lineRenderer.SetPositions(splinePoints.ToArray());
    }

    // Cubic Bezier interpolation
    private Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        //i think this is right, go back and check before submit
        Vector3 p = uuu * p0; // (1-t)^3 * P0
        p += 3 * uu * t * p1; // 3(1-t)^2 * t * P1
        p += 3 * u * tt * p2; // 3(1-t) * t^2 * P2
        p += ttt * p3; // t^3 * P3

        return p;
    }

    // Create control point sphere objects
    private void CreateControlPointObjects()
    {
        //iff adding more piecewise segmehts, delete and readd the points
        foreach (var obj in controlPointObjects)
        {
            Destroy(obj);
        }
        controlPointObjects.Clear();

        foreach (Vector3 point in controlPoints)
        {
            GameObject sphere = Instantiate(controlPointPrefab, point, Quaternion.identity);
            sphere.transform.SetParent(transform); // Set the parent to keep the hierarchy clean
            controlPointObjects.Add(sphere);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for input to move control points
        for (int i = 0; i < controlPoints.Count; i++)
        {
            controlPoints[i] = controlPointObjects[i].transform.position;
        }
        //recalculate spline every update
        GenerateBezierSpline(); // Regenerate spline after control point movement
    }

    public List<Vector3> GetSplinePoints()
    {
        List<Vector3> splinePoints = new List<Vector3>();

        for (int i = 0; i + 3 < controlPoints.Count; i += 3)
        {
            for (int j = 0; j <= segments; j++)
            {
                float t = (float)j / segments;
                Vector3 point = CubicBezier(controlPoints[i], controlPoints[i + 1], controlPoints[i + 2], controlPoints[i + 3], t);
                splinePoints.Add(point);
            }
        }

        return splinePoints;
    }


    public void AddBezierPiece()
    {
        // Ensure there are at least 4 control points to create a new piece
        if (controlPoints.Count < 4)
        {
            Debug.Log("not got enough control points");
            return;
        }

        // Get the last control point index
        int lastIndex = controlPoints.Count - 1;

        // Calculate the tangent at the last point
        Vector3 tangent = controlPoints[lastIndex] - controlPoints[lastIndex - 1];
        Vector3 direction = tangent.normalized;

        // Calculate new control points based on tangent direction
        Vector3 point1 = controlPoints[lastIndex] + direction * 1f; // Adjust the distance as needed
        Vector3 point2 = controlPoints[lastIndex] + direction * 2f;
        Vector3 point3 = controlPoints[lastIndex] + direction * 3f;

        // Add the new control points
        controlPoints.Add(point1);
        controlPoints.Add(point2);
        controlPoints.Add(point3);

        // Regenerate bezier spline
        GenerateBezierSpline();
        CreateControlPointObjects();
    }
}
