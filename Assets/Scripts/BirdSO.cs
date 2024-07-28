using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BirdSO", menuName = "Birds/Bird")]
public class BirdSO : ScriptableObject
{

// create an asset menu so I can make a new bug type



    public string birdName = "New bird";

    [Header("Bird Movement/Roaming")]
    public float moveSpeed = 2f; // Speed at which the bird moves
    public float changeDirectionInterval = 2f; // Interval after which the bird changes direction

    [Header("Other options")]
    public int birdHealth = 1;
    public int birdPoints = 1;

    [Header("Bird Chase Bugs")]
    public float catchDistance = 1f; // Distance at which the bird catches the bug
    public float catchBugInterval = 2f; // Interval for checking bugs and roaming

  


}
