using UnityEngine;

public class Bugs : MonoBehaviour
{
    public int points;
    public float moveSpeed = 2f; // Speed at which the bug moves
    public float changeDirectionInterval = 2f; // Interval after which the bug changes direction

    private Vector2 moveDirection; // Current movement direction
    private float changeDirectionTimer; // Timer to track when to change direction
    private float minX, maxX, minY, maxY; // Screen boundaries

    public BugSO bugSO; // The bug's type

    void Start()
    {
        InitBugStats();
        CalculateScreenBoundaries();
        ChangeDirection(); // Set initial direction
    }

    void InitBugStats()
    {
        moveSpeed = bugSO.bugSpeed;
        changeDirectionInterval = bugSO.changeDirectionInterval;
        points = bugSO.points;
    }

    void Update()
    {
        MoveBug(); // Move the bug
        UpdateDirectionChange(); // Update the timer and change direction if needed
    }

    void MoveBug()
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

    void UpdateDirectionChange()
    {
        changeDirectionTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0f)
        {
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized; // Set a random direction
        changeDirectionTimer = changeDirectionInterval; // Reset the timer
    }

    void CalculateScreenBoundaries()
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
}
