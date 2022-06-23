using UnityEditor;
using UnityEngine;

// Editor extension for snappping GUI anchors to corners and corners to anchors
// reference: http://answers.unity3d.com/questions/782478/unity-46-beta-anchor-snap-to-button-new-ui-system.html
public class UnityUITools : MonoBehaviour
{
    [MenuItem("Custom/Unity UI/Anchors to Corners %[")]
    private static void AnchorsToCorners()
    {
        RectTransform t = Selection.activeTransform as RectTransform;
        RectTransform pt = Selection.activeTransform.parent as RectTransform;

        if (t == null || pt == null)
        {
            return;
        }

        Vector2 newAnchorsMin = new Vector2(
            t.anchorMin.x + (t.offsetMin.x / pt.rect.width),
            t.anchorMin.y + (t.offsetMin.y / pt.rect.height));
        Vector2 newAnchorsMax = new Vector2(
            t.anchorMax.x + (t.offsetMax.x / pt.rect.width),
            t.anchorMax.y + (t.offsetMax.y / pt.rect.height));

        t.anchorMin = newAnchorsMin;
        t.anchorMax = newAnchorsMax;
        t.offsetMin = t.offsetMax = new Vector2(0, 0);
    }

    [MenuItem("Custom/Unity UI/Corners to Anchors %]")]
    private static void CornersToAnchors()
    {
        RectTransform t = Selection.activeTransform as RectTransform;

        if (t == null)
        {
            return;
        }

        t.offsetMin = t.offsetMax = new Vector2(0, 0);
    }

    [MenuItem("Custom/Unity UI/Anchors to Center %]")]
    private static void CornersToCenter()
    {
        RectTransform t = Selection.activeTransform as RectTransform;
        RectTransform pt = Selection.activeTransform.parent as RectTransform;

        if (t == null || pt == null)
        {
            return;
        }

        Vector2 previousSize = t.rect.size;
        Vector2 newAnchorsMin = new Vector2(
            t.anchorMin.x + (t.offsetMin.x / pt.rect.width) + ((t.rect.size.x / pt.rect.width) / 2f),
            t.anchorMin.y + (t.offsetMin.y / pt.rect.height) + ((t.rect.size.y / pt.rect.height) / 2f));

        t.anchorMin = newAnchorsMin;
        t.anchorMax = newAnchorsMin;
        t.offsetMin = new Vector2(-previousSize.x / 2f, -previousSize.y / 2f);
        t.offsetMax = new Vector2(previousSize.x / 2f, previousSize.y / 2f);
    }
}