using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    /*  
        Name: Tile.cs
        Description: This script checks if a tile is used, handles user input, and allows the player to build a tower.

    */
    private GameObject tower;
    public Vector3 positionOffset;

    /*---      FUNCTIONS     ---*/
    /*-  Checks if a mouse is on screen -*/
    void OnMouseDown()
    {
        if(tower != null)
        {
            Debug.Log("Already has tower");
            return;
        }

        Stats towerToBuild = PlayerUnits.playerUnitsInstance.GetUnitToPlace();
        GameObject playerTowerObj = ObjectPool.objectPoolInstance.SpawnFromPool("PlayerTower", transform.position + positionOffset, Quaternion.identity); //Spawn an player tower from the pool
        PlayerTowerController playerTower = playerTowerObj.GetComponent<PlayerTowerController>(); //Gets the PlayerTowerController component from the spawned playerTowerObj
        
        //if this playerTower exist
        if(playerTower != null)
        {
            playerTower.SetUnit(towerToBuild); //Sets enemy type based on random number generator
            playerTower.firingPoint.transform.position = transform.position + positionOffset;
        }
    }
}
