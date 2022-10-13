using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour, Idamageable
{
    /*  
        Name: TowerBehaviour.cs
        Description: This script controls the behaviour of a tower and how it reacts to other units and damage

    */
    /*[Header("Static Variables")]*/
    ObjectPool objectPool; 

    [Header("Script References")]
    private TowerController towerController;
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
        objectPool = ObjectPool.objectPoolInstance; 
    }

    /*-  Starts the units targeting behaviour -*/
    public void StartBehaviour()
    {
        StartCoroutine(UpdateTarget(towerController.attackRate)); //Calls UpdateTarget IEnumerator at attackRate
    }

    /*---      FUNCTIONS     ---*/
    /*-  Repeatedly updates a target and shoots, takes a float for the time -*/
    private IEnumerator UpdateTarget(float time)
    {
        yield return new WaitForSeconds(time); 
        Targeting();
        
        //if targetDetected does exist
        if(targetDetected != null)
        {
            Shoot();
        }
        StartCoroutine(UpdateTarget(towerController.attackRate)); //Recalls Aiming IEnumerator at attackRate
    }
    /*-  Controls targeting -*/
    private void Targeting()
    {
        float shortestDistance = Mathf.Infinity; 
        Transform nearestTarget = null;

        foreach(Unit unit in Unit.GetUnitList())
        {
            for(int i = 0; i < towerController.stat.targetTags.Length; i++)
            {
                //If the unit's tag is the target tag
                if(unit.gameObject.tag == towerController.stat.targetTags[i])
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
        if(nearestTarget != null && shortestDistance <= towerController.attackRange)
        {
            targetDetected = nearestTarget; 
        }
        else
        {
            targetDetected = null;
        }
    }
    /*-  Controls shooting -*/
    private void Shoot()
    {
        GameObject bulletObj = objectPool.SpawnFromPool(towerController.stat.sharedTags.bulletTag, firingPoint.position, firingPoint.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        //if this bullet exist
        if(bullet != null)
        {
            bullet.Seek(targetDetected, towerController.attack); //calls the bullet's seek function
        }
    }
    /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    public void TakeDamage(float damage)
    {
        towerController.health -= damage;
        towerController.healthBar.fillAmount = towerController.health/towerController.stat.unitHealth; //Resets healthBar

        //if health is less than or equal to 0
        if(towerController.health <= 0)
        {
            this.gameObject.SetActive(false); 
        }
    }

}
