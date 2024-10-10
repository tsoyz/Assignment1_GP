using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2.0f; // Speed of the enemy's movement
    public float changeDirectionInterval = 2.0f; // Time interval to change direction

    public Vector3 corner1 = new Vector3(88.9000015f, 50.5f, 192.119995f);
    public Vector3 corner2 = new Vector3(144.600006f, 50.5f, 192.119995f);
    public Vector3 corner3 = new Vector3(144.600006f, 50.5f, 33.6883698f);
    public Vector3 corner4 = new Vector3(88.9000015f, 50.5f, 33.6883698f);

    private Vector3 minBounds; // Minimum bounds of the movement area
    private Vector3 maxBounds; // Maximum bounds of the movement area
    private Vector3 moveDirection; // Current movement direction

    void Start()
    {
        // Calculate the movement bounds based on the 4 corners
        CalculateBounds();

        // Set a random initial movement direction
        SetRandomDirection();

        // Start the coroutine to change direction at intervals
        StartCoroutine(ChangeDirectionRoutine());
    }

    void Update()
    {
        // Move in the current direction
        MoveInCurrentDirection();

        // Check if the enemy is outside the bounds, and if so, adjust direction
        CheckBoundsAndAdjustDirection();
    }

    // Calculate the min and max bounds based on the 4 corners
    void CalculateBounds()
    {
        // Calculate the minimum and maximum bounds using the corner points
        Vector3[] corners = new Vector3[4] { corner1, corner2, corner3, corner4 };

        minBounds = corners[0];
        maxBounds = corners[0];

        foreach (Vector3 corner in corners)
        {
            minBounds = Vector3.Min(minBounds, corner);
            maxBounds = Vector3.Max(maxBounds, corner);
        }
    }

    // Move the enemy in the current direction
    void MoveInCurrentDirection()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    // Set a random movement direction
    void SetRandomDirection()
    {
        // Set a random direction in the X and Z axes
        moveDirection = new Vector3(
            Random.Range(-1f, 1f),
            0, // Keep Y as 0 for ground-based movement
            Random.Range(-1f, 1f)
        ).normalized; // Normalize to keep consistent speed
    }

    // Coroutine to change direction at regular intervals
    IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeDirectionInterval);
            SetRandomDirection(); // Change direction at each interval
        }
    }

    // Check if the enemy is outside the bounds, and if so, adjust direction
    void CheckBoundsAndAdjustDirection()
    {
        if (transform.position.x < minBounds.x || transform.position.x > maxBounds.x ||
            transform.position.z < minBounds.z || transform.position.z > maxBounds.z)
        {
            // If outside bounds, flip direction
            moveDirection = -moveDirection;
            SetPositionInsideBounds();
        }
    }

    // Ensure the enemy stays inside the bounds by repositioning them if necessary
    void SetPositionInsideBounds()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
        float clampedZ = Mathf.Clamp(transform.position.z, minBounds.z, maxBounds.z);
        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
    }

    // Draw the movement bounds in the Scene view for visualization
    private void OnDrawGizmosSelected()
    {
        // Draw the bounding box in the Scene view for visualization
        Gizmos.color = Color.cyan;

        // Draw lines between the manually set corner points
        Gizmos.DrawLine(corner1, corner2);
        Gizmos.DrawLine(corner2, corner3);
        Gizmos.DrawLine(corner3, corner4);
        Gizmos.DrawLine(corner4, corner1);

        // Calculate and draw the center and size of the bounding box
        Vector3 center = (minBounds + maxBounds) / 2;
        Vector3 size = maxBounds - minBounds;
        Gizmos.DrawWireCube(center, size);
    }
}
