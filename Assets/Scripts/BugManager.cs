using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BugManager : MonoBehaviour
{
    // create instance
    public static BugManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public GameObject[] bugPrefabs; // Array of different bug prefabs
    public float spawnInterval = 2f; // Time interval between spawns
    public int maxBugs = 10; // Maximum number of bugs that can be spawned at once
    public float spawnAreaWidth = 10f; // Width of the spawn area
    public float spawnAreaHeight = 5f; // Height of the spawn area

    private float minY; // Minimum Y boundary for spawning
    private Camera mainCamera;
    private int currentBugCount = 0; // Counter to keep track of the current number of bugs
    public List<BugSO> bugList = new List<BugSO>();

    void Start()
    {
        mainCamera = Camera.main;
        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenBottom = mainCamera.transform.position.y - mainCamera.orthographicSize;
        minY = screenBottom + (screenHeight * 0.4f); // Calculate 40% from the bottom of the screen

        InvokeRepeating("SpawnBug", 0f, spawnInterval); // Start spawning bugs at regular intervals
    }

    private void Update()
    {
        CheckBugTypeInScene(); 
    }
    void SpawnBug()
    {
        if (currentBugCount >= maxBugs) return; // Do not spawn if the maximum number of bugs is reached

        float xPosition = Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f);
        float yPosition = Random.Range(minY, minY + spawnAreaHeight);

        Vector2 spawnPosition = new Vector2(xPosition, yPosition);

        int bugIndex = Random.Range(0, bugPrefabs.Length); // Select a random bug type
        Instantiate(bugPrefabs[bugIndex], spawnPosition, Quaternion.identity);

        currentBugCount++;
        UIManager.instance.bugsRemainingText.text = "Bugs Remaining: " + GetCurrentBugCount();

        bugList.Add(bugPrefabs[bugIndex].GetComponent<Bugs>().bugSO);
    }

    public void BugDestroyed(BugSO bug, GameObject bugPrefab)
    {
        Debug.Log("Ate bug: " + bug.bugName); // Log the bug that was destroyed
        bugList.Remove(bug);
        currentBugCount--; // Decrease the bug count when a bug is destroyed
        UIManager.instance.bugsRemainingText.text = "Bugs Remaining: " + GetCurrentBugCount();
        Destroy(bugPrefab);
      
    }

    // create a get method for currentBugCount
    public int GetCurrentBugCount()
    {
        return currentBugCount;
    }


    public void CheckBugTypeInScene()
    {
        bool onlyWasps = true;

        foreach (var bug in bugList)
        {
            Debug.Log(bug.bugType.ToString());

            if (bug.bugType != BugSO.BugType.Wasp)
            {
                onlyWasps = false;
            }
        }

        if (onlyWasps && bugList.Count > 0)
        {
            Debug.Log("There are only wasps left in the scene.");
        }
    }

}

  
