using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    /*  
        Name: TowerBehaviour.cs
        Description: This script controls the behaviour of a tower and how it reacts to other units and damage

    */
    /*[Header("Static References")]*/
    GameManager gameManager;
    ObjectPool objectPool; 

    /*[Header("Components")]*/
    private TowerController towerController;

    [Header("Script Settings")]
    public Transform firingPoint; 
    private Transform targetDetected;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        towerController = this.GetComponent<TowerController>();
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
        objectPool = ObjectPool.instance; 
    }

    /*-  Starts the units targeting behaviour -*/
    public void StartBehaviour()
    {
        StartCoroutine(UpdateTarget(towerController.GetAttackRate())); //Calls UpdateTarget IEnumerator at attackRate
    }

    /*---      FUNCTIONS     ---*/
    /*-  Repeatedly updates a target and shoots, takes a float for the time -*/
    private IEnumerator UpdateTarget(float time)
    {
        yield return new WaitForSeconds(time); 

        //if gameStates is PLAYING
        if(gameManager.GetGameState() == GameStates.PLAYING)
        {
            Targeting();
            
            //if targetDetected does exist
            if(targetDetected != null)
            {
                Shoot();
            }
        }

        //if gameStates isn't WIN or LOSE
        if(!(gameManager.GetGameState() == GameStates.WIN 
        || gameManager.GetGameState() == GameStates.LOSE))
        {
            StartCoroutine(UpdateTarget(towerController.GetAttackRate())); //Recalls Aiming IEnumerator at attackRate
        }
    }
    /*-  Controls targeting -*/
    private void Targeting()
    {
        float shortestDistance = Mathf.Infinity; 
        Transform nearestTarget = null;

        foreach(Unit unit in Unit.GetUnitList())
        {
            for(int i = 0; i < towerController.GetStats().targetTags.Length; i++)
            {
                //If the unit's tag is the target tag
                if(unit.gameObject.tag == towerController.GetStats().targetTags[i])
                {
                    float distanceToTarget = Vector3.Distance(transform.position, unit.transform.position); //calculates the distance to that enemy

                    //if the distanceToTarget is lesser than shortestDistance
                    if(distanceToTarget < shortestDistance)
                    {
                        shortestDistance = distanceToTarget; 
                        nearestTarget = unit.transform;
                    }
                }
            }
        }
        //if the nearestTarget does exist and shortestDistance is less than or equal to the tower's range
        if(nearestTarget != null && shortestDistance <= towerController.GetStats().unitAttackRange)
        {
            targetDetected = nearestTarget;
            towerController.animator.SetBool("aAttacking", true);
        }
        else
        {
            targetDetected = null;
            towerController.animator.SetBool("aAttacking", false);
        }
    }
    /*-  Controls shooting -*/
    private void Shoot()
    {
        GameObject bulletObj = objectPool.SpawnFromPool(towerController.GetStats().sharedTags.bulletTag, firingPoint.position, firingPoint.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        //if this bullet exist
        if(bullet != null)
        {
            bullet.Seek(targetDetected, towerController.GetAttack()); //calls the bullet's seek function
        }
    }
}
