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
    protected GameManager gameManager;
    protected LevelManager levelManager;

    /*[Header("Components")]*/
    protected TroopController troopController;
    protected TroopBehaviour troopBehaviour;

    /*[Header("Script Settings")]*/
    protected Transform path;
    protected int wavePointIndex; //Keeps track of which waypoint the troop is at
    protected bool hasReachedEnd = false;
    protected Vector3 dir;
    protected Transform playerLocation;

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
        levelManager = LevelManager.instance;
        playerLocation = levelManager.GetPlayerAvatar().transform;
    }
    /*-  Sets the troops first waypoint depending on if the unit is an enemy -*/
    public virtual void StartMovement()
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
    protected virtual void Update()
    {
        //if gameStates is not PLAYING
        if(!gameManager.CheckIfPlaying())
        {
            return;
        }

        if(troopBehaviour.GetTargetDetected() == null && hasReachedEnd)
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
        else if(troopBehaviour.GetTargetDetected() == null && troopBehaviour.GetPlayerDetected() == null && !hasReachedEnd)
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

    /*---      FUNCTIONS     ---*/
    /*-  Gets the next waypoint for the troop -*/
    protected void GetNextWaypoint()
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
