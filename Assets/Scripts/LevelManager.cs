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



}
