using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float speed = 5f; // Speed of the bird
    public float catchDistance = 1f; // Distance at which the bird catches the bug
    public float roamRadius = 10f; // Radius within which the bird roams
    public float roamWaitTime = 2f; // Time to wait before choosing a new roaming position
    public GameObject birdGO; // Reference to the bird GameObject

    private Transform targetBug; // Current target bug
    private Vector3 roamPosition; // Current random roam position
    private bool isRoaming = false; // Is the bird currently roaming

    void Start()
    {
        StartCoroutine(FindBugsAndCatch()); // Start the coroutine to find and catch bugs
        StartCoroutine(Roam()); // Start the coroutine to roam randomly
    }

    void Update()
    {
        if (targetBug != null)
        {
            // Move towards the target bug
            transform.position = Vector3.MoveTowards(transform.position, targetBug.position, speed * Time.deltaTime);

            // Rotate to face the target bug within limits
            RotateTowards(targetBug.position);

            // Check if the bird is close enough to catch the bug
            if (Vector3.Distance(transform.position, targetBug.position) < catchDistance)
            {
                // Catch the bug (destroy the bug or deactivate it)
                Destroy(targetBug.gameObject);

                // Reset the target bug
                targetBug = null;
            }
        }
        else if (isRoaming)
        {
            // Move towards the roam position
            transform.position = Vector3.MoveTowards(transform.position, roamPosition, speed * Time.deltaTime);

            // Rotate to face the roam position within limits
            RotateTowards(roamPosition);

            // Check if the bird has reached the roam position
            if (Vector3.Distance(transform.position, roamPosition) < 0.1f)
            {
                isRoaming = false; // Stop roaming when the position is reached
            }
        }
    }

    private IEnumerator FindBugsAndCatch()
    {
        while (true)
        {
            // Find the closest bug
            targetBug = FindClosestBug();

            // Wait for a short time before checking again
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator Roam()
    {
        while (true)
        {
            if (targetBug == null && !isRoaming)
            {
                // Choose a random position within the roam radius
                Vector2 randomDirection = Random.insideUnitCircle * roamRadius;
                roamPosition = new Vector3(randomDirection.x, randomDirection.y, 0) + transform.position;
                isRoaming = true;
            }

            // Wait for the specified roam wait time before choosing a new position
            yield return new WaitForSeconds(roamWaitTime);
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

        if (closestBug != null)
        {
            return closestBug.transform;
        }
        else
        {
            return null;
        }
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float limitedAngle = Mathf.Clamp(angle, -45f, 45f);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, limitedAngle));
    }
}
