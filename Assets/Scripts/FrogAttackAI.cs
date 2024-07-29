using UnityEngine;
using System.Collections;
public class FrogAttackAI : MonoBehaviour
{
    [Header("Evil Frogs stats")]
    public int health = 3;
    public int points;
    public ParticleSystem deathEffect;

    [Header("References")]
    public LineRenderer tongueLineRenderer;
    public GameObject tongueTip;
    public Transform attackPoint;
    public Collider2D tongueCollider;
    public Animator frogAnim;

    [Header("Tongue Settings")]
    public float tongueSpeed = 5f;
    public float retractSpeed = 0.5f;
    public float tongueExtendOffSet = 1.5f;
    public float attackInterval = 2f; // Interval in seconds between each automatic attack
    public bool canTongueAttack = true;

    [Header("Audio Settings")]
    public AudioClip tongueAttackSound;
    public AudioClip bugEatSound;

    // Internal Variables
    private Vector3 targetPosition;
    public bool isAttacking = false;
    public GameObject grabbedBug;

    void Start()
    {
        tongueLineRenderer.positionCount = 2;
        tongueLineRenderer.enabled = false;
        tongueTip.SetActive(false);
        tongueCollider.enabled = false;

        // Start the automatic attack coroutine
        StartCoroutine(AutomaticTongueAttack());
    }

    private void Update()
    {
        if (isAttacking)
        {
            ExtendTongue();
        }
    }

    IEnumerator AutomaticTongueAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            if (canTongueAttack && !isAttacking)
            {
                PerformTongueAttack();
            }
        }
    }

    void PerformTongueAttack()
    {
        // Find all GameObjects with the "Bug" tag
        GameObject[] bugs = GameObject.FindGameObjectsWithTag("Bug");

        if (bugs.Length == 0)
        {
            // No bugs found, return or select a random position (optional)
            return;
        }

        // Initialize the closest distance and the target bug
        float closestDistance = Mathf.Infinity;
        GameObject closestBug = null;

        Vector3 frogPosition = attackPoint.position;

        // Iterate through the bugs to find the closest one
        foreach (GameObject bug in bugs)
        {
            float distance = Vector3.Distance(frogPosition, bug.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestBug = bug;
            }
        }

        if (closestBug != null)
        {
            // Set the target position to the position of the closest bug
            targetPosition = closestBug.transform.position;
            frogAnim.SetTrigger("TongueAttack");
            StartTongueAttack();
        }
    }


    void StartTongueAttack()
    {
        tongueLineRenderer.enabled = true;
        tongueLineRenderer.SetPosition(0, attackPoint.position);
        tongueLineRenderer.SetPosition(1, attackPoint.position);

        tongueTip.SetActive(true);
        tongueTip.transform.position = attackPoint.position;

        isAttacking = true;
        canTongueAttack = false;
        tongueCollider.enabled = true;
        AudioManager.instance.PlaySound(tongueAttackSound);
    }

    void ExtendTongue()
    {
        tongueLineRenderer.SetPosition(0, attackPoint.position);

        Vector3 direction = (targetPosition - tongueLineRenderer.GetPosition(0)).normalized;
        Vector3 extendedTargetPosition = targetPosition + direction * tongueExtendOffSet;

        Vector3 endPosition = Vector3.MoveTowards(tongueLineRenderer.GetPosition(1), extendedTargetPosition, tongueSpeed * Time.deltaTime);
        tongueLineRenderer.SetPosition(1, endPosition);
        tongueTip.transform.position = endPosition;

        direction = endPosition - tongueLineRenderer.GetPosition(0);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        tongueTip.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Vector3.Distance(endPosition, extendedTargetPosition) < 0.1f)
        {
            if (grabbedBug == null)
            {
                isAttacking = false;
                RetractTongue();
            }
        }
    }

    public void RetractTongue()
    {
        StartCoroutine(RetractTongueCoroutine());
    }

    private IEnumerator RetractTongueCoroutine()
    {
        tongueCollider.enabled = false;

        Vector3 startPosition = tongueLineRenderer.GetPosition(1);
        float elapsedTime = 0f;

        while (elapsedTime < retractSpeed)
        {
            Vector3 retractPosition = Vector3.Lerp(startPosition, attackPoint.position, elapsedTime / retractSpeed);
            tongueLineRenderer.SetPosition(1, retractPosition);
            tongueTip.transform.position = retractPosition;

            Vector3 direction = attackPoint.position - retractPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            tongueTip.transform.rotation = Quaternion.Euler(0, 0, angle);

            if (grabbedBug != null)
            {
                grabbedBug.transform.position = retractPosition;
            }

            tongueLineRenderer.SetPosition(0, attackPoint.position);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tongueLineRenderer.SetPosition(1, attackPoint.position);
        tongueLineRenderer.enabled = false;
        tongueTip.SetActive(false);
        canTongueAttack = true;

        if (grabbedBug != null)
        {
            Bugs bugScript = grabbedBug.GetComponent<Bugs>();
          //  UIManager.instance.UpdateScore(bugScript.points);
            BugManager.instance.BugDestroyed(bugScript.bugSO, grabbedBug);
            AudioManager.instance.PlaySound(bugEatSound);
            grabbedBug = null;
        }
    }

    public void TakeDamage(int damage)
    {
        if (health <= 0)
        {
            return;
        }

        health -= damage;


    }
}
