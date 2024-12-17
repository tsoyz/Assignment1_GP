using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private Animator animator;
    public GameObject bulletPrefab; // Bullet prefab
    public GameObject shootPoint; // Point where the bullet is shot from
    public GameObject shootEffect; // Particle effect for shooting\
    public AudioSource shootSound1;

    public GameObject minePrefab; // Mine prefab to drop
    public Transform mineSpawnPoint; // Point where the mine is dropped

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
                // Handle shooting with the left mouse button
            if (Input.GetKeyDown(KeyCode.Mouse0)) // Left mouse button
            {
                ShootBullet();
                animator.SetBool("fire", true);
                //if (animator.GetBool("walk") || animator.GetBool("run"))
                //{
                //    animator.SetBool("walkFiring", true);
                //} else 
                //{
                //    animator.SetBool("standFiring", true);
                //}
            }
        
            if (Input.GetKeyUp(KeyCode.Mouse0)) // Left mouse button
            {
                animator.SetBool("fire", false);
                //if (animator.GetBool("walk") || animator.GetBool("run"))
                //{
                //    animator.SetBool("walkFiring", false);
                //} else 
                //{
                //    animator.SetBool("standFiring", false);
                //}
            }

            // Handle dropping the mine with the "E" key
            if (Input.GetKeyDown(KeyCode.E)) // 'E' key for mine
            {
                DropMine();
            }
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
            shootSound1.Play();
        }

        animator.SetBool("fire", false);
    }

    // Method to drop the mine
    void DropMine()
    {
        // Instantiate the mine at the mine spawn point
        Instantiate(minePrefab, mineSpawnPoint.position, mineSpawnPoint.rotation);
    }
}
