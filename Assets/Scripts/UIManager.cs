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

    [Header("Point Prompt")]
    public TextMeshProUGUI pointPrompt;
    private Transform grabbedBugTransform; // Transform of the grabbed bug
    private int currentPoints; // Current points for the grabbed bug
    private Vector3 initialPosition;

    void Start()
    {
        backgroundImage.sprite = backgroundSprites[0];
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

    public void PointPrompt(int point, Transform bugTransform)
    {
        currentPoints = point;
        grabbedBugTransform = bugTransform;

        Vector3 offsetPosition = bugTransform.position + new Vector3(0.5f, 0.5f, 0); // Initial offset

        if (point > 0)
        {
            pointPrompt.text = point.ToString();
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
}

    

