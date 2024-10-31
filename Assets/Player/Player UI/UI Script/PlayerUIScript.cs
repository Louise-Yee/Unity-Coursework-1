using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public Text speedText; // UI text element for speed
    public Text jumpHeightText; // UI text element for jump height
    private PlayerMovement player; // Reference to the PlayerMovement script

    private void Start()
    {
        // Get the PlayerMovement component attached to the player
        player = FindObjectOfType<PlayerMovement>();

        // Check if the UI elements are assigned
        if (speedText == null || jumpHeightText == null)
        {
            Debug.LogError("UI text elements are not assigned.");
        }
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }
        // Update the speed text with the current player speed
        if (speedText != null)
        {
            speedText.text = "Speed: " + player.GetCurrentSpeed().ToString("F1"); // Display speed with 2 decimal points
        }

        // Update the jump height text with the player's jump height
        if (jumpHeightText != null)
        {
            jumpHeightText.text = "Jump Height: " + player.GetCurrentJumpHeight().ToString("F2");
        }
    }
}
