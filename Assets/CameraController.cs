using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 2f;
    public float rotateSpeed = 2f;
    public float zoomSpeed = 5f;

    private Vector3 lastPanPosition;
    private Vector3? lastRotationPosition;
    private Vector3 lastZoomPosition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            SetCameraOrthographic(new Vector3(0, 0, -10), new Vector3(0, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            SetCameraOrthographic(new Vector3(10, 0, 0), new Vector3(0, -90, 0));
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            SetCameraOrthographic(new Vector3(0, 10, 0), new Vector3(90, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            SetCameraPerspective(new Vector3(0, 0, -10), new Vector3(0, 0, 0));
        }


        if (Input.GetMouseButtonDown(2))
        {
            lastPanPosition = Input.mousePosition;
            print(lastPanPosition);
        }
        else if (Input.GetMouseButton(2))
        {
            PanCamera(Input.mousePosition);
        }

        if (Input.GetMouseButtonDown(1))
        {
            lastRotationPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            RotateCamera(Input.mousePosition);
        }

        // Zoom camera with scroll wheel
        ZoomCamera();
    }

    void SetCameraOrthographic(Vector3 position, Vector3 rotation)
    {
        Camera.main.orthographic = true;
        Camera.main.transform.position = position;
        Camera.main.transform.rotation = Quaternion.Euler(rotation);
    }

    void SetCameraPerspective(Vector3 position, Vector3 rotation)
    {
        Camera.main.orthographic = false;
        Camera.main.transform.position = position;
        Camera.main.transform.rotation = Quaternion.Euler(rotation);
    }

    void PanCamera(Vector3 newPanPosition)
    {
        // Calculate pan direction in screen space
        Vector3 offset = Camera.main.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * panSpeed, offset.y * panSpeed, 0);
        print(move);
        // Adjust move direction based on camera rotation
        move = Camera.main.transform.rotation * move;

        // Apply pan
        transform.Translate(move, Space.World);

        // Update last pan position
        lastPanPosition = newPanPosition;
    }

    void RotateCamera(Vector3 newRotationPosition)
    {
        if (lastRotationPosition != null)
        {
            // Calculate rotation delta
            Vector3 delta = newRotationPosition - lastRotationPosition.Value;
            Quaternion rotation = Quaternion.Euler(new Vector3(-delta.y * rotateSpeed, delta.x * rotateSpeed, 0f));

            // Apply rotation
            Camera.main.transform.rotation *= Quaternion.Slerp(Quaternion.identity, rotation, 0.5f);
        }

        // Update last rotation position
        lastRotationPosition = newRotationPosition;
    }

    void ZoomCamera()
    {
        // Get scroll wheel input
        float scroll = Input.mouseScrollDelta.y;

        // Calculate zoom amount
        Vector3 zoom = Camera.main.transform.forward * scroll * zoomSpeed * Time.deltaTime;

        // Apply zoom
        transform.Translate(zoom, Space.World);
    }
}
