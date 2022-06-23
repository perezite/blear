using System;
using UnityEngine;

// Component extension for SpriteRenderer:
// Chooses the correct sprite depending on the camera aspect ratio
public class ResponsiveSpriteRenderer : MonoBehaviour
{
    [Tooltip("Target camera events")]
    public MainCamera CameraEvents;

    [Tooltip("Sprites with their aspect ratio. The best matching sprite for the camera aspect ratio is chosen")]
    public ResponsiveSpriteEntry[] Sprites;

    // extended sprite renderer
    private SpriteRenderer spriteRenderer;

    private void OnCameraResolutionChanged()
    {
        // get sprite which matches camera aspect ratio the best
        float aspectRatio = CameraHelper.GetScreenPortraitAspectRatio();
        spriteRenderer.sprite = CameraHelper.GetBestMatchingSprite(Sprites, aspectRatio);
    }

    // Use this for initialization
    private void OnEnable()
    {
        // setup camera
        CameraEvents.CameraResolutionChanged += OnCameraResolutionChanged;

        // create and init extended sprite renderer
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
}