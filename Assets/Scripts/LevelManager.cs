using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public Sprite[] backgroundLevelSprites;
    public int currentLevel;
    public LevelScoreData levelScoreData;

    private void Start()
    {
        levelScoreData.ResetScores();
        levelScoreData.Initialize(backgroundLevelSprites.Length); // Initialize score data for the number of levels
        LoadLevel0();
    }

    public void NextLevel()
    {
        SaveCurrentLevelScore(); // Save score before changing level
        UIManager.instance.ResetScore();
        UIManager.instance.DisplayScore();
        currentLevel++;
        LoadLevel(currentLevel);
    }

    public void RestartLevel()
    {
        LoadLevel(currentLevel);
    }

    public void LoadLevel(int level)
    {
        // Check if the level index is within the bounds of the backgroundLevelSprites array
        if (level < 0 || level >= backgroundLevelSprites.Length)
        {
            Debug.LogError("Level index out of bounds. Level: " + level);
            return;
        }

        currentLevel = level;
        UIManager.instance.backgroundImage.sprite = backgroundLevelSprites[currentLevel];
        InitLevelVariables();
    }


    public void LoadLevel0()
    {
        currentLevel = 0;
        UIManager.instance.backgroundImage.sprite = backgroundLevelSprites[currentLevel];
        InitLevelVariables();
    }

    public void InitLevelVariables()
    {
        switch (currentLevel)
        {
            case 0:
                BugManager.instance.UpdateSpawnIntervals(1f, 5.0f);
                break;
            case 1:
                BugManager.instance.UpdateSpawnIntervals(1f, 4.0f);
                break;
            case 2:
                BugManager.instance.UpdateSpawnIntervals(1.0f, 3.0f);
                break;
            case 3:
                BugManager.instance.UpdateSpawnIntervals(1.0f, 2.0f);
                break;
            case 4:
                BugManager.instance.UpdateSpawnIntervals(1.0f, 1.0f);
                break;
            case 5:
                BugManager.instance.UpdateSpawnIntervals(1.0f, 0.8f);
                break;
            default:
                Debug.LogError("Level not recognized. Default settings applied.");
                break;
        }

        UIManager.instance.scoreText.text = "Score: " + levelScoreData.GetScoreForLevel(currentLevel);
    }

    public void SaveCurrentLevelScore()
    {
        int currentScore = UIManager.instance.currentScore; // Assuming the current score is stored in the UIManager
        levelScoreData.UpdateScore(currentLevel, currentScore);
    }

    public int GetOverallScore()
    {
        return levelScoreData.overallScore;
    }
}
