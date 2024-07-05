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
    public float cooldownDuration = 5f; // Duration of the cooldown
    public float alphaChangeDuration = 1f; // Duration over which alpha changes
    private float cooldownTimer = 0f;
    private bool isOnCooldown = false;

    private Transform targetBug;
    private bool isEatingBugs = false;
    private Material turtleMaterial;
    private Color originalColor;

    private void Start()
    {
        turtleMaterial = GetComponent<Renderer>().material;
        originalColor = turtleMaterial.color;
    }

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
        if (!isOnCooldown && BugsAvailable())
        {
         //   PlayAnimation("HappyJump");
            StartEatingBugs();
        }
        PlayAnimation("HappyJump");
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
                    if (hit.transform == transform && !isOnCooldown && BugsAvailable())
                    {
                    
                        StartEatingBugs();
                    }
                }
                PlayAnimation("HappyJump");
            }
        }

        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                isOnCooldown = false;
                StartCoroutine(ChangeAlpha(0f, 1f, alphaChangeDuration)); // Fade in
            }
        }
    }

    private bool BugsAvailable()
    {
        GameObject[] bugs = GameObject.FindGameObjectsWithTag("EvilBug");
        return bugs.Length > 0;
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
                    int eatBugPoint = 1;
                    UIManager.instance.PointPrompt(eatBugPoint, targetBug);
                    UIManager.instance.UpdateScore(eatBugPoint);
                    Destroy(targetBug.gameObject);
                    targetBug = null;
                }
            }

            yield return null;
        }

        isEatingBugs = false;
        turtleAnim.enabled = true;

        // Start cooldown
        isOnCooldown = true;
        cooldownTimer = cooldownDuration;
        StartCoroutine(ChangeAlpha(1f, 0f, alphaChangeDuration)); // Fade out
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

    private IEnumerator ChangeAlpha(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color color = turtleMaterial.color;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            color.a = alpha;
            turtleMaterial.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        turtleMaterial.color = color;
    }
}
