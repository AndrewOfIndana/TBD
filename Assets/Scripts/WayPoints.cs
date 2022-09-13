using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    /*  
        Name: GameManager.cs
        Description: This script controls the waypoint system

    */

    public static Transform[] points;  //A static transform array that can be accessed in any script 

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts when the script is awake -*/
    void Awake()
    {
        points = new Transform[transform.childCount]; //Instantiates a new transform array with a max number named points

        //for loop with the max being the points.length
        for(int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i); //Adds and orders points based on how it is layered in the editor
        }
    }
}
