using System.Collections;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    // make instance
    public static BirdManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public GameObject birdGO; // Reference to the bird GameObject
    private float spawnInterval; // Interval in seconds between each bird spawn

    private bool canSpawn = false; // Flag to control spawning

     public Vector2 spawnAreaMin; // Minimum coordinates for the spawn area
     public Vector2 spawnAreaMax; // Maximum coordinates for the spawn area

    void Start()
    {
        // Start the coroutine to spawn birds periodically
        StartCoroutine(SpawnBirdsPeriodically(spawnInterval));
    }

    public void SpawnBird(bool canSpawn, float spawnInterval)
    {
        this.canSpawn = canSpawn;

        if (canSpawn)
        {
            // Start the coroutine to spawn birds periodically
            StartCoroutine(SpawnBirdsPeriodically(spawnInterval));
        }
        else
        {
            // Stop the coroutine to stop spawning birds
            StopCoroutine(SpawnBirdsPeriodically(spawnInterval));
        }
    }

    private IEnumerator SpawnBirdsPeriodically(float spawnTime)
    {
        while (canSpawn)
        {
            // Generate random coordinates within the specified range
            float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

            // Instantiate the bird at the random location
            Instantiate(birdGO, new Vector2(randomX, randomY), Quaternion.identity);

            // Wait for the specified interval before spawning the next bird
            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void DestroyAllBirds()
    {
        canSpawn = false;
        GameObject[] birds = GameObject.FindGameObjectsWithTag("Bird");

        if(birds.Length > 0 )
        {
            foreach (var bird in birds)
            {
                Destroy(bird);
            }
            StopCoroutine(SpawnBirdsPeriodically(spawnInterval));
        } else
        {
            Debug.Log("No birds to destroy");
        }
      
   

    }


}
