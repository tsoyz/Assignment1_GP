//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement; // Import the SceneManagement namespace

//public class Player : MonoBehaviour
//{
//    public int maxHealth = 100; // Maximum health for the player
//    private int currentHealth;  // Current health of the player


//    public delegate void PlayerDied(); // Define a delegate for the event
//    public static event PlayerDied OnPlayerDeath; // Custom event for player death

//    void Start()
//    {
//        currentHealth = maxHealth; // Initialize current health to maximum health
//    }

//    void OnCollisionEnter(Collision collision)
//    {
//        // Check if the collision is with a car (ensure the car is tagged as "Car")
//        if (collision.gameObject.CompareTag("Car"))
//        {
//            Die(); // Trigger the player's death if hit by a car
//        }
//    }

//    public void TakeDamage(int damageAmount)
//    {
//        currentHealth -= damageAmount; // Decrease health by the damage amount
//        Debug.Log("Player took damage! Current health: " + currentHealth);

//        // Check if the player's health has dropped to zero or below
//        if (currentHealth <= 0)
//        {
//            Die(); // Trigger player's death if health is zero or below
//        }
//    }

//    void Die()
//    {
//        // Trigger the custom event for player death
//        if (OnPlayerDeath != null)
//        {
//            OnPlayerDeath();
//        }

//        // Log the player's death for debugging purposes
//        Debug.Log("Player died! Loading LoseScene...");

//        // Load the LoseScene after the player dies
//        SceneManager.LoadScene("LoseScene");

//        // Optionally, disable player movement or trigger a death animation
//        gameObject.SetActive(false); // Disable the player object after death
//    }
//}

//// PlayerHealth script
//public class PlayerHealth : MonoBehaviour
//{
//    private Animator animator;
//    public int maxHealth = 100;
//    private int currentHealth;

//    void Start()
//    {
//        currentHealth = maxHealth;
//    }

//    public void TakeDamage(int damageAmount)
//    {
//        animator = GetComponent<Animator>();
//        currentHealth -= damageAmount;
//        Debug.Log("Player Health: " + currentHealth);

//        if (currentHealth <= 0)
//        {
//            Die();
//        }
//    }

//    void Die()
//    {
//        Debug.Log("Player died");
//        animator.SetBool("dead", true);
//        // Add additional logic for player death, e.g., respawning, game over screen
//    }
//}

using UnityEngine;
using UnityEngine.UI; // For health bar
using UnityEngine.SceneManagement; // For scene loading
using System.Collections;

public class Player : MonoBehaviour
{
    public int maxHealth = 100; // Maximum player health
    private int currentHealth;  // Current health of the player

    public Slider healthBar; // Reference to the health bar slider (UI)
    private Animator animator; // Reference to the Animator component
    public bool isDead = false; // Prevent multiple death triggers

    //public GameObject loseScreen; // Reference to the lose screen UI

    void Start()
    {
        currentHealth = maxHealth; // Initialize health
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        animator = GetComponent<Animator>(); // Get Animator component
    }

    void Update()
    {
        // DEBUG: Simulate damage for testing purposes
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // If the player is hit by an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10); // Adjust damage as needed
        }

        // If the player is hit by a car
        if (collision.gameObject.CompareTag("Car"))
        {
            Die(); // Instantly trigger death
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return; // Prevent damage after death

        currentHealth -= damageAmount;
        healthBar.value = currentHealth; // Update health bar UI

        Debug.Log("Player took damage! Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return; // Prevent multiple death triggers
        isDead = true;

        Debug.Log("Player died.");
        animator.SetBool("dead", true);

        // Trigger Lose Screen after animation
        StartCoroutine(LoadLoseSceneAfterDeath());
    }

    private IEnumerator LoadLoseSceneAfterDeath()
    {
        // Get the length of the death animation
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        // Wait for the animation to finish
        yield return new WaitForSeconds(animationLength);

        // Load the LoseScene
        SceneManager.LoadScene("LoseScene");
    }

}
