using UnityEngine;

public class PlayerFlip : MonoBehaviour
{
    private bool isFacingRight = false; // Start assuming the player is facing left

    void Update()
    {
        // Get horizontal input (assuming this controls left and right movement)
        float horizontal = Input.GetAxis("Horizontal");

        // If moving right and currently not facing right, flip the sprite
        if (horizontal > 0 && !isFacingRight)
        {
            Flip();
        }
        // If moving left and currently facing right, flip the sprite
        else if (horizontal < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        // Toggle the direction
        isFacingRight = !isFacingRight;

        // Multiply the player's local scale by -1 to flip the sprite
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
