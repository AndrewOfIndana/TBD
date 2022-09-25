using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    /*  
        Name: Tile.cs
        Description: This script checks if a mouse is on a tile, allows the player to create a tower on top of said tile, and checks if the tile is used or not

    */
    public Vector3 positionOffset; //Stores the offset of an spawned GameObject tower
    private PlayerController playerController;

    void Start()
    {
        playerController = PlayerController.playerControllerInstance;
    }

    /*---      FUNCTIONS     ---*/
    /*-  Checks if a mouse is on screen -*/
    void OnMouseDown()
    {
        // if(Input.GetMouseButtonDown(1))
        // {
            if(playerController.CheckManaCost(playerController.towerToPlace.unitCost))
            {
                GameObject playerTowerObj = ObjectPool.objectPoolInstance.SpawnFromPool("AllyTower", transform.position + positionOffset, Quaternion.identity); //Spawn an player tower from the pool
                TowerController playerTower = playerTowerObj.GetComponent<TowerController>(); //Gets the PlayerTowerController component from the spawned playerTowerObj

                //if this playerTower exist
                if(playerTower != null)
                {
                    playerTower.SetUnit(playerController.GetTowerToPlace()); //Sets player tower type and stats using playerControllerInstance.GetTowerToPlace()
                    playerTower.firingPoint.transform.position = playerTower.firingPoint.transform.position + positionOffset; //Offsets playerTower's firingPoint's position
                }
            }
        // }
    }
}
