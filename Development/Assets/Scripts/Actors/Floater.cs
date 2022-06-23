using System.Linq;
using UnityEngine;

// behaviour which makes the transform move around 
public class Floater : MonoBehaviour
{
    [Tooltip("Movement speed")]
    public float MovementSpeed = 1f;

    [Tooltip("Top margin for movement boundaries")]
    public float TopBoundaryMargin = 0f;

    [Tooltip("Bottom margin for movement boundaries")]
    public float BottomBoundaryMargin = 0f;

    // time at last target position update
    private float startTime;

    // position at last target position update
    private Vector2 startPosition;

    // target position
    private Vector2 targetPosition;
    
    // collider
    private Collider2D coll;

    // movement field 
    private WorldSpaceRect unrestrictedBoundaries = new WorldSpaceRect(-5f, -3f, 10f, 9f);

    // movement field minus margins
    private WorldSpaceRect boundaries;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
        var bounds = coll.bounds;
        boundaries = new WorldSpaceRect(
            unrestrictedBoundaries.Left + bounds.extents.x, 
            unrestrictedBoundaries.Bottom + bounds.extents.y + BottomBoundaryMargin,
            unrestrictedBoundaries.Width - bounds.size.x, 
            unrestrictedBoundaries.Height - bounds.size.y - TopBoundaryMargin - BottomBoundaryMargin);
        UpdateTargetPosition();
    }

    private void Update()
    {
        // float to target
        var totalTime = (targetPosition - startPosition).magnitude / MovementSpeed;
        var t = (Time.time - startTime) / totalTime;
        transform.position = VectorHelper.SmoothStep(startPosition, targetPosition, t);

        // update target if reached
        var distance = Vector2.Distance(transform.position, targetPosition);
        if (Mathf.Approximately(distance, 0))
        {
            UpdateTargetPosition();
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        // reflect movement direction
        var avgNormal = coll.contacts.Select(x => x.normal).ToList().Sum() / coll.contacts.Length;
        var currentDir = targetPosition - (Vector2)transform.position;
        var newDir = Vector2.Reflect(currentDir, avgNormal);
        UpdateTargetPosition(newDir);
    }

    private void UpdateTargetPosition()
    {
        startTime = Time.time;
        startPosition = transform.position;
        targetPosition = new Vector2(
            Random.Range(boundaries.Left, boundaries.Right), 
            Random.Range(boundaries.Bottom, boundaries.Top));
    }

    // update target position with fixed movement direction
    private void UpdateTargetPosition(Vector2 newDir)
    {
        // compute distance to nearest boundary
        var distances = new float[4];
        distances[0] = Mathf.Abs(boundaries.Left - transform.position.x);
        distances[1] = Mathf.Abs(boundaries.Right - transform.position.x);
        distances[2] = Mathf.Abs(boundaries.Top - transform.position.y);
        distances[3] = Mathf.Abs(boundaries.Bottom - transform.position.y);
        var distance = distances.Min();

        // choose the new target position along the given direction clamped to the length of the minimal distance to the boundaries
        startTime = Time.time;
        startPosition = transform.position;
        var newDirScaled = newDir.normalized * distance;
        targetPosition = (Vector2)transform.position + newDirScaled;
    }
}