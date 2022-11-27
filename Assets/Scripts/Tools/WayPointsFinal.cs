using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointsFinal : MonoBehaviour
{
    /*  
        Name: WayPointsFinal.cs
        Description: This script holds a static list of transforms of each waypoint, and changes the order of the points by the way they are oriented in the editor
        
    */
    public static Transform[] points;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        points = new Transform[transform.childCount]; //Initializes an array with a max number named points

        for(int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i); //Adds and orders points based on how it is layered in the editor
        }
    }
}
