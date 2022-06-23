using UnityEngine;

// Class for measuring the velocity of a game object
public class Velocimeter : MonoBehaviour
{
    [Tooltip("Draw a line for debugging the velocity")]
    public bool DebugDrawVelocity = false;

    [Tooltip("factor for smoothing the velocity measurement (1 = no smoothing, 0 = infinite smoothing)")]
    public float SmoothFactor = 1f;

    // current measured velocity
    private Vector3 currentVelocity = Vector3.zero;

    // debug line renderer
    private LineRenderer lineRenderer;

    // previous position
    private Vector3 previousPosition;

    // previous measured velocity
    private Vector3 previousVelocity = Vector3.zero;

    public Vector3 CurrentVelocity
    {
        get
        {
            return currentVelocity;
        }

        private set
        {
            currentVelocity = value;
        }
    }

    // Debug Draw
    private void DebugDraw()
    {
        Vector3 start = transform.position;
        Vector3 end = transform.position + (currentVelocity / 10f);
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.SetWidth(0.1f, 0.1f);
    }

    // Start function
    private void Start()
    {
        // init smooth factor
        if (SmoothFactor <= 0f || SmoothFactor > 1)
        {
            throw new System.Exception("SpeedMeter.SmoothFactor must be larger than 0 and smaller than or equal to 1!");
        }

        // init position tracking
        previousPosition = transform.position;

        // init line renderer
        if (DebugDrawVelocity)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.hideFlags = HideFlags.HideInInspector;
            lineRenderer.SetVertexCount(2);
        }
    }

    // Track velocity
    private void Track()
    {
        Vector3 currentPosition = transform.position;
        if (!Mathf.Approximately(Time.deltaTime, 0f))
        {
            var instantVelocity = (currentPosition - previousPosition) / Time.deltaTime;
            CurrentVelocity += (instantVelocity - previousVelocity) * SmoothFactor;
        }

        previousPosition = transform.position;
        previousVelocity = CurrentVelocity;
    }

    // Update is called once per frame
    private void Update()
    {
        Track();

        if (DebugDrawVelocity)
        {
            DebugDraw();
        }
    }
}