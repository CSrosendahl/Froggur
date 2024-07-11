using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterAttackScript : MonoBehaviour
{
    public ParticleSystem splashParticle; // Assign your WaterSplashParticle here
    public AudioClip waterHitSound; // Sound for the water attack
    


    private void Start()
    {
        StartCoroutine(WaitToDestroy());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bug") || collision.gameObject.CompareTag("EvilBug"))
        {
            GameObject bugGO = collision.gameObject;

            GameObject waterDropGO = this.gameObject;

            int evilBugPoint = 1;

            if(bugGO.CompareTag("EvilBug"))
            {

                UIManager.instance.PointPrompt(evilBugPoint, collision.transform);
                UIManager.instance.UpdateScore(evilBugPoint);
            }

            ParticleSystem splash = Instantiate(splashParticle, transform.position, Quaternion.identity);
            splash.Play();
            Destroy(splash.gameObject, splash.main.duration);

            Destroy(bugGO);
            Destroy(waterDropGO);
        }

        if (collision.gameObject.CompareTag("Bird")) {

            Bird birdScript = collision.gameObject.GetComponent<Bird>();
            int birdPoint = birdScript.birdPoints;

            GameObject birdGO = collision.gameObject;

            GameObject waterDropGO = this.gameObject;

            birdScript.TakeDamage(1);

            ParticleSystem splash = Instantiate(splashParticle, transform.position, Quaternion.identity);
            splash.Play();

            Destroy(splash.gameObject, splash.main.duration);
            Destroy(waterDropGO);


            if (birdScript.birdHealth <= 0)
            {
                ParticleSystem birdDeathEffect = Instantiate(birdScript.birdDeathEffect, transform.position, Quaternion.identity);
                birdDeathEffect.Play();
              

                UIManager.instance.PointPrompt(birdPoint, collision.transform);
                UIManager.instance.UpdateScore(birdPoint);


                TurtleFriend.instance.PlayAnimation("HappyJump2");
                AudioManager.instance.PlaySound(TurtleFriend.instance.hitBirdSound);
                Destroy(birdDeathEffect.gameObject, 1);
                Destroy(birdGO);
  
            }
          

           
        }
        AudioManager.instance.PlaySound(waterHitSound);

    }
    IEnumerator WaitToPlayAnim(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
       
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
