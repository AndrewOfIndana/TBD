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
        }
        else if(!troopController.GetStats().isUnitEnemy) //if the troop isn't an enemy
        {
            wavePointIndex = WayPoints.points.Length - 1; 
            path = WayPoints.points[(WayPoints.points.Length - 1)]; //Sets the path transform to the last wayPoint 
        }
    }
    
    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Update is called once per frame -*/
    private void Update()
    {
        //if gameStates is not PLAYING
        if(gameManager.GetGameState() != GameStates.PLAYING)
        {
            return;
        }

        //if targetDetected doesn't exist and this unit's behaviour isn't DEFEND
        if(troopBehaviour.GetTargetDetected() == null && troopBehaviour.GetPlayerDetected() == null)
        {
            /* MOVES TROOP TO WAYPOINT */
            
            troopController.animator.SetBool("aAttacking", false);
            troopController.animator.SetBool("aIdle", false);
            Vector3 dir = path.position - transform.position;
            transform.Translate(dir.normalized * troopController.GetSpeed() * Time.deltaTime, Space.World);

            //If the troop has reached a waypoint calculate a new waypoint
            if (Vector3.Distance(transform.position, path.position) <= 1f)
            {
                GetNextWaypoint();
            }
        }
        else if(troopController.GetStats().unitBehaviour == Behaviour.DEFEND && troopBehaviour.GetPlayerDetected() != null && troopBehaviour.GetTargetDetected() == null)
        {
            if(Vector3.Distance(transform.position, troopBehaviour.GetPlayerDetected().position) >= 2f)
            {
                Vector3 dir = troopBehaviour.GetPlayerDetected().position - transform.position; 
                transform.Translate(dir.normalized * troopController.GetSpeed() * Time.deltaTime, Space.World);
                troopController.animator.SetBool("aIdle", false);

            }
            else
            {
                troopController.animator.SetBool("aIdle", true);
            }
        }
        else if(troopBehaviour.GetTargetDetected() != null && troopController.GetStats().unitBehaviour != Behaviour.RANGED) //if targetDetected does exist and this unit's behaviour isn't RANGED
        {
            /* MOVES TROOP TO Player */

            if(Vector3.Distance(transform.position, troopBehaviour.GetTargetDetected().position) >= 2f)
            {
                Vector3 dir = troopBehaviour.GetTargetDetected().position - transform.position; 
                transform.Translate(dir.normalized * troopController.GetSpeed() * Time.deltaTime, Space.World);
            }
            else if(troopController.GetStats().unitBehaviour != Behaviour.SUPPORT)
            {
                troopController.animator.SetBool("aAttacking", true);
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
                this.gameObject.SetActive(false);
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
