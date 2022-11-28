using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TowerBehaviour : MonoBehaviour
{
    /*  
        Name: TowerBehaviour.cs
        Description: This script controls the behaviour of a tower and how it reacts to other units

    */
    /*[Header("Static References")]*/
    protected GameManager gameManager;
    protected ObjectPool objectPool; 

    /*[Header("Components")]*/
    protected TowerController towerController;

    [Header("Script Settings")]
    public Transform firingPoint; 
    protected Transform targetDetected; //What the unit detects

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
    public virtual void StartBehaviour()
    {
        StartCoroutine(UpdateTarget(towerController.GetAttackRate()));
    }

    /*---      FUNCTIONS     ---*/
    /*-  Repeatedly updates a target and shoots, takes a float for the time -*/
    protected virtual IEnumerator UpdateTarget(float time)
    {
        yield return new WaitForSeconds(time); 

        //if gameStates is PLAYING
        if(gameManager.CheckIfPlaying())
        {
            Targeting();

            //if targetDetected does exist
            if(targetDetected != null)
            {
                if(towerController.GetStats().unitUtils.unitBehaviour == Behaviour.RANGED)
                {
                    Shoot();
                }
                else if(towerController.GetStats().unitUtils.unitBehaviour == Behaviour.AOE)
                {
                    ApplyAreaOfEffect(true);
                }
            }
        }

        //if gameStates isn't WIN or LOSE
        if(!gameManager.CheckIfWinOrLose())
        {
            StartCoroutine(UpdateTarget(towerController.GetAttackRate()));
        }
            //if gameStates is WIN or LOSE
        else if(gameManager.CheckIfWinOrLose())
        {
            this.gameObject.SetActive(false);
        }
    }
    /*-  Controls targeting -*/
    protected void Targeting()
    {
        float shortestDistance = Mathf.Infinity;
        Transform nearestTarget = null;
        
        foreach(Unit unit in Unit.GetUnitList())
        {
            //If the unit's target tags contain the other units's tag
            if(towerController.GetStats().unitUtils.targetTags.Any(x => x.Contains(unit.gameObject.tag)))
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
        
        //if the nearestTarget does exist and shortestDistance is less than or equal to the units's attackRange
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
    protected void Shoot()
    {
        GameObject bulletObj = objectPool.SpawnFromPool(towerController.GetStats().unitUtils.sharedTags.bulletTag, firingPoint.position, firingPoint.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        //if this bullet exist
        if(bullet != null)
        {
            bullet.Seek(targetDetected, towerController.GetAttack()); //calls the bullet's seek function
        }
    }
    /*-  Controls shooting -*/
    private void ApplyAreaOfEffect(bool isAttack)
    {
        GameObject aoeObj = objectPool.SpawnFromPool("AreaOfEffect", this.transform.position, this.transform.rotation);
        AreaOfEffect aoe = aoeObj.GetComponent<AreaOfEffect>();

        //if this bullet exist
        if(aoe != null)
        {
            if(isAttack)
            {
                aoe.SetAOE(towerController.GetStats().unitAttackRange, towerController.GetStats().isUnitEnemy, !towerController.GetStats().isUnitEnemy, towerController.GetAttack()); //calls the bullet's seek function
            }
        }
    }
    /*-  Sets all target to null  -*/
    public virtual void VoidTargets()
    {
        targetDetected = null;
        towerController.animator.SetBool("aAttacking", false);
    }
}
