using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopMovement : MonoBehaviour
{
    /*  
        Name: TroopMovement.cs
        Description: This script controls the troop's movement

    */
    /*[Header("Script References")]*/
    private TroopController troopController;
    private TroopBehaviour troopBehaviour;
    private Transform path;
    private int wavePointIndex; //Keeps track of which waypoint the troop is at

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        troopController = this.GetComponent<TroopController>();
        troopBehaviour = this.GetComponent<TroopBehaviour>();
    }
    /*-  Sets the troops first waypoint depending on if the unit is an enemy -*/
    public void StartMovement()
    {
        //if the troop is an enemy 
        if(troopController.stat.isUnitEnemy)
        {
            wavePointIndex = 0; 
            path = WayPoints.points[0]; //Sets the path transform to the first wayPoint 
        }
        else if(!troopController.stat.isUnitEnemy) //if the troop isn't an enemy
        {
            wavePointIndex = WayPoints.points.Length - 1; 
            path = WayPoints.points[(WayPoints.points.Length - 1)]; //Sets the path transform to the last wayPoint 
        }
    }
    
    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Update is called once per frame -*/
    private void Update()
    {
        //if targetDetected doesn't exist and this unit's behaviour isn't DEFEND
        if(troopBehaviour.targetDetected == null && troopController.stat.unitBehaviour != Behaviour.DEFEND)
        {
            /* MOVES TROOP TO WAYPOINT */

            Vector3 dir = path.position - transform.position;
            transform.Translate(dir.normalized * troopController.speed * Time.deltaTime, Space.World);

            //If the troop has reached a waypoint calculate a new waypoint
            if (Vector3.Distance(transform.position, path.position) <= 1f)
            {
                GetNextWaypoint();
            }
        }
        else if(troopBehaviour.targetDetected != null && troopController.stat.unitBehaviour != Behaviour.RANGED) //if targetDetected does exist and this unit's behaviour isn't RANGED
        {
            /* MOVES TROOP TO OPPONENT */

            if(Vector3.Distance(transform.position, troopBehaviour.targetDetected.position) >= 2f)
            {
                Vector3 dir = troopBehaviour.targetDetected.position - transform.position; 
                transform.Translate(dir.normalized * troopController.speed * Time.deltaTime, Space.World);
            }
        }
    }

    /*---      FUNCTIONS     ---*/
    /*-  Gets the next waypoint for the troop -*/
    private void GetNextWaypoint()
    {
        //if the troop is an enemy 
        if(troopController.stat.isUnitEnemy)
        {
            //if the troop reaches the last waypoint
            if(wavePointIndex >= WayPoints.points.Length - 1)
            {
                this.gameObject.SetActive(false);
                return;
            }
            else
            {
                wavePointIndex++;
                path = WayPoints.points[wavePointIndex]; //Sets path to new waypoint
            }
        }
        else if(!troopController.stat.isUnitEnemy) //if the troop isn't an enemy
        {
            //if the troop reaches the first waypoint
            if(wavePointIndex <= 0)
            {
                this.gameObject.SetActive(false);
                return;
            }
            else
            {
                wavePointIndex--; 
                path = WayPoints.points[wavePointIndex]; //Sets path to new waypoint
            }
        }
    }

}
