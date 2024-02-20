using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasButtons : MonoBehaviour
{
    public GameObject horizontalSplinePrefab; // Reference to the spline prefab
    public GameObject verticalSplinePrefab; // Reference to the spline prefab
    public GameObject meshPrefab; // Reference to the mesh prefab

    public GameObject currentMesh;
    public GameObject currentHSpline;
    public GameObject currentVSpline;
    // Start is called before the first frame update
    public void CreateNewMesh()
    {
        GameObject hSpline = Instantiate(horizontalSplinePrefab);
        GameObject vSpline = Instantiate(verticalSplinePrefab);
        GameObject meshObject = Instantiate(meshPrefab);
        currentMesh = meshObject;
        currentHSpline = hSpline;
        currentVSpline = vSpline;

        CreateBezier hBezier = hSpline.GetComponent<CreateBezier>();
        CreateBezier vBezier = vSpline.GetComponent<CreateBezier>();
        SplinesToMesh meshCreation = meshObject.GetComponent<SplinesToMesh>();

        meshCreation.spline1 = hBezier;
        meshCreation.spline2 = vBezier;
    }
    public void CompleteMesh()
    {
        // Check if currentMesh is assigned
        if (currentMesh == null)
        {
            Debug.LogWarning("Mesh GameObject not assigned.");
            return;
        }

        // Remove the SplinesToMesh script from the mesh object
        Destroy(currentMesh.GetComponent<SplinesToMesh>());

        // Check if currentHSpline is assigned and destroy it
        if (currentHSpline != null)
        {
            Destroy(currentHSpline);
        }

        // Check if currentVSpline is assigned and destroy it
        if (currentVSpline != null)
        {
            Destroy(currentVSpline);
        }
        MeshCollider meshCollider = currentMesh.GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = currentMesh.AddComponent<MeshCollider>();
        }
        meshCollider.sharedMesh = currentMesh.GetComponent<MeshFilter>().sharedMesh;
        currentMesh.AddComponent<controlPointScript>();

        // Reset currentMesh, currentHSpline, and currentVSpline references
        currentMesh = null;
        currentHSpline = null;
        currentVSpline = null;
    }
    public void flipNormals()
    {
        if (currentMesh == null) return;
        currentMesh.GetComponent<SplinesToMesh>().flippedNormals = !currentMesh.GetComponent<SplinesToMesh>().flippedNormals;
    }
    public void flipSplines()
    {
        if (currentMesh == null) return;
        SplinesToMesh tempSplineToMesh = currentMesh.GetComponent<SplinesToMesh>();
        CreateBezier tempSpline = tempSplineToMesh.spline1;
        tempSplineToMesh.spline1 = tempSplineToMesh.spline2;
        tempSplineToMesh.spline2 = tempSpline;
        
    }

    public void ChangeVisibilityHSpline()
    {
        // Check if hSpline is assigned
        if (currentHSpline == null)
        {
            Debug.LogWarning("Horizontal spline GameObject not assigned.");
            return;
        }

        // Toggle LineRenderer visibility
        LineRenderer lineRenderer = currentHSpline.GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.enabled = !lineRenderer.enabled;
        }

        // Toggle visibility of child MeshRenderers and SphereColliders
        Renderer[] meshRenderers = currentHSpline.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in meshRenderers)
        {
            renderer.enabled = !renderer.enabled;
        }

        SphereCollider[] sphereColliders = currentHSpline.GetComponentsInChildren<SphereCollider>();
        foreach (SphereCollider collider in sphereColliders)
        {
            collider.enabled = !collider.enabled;
        }
    }

    public void ChangeVisibilityVSpline()
    {
        // Check if hSpline is assigned
        if (currentVSpline == null)
        {
            Debug.LogWarning("Vertical spline GameObject not assigned.");
            return;
        }

        // Toggle LineRenderer visibility
        LineRenderer lineRenderer = currentVSpline.GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.enabled = !lineRenderer.enabled;
        }

        // Toggle visibility of child MeshRenderers and SphereColliders
        Renderer[] meshRenderers = currentVSpline.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in meshRenderers)
        {
            renderer.enabled = !renderer.enabled;
        }

        SphereCollider[] sphereColliders = currentVSpline.GetComponentsInChildren<SphereCollider>();
        foreach (SphereCollider collider in sphereColliders)
        {
            collider.enabled = !collider.enabled;
        }
    }

    public void HAddControlPointsToBezier()
    {
        // Get the CreateBezier script attached to hSpline
        CreateBezier createBezier = currentHSpline.GetComponent<CreateBezier>();
        createBezier.AddBezierPiece();
    }
    public void VAddControlPointsToBezier()
    {
        // Get the CreateBezier script attached to hSpline
        CreateBezier createBezier = currentVSpline.GetComponent<CreateBezier>();
        createBezier.AddBezierPiece();
    }
}
