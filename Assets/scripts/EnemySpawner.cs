using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array to hold different enemy prefabs
    public int numberOfSkeletons = 10; // Number of skeletons to spawn
    public int numberOfGhosts = 5; // Number of ghosts to spawn

    // Replace Vector3 corners with GameObjects
    public GameObject corner1Object;
    public GameObject corner2Object;
    public GameObject corner3Object;
    public GameObject corner4Object;
    public GameObject spawnPointObject; // Object that defines the spawn point

    public float spawnHeight = 5.0f; // Height above the spawn point for spawning
    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Track currently spawned enemies

    private Vector3 minBounds; // Minimum bounds of the spawn area
    private Vector3 maxBounds; // Maximum bounds of the spawn area

    void Start()
    {
        CalculateBounds();
        SpawnInitialEnemies();
    }

    // Calculate the min and max bounds based on the GameObjects' positions
    void CalculateBounds()
    {
        Vector3[] corners = new Vector3[4]
        {
            corner1Object.transform.position,
            corner2Object.transform.position,
            corner3Object.transform.position,
            corner4Object.transform.position
        };

        minBounds = corners[0];
        maxBounds = corners[0];

        foreach (Vector3 corner in corners)
        {
            minBounds = Vector3.Min(minBounds, corner);
            maxBounds = Vector3.Max(maxBounds, corner);
        }
    }

    // Spawns the initial batch of enemies
    void SpawnInitialEnemies()
    {
        // Check if enemyPrefabs array has enough elements to avoid IndexOutOfRangeException
        if (enemyPrefabs.Length > 0)
        {
            for (int i = 0; i < numberOfSkeletons; i++)
            {
                SpawnEnemy(enemyPrefabs[0]); // Assuming skeleton is at index 0
            }
        }

        if (enemyPrefabs.Length > 1)
        {
            for (int i = 0; i < numberOfGhosts; i++)
            {
                SpawnEnemy(enemyPrefabs[1]); // Assuming ghost is at index 1
            }
        }

        // Additional enemy types if available in the prefabs array
        if (enemyPrefabs.Length > 2)
        {
            for (int i = 0; i < numberOfGhosts; i++) // Adjust loop as per requirements
            {
                SpawnEnemy(enemyPrefabs[2]); // Assuming another enemy type at index 2
            }
        }

        if (enemyPrefabs.Length > 3)
        {
            for (int i = 0; i < numberOfGhosts; i++) // Adjust loop as per requirements
            {
                SpawnEnemy(enemyPrefabs[3]); // Assuming another enemy type at index 3
            }
        }
    }

    // Spawn a single enemy at a random position within the spawn area bounds
    void SpawnEnemy(GameObject enemyToSpawn)
    {
        // Get the spawn point's Y position from the spawnPointObject
        Vector3 spawnPointPosition = spawnPointObject.transform.position;

        // Generate a random position within the spawn area bounds and above the spawn point
        Vector3 spawnPosition = new Vector3(
            Random.Range(minBounds.x, maxBounds.x), // Random X position within bounds
            spawnPointPosition.y + spawnHeight,     // Spawn height above the spawn point
            Random.Range(minBounds.z, maxBounds.z)  // Random Z position within bounds
        );

        // Instantiate the enemy
        GameObject newEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);

        // Get the Enemy component and register for the OnEnemyDestroyed event
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.OnEnemyDestroyed += HandleEnemyDestroyed;
        }

        // Track the spawned enemy
        spawnedEnemies.Add(newEnemy);

        // Notify the GameManager that an enemy has been spawned (for total count)
        GameManager.Instance.RegisterEnemy(enemyScript);
    }

    // Handle when an enemy is destroyed
    void HandleEnemyDestroyed(GameObject destroyedEnemy)
    {
        // Remove the destroyed enemy from the tracking list
        spawnedEnemies.Remove(destroyedEnemy);

        // Notify GameManager that an enemy has been destroyed
        GameManager.Instance.EnemyKilled(destroyedEnemy);
    }

    // Draw the spawn area bounds in the Scene view for visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        // Draw lines between the corner GameObjects
        if (corner1Object && corner2Object && corner3Object && corner4Object)
        {
            Gizmos.DrawLine(corner1Object.transform.position, corner2Object.transform.position);
            Gizmos.DrawLine(corner2Object.transform.position, corner3Object.transform.position);
            Gizmos.DrawLine(corner3Object.transform.position, corner4Object.transform.position);
            Gizmos.DrawLine(corner4Object.transform.position, corner1Object.transform.position);
        }

        // Draw the spawn point location
        if (spawnPointObject)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spawnPointObject.transform.position, 0.3f); // Red sphere at the spawn point for visualization
        }
    }
}
