using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    [Header("End screen Score")]
    public TextMeshProUGUI[] levelScores; // Array of TextMeshProUGUI elements to display level scores
    public TextMeshProUGUI totalScore; // TextMeshProUGUI element to display the total score
    public LevelScoreData levelScoreData; // Reference to the LevelScoreData scriptable object

    private void Start()
    {
        PopulateLevelScore();
        UpdateTotalScore();
    }

    // Method to start the game
    public void StartGame()
    {
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    // Method to quit the game
    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
    }

    // Method to populate the level scores on the end screen
    public void PopulateLevelScore()
    {
        // Ensure the number of scores does not exceed the number of levelScores UI elements
        int numberOfLevels = Mathf.Min(levelScores.Length, levelScoreData.levelScores.Length);

        for (int i = 0; i < numberOfLevels; i++)
        {
            // Set the text of each level score UI element to the corresponding score with a message
            levelScores[i].text = $"For Level {i}, you scored {levelScoreData.levelScores[i]} points.";
        }
    }

    // Method to update the total score on the end screen
    public void UpdateTotalScore()
    {
        // Set the text of the total score UI element to the overall score with a message
        totalScore.text = $"Total score: {levelScoreData.overallScore} points.";
    }
}
