using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  // Import the new Input System namespace

public class Cameramovement : MonoBehaviour
{
    public float normalMoveSpeed = 10f;  // Normal move speed
    public float fastMoveSpeed = 50f;    // Fast move speed when holding Shift
    public float rotationSensitivity = 3f;  // Sensitivity for mouse rotation

    private Vector2 lookInput;  // To store mouse look input
    private float rotationX = 0f;  // To track camera rotation in the X-axis (up/down)
    private float rotationY = 0f;  // To track camera rotation in the Y-axis (left/right)

    void Update()
    {
        // Handle camera movement
        HandleMovement();

        // Handle camera rotation using the mouse
        HandleMouseLook();
    }

    void HandleMovement()
    {
        // Set movement speed, Shift to move faster
        float moveSpeed = Keyboard.current.leftShiftKey.isPressed ? fastMoveSpeed : normalMoveSpeed;

        Vector3 move = new Vector3();

        // WASD or Arrow keys movement
        if (Keyboard.current.wKey.isPressed)
            move += transform.forward;
        if (Keyboard.current.sKey.isPressed)
            move -= transform.forward;
        if (Keyboard.current.aKey.isPressed)
            move -= transform.right;
        if (Keyboard.current.dKey.isPressed)
            move += transform.right;

        // Up and down movement (Q/E or Space/C)
        if (Keyboard.current.spaceKey.isPressed)
            move += transform.up;
        if (Keyboard.current.cKey.isPressed || Keyboard.current.qKey.isPressed)
            move -= transform.up;

        // Apply movement
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    void HandleMouseLook()
    {
        // Capture mouse look input if right mouse button is held down
        if (Mouse.current.rightButton.isPressed)
        {
            // Get mouse delta (change in mouse position)
            lookInput = Mouse.current.delta.ReadValue();

            // Rotate camera based on mouse movement
            rotationX -= lookInput.y * rotationSensitivity;  // Up and down
            rotationY += lookInput.x * rotationSensitivity;  // Left and right

            // Apply rotation to the camera
            transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        }
    }
}
