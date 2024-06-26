using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Level Settings")]
    public Sprite[] backgroundLevelSprites;
    public int currentLevel;
    public LevelScoreData levelScoreData;

    [Header("Spawn Timers")]
    public int[] level0SpawnTimer;
    public int[] level1SpawnTimer;
    public int[] level2SpawnTimer;
    public int[] level3SpawnTimer;
    public int[] level4SpawnTimer;
    public int[] level5SpawnTimer;

  

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
        UIManager.instance.OnStageChangeScore(currentLevel,false);
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
            EndGame();
           
        }else
        {
            currentLevel = level;
            UIManager.instance.backgroundImage.sprite = backgroundLevelSprites[currentLevel];
            InitLevelVariables();
        }

      
    }


    public void LoadLevel0()
    {
        currentLevel = 0;
        UIManager.instance.backgroundImage.sprite = backgroundLevelSprites[currentLevel];
        InitLevelVariables();
    }

    public void InitLevelVariables()
    {
        float foodBugSpawnTime = 0f;
        float evilBugSpawnTime = 0f;

        switch (currentLevel)
        {
            case 0:
                if (level0SpawnTimer.Length >= 2)
                {
                    foodBugSpawnTime = level0SpawnTimer[0];
                    evilBugSpawnTime = level0SpawnTimer[1];
                }
                break;
            case 1:
                if (level1SpawnTimer.Length >= 2)
                {
                    foodBugSpawnTime = level1SpawnTimer[0];
                    evilBugSpawnTime = level1SpawnTimer[1];
                }
                break;
            case 2:
                if (level2SpawnTimer.Length >= 2)
                {
                    foodBugSpawnTime = level2SpawnTimer[0];
                    evilBugSpawnTime = level2SpawnTimer[1];
                }
                break;
            case 3:
                if (level3SpawnTimer.Length >= 2)
                {
                    foodBugSpawnTime = level3SpawnTimer[0];
                    evilBugSpawnTime = level3SpawnTimer[1];
                }
                break;
            case 4:
                if (level4SpawnTimer.Length >= 2)
                {
                    foodBugSpawnTime = level4SpawnTimer[0];
                    evilBugSpawnTime = level4SpawnTimer[1];
                }
                break;
            case 5:
                if (level5SpawnTimer.Length >= 2)
                {
                    foodBugSpawnTime = level5SpawnTimer[0];
                    evilBugSpawnTime = level5SpawnTimer[1];
                }
                break;
            default:
                Debug.LogError("Level not recognized. Default settings applied.");
                break;
        }

        BugManager.instance.UpdateSpawnIntervals(foodBugSpawnTime, evilBugSpawnTime);
        UIManager.instance.ResetWaterAttackOnLevelChange();

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
    public void EndGame()
    {
     
        SaveCurrentLevelScore(); // Save score before changing level

        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreen");
        Debug.Log("Last level");
    }
}
