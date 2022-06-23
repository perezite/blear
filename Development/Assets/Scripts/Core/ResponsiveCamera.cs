using System;
using System.Collections.Generic;
using UnityEngine;

// Component extension for Camera:
// Resizes the camera, such that sprites rendered by ResponsiveSpriteRenderer are visible at their full height
public class ResponsiveCamera : MonoBehaviour
{
    [Tooltip("Design time aspect ratio")]
    public float DesignTimeAspectRatio;

    [Tooltip("List of native portrait aspect ratios. All other ratios are scaled to the next matching native ratio using letterboxing")]
    public List<float> NativeAspectRatios = new List<float>();

    // camera fit completed action
    public Action FitCompleted;

    // initial camera size
    private float initialOrthographicSize;

    // the camera component
    private Camera targetCamera;

    private void AddLetterbox(float targetAspectRatio)
    {
        // ensure that the pixels which are going to be outside the viewport are properly cleared
        GL.Clear(true, true, Color.black);

        float viewportHeight = targetAspectRatio / CameraHelper.GetScreenPortraitAspectRatio();
        float viewportVerticalPosition = (1f - viewportHeight) / 2f;
        targetCamera.SetViewport(viewportHeight, viewportVerticalPosition);
    }

    private float GetTargetAspectRatio()
    {
        float screenPortraitAspectRatio = CameraHelper.GetScreenPortraitAspectRatio();
        int indexOfBestMatchingRatio = CameraHelper.GetIndexOfNextWiderPortraitAspectRatio(NativeAspectRatios, screenPortraitAspectRatio);
        float targetAspectRatio = NativeAspectRatios[indexOfBestMatchingRatio];

        return targetAspectRatio;
    }

    private void OnCameraResolutionChanged()
    {
        float targetAspectRatio = GetTargetAspectRatio();
        ResizeToFitHeight(targetAspectRatio);
        AddLetterbox(targetAspectRatio);
        if (FitCompleted != null)
        {
            FitCompleted();
        }
    }

    // adjust ortho size to match camera width to target aspect ratio
    private void ResizeToFitHeight(float targetAspectRatio)
    {
        float orthoSizeFactorToFitHeight = targetAspectRatio / DesignTimeAspectRatio;
        targetCamera.orthographicSize = orthoSizeFactorToFitHeight * initialOrthographicSize;
    }

    // Use this for initialization
    private void Start()
    {
        // assert
        if (DesignTimeAspectRatio <= 0f)
        {
            throw new Exception("You must specify a valid design time aspect ratio");
        }

        // setup camera
        targetCamera = gameObject.GetComponent<Camera>();
        initialOrthographicSize = targetCamera.orthographicSize;
        targetCamera.GetComponent<MainCamera>().CameraResolutionChanged += OnCameraResolutionChanged;
    }
}