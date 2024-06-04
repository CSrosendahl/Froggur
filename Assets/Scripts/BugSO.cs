using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// create an asset menu so I can make a new bug type

[CreateAssetMenu(fileName = "BugSO", menuName = "Bugs/Bug") ]
public class BugSO : ScriptableObject
{

    public string bugName = "New bug";
    public float changeDirectionInterval = 2f;
    public float bugSpeed = 1f;
    public int points = 1;


    public BugType bugType;
    public enum BugType
    {
        Fly,
        Dragonfly,
        Mosquito,
        Wasp,
        DaddyLongLegs
    }

    //private void OnValidate()
    //{
    //    switch (bugType)
    //    {
    //        case BugType.Fly:
    //            bugSpeed = 1;
    //            points = 1;
    //            break;
    //        case BugType.Dragonfly:
    //            bugSpeed = 2;
    //            points = 2;
    //            break;
    //        case BugType.Mosquito:
    //            bugSpeed = 3;
    //            points = 3;
    //            break;
    //        case BugType.Wasp:
    //            bugSpeed = 4;
    //            points = 4;
    //            break;
    //        case BugType.DaddyLongLegs:
    //            bugSpeed = 5;
    //            points = 5;
    //            break;
    //    }
    //}


}
