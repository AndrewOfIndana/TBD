using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabber : MonoBehaviour
{
    /*  
        Name: PlayerGrabber.cs
        Description: This Script controls the dragging and deployment of the player's towers

    */    
    /*[Header("Static Variables")]*/
    LevelManager levelManager;
    ObjectPool objectPool; 

    [Header("Script Variables")]
    public float snapSensitivity = 2;
    public float dragOffset = 5f;
    public float deployOffset = 1.5f;

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

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Update is called once per frame -*/
    private void Update()
    {
        //if selectedTower does exist 
        if(selectedTower != null)
        {
            /* dragging code */
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedTower.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedTower.transform.position = new Vector3(worldPosition.x, worldPosition.y, Camera.main.transform.position.z + dragOffset);

            /* dropping code */
            //if the player clicks the left mouse button
            if(Input.GetMouseButtonDown(0))
            {
                snappedPosition = levelManager.playerAvatar.closestTilt.position; //Gets the last tile transform from the playerAvatar
                selectedTower.transform.position = new Vector3(snappedPosition.x, snappedPosition.y + deployOffset, snappedPosition.z);
                selectedTower = null;
                Cursor.visible = true;
            }
        }
    }
    /*---      FUNCTIONS     ---*/
    /*-  Sets the selected tower from the player controller -*/
    public void SetSelectedTower(Stats unitToSpawn)
    {
        selectedTower = objectPool.SpawnFromPool("AllyTower", transform.position, Quaternion.identity); 
        TowerController allyTower = selectedTower.GetComponent<TowerController>();

        //if this unit exist
        if(allyTower != null)
        {
            allyTower.SetUnit(unitToSpawn); 
        }
        Cursor.visible = false;
    }
}
