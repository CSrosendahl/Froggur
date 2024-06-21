using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BugSO;

public class WaterAttackScript : MonoBehaviour
{
    public ParticleSystem splashParticle; // Assign your WaterSplashParticle here

    private void Start()
    {
        StartCoroutine(WaitToDestroy());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bug") || collision.gameObject.CompareTag("EvilBug"))
        {
            GameObject bugGO = collision.gameObject;

       //     Bugs bugScript = bugGO.GetComponent<Bugs>(); // Not being used rn

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
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
