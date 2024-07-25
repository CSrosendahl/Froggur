using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelVariables
{
    public float bugSpawnTime;
    public float evilBugSpawnTime;
    public float birdSpawnDelay;
    public float birdRespawnInterval;
    public bool birdSpawnEnabled;
    public int requiredPointsForNextLevel;


    public LevelVariables(float bugSpawnTime, float evilBugSpawnTime, bool canSpawnBird, float birdSpawnDelay, float birdRespawnInterval)
    {
        this.bugSpawnTime = bugSpawnTime;
        this.evilBugSpawnTime = evilBugSpawnTime;
        this.birdSpawnEnabled = canSpawnBird;
        this.birdSpawnDelay = birdSpawnDelay;
        this.birdRespawnInterval = birdRespawnInterval;
    }
}
