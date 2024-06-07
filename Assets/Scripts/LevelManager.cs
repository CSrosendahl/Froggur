using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update

    // Create instance
    public static LevelManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public Sprite[] backgroundLevelSprites;
    public int currentLevel;

    private void Start()
    {
        LoadLevel0();
    }
    public void NextLevel()
    {
        currentLevel++;
        UIManager.instance.backgroundImage.sprite = backgroundLevelSprites[currentLevel];
     
    }
    public void RestartLevel()
    {
        UIManager.instance.backgroundImage.sprite = backgroundLevelSprites[currentLevel];
        
    }
    public void LoadLevel(int level)
    {
        currentLevel = level;
        UIManager.instance.backgroundImage.sprite = backgroundLevelSprites[currentLevel];
       
    }
    public void LoadLevel0()
    {
       
        currentLevel = 0;
        UIManager.instance.backgroundImage.sprite = backgroundLevelSprites[currentLevel];
      

    }

    public void InitLevelVariables()
    {
      
        
        switch (currentLevel)
        {
            case 0:
                // Initialize variables for level 1
                
            
            
                BugManager.instance.UpdateSpawnIntervals(1f, 3.0f);
                Debug.Log("Initialized variables for Level 1");
                break;

            case 1:
                // Initialize variables for level 2

                BugManager.instance.UpdateSpawnIntervals(1f, 4.0f);
                Debug.Log("Initialized variables for Level 2");
                break;

            case 2:
                // Initialize variables for level 3


                BugManager.instance.UpdateSpawnIntervals(1.0f, 3.0f);
                Debug.Log("Initialized variables for Level 3");
                break;

            // Add more cases for additional levels as needed
            default:
                // Default case if the level is not recognized
                Debug.LogError("Level not recognized. Default settings applied.");
              
                break;
        }

        // Update the UI with the new variables
       
    }

}
