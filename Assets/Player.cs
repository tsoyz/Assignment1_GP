using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

public class Player : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health for the player
    private int currentHealth;  // Current health of the player

    public delegate void PlayerDied(); // Define a delegate for the event
    public static event PlayerDied OnPlayerDeath; // Custom event for player death

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health to maximum health
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a car (ensure the car is tagged as "Car")
        if (collision.gameObject.CompareTag("Car"))
        {
            Die(); // Trigger the player's death if hit by a car
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // Decrease health by the damage amount
        Debug.Log("Player took damage! Current health: " + currentHealth);

        // Check if the player's health has dropped to zero or below
        if (currentHealth <= 0)
        {
            Die(); // Trigger player's death if health is zero or below
        }
    }

    void Die()
    {
        // Trigger the custom event for player death
        if (OnPlayerDeath != null)
        {
            OnPlayerDeath();
        }

        // Log the player's death for debugging purposes
        Debug.Log("Player died! Loading LoseScene...");

        // Load the LoseScene after the player dies
        SceneManager.LoadScene("LoseScene");

        // Optionally, disable player movement or trigger a death animation
        gameObject.SetActive(false); // Disable the player object after death
    }
}

// PlayerHealth script
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died");
        // Add additional logic for player death, e.g., respawning, game over screen
    }
}
