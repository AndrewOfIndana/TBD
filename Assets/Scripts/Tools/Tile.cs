using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    /*  
        Name: Tile.cs
        Description: This script holds a static list of transforms of each tile

    */
    public static List<Transform> tiles = new List<Transform>(); //A static list of transform

    /*---      SETUP FUNCTIONS     ---*/
    /*-  OnEnable is called when the object becomes enabled -*/
    private void OnEnable()
    {
        tiles.Add(this.gameObject.transform); //Adds this transform to tiles
    }

    /*---      FUNCTIONS     ---*/
    /*-  Returns the lists of transform -*/
    public static List<Transform> GetTiles()
    {
        return tiles;
    }
    /*-  OnDisable is called when the object becomes disabled -*/
    private void OnDisable()
    {
        tiles.Remove(this.gameObject.transform); //Removes this transform to tiles
    }
}
