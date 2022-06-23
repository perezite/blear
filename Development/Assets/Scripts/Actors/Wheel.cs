using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [Tooltip("Angular velocity in degrees per second")]
    public float Omega;

    [Tooltip("List of Waypoints")]
    public List<Transform> Waypoints;

    [Tooltip("Linear speed")]
    public float Speed;

    // collider
    private Collider2D wheelCollider;

    // current target waypoint
    private Transform currentTargetWaypoint;

    // visit waypoints forward
    private bool visitWaypointsForward = true;

    private void Start()
    {
        wheelCollider = GetComponent<Collider2D>();

        // target closest waypoint
        if (Waypoints != null)
        {
            var itemsByDistance = Waypoints.OrderBy(w => Vector2.Distance(w.transform.position, transform.position));
            currentTargetWaypoint = itemsByDistance.FirstOrDefault();
        }
    }

    private void Update()
    {
        // rotate
        transform.Rotate(0f, 0f, Omega * Time.deltaTime);

        if (currentTargetWaypoint)
        {
            // move to target waypoint
            float step = Speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, currentTargetWaypoint.transform.position, step);

            // proceed to next waypoint if needed
            ProceedToNextWapoint();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Ball")
        {
            var ballCollider = other.transform.GetComponent<Collider2D>();
            var ballPhysics = other.transform.GetComponent<BallPhysics>();

            // apply tangential deflection to ball
            if (ballCollider)
            {
                var outward = ballCollider.bounds.center - wheelCollider.bounds.center;
                var tangential = Quaternion.AngleAxis(90 * Mathf.Sign(Omega), Vector3.forward) * outward;
                ballPhysics.Velocity = tangential.normalized * ballPhysics.Velocity.magnitude;
            }
        }
    }

    private void ProceedToNextWapoint()
    {
        var distance = Vector2.Distance(transform.position, currentTargetWaypoint.transform.position);
        if (Mathf.Approximately(distance, 0f))
        {
            var currentWaypointIndex = Waypoints.IndexOf(currentTargetWaypoint);

            // switch dirction
            if (currentWaypointIndex == 0)
            {
                visitWaypointsForward = true;
            }

            if (currentWaypointIndex == Waypoints.Count - 1)
            {
                visitWaypointsForward = false;
            }

            var nextWaypointIndex = visitWaypointsForward ? (currentWaypointIndex + 1) : (currentWaypointIndex - 1);
            currentTargetWaypoint = Waypoints[nextWaypointIndex];
        }
    }
}
