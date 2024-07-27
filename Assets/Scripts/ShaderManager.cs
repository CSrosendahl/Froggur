using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderManager : MonoBehaviour
{
    // create instance
    public static ShaderManager instance;

    public Material glowMat;
    public Material defaultMat;
    public Material frogEatEvilBugMat;

    public GameObject frogGO;
 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
 
    public void GlowShader(GameObject bugGo)
    {
        // Create a new instance of the material based on the existing 'test' material
        Material newMaterial = new Material(glowMat);

        // Optionally, modify the new material as needed
        // Apply the new material to the desired object
        bugGo.GetComponent<SpriteRenderer>().material = newMaterial;

       
       
    }
    public void AssignShaderMat(GameObject go, Material mat, bool hasTimer,float duration)
    {
       

        Material newMaterial = new Material(mat);

        go.GetComponent<SpriteRenderer>().material = newMaterial;


        if(hasTimer)
        {
            StartCoroutine(TimeBeforeChangingMat(duration,go));
        }
    }
    IEnumerator TimeBeforeChangingMat(float duration, GameObject go)
    {
        yield return new WaitForSeconds(duration);
        SetDefaultShaderMat(go);
    }

    public void SetDefaultShaderMat(GameObject go)
    {
        go.GetComponent<SpriteRenderer>().material = defaultMat;
    }
   
   

}
