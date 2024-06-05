using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueTip : MonoBehaviour
{
    public FrogAttack frogAttack; // Reference to the FrogAttack script
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bug"))
        {
          
            Debug.Log("Caught bug yum yum!");

            frogAttack.grabbedBug = other.gameObject; // Grab the bug
            int bugPoint = frogAttack.grabbedBug.GetComponent<Bugs>().points;

            UIManager.instance.PointPrompt(bugPoint, other.transform);
            frogAttack.RetractTongue();
            frogAttack.isAttacking = false;
        
        }
    }
}
