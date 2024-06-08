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
    public float spawnIntervalFoodBug = 2f; // Time interval between spawns
    public float spawnIntervalEvilBug = 5f;
    public int maxBugs = 10; // Maximum number of bugs that can be spawned at once
    public float spawnAreaWidth = 10f; // Width of the spawn area
    public float spawnAreaHeight = 5f; // Height of the spawn area

    private float minY; // Minimum Y boundary for spawning
    private Camera mainCamera;
    private int currentBugCount = 0; // Counter to keep track of the current number of bugs
    public List<BugSO> bugList = new List<BugSO>();

    public bool canSpawnBugs = false;

    void Start()
    {
        mainCamera = Camera.main;
        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenBottom = mainCamera.transform.position.y - mainCamera.orthographicSize;
        minY = screenBottom + (screenHeight * 0.4f); // Calculate 40% from the bottom of the screen

        InvokeRepeating("SpawnBug", 0f, spawnIntervalFoodBug); // Start spawning bugs at regular intervals
        InvokeRepeating("SpawnEvilBug", 0f, spawnIntervalEvilBug);
    }

    private void Update()
    {
        CheckBugTypeInScene(); 
    }
    void SpawnBug()
    {
        if(!canSpawnBugs)
        {
            Debug.Log("Bug spawner inactive");
            return;
        }

        if (currentBugCount >= maxBugs) return; // Do not spawn if the maximum number of bugs is reached

        float xPosition = Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f);
        float yPosition = Random.Range(minY, minY + spawnAreaHeight);

        Vector2 spawnPosition = new Vector2(xPosition, yPosition);

        int bugIndex = Random.Range(0, bugPrefabs.Length-1); // Select a random bug type, excluding the evil bug, which is always at the last index of the list
        Instantiate(bugPrefabs[bugIndex], spawnPosition, Quaternion.identity);

        currentBugCount++;
        UIManager.instance.bugsRemainingText.text = "Bugs Remaining: " + GetCurrentBugCount();

      //  bugList.Add(bugPrefabs[bugIndex].GetComponent<Bugs>().bugSO);


    }

    void SpawnEvilBug()
    {
        if (!canSpawnBugs)
        {
            return;
        }
        float xPosition = Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f);
        float yPosition = Random.Range(minY, minY + spawnAreaHeight);

        Vector2 spawnPosition = new Vector2(xPosition, yPosition);
        Instantiate(bugPrefabs[5], spawnPosition, Quaternion.identity);

    }
    public void UpdateSpawnIntervals(float newFoodBugInterval, float newEvilBugInterval)
    {
        // Cancel the existing invocations
        CancelInvoke("SpawnBug");
        CancelInvoke("SpawnEvilBug");

        // Update the intervals
        spawnIntervalFoodBug = newFoodBugInterval;
        spawnIntervalEvilBug = newEvilBugInterval;

        // Reinvoke with the new intervals
        InvokeRepeating("SpawnBug", 0f, spawnIntervalFoodBug);
        InvokeRepeating("SpawnEvilBug", 0f, spawnIntervalEvilBug);
    }

    public void BugDestroyed(BugSO bug, GameObject bugPrefab)
    {
        Debug.Log("Ate bug: " + bug.bugName); // Log the bug that was destroyed
        bugList.Remove(bug);
        currentBugCount--; // Decrease the bug count when a bug is destroyed
        Destroy(bugPrefab);
      
    }

    // create a get method for currentBugCount
    public int GetCurrentBugCount()
    {
        return currentBugCount;
    }
    public void DestroyAllBugs()
    {
        canSpawnBugs = false;
        GameObject[] bugs = GameObject.FindGameObjectsWithTag("Bug");
        foreach (var bug in bugs)
        {
            Destroy(bug);
        }
        bugList.Clear();
        currentBugCount = 0;
    

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

  
