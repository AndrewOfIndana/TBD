using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTowerDeployer : MonoBehaviour
{
    /*  
        Name: PlayerGrabber.cs
        Description: This Script controls the dragging and deployment of the player's towers

    */
    /*[Header("Static Variables")]*/
    LevelManager levelManager;
    ObjectPool objectPool;

    [Header("Script Variables")]
    public float yOffset = 1.5f;

    /*[Header("Script References")]*/
    private GameObject selectedTower;
    private Vector3 snappedPosition;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        levelManager = LevelManager.levelManagerInstance;
        objectPool = ObjectPool.objectPoolInstance;
    }

    /*---      FUNCTIONS     ---*/
    /*-  Sets the selected tower from the player controller -*/
    public void SetSelectedTower(Stats unitToSpawn)
    {
        selectedTower = objectPool.SpawnFromPool("AllyTower", transform.position, Quaternion.identity);
        TowerController allyTower = selectedTower.GetComponent<TowerController>();

        //if this unit exist
        if (allyTower != null)
        {
            allyTower.SetUnit(unitToSpawn);
            allyTower.StartController();
        }

        /* Deploys tower */
        snappedPosition = levelManager.playerAvatar.closestTile.position;
        snappedPosition.y += yOffset;
        selectedTower.transform.position = snappedPosition;
        selectedTower = null;
    }
}
