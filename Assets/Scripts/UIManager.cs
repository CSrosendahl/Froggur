using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // create instance
    public static UIManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
   

    [Header("Background")]
    public Sprite[] backgroundSprites;
    public Image backgroundImage;

    [Header ("Score")]
    public int currentScore = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bugsRemainingText;
    [Header ("Timer")]
    public TextMeshProUGUI timerText;
    public float currentTime = 60;
    private bool timesUp;


  
    void Start()
    {
        backgroundImage.sprite = backgroundSprites[1];
        bugsRemainingText.text = "Bugs Remaining: " + BugManager.instance.GetCurrentBugCount();
        scoreText.text = "Score: " + currentScore;
    }
    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if (currentTime > 0)
        {
            timesUp = false;
            currentTime -= Time.deltaTime; // Decrement time
            timerText.text = "Time: " + Mathf.Max(currentTime, 0).ToString("F2"); // Update timer text, ensure it doesn't go below 0

            if (currentTime <= 0 && !timesUp)
            {
                timesUp = true;
                Debug.Log("Time's up!");
            }
        }
    }




}
