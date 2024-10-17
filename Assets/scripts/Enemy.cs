using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 50f; // Health value of the enemy
    public GameObject hitEffect; // Particle effect to play when bullet hits the enemy

    private bool isDestroyed = false; // Flag to prevent multiple destruction calls

    public delegate void EnemyDestroyedHandler(GameObject enemy);
    public event EnemyDestroyedHandler OnEnemyDestroyed;

    public void TakeDamage(float damageAmount)
    {
        if (isDestroyed) return; // Prevent taking damage if already destroyed

        health -= damageAmount; // Reduce health by the damage amount
        Debug.Log("Enemy took damage! Current health: " + health);

        // Play hit particle effect if available
        if (hitEffect != null)
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f); // Destroy the effect after 2 seconds
        }

        // If health drops below zero, destroy the enemy
        if (health <= 0f)
        {
            DestroyEnemy(); // Call the method to handle enemy destruction
        }
    }

    // Method to handle the destruction of the enemy
    public void DestroyEnemy()
    {
        if (isDestroyed) return; // Prevent double triggering
        isDestroyed = true; // Mark as destroyed

        Debug.Log("Enemy destroyed!");

        // Trigger the OnEnemyDestroyed event before the enemy is destroyed
        if (OnEnemyDestroyed != null)
        {
            OnEnemyDestroyed(gameObject); // Notify listeners that the enemy is destroyed
        }

        // Destroy the enemy GameObject
        Destroy(gameObject);
    }

    // Collision or trigger logic to detect bullets
    private void OnCollisionEnter(Collision collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            TakeDamage(bullet.damage); // Take damage from the bullet
            Destroy(bullet.gameObject); // Destroy the bullet
        }
    }
}
