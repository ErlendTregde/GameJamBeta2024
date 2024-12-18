using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the tag "Player"
        {
            RhythmManager rhythmManager = FindObjectOfType<RhythmManager>();
            if (rhythmManager != null)
            {
                rhythmManager.DecreaseScore(1000); // Decrease the score by 1000
            }

            Destroy(gameObject); // Destroy the projectile after hitting the player
        }
    }
}
