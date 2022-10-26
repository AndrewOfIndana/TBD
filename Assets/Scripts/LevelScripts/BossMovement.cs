using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    /*  
        Name: bossMovement.cs
        Description: This script controls the boss's movement

    */
    /*[Header("Static References")]*/
    GameManager gameManager;

    /*[Header("Components")]*/
    private BossController bossController;
    private BossBehaviour bossBehaviour;

    /*[Header("Script Settings")]*/
    private Vector3 dir;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        bossController = this.GetComponent<BossController>();
        bossBehaviour = this.GetComponent<BossBehaviour>();
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Update is called once per frame -*/
    private void Update()
    {
        //if gameStates is not PLAYING
        if(!gameManager.CheckIfPlaying() && bossController.GetStats().unitBehaviour == Behaviour.RANGED)
        {
            return;
        }

        //if targetDetected doesn't exist, playerDetected doesn't exist
        if(bossBehaviour.GetTargetDetected() == null && bossBehaviour.GetPlayerDetected() == null)
        {
            /* MOVES BOSS GENERALLY TO PLAYER BY DEFAULT */
            dir = bossBehaviour.GetPlayerAvatar().position - transform.position;
            transform.Translate(dir.normalized * bossController.GetSpeed() * Time.deltaTime, Space.World);
        }
        //if the boss's behaviour is defend, and playerDetected does exist
        else if(bossController.GetStats().unitBehaviour == Behaviour.DEFEND && bossBehaviour.GetPlayerDetected() != null)
        {
            /* MOVES BOSS TO PLAYER */

            //if this position and playerDetected's position is greater 4
            if(Vector3.Distance(transform.position, bossBehaviour.GetPlayerDetected().position) >= 4f)
            {
                dir = bossBehaviour.GetPlayerDetected().position - transform.position; 
                transform.Translate(dir.normalized * bossController.GetSpeed() * Time.deltaTime, Space.World);
            }
        }
        else if(bossBehaviour.GetTargetDetected() != null && bossBehaviour.GetPlayerDetected() == null) //if targetDetected does exist, and playerDetected doesn't exist
        {
            /* MOVES BOSS TO OPPONENT */

            //if this position and targetDetected's position is greater 2
            if(Vector3.Distance(transform.position, bossBehaviour.GetTargetDetected().position) >= 4f)
            {
                dir = bossBehaviour.GetTargetDetected().position - transform.position; 
                transform.Translate(dir.normalized * bossController.GetSpeed() * Time.deltaTime, Space.World);
            }
        }
    }
}
