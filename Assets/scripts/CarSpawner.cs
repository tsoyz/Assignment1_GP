using System.Collections;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPrefab; // The car prefab to spawn
    public Transform spawnPoint; // The position where the cars will spawn
    public Transform[] waypoints; // The waypoints the car will follow

    public float minSpawnTime = 2f; // Minimum time between spawns
    public float maxSpawnTime = 5f; // Maximum time between spawns

    void Start()
    {
        // Start the spawning process
        StartCoroutine(SpawnCars());
    }

    IEnumerator SpawnCars()
    {
        while (true) // Infinite loop to keep spawning
        {
            float randomTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(randomTime);

            // Instantiate the car
            GameObject newCar = Instantiate(carPrefab, spawnPoint.position, spawnPoint.rotation);

            // Pass the waypoints to the car's movement script
            CarMovement carMovement = newCar.GetComponent<CarMovement>();
            if (carMovement != null)
            {
                carMovement.SetWaypoints(waypoints);
            }
        }
    }
}
