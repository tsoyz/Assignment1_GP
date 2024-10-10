using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f; // Damage value of the bullet
    public float maxTravelDistance = 50f; // Maximum distance the bullet can travel
    private Vector3 initialPosition; // Initial position when the bullet is spawned

    private void Start()
    {
        initialPosition = transform.position; // Store the initial position when the bullet is instantiated

        // Optionally, destroy the bullet after 5 seconds to avoid it lingering too long
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        // Check if the bullet has traveled beyond the maximum distance
        float distanceTraveled = Vector3.Distance(initialPosition, transform.position);
        if (distanceTraveled > maxTravelDistance)
        {
            Destroy(gameObject); // Destroy the bullet if it exceeds the maximum travel distance
        }
    }

}
