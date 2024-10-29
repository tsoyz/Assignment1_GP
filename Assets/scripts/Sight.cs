using UnityEngine;

public class Sight : MonoBehaviour
{
    public float distance = 10f;         // Distance the enemy can see
    public float angle = 45f;            // Field of view angle
    public LayerMask objectsLayers;      // Layer of objects to detect (e.g., Player)
    public LayerMask obstaclesLayers;    // Layer of obstacles that can block vision

    public Collider detectedObject;      // Detected object reference

    private void Update()
    {
        // Detect colliders within the vision range
        Collider[] colliders = Physics.OverlapSphere(transform.position, distance, objectsLayers);
        detectedObject = null;

        foreach (Collider collider in colliders)
        {
            Vector3 directionToTarget = (collider.bounds.center - transform.position).normalized;

            // Check if the collider is within the field of view angle
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                // Check if there's a clear line of sight to the target
                if (!Physics.Linecast(transform.position, collider.bounds.center, obstaclesLayers))
                {
                    detectedObject = collider; // Set detected object
                    break;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distance);

        Vector3 rightDirection = Quaternion.Euler(0, angle / 2, 0) * transform.forward * distance;
        Vector3 leftDirection = Quaternion.Euler(0, -angle / 2, 0) * transform.forward * distance;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, rightDirection);
        Gizmos.DrawRay(transform.position, leftDirection);
    }
}
