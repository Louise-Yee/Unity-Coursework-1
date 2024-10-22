using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f; // Speed of the projectile
    public float lifetime = 2f; // How long the projectile will exist

    private float lifespanTimer;

    private void OnEnable()
    {
        lifespanTimer = lifetime; // Reset the timer when the projectile is activated
    }

    private void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Decrease the lifespan timer
        lifespanTimer -= Time.deltaTime;
        if (lifespanTimer <= 0f)
        {
            gameObject.SetActive(false); // Deactivate instead of destroying
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Handle collision with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit the player!");
            gameObject.SetActive(false); // Deactivate instead of destroying
        }
    }
}
