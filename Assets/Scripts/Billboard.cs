using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour 
{
    /*  
        Name: Billboard.cs
        Description: This script allows for object to face the camera

    */
    private Transform cam; //Private transform reference to a camera

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts on the first frame -*/
    private void Start() 
    {
        cam = Camera.main.transform; //Get the main camera's transform
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Is called every frame late -*/
    void LateUpdate() 
    {
        transform.LookAt(transform.position + cam.forward); //Faces object toward the camera
    }
}

