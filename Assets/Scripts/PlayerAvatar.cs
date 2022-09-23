using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    /*  
        Name: PlayerAvatar.cs
        Description: This script controls the player avatar
        
    */
    private Rigidbody playerRb; //Reference to a rigidbody

    [Header("Movement Variables")]
    public float playerSpeed = 10f; //Stores the default speed of the player
    private Vector3 velocity; //Stores the velocity of the player

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts when script is awake -*/
    void Awake()
    {
        playerRb = this.GetComponent<Rigidbody>(); //Gets the rigidbody component
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Is called every frame -*/
    void Update()
    {
        velocity.x = Input.GetAxis("Horizontal"); //Set velocity x from the input horizontal axis
        velocity.z = Input.GetAxis("Vertical"); //Set velocity z from the input vertical axis

        playerRb.MovePosition(playerRb.position + velocity * playerSpeed * Time.deltaTime); //Moves player
    }
}
