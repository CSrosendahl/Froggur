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

    [Header("Level conditions")]
    public LevelVariables[] levelVariables;

  

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
        SaveCurrentLevelScore(); // Save score before changing level
        levelScoreData.ResetScoreForLevel(currentLevel);
        UIManager.instance.ResetScore();
        UIManager.instance.DisplayScore();
        UIManager.instance.OnStageChangeScore(currentLevel, false);
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

    //public void InitLevelVariables()
    //{
    //    float foodBugSpawnTime = 0f;
    //    float evilBugSpawnTime = 0f;

    //    switch (currentLevel)
    //    {
    //        case 0:


    //            foodBugSpawnTime = levelVariables[0].bugSpawnTime;
    //            evilBugSpawnTime = levelVariables[0].evilBugSpawnTime;

    //            if (levelVariables[0].birdSpawnEnabled)
    //            {
    //                StartCoroutine(WaitToSpawnBird(5f, 10));
    //            }

              


    //            break;
    //        case 1:
    //            foodBugSpawnTime = levelVariables[1].bugSpawnTime;
    //            evilBugSpawnTime = levelVariables[1].evilBugSpawnTime;

    //            if (levelVariables[1].birdSpawnEnabled)
    //            {
    //                StartCoroutine(WaitToSpawnBird(5f, 10));
    //            }

                
    //            break;
    //        case 2:
    //            foodBugSpawnTime = levelVariables[2].bugSpawnTime;
    //            evilBugSpawnTime = levelVariables[2].evilBugSpawnTime;

    //            if (levelVariables[2].birdSpawnEnabled)
    //            {
    //                StartCoroutine(WaitToSpawnBird(5f, 10));
    //            }
    //            break;
    //        case 3:
    //            foodBugSpawnTime = levelVariables[3].bugSpawnTime;
    //            evilBugSpawnTime = levelVariables[3].evilBugSpawnTime;

    //            if (levelVariables[4].birdSpawnEnabled)
    //            {
    //                StartCoroutine(WaitToSpawnBird(5f, 10));
    //            }
    //            break;
    //        case 4:
    //            foodBugSpawnTime = levelVariables[4].bugSpawnTime;
    //            evilBugSpawnTime = levelVariables[4].evilBugSpawnTime;

    //            if (levelVariables[4].birdSpawnEnabled)
    //            {
    //                StartCoroutine(WaitToSpawnBird(5f, 10));
    //            }
    //            break;
    //        case 5:
    //            foodBugSpawnTime = levelVariables[5].bugSpawnTime;
    //            evilBugSpawnTime = levelVariables[5].evilBugSpawnTime;

    //            if (levelVariables[5].birdSpawnEnabled)
    //            {
    //                StartCoroutine(WaitToSpawnBird(5f, 10));
    //            }
    //            break;
    //        default:
    //            Debug.LogError("Level not recognized. Default settings applied.");
    //            break;
    //    }

    //    BugManager.instance.UpdateSpawnIntervals(foodBugSpawnTime, evilBugSpawnTime);
    //    UIManager.instance.ResetWaterAttackOnLevelChange();

    //    UIManager.instance.scoreText.text = "Score: " + levelScoreData.GetScoreForLevel(currentLevel);
    //}
    public void InitLevelVariables()
    {
        if (currentLevel < 0 || currentLevel >= levelVariables.Length)
        {
            Debug.LogError("Level not recognized. Default settings applied.");
            return;
        }

        float foodBugSpawnTime = levelVariables[currentLevel].bugSpawnTime;
        float evilBugSpawnTime = levelVariables[currentLevel].evilBugSpawnTime;

        if (levelVariables[currentLevel].birdSpawnEnabled)
        {
            StartCoroutine(WaitToSpawnBird(levelVariables[currentLevel].birdSpawnDelay, levelVariables[currentLevel].birdRespawnInterval));
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

    IEnumerator WaitToSpawnBird(float timeBeforeSpawn, float birdSpawnInterval)
    {
        timeBeforeSpawn = timeBeforeSpawn + UIManager.instance.countDownToStart;
        yield return new WaitForSeconds(timeBeforeSpawn);
        BirdManager.instance.SpawnBird(true, birdSpawnInterval);
       
   
    }
 
}
