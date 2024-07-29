using UnityEngine;
using System.Collections;

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

    public int combinedScore;
    private Coroutine birdSpawnCoroutine;

    private void Start()
    {
        combinedScore = PlayerPrefs.GetInt("CombinedScore");
        levelScoreData.Initialize(backgroundLevelSprites.Length); // Initialize score data for the number of levels
        int savedLevel = PlayerPrefs.GetInt("SavedLevel", 0);

        LoadLevel(savedLevel); // Load the saved level or default to level 0
    }

    public void NextLevel()
    {
        SaveCurrentLevelScore();
        UIManager.instance.ResetScore();
        UIManager.instance.DisplayScore();
        UIManager.instance.OnStageChangeScore(currentLevel, false);
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
        }
        else
        {
            currentLevel = level;
            UIManager.instance.backgroundImage.sprite = backgroundLevelSprites[currentLevel];
            SaveCurrentLevelScore();
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
        if (currentLevel < 0 || currentLevel >= levelVariables.Length)
        {
            Debug.LogError("Level not recognized. Default settings applied.");
            return;
        }

        float foodBugSpawnTime = levelVariables[currentLevel].bugSpawnTime;
        float evilBugSpawnTime = levelVariables[currentLevel].evilBugSpawnTime;

        float birdSpawnDelay = levelVariables[currentLevel].birdSpawnDelay;
        float birdRespawnInterval = levelVariables[currentLevel].birdRespawnInterval;

        int maxBugs = levelVariables[currentLevel].maxBugs;
        BugManager.instance.SetMaxBugs(maxBugs);

        // Stop the previous bird spawn coroutine if it's running
        if (birdSpawnCoroutine != null)
        {
            StopCoroutine(birdSpawnCoroutine);
            StopCoroutine(BirdManager.instance.SpawnBirdsPeriodically(0));
            birdSpawnCoroutine = null;
        }

        // Clear existing birds (if any)
        BirdManager.instance.DestroyAllBirds();

        if (levelVariables[currentLevel].birdSpawnEnabled)
        {
            birdSpawnCoroutine = StartCoroutine(WaitToSpawnBird(birdSpawnDelay, birdRespawnInterval));
        }

        if (levelVariables[currentLevel].frogEnemy_Enabled)
        {
            FrogAIManager.instance.EnableEnemyFrog();
        }

        BugManager.instance.UpdateSpawnIntervals(foodBugSpawnTime, evilBugSpawnTime);
        UIManager.instance.ResetWaterAttackOnLevelChange();
        UIManager.instance.scoreText.text = "Score: " + levelScoreData.GetScoreForLevel(currentLevel);

        Debug.Log("All level variables initialized");
    }

    public void SaveCurrentLevelScore()
    {
        int currentScore = UIManager.instance.currentScore; // Assuming the current score is stored in the UIManager
        levelScoreData.UpdateScore(currentLevel, currentScore);
        PlayerPrefs.SetInt("SavedLevel", currentLevel); // Save the current level
        PlayerPrefs.SetInt("CombinedScore", GetOverallScore());
        PlayerPrefs.Save();
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

    private IEnumerator WaitToSpawnBird(float timeBeforeSpawn, float birdSpawnInterval)
    {
        // Wait for the initial countdown before starting the bird spawn timer
        float initialWaitTime = timeBeforeSpawn + UIManager.instance.countDownToStart;
        yield return new WaitForSeconds(initialWaitTime);

        // Spawn the bird after the initial wait time
        BirdManager.instance.SpawnBird(true, birdSpawnInterval);
    }
}
