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
            frogAttack.RetractTongue();
            frogAttack.isAttacking = false;
        
        }
    }
}
