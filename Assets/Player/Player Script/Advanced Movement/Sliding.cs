using System.Collections;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private CharacterController characterController; // CharacterController reference
    public Transform playerCamera;
    private Vector3 originalCameraPosition;
    private float originalHeight;
    public float slideSpeed = 15f;
    public float slideHeight = 0.9f;
    public float slideDuration = 1f;
    public float returnDuration = 0.2f; // Duration for the return transition
    private bool isSlidingActive = false;

    private void Start()
    {
        // Get PlayerMovement reference
        playerMovement = GetComponent<PlayerMovement>();

        // Check if playerMovement is null
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component is missing on this GameObject.");
            return; // Exit if PlayerMovement is not present
        }

        // Get CharacterController reference
        characterController = playerMovement.Controller;

        // Check if characterController is null
        if (characterController == null)
        {
            Debug.LogError(
                "CharacterController is missing. Ensure it is attached to the PlayerMovement."
            );
            return; // Exit if CharacterController is not present
        }

        originalHeight = characterController.height;

        if (playerCamera != null)
        {
            originalCameraPosition = playerCamera.localPosition; // Store original camera position
        }
        else
        {
            Debug.LogError("Player camera is not set. Please assign it in the inspector.");
        }
    }

    private void Update()
    {
        HandleSliding();
    }

    private void HandleSliding()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isSlidingActive && playerMovement.isRunning)
        {
            StartCoroutine(Slide());
        }
    }

    private IEnumerator Slide()
    {
        if (playerMovement == null || characterController == null)
        {
            Debug.LogError(
                "PlayerMovement or CharacterController is not set. Sliding cannot proceed."
            );
            yield break; // Exit if any component is missing
        }

        isSlidingActive = true;
        playerMovement.isSliding = true;

        Vector3 slideDirection = transform.forward;
        float elapsedTime = 0f;

        // Smoothly decrease character height and camera position
        Vector3 targetCameraPosition = originalCameraPosition + Vector3.down * slideHeight;

        // Use the public CharacterController property to move the player
        while (elapsedTime < slideDuration)
        {
            // Move the player
            characterController.Move(slideDirection * slideSpeed * Time.deltaTime);

            // Smoothly adjust character height
            characterController.height = Mathf.Lerp(
                characterController.height,
                slideHeight,
                elapsedTime / slideDuration
            );

            // Smoothly adjust camera position
            playerCamera.localPosition = Vector3.Lerp(
                playerCamera.localPosition,
                targetCameraPosition,
                elapsedTime / slideDuration
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restore height and camera position after sliding
        elapsedTime = 0f; // Reset elapsed time for the return transition
        while (elapsedTime < returnDuration)
        {
            // Smoothly restore character height
            characterController.height = Mathf.Lerp(
                slideHeight,
                originalHeight,
                elapsedTime / returnDuration
            );

            // Smoothly restore camera position
            playerCamera.localPosition = Vector3.Lerp(
                targetCameraPosition,
                originalCameraPosition,
                elapsedTime / returnDuration
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final values are set
        characterController.height = originalHeight;
        playerCamera.localPosition = originalCameraPosition;

        playerMovement.isSliding = false;
        isSlidingActive = false;
    }
}
