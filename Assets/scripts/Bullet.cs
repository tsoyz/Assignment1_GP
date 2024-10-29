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

    // Handle collision with enemies
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hits an enemy
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null && !enemy.isDestroyed) // Add isDestroyed check
        {
            // Apply damage to the enemy
            enemy.TakeDamage(damage);
        }

        // Destroy the bullet on impact
        Destroy(gameObject);
    }
}