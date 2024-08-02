using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueTip : MonoBehaviour
{
    public FrogAttack frogAttack; // Reference to the FrogAttack script
    public AudioClip hitBird;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bird"))
        {
            AudioManager.instance.PlaySoundNoSkip(hitBird);
           
            frogAttack.RetractTongue();
            frogAttack.isAttacking = false;
            ShaderManager.instance.AssignShaderMat(frogAttack.transform.gameObject, ShaderManager.instance.frogEatEvilBugMat, true, 1.5f);

            frogAttack.canTongueAttack = false;
            StartCoroutine(frogAttack.ResetTongueAttack());

        }

        if (other.CompareTag("Bug") || other.CompareTag("EvilBug"))
        {
          
          

            AudioManager.instance.PlaySoundNoSkip(frogAttack.bugGrabSound);
            Debug.Log("Caught bug yum yum!");

            frogAttack.grabbedBug = other.gameObject; // Grab the bug


           Instantiate(frogAttack.catchBugParticle, other.transform.position, Quaternion.identity);
            
            
            int bugPoint = frogAttack.grabbedBug.GetComponent<Bugs>().points;

            UIManager.instance.PointPrompt(bugPoint, other.transform);
            frogAttack.RetractTongue();
            frogAttack.isAttacking = false;
          

        }
       
    }
}
