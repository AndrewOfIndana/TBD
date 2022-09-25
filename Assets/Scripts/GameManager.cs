using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    /*  
        Name: GameManager.cs
        Description: This script handles game states, counting points, and wins and lose conditions

    */
    public GameObject playerPrefab; //Reference to the player prefab GameObject
    public Transform playerSpawn; //Reference to the player spawn Transform
    public CinemachineVirtualCamera[] Vcams; //Array of Cinemachine Virtual Cameras references

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts on the first frame -*/
    void Start()
    {        
        Vcams[0].Priority = 1; //sets overworld camera to priority 1
        Vcams[1].Priority = 0; //sets player camera to priority 0
        Invoke("TempSpawnPlayer" , 5.0f); //Call TempSpawnPlayer in 5 seconds

    }
    /*-  Spawns player (TEMPORARY) -*/
    void TempSpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation); //Spawns player prefab and stores it in a GameObject
        Vcams[0].Priority = 0; //sets overworld camera to priority 0
        Vcams[1].Priority = 1; //sets player camera to priority 1
        Vcams[1].Follow = player.transform; //sets player camera's follow to the player's transform
    }

}
