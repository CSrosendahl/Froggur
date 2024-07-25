using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
  
    public GameObject resumeObject;
    public TextMeshProUGUI startGameText;

    public GameObject scoreObject;
    public TextMeshProUGUI currentScoreText;

    public AudioClip buttonClick;
    // start game button

    private void Start()
    {
        int lastPlayedLevel = PlayerPrefs.GetInt("SavedLevel");
        int combinedScore = PlayerPrefs.GetInt("CombinedScore");
       
        Debug.Log("Combined Score: " + combinedScore);
        Debug.Log("Last played Level: " + lastPlayedLevel);
       
      
        if(lastPlayedLevel > 0)
        {
            resumeObject.SetActive(true);
            scoreObject.SetActive(true);

            currentScoreText.text = "CURRENT HIGH SCORE:\n " + combinedScore;      

            startGameText.text = "START NEW GAME";

        } else
        {
            resumeObject.SetActive(false);
            scoreObject.SetActive(false);
            startGameText.text = "START GAME";
        }
    }

    public void StartNewGame()
    {
        StartCoroutine(WaitForButtonClick_StartGame());
        
    }
    
    //public void StartGame()
    //{
    //    // Load the game scene
    //    SceneManager.LoadScene("SampleScene");
    //}
    // quit game button
    public void QuitGame()
    {
        // Quit the game
        StartCoroutine(WaitForButtonClick_QuitGame());
       
    }

    public void ResumeGame()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
          int lastPlayedLevel = PlayerPrefs.GetInt("SavedLevel");
       
         SceneManager.LoadScene("SampleScene");
        



        }
     
    }
   
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteKey("SavedLevel");
        PlayerPrefs.DeleteKey("CombinedScore");
    }
    // open settings button
    
    IEnumerator WaitForButtonClick_StartGame()
    {
        AudioManager.instance.PlaySound(buttonClick);
        yield return new WaitForSeconds(0.5f);
        ResetPlayerPrefs();
        SceneManager.LoadScene("SampleScene");

    }
    IEnumerator WaitForButtonClick_Options()
    {

        yield return new WaitForSeconds(0.5f);
        

    }
    IEnumerator WaitForButtonClick_QuitGame()
    {

        yield return new WaitForSeconds(0.5f);
        Application.Quit();

    }


}
