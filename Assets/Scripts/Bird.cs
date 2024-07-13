using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [Header("Bird Chase Bugs")]
    public float catchDistance = 1f; // Distance at which the bird catches the bug
    public float catchBugInterval = 2f; // Interval for checking bugs and roaming

    [Header("Bird Movement/Roaming")]
    public float moveSpeed = 2f; // Speed at which the bird moves
    public float changeDirectionInterval = 2f; // Interval after which the bird changes direction

    [Header("Other options")]
    public GameObject birdGO; // Reference to the bird GameObject
    public ParticleSystem birdDeathEffect;
    public int birdHealth = 1;
    public int birdPoints = 1;

    private Vector2 moveDirection; // Current movement direction
    private float changeDirectionTimer; // Timer to track when to change direction
    private float minX, maxX, minY, maxY; // Screen boundaries

    private Transform targetBug; // Current target bug

    private bool isChasingBug = false; // Flag to track if the bird is chasing a bug

    void Start()
    {
        CalculateScreenBoundaries();
        ChangeDirection(); // Set initial direction
        StartCoroutine(CheckForBugs()); // Start the coroutine to check for bugs
    }

    void Update()
    {
        if (!isChasingBug)
        {
            BirdRoaming(); // Move the bird
            UpdateDirectionChange(); // Update the timer and change direction if needed
        }
        else
        {
            ChaseBug(); // Chase the target bug
        }
    }

    private IEnumerator CheckForBugs()
    {
        while (true)
        {
            if (!isChasingBug)
            {
                targetBug = FindClosestBug();

                if (targetBug != null)
                {
                    isChasingBug = true;
                }
            }

            yield return new WaitForSeconds(catchBugInterval);
        }
    }

    private Transform FindClosestBug()
    {
        GameObject[] bugs = GameObject.FindGameObjectsWithTag("Bug");
        GameObject closestBug = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject bug in bugs)
        {
            float distance = Vector3.Distance(transform.position, bug.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestBug = bug;
            }
        }

        return closestBug != null ? closestBug.transform : null;
    }

    private void BirdRoaming()
    {
        Vector2 newPosition = (Vector2)transform.position + moveDirection * moveSpeed * Time.deltaTime;

        // Check horizontal boundaries and reverse direction if necessary
        if (newPosition.x > maxX || newPosition.x < minX)
        {
            moveDirection.x = -moveDirection.x;
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        }

        // Check vertical boundaries and reverse direction if necessary
        if (newPosition.y > maxY || newPosition.y < minY)
        {
            moveDirection.y = -moveDirection.y;
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        }

        transform.position = newPosition;
    }

    private void UpdateDirectionChange()
    {
        changeDirectionTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0f)
        {
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized; // Set a random direction
        changeDirectionTimer = changeDirectionInterval; // Reset the timer
    }

    private void ChaseBug()
    {
        if (targetBug == null)
        {
            isChasingBug = false;
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetBug.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetBug.position) <= catchDistance)
        {
            // Handle bug catching logic here
            Destroy(targetBug.gameObject);
            targetBug = null;
            isChasingBug = false;
        }
    }

    private void CalculateScreenBoundaries()
    {
        Camera mainCamera = Camera.main;
        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenWidth = screenHeight * mainCamera.aspect;
        float screenBottom = mainCamera.transform.position.y - mainCamera.orthographicSize;
        float screenTop = mainCamera.transform.position.y + mainCamera.orthographicSize;
        float screenLeft = mainCamera.transform.position.x - (screenWidth / 2);
        float screenRight = mainCamera.transform.position.x + (screenWidth / 2);

        minX = screenLeft;
        maxX = screenRight;
        minY = screenBottom + (screenHeight * 0.4f); // 40% from the bottom
        maxY = screenTop;
    }

    public void TakeDamage(int damage)
    {
        if (birdHealth <= 0)
        {
            return;
        }

        birdHealth -= damage;

        //if (birdHealth <= 0)
        //{
        //    // Handle bird death logic here
        //    if (birdDeathEffect != null)
        //    {
        //        Instantiate(birdDeathEffect, transform.position, Quaternion.identity);
        //    }
        //    Destroy(birdGO);
        //}
    }
}
