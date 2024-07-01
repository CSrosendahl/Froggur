using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
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

    [Header("Score")]
    public int currentScore = 0;
    public int oneStarPointThreshold = 5;
    public int twoStarPointThreshold = 10;
    public int threeStarPointThreshold = 20;

    [HideInInspector] public TextMeshProUGUI scoreText;
    [HideInInspector] public TextMeshProUGUI bugsRemainingText;
    [HideInInspector] public TextMeshProUGUI combinedScore;
    [HideInInspector] public TextMeshProUGUI onStageChangePointText;
    [HideInInspector] public TextMeshProUGUI onStageChangeStageText;
    [HideInInspector] public GameObject nextStageButton;
    [HideInInspector] public GameObject restartLevelButton;
    [HideInInspector] public GameObject onChangeLevelGUI;
    [HideInInspector] public Animator onStageGUIAnimator;
    



    [Header("Timer")]
   

    public float gameTime = 60;  
    public float countDownToStart = 3.0f;
    [HideInInspector] public TextMeshProUGUI timerText;
    [HideInInspector] public TextMeshProUGUI countDownToStartText;

    private bool countDownActive = true;
    private float currentGameTime = 0;
    private bool gameTimerUp;
    private bool gameTimerActive = false;

   
    [Header("Point Prompt")]
    [HideInInspector] public TextMeshProUGUI pointPrompt;
    
    private Transform grabbedBugTransform; // Transform of the grabbed bug
    private int bugPoints; // Current points for the grabbed bug
    private Vector3 initialPosition;

    [Header("Fade")]
    [HideInInspector] public Image fadeImage;

    [Header("Other")]
    [HideInInspector] public Image waterAttackImage;
    [HideInInspector] public Button waterAttackButton;

    [HideInInspector] public FrogAttack frogAttackScript;
   

    void Start()
    {
        currentGameTime = gameTime;
        frogAttackScript.AttackState(false);
      //  bugsRemainingText.text = "Bugs Remaining: " + BugManager.instance.GetCurrentBugCount();
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
        if (gameTimerActive)
        {
            if (currentGameTime > 0)
            {
                gameTimerUp = false;
                currentGameTime -= Time.deltaTime; // Decrement time
                timerText.text = "Time: " + Mathf.Max(currentGameTime, 0).ToString("F2"); // Update timer text, ensure it doesn't go below 0

                if (currentGameTime <= 0 && !gameTimerUp)
                {
                    //countDownToStartText.enabled = true;
                   // countDownToStartText.text =  "Time is up! You scored: " + currentScore;
                    OnStageChangeScore(LevelManager.instance.currentLevel, true);

                    gameTimerUp = true;
                    frogAttackScript.AttackState(false);
                    BugManager.instance.DestroyAllBugs();
                    BirdManager.instance.DestroyAllBirds();

                }
            }
        }
    }

    public void FadeOutToNextStage()
    {
        FadeOut(3);
        StartCoroutine(WaitToFadeInNextLevel());
        nextStageButton.SetActive(false);
        restartLevelButton.SetActive(false);
    }
    public void FadeOutRestartStage()
    {
        FadeOut(3);
        StartCoroutine(WaitToFadeInRestartLevel());
        nextStageButton.SetActive(false);
        restartLevelButton.SetActive(false);
    }
    public void OnStageChangeScore(int currentStage, bool enabled)
    {
        if (enabled)
        {
            onChangeLevelGUI.SetActive(true);
            
            nextStageButton.SetActive(true);
            restartLevelButton.SetActive(true);

            onStageGUIAnimator.Play("FadeInAnim");
            onStageChangeStageText.enabled = true;
            onStageChangeStageText.text = "STAGE " + currentStage;

            onStageChangePointText.enabled = true;
            onStageChangePointText.text = currentScore.ToString() + " POINTS ";

            if (currentScore >= threeStarPointThreshold)
            {
                onStageGUIAnimator.SetTrigger("3Star");
                TurtleFriend.instance.PlayAnimation("HappyJump2");
            }
            else if (currentScore >= twoStarPointThreshold)
            {
                onStageGUIAnimator.SetTrigger("2Star");
                TurtleFriend.instance.PlayAnimation("HappyJump");
            }
            else if (currentScore >= oneStarPointThreshold)
            {
                onStageGUIAnimator.SetTrigger("1Star");
            }
        }
        else
        {
            onStageChangeStageText.enabled = false;
            onStageChangeStageText.text = "";

            onStageChangePointText.enabled = false;
            onStageChangePointText.text = "";


            onChangeLevelGUI.SetActive(false);
            nextStageButton.SetActive(false);
            restartLevelButton.SetActive(false);
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
               
                currentGameTime = gameTime;
                frogAttackScript.AttackState(true);
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
      //  combinedScore.text = "Overall Score: " + LevelManager.instance.levelScoreData.GetOverallScore();
    }

    #region IEnumerators/ResetScore
    IEnumerator WaitToFadeInRestartLevel()
    {
        yield return new WaitForSeconds(4);
        LevelManager.instance.RestartLevel();
        FadeIn(3f);
    }
    IEnumerator WaitToFadeInNextLevel()
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

    // Set level duration 
    public float SetLevelDuration(float duration)
    {
        return duration;
    }

    public void WaterAttackOnCoolDown()
    {
    
        waterAttackImage.fillAmount = 0;
        waterAttackImage.color = Color.white;
        frogAttackScript.canWaterAttack = false;
       // StartCoroutine(GradualFill(waterAttackImage, frogAttackScript.waterAttackCooldown));
    }

    public void ReplenishWaterAttack(float replenishAmount)
    {
        if(waterAttackImage.fillAmount == 1f)
        {
            frogAttackScript.canWaterAttack = true;
            return;
        }
    
        waterAttackImage.fillAmount += replenishAmount;

    }

 

    public void WaterAttackActive()
    {
        if (frogAttackScript.waterAttackActive)
        {
            waterAttackImage.color = Color.grey;
        } else
        {
            waterAttackImage.color = Color.white;
        }
    }
    public void ResetWaterAttackOnLevelChange()
    {
        
        waterAttackImage.fillAmount = 1;
        waterAttackImage.color = Color.white;
        frogAttackScript.canWaterAttack = true;
        frogAttackScript.waterAttackActive = false;
    
    }

 
    private IEnumerator GradualFill(Image image, float duration) // Not in use
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            image.fillAmount = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        image.fillAmount = 1f;
    }
}
