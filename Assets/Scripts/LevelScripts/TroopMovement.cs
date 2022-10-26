using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopMovement : MonoBehaviour
{
    /*  
        Name: TroopMovement.cs
        Description: This script controls the troop's movement

    */
    /*[Header("Static References")]*/
    GameManager gameManager;

    /*[Header("Components")]*/
    private TroopController troopController;
    private TroopBehaviour troopBehaviour;

    /*[Header("Script Settings")]*/
    private Transform path;
    private int wavePointIndex; //Keeps track of which waypoint the troop is at
    private bool hasReachedEnd = false;
    private Vector3 dir;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        troopController = this.GetComponent<TroopController>();
        troopBehaviour = this.GetComponent<TroopBehaviour>();
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
    }
    /*-  Sets the troops first waypoint depending on if the unit is an enemy -*/
    public void StartMovement()
    {
        //if the troop is an enemy 
        if(troopController.GetStats().isUnitEnemy)
        {
            wavePointIndex = 0; 
            path = WayPoints.points[0]; //Sets the path transform to the first wayPoint 
            hasReachedEnd = false;
        }
        else if(!troopController.GetStats().isUnitEnemy) //if the troop isn't an enemy
        {
            wavePointIndex = WayPoints.points.Length - 1; 
            path = WayPoints.points[(WayPoints.points.Length - 1)]; //Sets the path transform to the last wayPoint 
            hasReachedEnd = false;
        }
    }
    
    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Update is called once per frame -*/
    private void Update()
    {
        //if gameStates is not PLAYING
        if(!gameManager.CheckIfPlaying())
        {
            return;
        }

        //if targetDetected doesn't exist, playerDetected doesn't exist, and hasReachedEnd is false
        if(troopBehaviour.GetTargetDetected() == null && troopBehaviour.GetPlayerDetected() == null && !hasReachedEnd)
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
        else if(troopController.GetStats().unitBehaviour == Behaviour.DEFEND && troopBehaviour.GetPlayerDetected() != null && troopBehaviour.GetTargetDetected() == null)
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
        else if(troopBehaviour.GetTargetDetected() != null && troopController.GetStats().unitBehaviour != Behaviour.RANGED) //if targetDetected does exist and this unit's behaviour isn't RANGED
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

    /*---      FUNCTIONS     ---*/
    /*-  Gets the next waypoint for the troop -*/
    private void GetNextWaypoint()
    {
        //if the troop is an enemy 
        if(troopController.GetStats().isUnitEnemy)
        {
            //if the troop reaches the last waypoint
            if(wavePointIndex >= WayPoints.points.Length - 1)
            {
                hasReachedEnd = true;
                return;
            }
            else
            {
                wavePointIndex++;
                path = WayPoints.points[wavePointIndex]; //Sets path to new waypoint
            }
        }
        else if(!troopController.GetStats().isUnitEnemy) //if the troop isn't an enemy
        {
            //if the troop reaches the first waypoint
            if(wavePointIndex <= 0)
            {
                hasReachedEnd = true;
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
