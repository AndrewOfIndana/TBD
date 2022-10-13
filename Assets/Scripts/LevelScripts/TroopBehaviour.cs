using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopBehaviour : MonoBehaviour, Idamageable
{
    /*  
        Name: TroopBehaviour.cs
        Description: This script controls the behaviour of a troop and how it reacts to other units and damage

    */

    /*[Header("Script References")]*/
    private TroopController troopController;
    private TroopMovement troopMovement;
    [HideInInspector] public Transform targetDetected; //What the unit detects
    private Idamageable targetEngaged; //What the unit is fighting

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        troopController = this.GetComponent<TroopController>();
        troopMovement = this.GetComponent<TroopMovement>();
    }
    /*-  Starts the units targeting behaviour -*/
    public void StartBehaviour()
    {
        StartCoroutine(UpdateTarget(1f)); //Calls UpdateTarget IEnumerator at 1 second
    }

   /*---      FUNCTIONS     ---*/
    /*-  Repeatedly updates a target, takes a float for the time -*/
    private IEnumerator UpdateTarget(float time)
    {
        yield return new WaitForSeconds(time);
        Targeting();
        Engaging();
        StartCoroutine(UpdateTarget(1f)); //Recalls Aiming IEnumerator at attackRate
    }
    /*-  Controls targeting -*/
    private void Targeting()
    {
        float shortestDistance = Mathf.Infinity;
        Transform nearestTarget = null;
        
        foreach(Unit unit in Unit.GetUnitList())
        {
            for(int i = 0; i < troopController.stat.targetTags.Length; i++)
            {
                //If the unit's tag is the target tag
                if(unit.gameObject.tag == troopController.stat.targetTags[i])
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
        if(nearestTarget != null && shortestDistance <= troopController.attackRange)
        {
            targetDetected = nearestTarget;

            //if the unit's behaviour is RANGED
            if(troopController.stat.unitBehaviour == Behaviour.RANGED && targetEngaged == null)
            {
                targetEngaged = targetDetected.gameObject.GetComponent<Idamageable>(); 
                StartCoroutine(Combat(troopController.attackRate, 1.5f, 2f));
            }
        }
        else
        {
            targetDetected = null;
            targetEngaged = null;
        }
    }
    /*-  Controls engagement of targets  -*/
    private void Engaging()
    {
        //if targetDetected does exist
        if(targetDetected != null)
        {
            //if this position and targetDetected's position is greater 2 and unit's behaviour isn't RANGED and targetEngaged doesn't exist 
            if(Vector3.Distance(transform.position, targetDetected.position) <= 2f && troopController.stat.unitBehaviour != Behaviour.RANGED && targetEngaged == null)
            {
                targetEngaged = targetDetected.gameObject.GetComponent<Idamageable>(); 
                StartCoroutine(Combat(troopController.attackRate, 1, 1.5f));
            }
        }
    }
    /*-  Controls Combat between units for troops -*/
    private IEnumerator Combat(float rate, float minAtkTime, float maxAtkTime)
    {
        float randomRate = rate * Random.Range(minAtkTime, maxAtkTime);
        yield return new WaitForSeconds(randomRate);

        //if targetEngaged does exist
        if(targetEngaged != null && targetDetected.gameObject.activeSelf)
        {
            targetEngaged.TakeDamage(troopController.attack); //Transfer the enemy's troop's attack to the this script's TakeDamage function
            StartCoroutine(Combat(rate, minAtkTime, maxAtkTime)); //Recalls Combat IEnumerator at attackRate * random
        }
        else
        {
            targetDetected = null;
            targetEngaged = null;
        }
    }
    /*-  When a GameObject collides with another GameObject, Unity calls OnTriggerEnter. -*/
    private void OnTriggerEnter(Collider other)
    {
        //if the troop collides with an opposing bullet
        if (other.gameObject.CompareTag(troopController.stat.sharedTags.oncomingBulletTag))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>(); 
            TakeDamage(bullet.bulletAttack); //Transfer bulletAttack to the this script's TakeDamage function
            bullet.DestroyBullet();
        }
    }
    /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    public void TakeDamage(float damage)
    {
        troopController.health -= damage;
        troopController.healthBar.fillAmount = troopController.health/troopController.stat.unitHealth; //Resets healthBar

        //if health is less than or equal to 0
        if(troopController.health <= 0)
        {
            targetDetected = null;
            targetEngaged = null;
            this.gameObject.SetActive(false); 
        }
    }
}
