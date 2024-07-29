using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
  

    public AudioSource audioSource;
    public AudioSource backgroundSound;
    public GameObject resumeObject;
    public TextMeshProUGUI startGameText;

    public GameObject scoreObject;
    public TextMeshProUGUI currentScoreText;

    public AudioClip buttonClick;

    private Dictionary<AudioClip, float> clipLastPlayedTime = new Dictionary<AudioClip, float>();
    public float cooldownTime = 1.0f; // Cooldown time in seconds

    private bool allAudioMuted = false;
    private List<AudioSource> audioSources = new List<AudioSource>();

    public Image toggleAllButtonImage;
    // start game button

    private void Start()
    {

        // Find all AudioSource components in the scene and add them to the list
        AudioSource[] sources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in sources)
        {
            audioSources.Add(source);
        }


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
 
    public void QuitGame()
    {
        // Quit the game
        StartCoroutine(WaitForButtonClick_QuitGame());
       
    }

    public void ResumeGame()
    {
     StartCoroutine(WaitForButtonClick_ResumeGame());
     
    }
   
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteKey("SavedLevel");
        PlayerPrefs.DeleteKey("CombinedScore");
    }
    // open settings button
    
    IEnumerator WaitForButtonClick_StartGame()
    {
        PlaySound(buttonClick);
        yield return new WaitForSeconds(0.5f);
        ResetPlayerPrefs();
        SceneManager.LoadScene("SampleScene");

    }
    IEnumerator WaitForButtonClick_Options()
    {
        PlaySound(buttonClick);
        yield return new WaitForSeconds(0.5f);
        

    }
    IEnumerator WaitForButtonClick_QuitGame()
    {
        PlaySound(buttonClick);
        yield return new WaitForSeconds(0.5f);
        Application.Quit();

    }
    IEnumerator WaitForButtonClick_ResumeGame()
    {
        PlaySound(buttonClick);
        yield return new WaitForSeconds(0.5f);
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            int lastPlayedLevel = PlayerPrefs.GetInt("SavedLevel");

            SceneManager.LoadScene("SampleScene");


        }

    }
    public void ToggleAllMute()
    {


        allAudioMuted = !allAudioMuted;
        foreach (AudioSource source in audioSources)
        {


            source.mute = allAudioMuted;
        }

        if (allAudioMuted)
        {
            toggleAllButtonImage.color = Color.black;
        }
        else
        {
            toggleAllButtonImage.color = Color.yellow;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioClip is null, cannot play sound.");
            return;
        }

        // Check if the clip has a cooldown time set and if it has been played recently
        if (clipLastPlayedTime.ContainsKey(clip))
        {
            float lastPlayedTime = clipLastPlayedTime[clip];
            if (Time.time - lastPlayedTime < cooldownTime)
            {
                Debug.LogWarning("AudioClip is still in cooldown, skipping.");
                return;
            }
        }

        // Play the clip and update the last played time
        audioSource.PlayOneShot(clip);
        clipLastPlayedTime[clip] = Time.time;
    }

}
