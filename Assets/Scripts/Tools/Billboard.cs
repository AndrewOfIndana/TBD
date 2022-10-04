using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour 
{
    /*  
        Name: Billboard.cs
        Description: This script causes 2D sprites to face the camera

    */    
    private Transform cam;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Start is called before the first frame update -*/
    private void Start() 
    {
        cam = Camera.main.transform;
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  LateUpdate is called once per frame after Update -*/
    private void LateUpdate() 
    {
        transform.LookAt(transform.position + cam.forward);
    }
}

