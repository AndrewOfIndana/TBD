using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour 
{
    /*  
        Name: Billboard.cs
        Description: This script forces GameObjects to face the camera

    */
    private Transform cam; //Private transform reference to a current camera

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts on the first frame -*/
    private void Start() 
    {
        cam = Camera.main.transform; //Sets cam as the main camera's transform
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Is called every frame late -*/
    void LateUpdate() 
    {
        transform.LookAt(transform.position + cam.forward); //Faces GameObjects towards the camera
    }
}

