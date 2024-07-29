using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueTipAI : MonoBehaviour
{
    public FrogAttackAI frogAttack; // Reference to the FrogAttack script
                                    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bug"))
        {



         //   AudioManager.instance.PlaySoundNoSkip(frogAttack.bugGrabSound);
            Debug.Log("Frog AI caught bug yum yum!");

            frogAttack.grabbedBug = other.gameObject; // Grab the bug


           // Instantiate(frogAttack.catchBugParticle, other.transform.position, Quaternion.identity);


            frogAttack.RetractTongue();
            frogAttack.isAttacking = false;


        }
    }
}
