using UnityEngine;
using System.Collections;

public class FrogAttack : MonoBehaviour
{
    public LineRenderer tongueLineRenderer; // Reference to the LineRenderer component
    public GameObject tongueTip; // Reference to the TongueTip GameObject
    public Transform attackPoint; // Reference to the AttackPoint GameObject
    public float tongueSpeed = 5f; // Speed at which the tongue extends
    public float retractSpeed = 0.5f; // Speed at which the tongue retracts

    private Vector3 targetPosition; // Position where the tongue extends to
    [HideInInspector] public bool isAttacking = false; // Flag to check if the frog is attacking
    public Collider2D tongueCollider;

    public GameObject grabbedBug; // Reference to the grabbed bug
    public Animator frogAnim;

    void Start()
    {
        // Ensure the LineRenderer is properly configured
        tongueLineRenderer.positionCount = 2; // Set the position count to 2
        tongueLineRenderer.startWidth = 0.1f; // Set the start width
        tongueLineRenderer.endWidth = 0.1f; // Set the end width
        tongueLineRenderer.startColor = Color.red; // Set the start color
        tongueLineRenderer.endColor = Color.red; // Set the end color
        tongueLineRenderer.enabled = false; // Hide the tongue initially

        tongueTip.SetActive(false); // Hide the tongue tip initially
        tongueCollider.enabled = false; // Disable the collider initially
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking) // Detect mouse click
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Set z to 0 for 2D
            targetPosition = mousePos; // Set the target position to the clicked position
            frogAnim.SetTrigger("Attack");
            StartAttack();
        }

        if (isAttacking)
        {
            ExtendTongue();
        }
    }

    void StartAttack()
    {
        tongueLineRenderer.enabled = true; // Show the tongue
        tongueLineRenderer.SetPosition(0, attackPoint.position); // Set the start point of the tongue
        tongueLineRenderer.SetPosition(1, attackPoint.position); // Initialize the end point of the tongue

        tongueTip.SetActive(true); // Show the tongue tip
        tongueTip.transform.position = attackPoint.position; // Initialize the position of the tongue tip

        isAttacking = true;
        tongueCollider.enabled = true; // Enable the collider when attacking
    }

    void ExtendTongue()
    {
        Vector3 endPosition = Vector3.MoveTowards(tongueLineRenderer.GetPosition(1), targetPosition, tongueSpeed * Time.deltaTime);

        tongueLineRenderer.SetPosition(1, endPosition); // Update the end point of the tongue
        tongueTip.transform.position = endPosition; // Move the tongue tip to the end point

        // Rotate the tongue tip to face the target position
        Vector3 direction = endPosition - tongueLineRenderer.GetPosition(0);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        tongueTip.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Vector3.Distance(endPosition, targetPosition) < 0.1f) // Check if the tongue has reached the target
        {
            if (grabbedBug == null)
            {
                isAttacking = false;
                RetractTongue(); // Retract the tongue immediately
            }
        }
    }

    public void RetractTongue()
    {
        StartCoroutine(RetractTongueCoroutine());
    }

    private IEnumerator RetractTongueCoroutine()
    {
        tongueCollider.enabled = false; // Disable the collider when retracting

        Vector3 startPosition = tongueLineRenderer.GetPosition(1);
        float elapsedTime = 0f;

        while (elapsedTime < retractSpeed)
        {
            Vector3 retractPosition = Vector3.Lerp(startPosition, attackPoint.position, elapsedTime / retractSpeed);
            tongueLineRenderer.SetPosition(1, retractPosition);
            tongueTip.transform.position = retractPosition;

            // Rotate the tongue tip to face the frog
            Vector3 direction = attackPoint.position - retractPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            tongueTip.transform.rotation = Quaternion.Euler(0, 0, angle);

            if (grabbedBug != null)
            {
                grabbedBug.transform.position = retractPosition; // Move the bug along with the tongue tip
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tongueLineRenderer.SetPosition(1, attackPoint.position);
        tongueLineRenderer.enabled = false; // Hide the tongue
        tongueTip.SetActive(false); // Hide the tongue tip

        if (grabbedBug != null)
        {
            Bugs bugScript = grabbedBug.GetComponent<Bugs>();

            UIManager.instance.UpdateScore(bugScript.points);
            BugManager.instance.BugDestroyed(grabbedBug.GetComponent<Bugs>().bugSO, grabbedBug);
            grabbedBug = null;
          
        }
    }


   

}
