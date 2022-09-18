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
    public GameObject playerPrefab;
    public Transform playerSpawn;
    public CinemachineVirtualCamera[] Vcams;

    void Start()
    {        
        Vcams[0].Priority = 1;
        Vcams[1].Priority = 0;
        Invoke("TempSpawnPlayer" , 5.0f);
    }

    void TempSpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
        Vcams[0].Priority = 0;
        Vcams[1].Priority = 1;
        Vcams[1].Follow = player.transform;
    }

}
