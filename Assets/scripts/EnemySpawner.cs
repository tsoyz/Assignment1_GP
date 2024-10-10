using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array to hold different enemy prefabs
    public int enemyCount = 5; // Number of enemies to spawn

    public Vector3 corner1 = new Vector3(88.9000015f, 50.5f, 192.119995f);
    public Vector3 corner2 = new Vector3(144.600006f, 50.5f, 192.119995f);
    public Vector3 corner3 = new Vector3(144.600006f, 50.5f, 33.6883698f);
    public Vector3 corner4 = new Vector3(88.9000015f, 50.5f, 33.6883698f);
    public Vector3 spawnPoint; // Manually set the spawn point for spawning above it

    public float spawnHeight = 5.0f; // Height above the spawn point where enemies will spawn
    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Track currently spawned enemies

    private Vector3 minBounds; // Minimum bounds of the spawn area
    private Vector3 maxBounds; // Maximum bounds of the spawn area

    void Start()
    {
        CalculateBounds();
        SpawnInitialEnemies();
    }

    // Calculate the min and max bounds based on the 4 manually set corners
    void CalculateBounds()
    {
        Vector3[] corners = new Vector3[4] { corner1, corner2, corner3, corner4 };

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
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    // Spawn a single enemy at a random position within the spawn area bounds
    void SpawnEnemy()
    {
        // Choose a random enemy prefab
        GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Generate a random position within the spawn area bounds and above the spawn point
        Vector3 spawnPosition = new Vector3(
            Random.Range(minBounds.x, maxBounds.x), // Random X position within bounds
            spawnPoint.y + spawnHeight,             // Spawn height above the spawn point
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
    }

    void HandleEnemyDestroyed(GameObject destroyedEnemy)
    {
        // Remove the destroyed enemy from the tracking list
        spawnedEnemies.Remove(destroyedEnemy);

        // Spawn a new enemy to maintain the count
        SpawnEnemy();
    }

    // Draw the spawn area bounds in the Scene view for visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        // Draw lines between the manually set corner points
        Gizmos.DrawLine(corner1, corner2);
        Gizmos.DrawLine(corner2, corner3);
        Gizmos.DrawLine(corner3, corner4);
        Gizmos.DrawLine(corner4, corner1);

        // Draw the spawn point location
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spawnPoint, 0.3f); // Red sphere at the spawn point for visualization
    }
}
