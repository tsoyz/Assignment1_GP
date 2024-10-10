using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 50f; // Health value of the enemy
    public GameObject hitEffect; // Particle effect to play when bullet hits the enemy

    private bool isHit = false; // Flag to prevent multiple collisions

    public delegate void EnemyDestroyedHandler(GameObject enemy);
    public event EnemyDestroyedHandler OnEnemyDestroyed;

    public void TakeDamage(float damageAmount)
    {
        if (isHit) return; // Prevent multiple hits
        isHit = true;

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

    private void OnTriggerEnter(Collider other)
    {
        if (isHit) return; // Prevent multiple triggers
        Debug.Log("OnTriggerEnter detected: " + other.name);

        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null)
        {
            Debug.Log("Enemy hit by bullet! Bullet damage: " + bullet.damage);

            // Take damage from the bullet and destroy it
            TakeDamage(bullet.damage);
            Destroy(bullet.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isHit) return; // Prevent multiple collisions
        Debug.Log("OnCollisionEnter detected: " + collision.gameObject.name);

        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            Debug.Log("Enemy hit by bullet! Bullet damage: " + bullet.damage);

            // Take damage from the bullet and destroy it
            TakeDamage(bullet.damage);
            Destroy(bullet.gameObject);
        }
    }

    // Method to handle the destruction of the enemy
    public void DestroyEnemy()
    {
        Debug.Log("Enemy destroyed!");
        OnEnemyDestroyed?.Invoke(gameObject); // Notify listeners that the enemy is destroyed
        Destroy(gameObject); // Destroy the enemy GameObject
    }
}
