using System.Collections;
using UnityEngine;

public class MarshmallowAttack : MonoBehaviour
{
    public GameObject attackPrefab; // The projectile prefab (e.g., chocolate)
    public Transform player; // Reference to the player (campfire)
    public float attackInterval = 5f; // Time between attacks
    public float projectileSpeed = 5f; // Speed of the projectile

    private RhythmManager rhythmManager; // Reference to the RhythmManager to affect score
    private bool canAttack = false; // Add a flag to control when attacking is allowed

    private void Start()
    {
        rhythmManager = FindObjectOfType<RhythmManager>();
        StartCoroutine(AttackPlayer()); // Start attacking the player periodically
    }

    private void Update()
    {
        // Ensure it only attacks if the game has started
        canAttack = rhythmManager != null && rhythmManager.GameStarted();
    }

    private IEnumerator AttackPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            if (canAttack) // Only attack if the game has started
            {
                ShootProjectileAtPlayer();
            }
        }
    }

    private void ShootProjectileAtPlayer()
    {
        if (attackPrefab != null && player != null)
        {
            // Spawn the projectile at the marshmallow's position
            GameObject projectile = Instantiate(attackPrefab, transform.position, Quaternion.identity);

            // Calculate the direction to the player
            Vector2 direction = (player.position - transform.position).normalized;

            // Move the projectile toward the player
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }

            // Optionally, destroy the projectile after a certain time to avoid clutter
            Destroy(projectile, 5f);
        }
    }
}
