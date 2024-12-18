using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform target;
    public float despawnRadius = 0.1f;
    public RhythmManager rhythmManager;

    private bool isHit = false;
    private bool isEvaluated = false;
    private bool isDestroyed = false; // New flag to check if the note is destroyed

    void Update()
    {
        if (target != null && !isHit && !isDestroyed)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Check if the note is close enough to despawn
            if (Vector2.Distance(transform.position, target.position) < despawnRadius && !isEvaluated)
            {
                rhythmManager.NoteMissed(this);
            }
        }
    }

    public void MarkAsHit()
    {
        if (!isEvaluated && !isDestroyed) // Ensure only one evaluation
        {
            isHit = true;
            isEvaluated = true;
            DestroyNote(); // Call the method to destroy the note
        }
    }

    public void DestroyNote()
    {
        if (!isDestroyed) // Ensure the note is only destroyed once
        {
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    // Add the IsEvaluated method to check if the note has been evaluated
    public bool IsEvaluated()
    {
        return isEvaluated;
    }
}
