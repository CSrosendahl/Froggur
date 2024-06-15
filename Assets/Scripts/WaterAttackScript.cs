using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAttackScript : MonoBehaviour
{
  
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bug"))
        {
            GameObject bugGO = collision.gameObject;
            GameObject waterDropGO = this.gameObject;

            Destroy(bugGO);
            Destroy(waterDropGO);
        }
    }
}
