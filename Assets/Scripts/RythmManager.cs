using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class RhythmManager : MonoBehaviour
{
    public Transform marshmallow;
    public float perfectRadius = 1.0f;
    public GameObject perfectPrefab;
    public GameObject earlyPrefab;
    public GameObject latePrefab;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI startPromptText; // Text that shows "Press Space to Begin"
    public TextMeshProUGUI gameOverMessage; // Text to display final score at the end
    public AudioSource song; // Reference to the song
    public MarshmallowAttack marshmallowAttack;
    public LeaderboardManager leaderboardManager;

    private int score = 0;
    private bool gameStarted = false; // Flag to check if the game has started
    private bool gameEnded = false; // Flag to check if the game has ended
    private float spaceCooldown = 0.1f; // Set this to a small value, or remove it if you want no cooldown at all

    void Start()
    {
        UpdateScore();
        startPromptText.text = "Press Space to Begin"; // Set the initial prompt
        gameOverMessage.gameObject.SetActive(false); // Hide the game over message initially
        song.Stop(); // Ensure the song doesn't play on start
        marshmallowAttack.enabled = false;
    }

    void Update()
    {
        // Start the game when space is pressed and game hasn't started
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        // Allow space press without significant cooldown
        if (gameStarted && !gameEnded && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SpaceCooldown()); // Use the short cooldown

            // Find all notes on the screen and calculate closest note to marshmallow
            NoteMovement[] notes = FindObjectsOfType<NoteMovement>();
            NoteMovement closestNote = null;
            float closestDistance = float.MaxValue;

            foreach (NoteMovement note in notes)
            {
                if (note != null && !note.IsEvaluated())
                {
                    float distance = Vector2.Distance(note.transform.position, marshmallow.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestNote = note;
                    }
                }
            }

            // Check if closest note is within perfect range or hit early
            if (closestNote != null)
            {
                if (closestDistance <= perfectRadius)
                {
                    Debug.Log("Perfect Hit!");
                    score += 100;
                    DisplayFeedback(perfectPrefab);
                }
                else
                {
                    Debug.Log("Early Hit!");
                    score -= 10;
                    DisplayFeedback(earlyPrefab);
                }

                closestNote.MarkAsHit();
                UpdateScore();
            }
        }

        // Check if the song is finished and end the game
        if (gameStarted && !gameEnded && !song.isPlaying)
        {
            EndGame();
        }
    }

    public void NoteMissed(NoteMovement note)
    {
        if (!note.IsEvaluated())
        {
            Debug.Log("Late Hit!");
            score -= 10;
            DisplayFeedback(latePrefab);
            note.DestroyNote();
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    // SpaceCooldown function reduced to allow faster response time
    private IEnumerator SpaceCooldown()
    {
        yield return new WaitForSeconds(spaceCooldown); // Cooldown is reduced to 0.1 seconds
    }

    // Reduce feedback duration to make it appear quicker and disappear faster
    public void DisplayFeedback(GameObject feedbackPrefab)
    {
        Vector3 offset = new Vector3(1.5f, 1.5f, 0f);
        Vector3 feedbackPosition = marshmallow.position + offset;
        GameObject feedback = Instantiate(feedbackPrefab, feedbackPosition, Quaternion.identity);
        Destroy(feedback, 0.5f); // Display feedback for a shorter duration
    }

    public void DecreaseScore(int amount)
    {
        score -= amount; // Decrease the score by the specified amount
        UpdateScore(); // Update the score on the screen
        Debug.Log("Player hit! Score decreased by " + amount);
    }

    private void StartGame()
    {
        gameStarted = true; // Set the flag to true to start the game
        startPromptText.gameObject.SetActive(false); // Hide the "Press Space to Begin" text
        song.Play(); // Start the song
        marshmallowAttack.enabled = true;
    }

    public void EndGame()
    {
        gameEnded = true; // Set the flag to true to end the game
        marshmallowAttack.enabled = false; // Disable attacks when the game ends
        gameOverMessage.text = "Game Over! Your Score: " + score.ToString(); // Show the final score
        gameOverMessage.gameObject.SetActive(true); // Show the game over message
        marshmallowAttack.enabled = false;

        string playerName = MainMenuManager.playerName; // Ensure playerName is fetched correctly
    if (score > 0 && !string.IsNullOrEmpty(playerName) && playerName != "Enter your name")
    {
        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.Save();
    }

        SceneManager.LoadScene("MainMenu");
    }

    public bool GameStarted()
    {
        return gameStarted; // Return whether the game has started
    }
}
