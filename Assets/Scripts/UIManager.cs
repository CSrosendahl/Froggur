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
    private Coroutine floatingTextCoroutine; // Reference to the current coroutine
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
   

    [Header("Background")]
    public Image backgroundImage;

    [Header ("Score")]
    public int currentScore = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bugsRemainingText;
    public TextMeshProUGUI combinedScore;
   
    [Header ("Timer")]
    public TextMeshProUGUI timerText;
    public float currentGameTime = 5;
    private bool gameTimerUp;
    private bool gameTimerActive = true;
    public TextMeshProUGUI countDownToStartText;
    public float countDownToStart = 3.0f;
    private bool countDownActive = false;

    [Header("Point Prompt")]
    public TextMeshProUGUI pointPrompt;
    private Transform grabbedBugTransform; // Transform of the grabbed bug
    private int bugPoints; // Current points for the grabbed bug
    private Vector3 initialPosition;

    [Header("Fade")]
    public Image fadeImage;

    [HideInInspector]public FrogAttack frogAttackScript;

    void Start()
    {
     
        bugsRemainingText.text = "Bugs Remaining: " + BugManager.instance.GetCurrentBugCount();
        scoreText.text = "Score: " + currentScore;
      
        
       
    }
    private void Update()
    {
        UpdateTimer();
        CountDownTimer();


    }

    #region Fader
    public void FadeOut(float duration)
    {
        StartCoroutine(FadeToBlack(duration));
    }

    public void FadeIn(float duration)
    {
        StartCoroutine(FadeFromBlack(duration));
    }

    private IEnumerator FadeToBlack(float duration)
    {
        fadeImage.enabled = true;
        Color color = fadeImage.color;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, elapsed / duration);
            fadeImage.color = color;
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, 1);
        gameTimerActive = false;
    }

    private IEnumerator FadeFromBlack(float duration)
    {
        countDownActive = true;
        Color color = fadeImage.color;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, elapsed / duration);
            fadeImage.color = color;
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, 0);
       
    }
    #endregion

    #region Timers
    private void UpdateTimer()
    {
        if(gameTimerActive)
        {
            if (currentGameTime > 0)
            {
                gameTimerUp = false;
                currentGameTime -= Time.deltaTime; // Decrement time
                timerText.text = "Time: " + Mathf.Max(currentGameTime, 0).ToString("F2"); // Update timer text, ensure it doesn't go below 0

                if (currentGameTime <= 0 && !gameTimerUp)
                {
                    countDownToStartText.enabled = true;
                    countDownToStartText.text = "Time is up! You scored: " + currentScore;

                    gameTimerUp = true;
                    frogAttackScript.enabled = false;
                    BugManager.instance.DestroyAllBugs();
                    FadeOut(3);
                    StartCoroutine(WaitToFadeIn());
                }
            }
        }


      
    }

    public void CountDownTimer()
    {
        if (countDownActive)
        {
            countDownToStartText.enabled = true;
            if (countDownToStart > 0)
            {
                countDownToStart -= Time.deltaTime;
                countDownToStartText.text = Mathf.Ceil(countDownToStart).ToString();
            }
            else
            {
                countDownToStartText.text = "GO!";
                countDownToStart = 3.0f; 
                countDownActive = false;
                gameTimerActive = true;
                currentGameTime = 5;
                frogAttackScript.enabled = true;
                StartCoroutine(WaitToRemoveCountDownText());
                BugManager.instance.canSpawnBugs = true;
            }
        }

    }
    #endregion


    #region Popup text score
    public void PointPrompt(int point, Transform bugTransform)
    {
        bugPoints = point;
        grabbedBugTransform = bugTransform;

        Vector3 offsetPosition = bugTransform.position + new Vector3(0.5f, 0.5f, 0); // Initial offset

        if (point > 0)
        {
            pointPrompt.text = "+" + point.ToString();
            pointPrompt.color = Color.green;
        }
        else
        {
            pointPrompt.text = point.ToString();
            pointPrompt.color = Color.red;
        }

        pointPrompt.text = point.ToString();
        pointPrompt.transform.position = Camera.main.WorldToScreenPoint(offsetPosition); // Set the initial position of the point text
        pointPrompt.enabled = true;

        // Stop any existing coroutine before starting a new one
        if (floatingTextCoroutine != null)
        {
            StopCoroutine(floatingTextCoroutine);
        }

        floatingTextCoroutine = StartCoroutine(FloatingTextRoutine());
    }

    private IEnumerator FloatingTextRoutine()
    {
        float duration = 1.0f; // Duration of the float animation
        float elapsed = 0.0f;
        Vector3 startPosition = pointPrompt.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newY = startPosition.y + Mathf.Lerp(0, 50, elapsed / duration); // Adjust the 50 value to control how high it floats
            pointPrompt.transform.position = new Vector3(startPosition.x, newY, startPosition.z);
            yield return null;
        }

        // After the animation is complete, hide the point prompt
        pointPrompt.enabled = false;
    }
    #endregion



    public void UpdateScore(int points)
    {
        currentScore += points;
        DisplayScore();
    }


    // Display current score
    public void DisplayScore()
    {
        scoreText.text = "Score: " + currentScore;
        combinedScore.text = "Overall Score: " + LevelManager.instance.levelScoreData.GetOverallScore();
    }
    #region IEnumrators/Resetscore
    IEnumerator WaitToFadeIn()
    {
        yield return new WaitForSeconds(4);

        LevelManager.instance.NextLevel();
        FadeIn(3f);
        
    }
    IEnumerator WaitToRemoveCountDownText()
    {
        yield return new WaitForSeconds(1);
        countDownToStartText.enabled = false;
    }

    public void ResetScore()
    {
        currentScore = 0;
        scoreText.text = "Score: " + currentScore;
    }
    #endregion
}



