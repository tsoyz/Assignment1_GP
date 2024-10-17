using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Bullet prefab
    public GameObject shootPoint; // Point where the bullet is shot from
    public GameObject shootEffect; // Particle effect for shooting

    public GameObject minePrefab; // Mine prefab to drop
    public Transform mineSpawnPoint; // Point where the mine is dropped

    void Update()
    {
        // Handle shooting with the left mouse button
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Left mouse button
        {
            ShootBullet();
        }

        // Handle dropping the mine with the "E" key
        if (Input.GetKeyDown(KeyCode.E)) // 'E' key for mine
        {
            DropMine();
        }
    }

    // Method to shoot the bullet
    void ShootBullet()
    {
        // Instantiate the bullet
        GameObject clone = Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation);

        // Play shoot particle effect at the shoot point
        if (shootEffect != null)
        {
            GameObject effect = Instantiate(shootEffect, shootPoint.transform.position, shootPoint.transform.rotation);
            Destroy(effect, 2f); // Destroy the effect after 2 seconds
        }
    }

    // Method to drop the mine
    void DropMine()
    {
        // Instantiate the mine at the mine spawn point
        Instantiate(minePrefab, mineSpawnPoint.position, mineSpawnPoint.rotation);
    }
}
