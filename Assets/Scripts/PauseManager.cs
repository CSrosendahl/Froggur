using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // create instance
    public static PauseManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private bool isPaused = false;

    // Reference to the pause menu UI (optional)
    public GameObject pauseMenuUI;
    public FrogAttack frogAttackScript;
 

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
        frogAttackScript.enabled = false;
        UIManager.instance.waterAttackButton.interactable = false;
        // Optionally disable other game elements, such as player controls
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        frogAttackScript.enabled = true;
        UIManager.instance.waterAttackButton.interactable = true;
        // Optionally enable other game elements, such as player controls
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void QuitGame()
    {
        Application.Quit();
    }



}
