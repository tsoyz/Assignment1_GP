using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

public class Player : MonoBehaviour
{
    public delegate void PlayerDied(); // Define a delegate for the event
    public static event PlayerDied OnPlayerDeath; // Custom event for player death

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a car (ensure the car is tagged as "Car")
        if (collision.gameObject.CompareTag("Car"))
        {
            Die(); // Trigger the player's death if hit by a car
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
