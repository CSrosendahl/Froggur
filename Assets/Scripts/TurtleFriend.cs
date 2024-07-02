using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleFriend : MonoBehaviour
{
    public static TurtleFriend instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public Animator turtleAnim;
    public float moveSpeed = 2f;
    public float eatDuration = 10f; // Duration the turtle will attempt to eat bugs
    public float catchDistance = 1f; // Distance at which the turtle catches the bug

    private Transform targetBug;
    private bool isEatingBugs = false;

    public void PlayAnimation(string animationName)
    {
        if (turtleAnim != null)
        {
            turtleAnim.Play(animationName);
        }
        else
        {
            Debug.LogError("Animator component is not assigned.");
        }
    }

    private void OnMouseDown()
    {
        PlayAnimation("HappyJump"); // Replace "HappyJump" with the name of your animation
       // StartEatingBugs();
    }

    private void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch phase is Began (similar to mouse click)
            if (touch.phase == TouchPhase.Began)
            {
                // Convert touch position to a Ray
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Check if the ray hits this object's collider
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        PlayAnimation("HappyJump"); // Replace "HappyJump" with the name of your animation
                    }
                }
            }
        }
    }

    public void StartEatingBugs()
    {
        if (!isEatingBugs)
        {
            StartCoroutine(EatBugsCoroutine());
            turtleAnim.enabled = false;
        }
    }

    private IEnumerator EatBugsCoroutine()
    {
        isEatingBugs = true;
        float endTime = Time.time + eatDuration;

        while (Time.time < endTime)
        {
            targetBug = FindClosestBug();

            // End the coroutine if there are no bugs left
            if (targetBug == null)
            {
                break;
            }

            if (targetBug != null)
            {
                while (targetBug != null && Vector2.Distance(transform.position, targetBug.position) > catchDistance)
                {
                    MoveTowardsBug(targetBug);
                    yield return null;
                }

                if (targetBug != null && Vector2.Distance(transform.position, targetBug.position) <= catchDistance)
                {
                    Bugs bugScript = targetBug.GetComponent<Bugs>();
                    // Handle bug catching logic here
                    UIManager.instance.PointPrompt(bugScript.points, targetBug);
                    UIManager.instance.UpdateScore(bugScript.points);
                    Destroy(targetBug.gameObject);
                    targetBug = null;
                }
            }

            yield return null;
        }

        isEatingBugs = false;
        turtleAnim.enabled = true;
    }

    private void MoveTowardsBug(Transform bug)
    {
        Vector2 newPosition = Vector2.MoveTowards(transform.position, bug.position, moveSpeed * Time.deltaTime);
        transform.position = newPosition;
    }

    private Transform FindClosestBug()
    {
        GameObject[] bugs = GameObject.FindGameObjectsWithTag("EvilBug");
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
}
