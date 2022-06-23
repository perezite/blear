using System;
using System.Collections;
using UnityEngine;

// Component extension for the in game GUI -> canvas -> rect transform
// The canvas is rendered on the top of the screen and responds to aspect ratio changes
public class GUICanvasRectTransform : MonoBehaviour
{
    [Tooltip("Main camera used for responsive behaviour")]
    public MainCamera MainCamera;

    [Tooltip("The frame sprite. The GUI canvas will adapt to fit into the top part of the frame.")]
    public SpriteRenderer FrameSpriteRenderer;

    // The extended Canvas
    private Canvas canvas;

    // canvas bottom position
    private float canvasBottomPosition;

    // The extended Rect Transform
    private RectTransform rectTransform;

    // Camera resolution change event handler
    private void OnCameraResolutionChanged()
    {
        StartCoroutine(UpdateRectTransform(true));
    }

    // Use this for initialization
    private void OnEnable()
    {
        // assert
        canvas = GetComponent<Canvas>();
        if (canvas.renderMode != RenderMode.WorldSpace)
        {
            throw new Exception("Canvas.RenderMode must be set to WorldSpace!");
        }

        // setup
        rectTransform = GetComponent<RectTransform>();
        MainCamera.CameraResolutionChanged += OnCameraResolutionChanged;

        // compute
        canvasBottomPosition = rectTransform.GetWorldSpaceRect().Bottom;
    }

    private void Start()
    {
        StartCoroutine(UpdateRectTransform(false));
    }

    // Update RectTransform coroutine
    private IEnumerator UpdateRectTransform(bool delayed)
    {
        // wait until the frame sprite has updated
        if (delayed)
        {
            yield return new WaitForEndOfFrame();
        }

        // compute new dimensions
        float frameTopPosition = FrameSpriteRenderer.transform.position.y + FrameSpriteRenderer.bounds.extents.y;
        float newHeight = frameTopPosition - canvasBottomPosition;

        // set new dimensions
        rectTransform.position = new Vector2(rectTransform.position.x, canvasBottomPosition + (0.5f * newHeight));
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);
    }
}