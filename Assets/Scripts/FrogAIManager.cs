using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static FrogAIManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public GameObject enemyFrog_GO;

    private void Start()
    {
       
    }

    public void EnableEnemyFrog()
    {
        float duration = LevelManager.instance.levelVariables[LevelManager.instance.currentLevel].frogEnemy_Duration;
        enemyFrog_GO.SetActive(true);
        StartCoroutine(DisableFrogAfterTime(duration));
    }

    IEnumerator DisableFrogAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        enemyFrog_GO.SetActive(false);
    }
}
