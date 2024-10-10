using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 100f;
    public float jumpForce = 10f; // Increase this value to make the jump faster
    private Rigidbody rb; // Rigidbody component reference
    private bool isGrounded; // Check if the player is on the ground

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
    }

    void Update()
    {
        // Movement Controls
        if (Input.GetKeyDown(KeyCode.LeftShift)) { speed = 2.5f * speed; }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) { speed = speed / 2.5f; }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { transform.Translate(Vector3.forward * speed * Time.deltaTime); }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) { transform.Translate(Vector3.back * speed * Time.deltaTime); }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { transform.Translate(Vector3.left * speed * Time.deltaTime); }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { transform.Translate(Vector3.right * speed * Time.deltaTime); }

        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime);

        // Jump when the player presses the spacebar and is grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Using ForceMode.VelocityChange for an immediate jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

            // Alternatively, set the Rigidbody velocity directly for instant jump
            // rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

            isGrounded = false; // Set isGrounded to false after jumping
        }
    }

    // Check if the player is on the ground using collision
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.GetComponent<Terrain>())
        {
            isGrounded = true; // Set isGrounded to true when touching the ground
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.GetComponent<Terrain>())
        {
            isGrounded = false; // Set isGrounded to false when leaving the ground
        }
    }
}
