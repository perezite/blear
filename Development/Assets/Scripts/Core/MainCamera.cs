using System;
using System.Collections;
using UnityEngine;

// the main camera
public class MainCamera : MonoBehaviour
{
    // camera resolution changed action
    public Action CameraResolutionChanged;

    // current camera resolution
    private Vector2 currentCameraResolution;

    // camera component of object
    private Camera targetCamera;

    // Use this for initialization
    private void OnEnable()
    {
        targetCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        OnCameraResolutionChanged();
        StartCoroutine(LazyUpdate());
    }

    private IEnumerator LazyUpdate()
    {
        while (true)
        {
            if (HasCameraResolutionChanged())
            {
                OnCameraResolutionChanged();
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private bool HasCameraResolutionChanged()
    {
        Vector2 newCameraResolution = new Vector2(targetCamera.pixelWidth, targetCamera.pixelHeight);
        return currentCameraResolution != newCameraResolution;
    }

    private void OnCameraResolutionChanged()
    {
        currentCameraResolution = new Vector2(targetCamera.pixelWidth, targetCamera.pixelHeight);

        if (CameraResolutionChanged != null)
        {
            CameraResolutionChanged();
        }
    }
}