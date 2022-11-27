using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTroopMovement : TroopMovement
{
    /*  
        Name: FinalTroopMovement.cs
        Description: This script controls the troop's movement during the final boss

    */
    /*-  Sets the troops first waypoint depending on if the unit is an enemy -*/
    public override void StartMovement()
    {
        //the troop is an enemy 
        wavePointIndex = -1; 
        path = WayPointsFinal.points[Random.Range(0, 2)]; //Sets the path transform to the first wayPoint 
        hasReachedEnd = false;
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Update is called once per frame -*/
    protected override void Update()
    {
        //if gameStates is not PLAYING
        if(!gameManager.CheckIfPlaying())
        {
            return;
        }

        if(troopBehaviour.GetTargetDetected() == null && (hasReachedEnd || troopController.GetStats().unitUtils.unitType == UnitType.BOSS))
        {
            /* MOVES TROOP FOLLOWING TROOP TO PLAYER LOCATION */

            //if this position and playerDetected's position is greater 2
            if(Vector3.Distance(transform.position, playerLocation.position) >= 2f)
            {
                dir = playerLocation.position - transform.position; 
                transform.Translate(dir.normalized * troopController.GetSpeed() * Time.deltaTime, Space.World);
                troopController.animator.SetBool("aIdle", false);
            }
            else
            {
                /* STOPS TROOP */

                troopController.animator.SetBool("aIdle", true);
            }
        }
        //if targetDetected doesn't exist, playerDetected doesn't exist, and hasReachedEnd is false
        else if(troopBehaviour.GetTargetDetected() == null && troopController.GetStats().unitUtils.unitType == UnitType.TROOP && !hasReachedEnd)
        {
            /* MOVES TROOP TO WAYPOINT */

            troopController.animator.SetBool("aIdle", false);
            dir = path.position - transform.position;
            transform.Translate(dir.normalized * troopController.GetSpeed() * Time.deltaTime, Space.World);

            //If the troop has reached a waypoint calculate a new waypoint
            if (Vector3.Distance(transform.position, path.position) <= 1f)
            {
                GetNextWaypoint();
            }
        }
        //if the unit's behaviour is defend, playerDetected dose exist, and targetDetected doesn't exist
        else if(troopController.GetStats().unitUtils.unitBehaviour == Behaviour.DEFEND && troopBehaviour.GetPlayerDetected() != null && troopBehaviour.GetTargetDetected() == null)
        {
            /* MOVES TROOP FOLLOWING TROOP TO PLAYER */

            //if this position and playerDetected's position is greater 2
            if(Vector3.Distance(transform.position, troopBehaviour.GetPlayerDetected().position) >= 2f)
            {
                dir = troopBehaviour.GetPlayerDetected().position - transform.position; 
                transform.Translate(dir.normalized * troopController.GetSpeed() * Time.deltaTime, Space.World);
                troopController.animator.SetBool("aIdle", false);

            }
            else
            {
                /* STOPS TROOP */

                troopController.animator.SetBool("aIdle", true);
            }
        }
        else if(troopBehaviour.GetTargetDetected() != null && troopController.GetStats().unitUtils.unitBehaviour != Behaviour.RANGED) //if targetDetected does exist and this unit's behaviour isn't RANGED
        {
            /* MOVES TROOP TO OPPONENT */

            //if this position and targetDetected's position is greater 2
            if(Vector3.Distance(transform.position, troopBehaviour.GetTargetDetected().position) >= 2f)
            {
                dir = troopBehaviour.GetTargetDetected().position - transform.position; 
                transform.Translate(dir.normalized * troopController.GetSpeed() * Time.deltaTime, Space.World);
            }
        }
    }
}
