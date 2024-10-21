using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class WallStick : MonoBehaviour
{
    [Header("Wall Sticking")]
    public LayerMask whatIsWall;
    public float wallCheckDistance = 1f;

    [Header("References")]
    private PlayerMovement pm;
    private Rigidbody rb;

    // Camera reference
    public Camera playerCamera;
    public float tiltAngle = 15f; // Angle to tilt the camera

    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();

        // Ensure the playerCamera is assigned in the inspector
        if (playerCamera == null)
        {
            Debug.LogError("PlayerCamera reference is missing in WallStick.");
        }
    }

    private void Update()
    {
        CheckForWall();
        StickToWall();
    }

    private void CheckForWall()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f; // Adjust height based on your character's collider

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

        // Debug raycasts
        Debug.DrawRay(
            origin,
            -transform.right * wallCheckDistance,
            wallLeft ? Color.green : Color.red
        );
        Debug.DrawRay(
            origin,
            transform.right * wallCheckDistance,
            wallRight ? Color.green : Color.red
        );
    }

    private void StickToWall()
    {
        // Check if player is not grounded and against a wall while holding the spacebar
        if (!pm.groundedPlayer && (wallLeft || wallRight) && Input.GetKey(KeyCode.Space))
        {
            // Disable gravity to stick to the wall
            pm.playerSpeed = pm.isRunning ? 15f : 8f;
            rb.useGravity = false;

            // Stop horizontal movement
            rb.velocity = new Vector3(0, rb.velocity.y, 0);

            // Tilt the camera
            float tiltDirection = wallLeft ? tiltAngle : -tiltAngle; // Tilt left or right depending on the wall
            playerCamera.transform.localRotation = Quaternion.Euler(0, 0, tiltDirection);
        }
        else
        {
            // Re-enable gravity when not sticking to a wall
            rb.useGravity = true;

            // Reset camera tilt when not sticking
            playerCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
