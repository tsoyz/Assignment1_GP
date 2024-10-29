using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 50f; // Health value of the enemy
    public GameObject hitEffect; // Particle effect to play when bullet hits the enemy

    public bool isRegistered = false; // Flag to check if the enemy is registered with the GameManager
    public bool isDestroyed = false; // Flag to prevent multiple destruction calls

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
            Vector3 effectPosition = transform.position + new Vector3(0, 1.0f, 0); // Adjust Y value as needed
            GameObject effect = Instantiate(hitEffect, effectPosition, Quaternion.identity);
            Destroy(effect, 2f);
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

        // Unsubscribe all listeners to ensure no double notification
        OnEnemyDestroyed = null;

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