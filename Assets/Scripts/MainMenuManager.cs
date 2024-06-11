using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    // start game button
    public void StartGame()
    {
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    // quit game button
    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
    }
    // open settings button
 
}
