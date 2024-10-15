using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller; // Reference to CharacterController
    private Vector3 playerVelocity; // Track current velocity
    private bool groundedPlayer; // Check if the player is grounded

    public float playerSpeed = 5.0f; // Movement speed
    public float jumpHeight = 1.5f; // Jump height
    public float gravityValue = -9.81f; // Gravity acceleration value
    public float terminalVelocity = -50f; // Maximum downward speed

    public int maxJumps = 2; // Max number of jumps allowed (Double jump)
    private int jumpCount; // Track the current jump count

    public Transform cameraTransform; // Reference to the player's camera for movement direction
    public float mouseSensitivity = 100f; // Mouse sensitivity for looking around

    private float xRotation = 0f; // Track vertical camera rotation

    private void Start()
    {
        // Assign the CharacterController component to this player
        controller = GetComponent<CharacterController>();

        // Lock the cursor to the game screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleJumpAndGravity();
    }

    private void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Control vertical rotation (clamping it to avoid flipping)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotation to the camera
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player horizontally based on mouse input
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        // Check if the player is grounded
        groundedPlayer = controller.isGrounded;

        // If grounded, reset jump count and velocity
        if (groundedPlayer)
        {
            jumpCount = 0; // Reset the jump counter when on the ground
            playerVelocity.y = -2f; // Small negative value to keep the player grounded
        }

        // Get movement input (WASD or arrow keys)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Move relative to the player's forward direction
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Apply movement
        controller.Move(move * playerSpeed * Time.deltaTime);
    }

    private void HandleJumpAndGravity()
    {
        // Handle jump input
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            // Calculate the initial jump velocity using the physics formula:
            // v = sqrt(2 * gravity * jumpHeight)
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
            jumpCount++; // Increase the jump count
        }

        // Apply gravity over time (velocity increases as we fall)
        if (!groundedPlayer)
        {
            playerVelocity.y += gravityValue * Time.deltaTime; // Gravity accelerates the fall
        }

        // Clamp the vertical velocity to prevent falling too fast (terminal velocity)
        playerVelocity.y = Mathf.Max(playerVelocity.y, terminalVelocity);

        // Apply the final vertical movement
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
