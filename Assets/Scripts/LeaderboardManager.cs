using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public TextMeshProUGUI leaderboardText;
    public int maxEntries = 5;

    void Start()
    {
        CleanInvalidEntries();  // Clean old invalid entries
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);

        if (lastScore != 0 && !string.IsNullOrEmpty(MainMenuManager.playerName) && MainMenuManager.playerName != "Enter your name")
        {
            SaveScore(lastScore);
        }

        DisplayLeaderboard();
    }

    public void SaveScore(int score)
    {
        string playerName = MainMenuManager.playerName;

        // Do not save if score is 0 or if name is invalid
        if (score == 0 || string.IsNullOrEmpty(playerName) || playerName == "Enter your name")
        {
            Debug.Log("Invalid score or name, not saving.");
            return;
        }

        for (int i = 0; i < maxEntries; i++)
        {
            int existingScore = PlayerPrefs.GetInt("HighScore" + i, 0);
            string existingName = PlayerPrefs.GetString("HighScoreName" + i, "Anonymous");

            if (score > existingScore)
            {
                for (int j = maxEntries - 1; j > i; j--)
                {
                    PlayerPrefs.SetInt("HighScore" + j, PlayerPrefs.GetInt("HighScore" + (j - 1), 0));
                    PlayerPrefs.SetString("HighScoreName" + j, PlayerPrefs.GetString("HighScoreName" + (j - 1), "Anonymous"));
                }

                PlayerPrefs.SetInt("HighScore" + i, score);
                PlayerPrefs.SetString("HighScoreName" + i, playerName);
                break;
            }
        }

        PlayerPrefs.Save();
    }

        public void DisplayLeaderboard()
{
    leaderboardText.text = "Leaderboard:\n"; // Clear previous content

    for (int i = 0; i < maxEntries; i++)
    {
        int score = PlayerPrefs.GetInt("HighScore" + i, 0);
        string name = PlayerPrefs.GetString("HighScoreName" + i, "Anonymous");

        // Only display valid scores and names (exclude default "Enter your name")
        if (score > 0 && !string.IsNullOrEmpty(name) && name != "Enter your name")
        {
            // Prevent duplicate names in the leaderboard
            if (!leaderboardText.text.Contains(name)) 
            {
                leaderboardText.text += (i + 1) + ". " + name + " - " + score + "\n"; // Display each entry
            }
        }
    }
}



    private void CleanInvalidEntries()
    {
        for (int i = 0; i < maxEntries; i++)
        {
            string name = PlayerPrefs.GetString("HighScoreName" + i, "Anonymous");
            if (string.IsNullOrEmpty(name) || name == "Enter your name")
            {
                PlayerPrefs.SetInt("HighScore" + i, 0);
                PlayerPrefs.SetString("HighScoreName" + i, "Anonymous");
            }
        }

        PlayerPrefs.Save();
    }
}
