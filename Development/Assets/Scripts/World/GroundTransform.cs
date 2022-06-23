using System;
using UnityEngine;

// Extension for the Transform component
// hooks the transform component of the ground object onto the bottom of the camera viewport, making the ground responsive
public class GroundTransform : MonoBehaviour
{
    [Tooltip("The camera to hook the ground object on to")]
    public ResponsiveCamera Camera;

    [Tooltip("Offset of the transform relative to the bottom of the camera")]
    public float OffsetToCameraBottom;

    // The camera with the events
    private Camera targetCamera;

    // The extended transform
    private Transform targetTransform;

    private void OnCameraFitCompleted()
    {
        AdjustPositionToCamera();
    }

    private void AdjustPositionToCamera()
    {
        Vector3 cameraBottomLeft = targetCamera.ViewportToWorldPoint(new Vector3(0, 0, targetCamera.nearClipPlane));
        float cameraBottom = cameraBottomLeft.y;
        targetTransform.position = new Vector3(targetTransform.position.x, cameraBottom + OffsetToCameraBottom, targetTransform.position.z);
    }

    // Use this for initialization
    private void Start()
    {
        // get objects
        targetTransform = gameObject.GetComponent<Transform>();
        targetCamera = Camera.gameObject.GetComponent<Camera>();

        // register events
        Camera.FitCompleted += OnCameraFitCompleted;

        // adjust the position
        AdjustPositionToCamera();
    }
}