using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    public float speed = 5f;
    public float rotationSpeed = 100f;
    public float jumpForce = 10f; // Increase this value to make the jump faster
    private Rigidbody rb; // Rigidbody component reference
    private bool isGrounded; // Check if the player is on the ground
    public AudioSource footsteps; // Footsteps sound source
    public float stepInterval = 0.3f; // Time interval between each footstep

    private bool isMoving; // Tracks if the player is moving
    private float stepTimer; // Tracks time since last footstep sound

    void Start()
    {
        animator = GetComponent<Animator>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        stepTimer = stepInterval; // Initialize the step timer
    }

    void Update()
    {
        // Movement Controls
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            speed = 2.5f * speed;
            //animator.SetBool("run", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            speed = speed / 2.5f;
            //animator.SetBool("run", false);
        }

        isMoving = false; // Reset movement flag

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            MovePlayer(Vector3.forward);
            animator.SetBool("walk", true);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            MovePlayer(Vector3.back);
            animator.SetBool("walk", true);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            MovePlayer(Vector3.left);
            animator.SetBool("walk", true);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            MovePlayer(Vector3.right);
            animator.SetBool("walk", true);
        }
        else
        {
             //Stop walking when no keys are pressed
            animator.SetBool("walk", false);
        }


        // Add a deadzone for mouse movement to prevent unintended rotation
        float mouseX = Input.GetAxis("Mouse X");

        if (Mathf.Abs(mouseX) > 0.1f) // 0.1f is the deadzone threshold, adjust as needed
        {
            transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
        }

        // Jump when the player presses the spacebar and is grounded
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            isGrounded = false; // Set isGrounded to false after jumping
            //animator.SetBool("jump", true);
        } else
        {
            //animator.SetBool("jump", false);
        }
        

        // Play footsteps sound at regular intervals
        HandleFootsteps();
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

    // Move player in a given direction
    private void MovePlayer(Vector3 direction)
    {
        transform.Translate(direction * speed * Time.deltaTime);
        isMoving = true; // Set movement flag to true
    }

    // Handle playing the footsteps sound
    private void HandleFootsteps()
    {
        if (isMoving && isGrounded)
        {
            // Increment timer
            stepTimer += Time.deltaTime;

            // Play footstep sound if enough time has passed
            if (stepTimer >= stepInterval)
            {
                footsteps.Play();
                stepTimer = 0f; // Reset the timer
            }
        }
        else
        {
            // Reset timer when not moving or grounded
            stepTimer = stepInterval;
        }
    }
}
