using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelVariables
{
    [Header ("Bug Settings")]
    public float bugSpawnTime;
    public float evilBugSpawnTime;
    public int maxBugs;
    [Header("Bird Settings")]
    public float birdRespawnInterval;
    public bool birdSpawnEnabled;
    public GameObject bird;
    [HideInInspector] public float birdSpawnDelay = 5f;

    [Header("Misc Settings")]
    public int requiredPointsForNextLevel;


    [Header("Frog Settings")] // Hiding this because it is not being used rn (Can be used to spawn a frog that eats bugs)
    [HideInInspector] public bool frogEnemy_Enabled;
    [HideInInspector] public float frogEnemy_Duration = 10f;





    public LevelVariables(float bugSpawnTime, float evilBugSpawnTime, bool canSpawnBird, float birdSpawnDelay, float birdRespawnInterval)
    {
        this.bugSpawnTime = bugSpawnTime;
        this.evilBugSpawnTime = evilBugSpawnTime;
        this.birdSpawnEnabled = canSpawnBird;
        this.birdSpawnDelay = birdSpawnDelay;
        this.birdRespawnInterval = birdRespawnInterval;
    }
}
