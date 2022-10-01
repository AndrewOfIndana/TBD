using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*  
        Name: GameManager.cs
        Description: This script handles game states, counting points, and wins and lose conditions

    // */

    // /*---      SETUP FUNCTIONS     ---*/
    // /*-  Starts on the first frame -*/
    // void Start()
    // {        
    //     Vcams[0].Priority = 1; //sets overworld camera to priority 1
    //     Vcams[1].Priority = 0; //sets player camera to priority 0
    //     Invoke("TempSpawnPlayer" , 5.0f); //Call TempSpawnPlayer in 5 seconds

    // }
    // /*-  Spawns player (TEMPORARY) -*/
    // void TempSpawnPlayer()
    // {
    //     playerAvatar = Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation); //Spawns player prefab and stores it in a GameObject
    //     Vcams[0].Priority = 0; //sets overworld camera to priority 0
    //     Vcams[1].Priority = 1; //sets player camera to priority 1
    //     Vcams[1].Follow = playerAvatar.transform; //sets player camera's follow to the player's transform
    // }
}
