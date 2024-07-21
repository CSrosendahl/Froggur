using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class FrogAttack : MonoBehaviour
{
    [Header("References")]
    public LineRenderer tongueLineRenderer; // Reference to the LineRenderer component for tongue attack
    public GameObject tongueTip; // Reference to the TongueTip GameObject
    public Transform attackPoint; // Reference to the AttackPoint GameObject
    public Collider2D tongueCollider; // Reference to the tongue collider
    public Animator frogAnim; // Reference to the frog animator

    // Tongue Settings
    [Header("Tongue Settings")]
    public float tongueSpeed = 5f; // Speed at which the tongue extends
    public float retractSpeed = 0.5f; // Speed at which the tongue retracts
    public float tongueExtendOffSet = 1.5f;
    public bool tongueAttackActive = true;
    public bool canTongueAttack = true;
   

    // Water Attack Settings
    [Header("Water Attack Settings")]
    public GameObject waterAttackGO; // Reference to the water attack GameObject
    public float waterAttackForce = 10f; // Force applied to the water attack
    public float waterBurstDelay = 0.1f; // Delay between each water droplet in the burst
    public float waterAttackCooldown = 2f; // Cooldown duration for the water attack
    public int waterBurstAttacks = 5;
    public bool waterAttackActive = false;
    public bool canWaterAttack = true;

    [Header("Audio Settings")]
    public AudioClip tongueAttackSound; // Sound for the tongue attack
    public AudioClip bugGrabSound; // Sound for grabbing a bug
    public AudioClip bugEatSound; // Sound for eating a bug
    public AudioClip evilBugEatSound; //  Sound for eating an evil bug
    public AudioClip waterShootSound; // Sound for the water attack

    // Internal Variables
    private Vector3 targetPosition; // Position where the tongue or water extends to
    [HideInInspector] public bool isAttacking = false; // Flag to check if the frog is attacking
    [HideInInspector] public GameObject grabbedBug; // Reference to the grabbed bug

    // UI Layer Mask
    public LayerMask layerMask; // Assign this in the inspector to the desired UI layer

    void Start()
    {
        // Ensure the LineRenderer is properly configured
        tongueLineRenderer.positionCount = 2; // Set the position count to 2
        tongueLineRenderer.enabled = false; // Hide the tongue initially

        tongueTip.SetActive(false); // Hide the tongue tip initially
        tongueCollider.enabled = false; // Disable the collider initially
    }

    void Update()
    {
        if (waterAttackActive && canWaterAttack)
        {
            if (Input.GetMouseButtonDown(0) && !isAttacking && !IsPointerOverLayerMaskObject()) // Detect mouse click for water attack
            {
                Debug.Log("Water attack");
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0; // Set z to 0 for 2D
                targetPosition = mousePos; // Set the target position to the clicked position
                frogAnim.SetTrigger("WaterAttack");
                waterAttackActive = false;
                tongueAttackActive = true;
                StartCoroutine(WaterBurstAttack(targetPosition));
                UIManager.instance.WaterAttackOnCoolDown();
               
            }
        }
        else if (waterAttackActive && !canWaterAttack)
        {
            Debug.Log("Water attack on cooldown");
        }

        else if (tongueAttackActive && canTongueAttack)
        {
            if (Input.GetMouseButtonDown(0) && !isAttacking && !IsPointerOverLayerMaskObject()) // Detect mouse click for tongue attack
            {
                Debug.Log("Tongue attack");
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0; // Set z to 0 for 2D
                targetPosition = mousePos; // Set the target position to the clicked position
                frogAnim.SetTrigger("TongueAttack");
                StartTongueAttack();
              
            }
        }

        if (isAttacking)
        {
            ExtendTongue();
        }
    }

    
 

    void StartTongueAttack()
    {
        tongueLineRenderer.enabled = true; // Show the tongue
        tongueLineRenderer.SetPosition(0, attackPoint.position); // Set the start point of the tongue
        tongueLineRenderer.SetPosition(1, attackPoint.position); // Initialize the end point of the tongue

        tongueTip.SetActive(true); // Show the tongue tip
        tongueTip.transform.position = attackPoint.position; // Initialize the position of the tongue tip

        isAttacking = true;
        canTongueAttack = false;
        tongueCollider.enabled = true; // Enable the collider when attacking
        AudioManager.instance.PlaySound(tongueAttackSound);
    }

    void ExtendTongue()
    {
        // Ensure the tongue starts from the correct position
        tongueLineRenderer.SetPosition(0, attackPoint.position); // Dynamically update the start point

        // Calculate direction and normalize
        Vector3 direction = (targetPosition - tongueLineRenderer.GetPosition(0)).normalized;

        // Add a small offset (e.g., 0.5f) to the target position
        Vector3 extendedTargetPosition = targetPosition + direction * tongueExtendOffSet;

        Vector3 endPosition = Vector3.MoveTowards(tongueLineRenderer.GetPosition(1), extendedTargetPosition, tongueSpeed * Time.deltaTime);

        tongueLineRenderer.SetPosition(1, endPosition); // Update the end point of the tongue
        tongueTip.transform.position = endPosition; // Move the tongue tip to the end point

        // Rotate the tongue tip to face the target position
        direction = endPosition - tongueLineRenderer.GetPosition(0);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        tongueTip.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Vector3.Distance(endPosition, extendedTargetPosition) < 0.1f) // Check if the tongue has reached the extended target
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

            // Update the start position dynamically
            tongueLineRenderer.SetPosition(0, attackPoint.position);
          

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tongueLineRenderer.SetPosition(1, attackPoint.position);
        tongueLineRenderer.enabled = false; // Hide the tongue
        tongueTip.SetActive(false); // Hide the tongue tip
        canTongueAttack = true;
     
        if (grabbedBug != null)
        {
            Bugs bugScript = grabbedBug.GetComponent<Bugs>();

            UIManager.instance.UpdateScore(bugScript.points);
            BugManager.instance.BugDestroyed(grabbedBug.GetComponent<Bugs>().bugSO, grabbedBug);
            
            //   AudioManager.instance.PlaySound(bugEatSound); // Too much sound
            grabbedBug = null;
        }
       


    }

    private IEnumerator WaterBurstAttack(Vector3 targetPosition)
    {
        for (int i = 0; i < waterBurstAttacks; i++)
        {
            WaterAttack(targetPosition);
            yield return new WaitForSeconds(waterBurstDelay);
        }
    }

    public void WaterAttack(Vector3 targetPosition)
    {
        // Instantiate the water attack GameObject at the attack point
        GameObject waterAttack = Instantiate(waterAttackGO, attackPoint.position, Quaternion.identity);
        AudioManager.instance.PlaySound(waterShootSound); // Play the water attack sound
        // Calculate the direction to the target position
        Vector3 direction = (targetPosition - attackPoint.position).normalized;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the rotation of the water attack to match the direction
        waterAttack.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Apply a force to the water attack in the calculated direction
        Rigidbody2D rb = waterAttack.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction * waterAttackForce, ForceMode2D.Impulse);
        }
      
    }


    //private IEnumerator WaterAttackCooldown()
    //{
    //    canWaterAttack = false;
    //    UIManager.instance.WaterAttackOnCoolDown();
    //    yield return new WaitForSeconds(waterAttackCooldown);
    //   // UIManager.instance.ReplenishWaterAttack();
    //    canWaterAttack = true;
    //}

    public void ActivateDeactivateWaterAttack()
    {
        Debug.Log("Activating water attack");
        UIManager.instance.ReplenishWaterAttack(0);
        if(canWaterAttack)
        {
            waterAttackActive = !waterAttackActive;
        } else
        {
            Debug.Log("Water attack inactive");
        }
        UIManager.instance.WaterAttackActive();


    }

    public void AttackState(bool attackState)
    {
        if(attackState)
        {
            UIManager.instance.waterAttackButton.interactable = true;
            tongueAttackActive = true;
            canWaterAttack = true;
        }else
        {
            UIManager.instance.waterAttackButton.interactable = false;
            tongueAttackActive = false;
            canWaterAttack = false;
        }
    }
    private bool IsPointerOverLayerMaskObject() // Function to check if the pointer is over a predefined layermask object
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (((1 << result.gameObject.layer) & layerMask) != 0)
            {
                return true;
            }
        }
        return false;
    }
}
