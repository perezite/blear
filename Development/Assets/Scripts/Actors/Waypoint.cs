using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Tooltip("Show gizmo at runtime")]
    public bool ShowGizmoAtRuntime = false;

    // Use this for initialization
    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = ShowGizmoAtRuntime;
    }
}
