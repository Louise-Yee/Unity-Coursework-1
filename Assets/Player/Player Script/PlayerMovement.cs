using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // CharacterController and Rigidbody references
    private CharacterController controller;
    private Rigidbody rb;

    // Player state variables
    private Vector3 playerVelocity;
    public bool groundedPlayer;
    private bool enableMovement = true;

    // Movement properties
    [Header("Movement Properties")]
    public float playerSpeed = 5.0f;
    public float runSpeed = 12.0f;
    public float jumpHeight = 1.5f;
    public float gravityValue = -9.81f;
    public float terminalVelocity = -50f;
    public bool isRunning = false;

    // Jumping properties
    [Header("Jumping Properties")]
    public int maxJumps = 2;
    public int jumpCount;
    public float jumpCooldown = 1.0f;
    public bool jumpBlocked = false;

    // Camera and ground checking
    [Header("Camera and Ground Check")]
    public Transform cameraTransform;
    public Transform groundChecker;
    public float groundCheckerDist = 0.2f;

    // Mouse sensitivity and rotation
    [Header("Mouse Control")]
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    // Velocity cap
    [Header("Velocity Cap")]
    public float maximumPlayerSpeed = 150.0f;

    // Sliding and crouching properties
    [Header("Sliding and Crouching")]
    public bool isSliding = false;
    public float crouchHeight = 0.5f; // Height when crouching
    private bool isCrouching = false; // Track if player is crouching
    private float originalHeight; // Store original height

    [Header("Wall Run")]
    public LayerMask whatIsWall;
    public float wallCheckDistance = 1f;
    private bool wallLeft;
    private bool wallRight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    public CharacterController Controller
    {
        get
        {
            if (controller == null)
            {
                controller = GetComponent<CharacterController>();
                if (controller == null)
                {
                    Debug.LogError("CharacterController component is missing on this GameObject.");
                }
            }
            return controller;
        }
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        Cursor.lockState = CursorLockMode.Locked;

        // Store the original height of the character controller
        originalHeight = controller.height;
    }

    private void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleJumpAndGravity();
        HandleCrouching();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical rotation for looking up and down (applied to the camera)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevents the player from looking too far up or down
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation for looking left and right (applied to the player body)
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer)
        {
            jumpCount = 0;
            playerVelocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Check if the player is running or crouching

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            isRunning = true; // Set running state to true
        }
        else
        {
            isRunning = false; // Set running state to false
        }

        // Use run speed if running; otherwise, use player speed
        float speed = isRunning ? runSpeed : playerSpeed;

        // Move the character using the calculated speed
        controller.Move(move * speed * Time.deltaTime);
    }

    private void HandleJumpAndGravity()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        // Check for left wall
        wallLeft = Physics.Raycast(
            origin,
            -transform.right,
            out leftWallHit,
            wallCheckDistance,
            whatIsWall
        );

        // Check for right wall
        wallRight = Physics.Raycast(
            origin,
            transform.right,
            out rightWallHit,
            wallCheckDistance,
            whatIsWall
        );
        // Check if the player is sticking to a wall
        if (!groundedPlayer && (wallLeft || wallRight) && Input.GetMouseButton(1))
        {
            // Do not apply gravity while sticking to the wall
            return;
        }

        // Jump logic
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps && !jumpBlocked)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
            jumpCount++;
            jumpBlocked = true;
            Invoke("UnblockJump", jumpCooldown);
        }

        if (!groundedPlayer)
        {
            playerVelocity.y += gravityValue * Time.deltaTime; // Gravity accelerates the fall
        }

        // Clamp the vertical velocity to prevent falling too fast (terminal velocity)
        playerVelocity.y = Mathf.Max(playerVelocity.y, terminalVelocity);
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void HandleCrouching()
    {
        // Check if Left Control is held down for crouching
        if (Input.GetKey(KeyCode.LeftControl) && !isSliding)
        {
            StartCrouch(); // Start crouching if not already crouching
        }
        else
        {
            if (isCrouching) // Only attempt to stop crouching if currently crouching
            {
                StopCrouch();
            }
        }
    }

    private void StartCrouch()
    {
        if (!isCrouching) // Only change state if not already crouching
        {
            isCrouching = true;
            controller.height = crouchHeight; // Set character height to crouch height
        }
    }

    private void StopCrouch()
    {
        if (isCrouching) // Only change state if currently crouching
        {
            isCrouching = false;
            controller.height = originalHeight; // Restore character height to original
        }
    }

    private void UnblockJump()
    {
        jumpBlocked = false;
    }

    private void FixedUpdate()
    {
        if (!enableMovement)
            return;

        rb.velocity = ClampMagnitude(rb.velocity, maximumPlayerSpeed);
    }

    public void EnableMovement() => enableMovement = true;

    public void DisableMovement() => enableMovement = false;

    private Vector3 ClampMagnitude(Vector3 vector, float maxMagnitude)
    {
        if (vector.sqrMagnitude > maxMagnitude * maxMagnitude)
        {
            vector = vector.normalized * maxMagnitude;
        }
        return vector;
    }

    public float GetCurrentJumpHeight()
    {
        return transform.position.y; // Returns the player's current vertical position
    }

    public float GetCurrentSpeed()
    {
        return rb.velocity.magnitude; // This gives the total speed of the Rigidbody
    }
}
