using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 5f; // Speed of the car
    private Transform[] waypoints; // The waypoints the car will follow
    private int currentWaypointIndex = 0; // Current waypoint the car is heading to

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return; // Return if there are no waypoints

        // Move towards the current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Rotate to face the direction of movement
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * speed);

        // Check if the car reached the current waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // Move to the next waypoint
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                Destroy(gameObject); // Destroy the car once it finishes the route
            }
        }
    }

    // Set the waypoints for the car to follow
    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
    }
}
