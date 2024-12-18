using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject startButton;     // Reference to the start button
    public GameObject quitButton;      // Reference to the quit button
    public Camera mainMenuCamera;      // Reference to the Main Menu camera
    public TMP_InputField usernameInput; // Reference to the username input field
    public static string playerName;   // Store the player's username

    void Start()
    {
        // Optionally, you can check if the username has been set before
        // If so, pre-fill the usernameInput field
        if (!string.IsNullOrEmpty(playerName))
        {
            usernameInput.text = playerName;
        }
    }

    // Start the game and disable the Main Menu camera
    public void StartGame()
    {
        playerName = usernameInput.text;  // Store the username

        // Ensure the player has entered a username
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Please enter a username.");
            return;
        }

        // Disable the Main Menu camera if you're staying in the same scene
        if (mainMenuCamera != null)
        {
            mainMenuCamera.gameObject.SetActive(false); // Deactivate the Main Menu camera
        }

        // Load the gameplay scene or keep playing in the current scene
        SceneManager.LoadScene("SampleScene");
    }

    // Quit the game
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
