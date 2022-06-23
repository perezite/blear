using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// patrols a list of waypoints, where the first waypoint to patrol can be chosen
public class Patroller : MonoBehaviour
{
    [Tooltip("Speed")]
    public float Speed = 1f;

    [Tooltip("Ordered list of waypoints")]
    public List<Waypoint> Waypoints = new List<Waypoint>();

    [Tooltip("First waypoint to visit")]
    public Waypoint FirstWaypoint;

    // indexof current waypoint 
    private int currentWaypointIndex;

    private void Start()
    {
        currentWaypointIndex = Waypoints.IndexOf(FirstWaypoint);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Waypoints.Any())
        {
            var targetPosition = Waypoints[currentWaypointIndex].transform.position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.fixedDeltaTime * Speed);

            if (Mathf.Approximately(Vector2.Distance(targetPosition, transform.position), 0))
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % Waypoints.Count;
            }
        }
    }
}
