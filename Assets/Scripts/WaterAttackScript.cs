using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAttackScript : MonoBehaviour
{
    public ParticleSystem splashParticle; // Assign your WaterSplashParticle here

    private void Start()
    {
        StartCoroutine(WaitToDestroy());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bug"))
        {
            GameObject bugGO = collision.gameObject;
            GameObject waterDropGO = this.gameObject;

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
