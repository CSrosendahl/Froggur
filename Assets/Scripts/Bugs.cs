using UnityEngine;

public class Bugs : MonoBehaviour
{
    public int points;
    public float moveSpeed = 2f; // Speed at which the bug moves
    public float changeDirectionInterval = 2f; // Interval after which the bug changes direction
    public float boundaryX = 5f; // Horizontal boundary for bug movement
    public float boundaryY = 5f; // Vertical boundary for bug movement

    private Vector2 moveDirection; // Current movement direction
    private float changeDirectionTimer; // Timer to track when to change direction
    private float minY; // Minimum Y boundary to restrict bug movement

    public BugSO bugSO; // The bug's type

    void Start()
    {
        Camera mainCamera = Camera.main;
        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenBottom = mainCamera.transform.position.y - mainCamera.orthographicSize;
        minY = screenBottom + (screenHeight * 0.4f); // Calculate 40% from the bottom of the screen

        InitBugStats();
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
        if (Mathf.Abs(newPosition.x) > boundaryX)
        {
            moveDirection.x = -moveDirection.x;
            newPosition.x = Mathf.Clamp(newPosition.x, -boundaryX, boundaryX);
        }

        // Check vertical boundaries and reverse direction if necessary
        if (newPosition.y > boundaryY || newPosition.y < minY)
        {
            moveDirection.y = -moveDirection.y;
            newPosition.y = Mathf.Clamp(newPosition.y, minY, boundaryY);
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
}
