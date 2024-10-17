using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float explosionRadius = 5f; // Radius within which objects are affected
    public GameObject explosionEffect; // Explosion effect prefab
    public float effectDuration = 2f; // Time before the explosion effect is removed

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is an enemy
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Trigger the explosion and destroy the enemy
            Explode();
            enemy.DestroyEnemy(); // Destroy the enemy and trigger the event
        }
    }

    void Explode()
    {
        // Instantiate explosion effect at the mine's position
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(effect, effectDuration); // Destroy the effect after the set duration
        }

        // Destroy the mine itself after the explosion
        Destroy(gameObject);
    }

    // Visualize the explosion radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
