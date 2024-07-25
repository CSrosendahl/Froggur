using UnityEngine;

[CreateAssetMenu(fileName = "LevelScoreData", menuName = "ScriptableObjects/LevelScoreData", order = 1)]
public class LevelScoreData : ScriptableObject
{
    public int[] levelScores;
    public int overallScore;
    public int lastPlayedLevel;

    private void Awake()
    {
        overallScore = GetOverallScore();
       
    }
    public void Initialize(int numberOfLevels)
    {
        levelScores = new int[numberOfLevels];
    }

    public void UpdateScore(int level, int score)
    {
        if (level >= 0 && level < levelScores.Length)
        {
            levelScores[level] = score;
            CalculateOverallScore();
        }
    }

    private void CalculateOverallScore()
    {
        overallScore = 0;
        foreach (int score in levelScores)
        {
            overallScore += score;
        }
    }


    public int GetScoreForLevel(int level)
    {
        if (level >= 0 && level < levelScores.Length)
        {
            return levelScores[level];
        }
        return 0;
    }

    // Reset score
    public void ResetScores()
    {
        for (int i = 0; i < levelScores.Length; i++)
        {
            levelScores[i] = 0;
        }
        CalculateOverallScore();
    }

    public void ResetScoreForLevel(int level)
    {
        if (level >= 0 && level < levelScores.Length)
        {
            levelScores[level] = 0;
            CalculateOverallScore();
        }
        else
        {
            Debug.LogError("Level index out of range.");
        }
    }

    // Get overall score
    public int GetOverallScore()
    {
        return overallScore;
    }
  
}
