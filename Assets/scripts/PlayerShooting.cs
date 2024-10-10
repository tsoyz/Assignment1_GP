using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject prefab; // Bullet prefab
    public GameObject shootPoint; // Point where the bullet is shot from
    public GameObject shootEffect; // Particle effect for shooting

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Left mouse button
        {
            // Instantiate the bullet
            GameObject clone = Instantiate(prefab, shootPoint.transform.position, shootPoint.transform.rotation);

            // Play shoot particle effect at the shoot point
            if (shootEffect != null)
            {
                GameObject effect = Instantiate(shootEffect, shootPoint.transform.position, shootPoint.transform.rotation);
                Destroy(effect, 2f); // Destroy the effect after 2 seconds
            }

            // Assign the shootEffect to the bullet if needed (for visual reference)
            //Bullet bulletScript = clone.GetComponent<Bullet>();
            //if (bulletScript != null && shootEffect != null)
            //{
            //    bulletScript.shootEffect = shootEffect; // Assign the shoot effect to the bullet script
            //}
        }
    }
}
