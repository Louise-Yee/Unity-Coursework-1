using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Camera playerCamera; // Reference to the camera
    public float mouseSensitivity = 100f; // Sensitivity for looking around
    private float xRotation = 0f; // Tracks vertical rotation

    void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Set the FOV to 103
        if (playerCamera != null)
        {
            playerCamera.fieldOfView = 103f;
        }
    }

    void Update()
    {
        HandleCameraRotation();
    }

    // Method to rotate the camera based on mouse input
    void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical rotation (clamping to prevent flipping)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotation to camera and player
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
