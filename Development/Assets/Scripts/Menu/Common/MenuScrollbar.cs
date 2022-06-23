using System;

using UnityEngine;
using UnityEngine.UI;

// extension for UI-scrollbar
public class MenuScrollbar : MonoBehaviour
{
    [Tooltip("The initial scrollbar position. The scrollbar will be forced to have this value upon camera resolution changed")]
    [Range(0f, 1f)]
    public float InitialScrollbarPosition = 1f;

    [Tooltip("target camera events")]
    public MainCamera TargetCameraEvents;

    // the extended scrollbar
    private Scrollbar scrollbar;

    // on camera changed event handler
    public void OnCameraResolutionChanged()
    {
        // the scrollbar behaves strangely when the resolution changes,
        // so we just reset it to its initial value in this case
        scrollbar.value = InitialScrollbarPosition;
    }

    // Use this for initialization
    private void Start()
    {
        // get extended component
        scrollbar = GetComponent<Scrollbar>();

        // register camera event
        TargetCameraEvents.CameraResolutionChanged += OnCameraResolutionChanged;
    }
}