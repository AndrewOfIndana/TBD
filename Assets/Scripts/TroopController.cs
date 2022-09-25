using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopController : MonoBehaviour, IUnitController
{
    /*  
        Name: TroopController.cs
        Description: This script controls the basic behavior of all troops

    */

    [Header("UnitStats References")]
    public Image healthBar; //Reference to the health bar image of the troop
    public SpriteRenderer thisSprite; //Reference to the SpriteRenderer of the troop
    public BoxCollider thisCollider; //Reference to the box collider of the troop

    /*-  UnitStats Values -*/
    [HideInInspector] public Stats stat; //Stores the Stats of the troop
    [HideInInspector] public float attack; //Store the attack of the troop
    [HideInInspector] public float health; //Store the health of the troop
    [HideInInspector] public float speed; //Store the speed of the troop
    [HideInInspector] public float attackRate; //Store the attack rate of the troop
    [HideInInspector] public float attackRange; //Store the attack range of the troop

    /*-  Script References -*/
    private Transform path; //A reference to the path transform
    private int wavePointIndex; //Keeps track of which waypoint the troop is at
    private Transform targetDetected; //Private reference to the target the troop is trying to attack
    private IUnitController targetEngaged; //Private reference to the enemy troop the troop is engaged with

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Sets the units stats when the object has spawned from pool using the newStats Stats variables -*/
    public void SetUnit(Stats newStats)
    {
        stat = newStats; //Sets stats to newStats
        attack = newStats.unitAttack; //Sets attack to the newStats's unitAttack
        health = newStats.unitHealth; //Sets health to the newStats's unitHealth
        speed = newStats.unitSpeed; //Sets speed to the newStats's unitSpeed
        attackRate = newStats.unitAttackRate; //Sets attackRate to the newStats's unitAttackRate
        attackRange = newStats.unitAttackRange; //Sets attackRange to the newStats's unitAttackRange
        thisSprite.sprite = newStats.unitSprite; //Sets troopSprite's sprite to the newStats's unitSprite
        thisCollider.size =  newStats.unitSize; //Sets troopCollider's size to the newStats's collider's unitSize
        healthBar.fillAmount = health/newStats.unitHealth; //Resets healthBar by dividing health by maxHealth

        //if the troop is an enemy 
        if(stat.isUnitEnemy)
        {
            wavePointIndex = 0; //Resets wayPointIndex to 0 when the troop spawns
            path = WayPoints.points[0]; //Sets the path transform to the first wayPoint 
        }
        else if(!stat.isUnitEnemy) //if the troop isn't an enemy
        {
            wavePointIndex = WayPoints.points.Length; //Resets wayPointIndex to last wayPoint index when the troop spawns
            path = WayPoints.points[(WayPoints.points.Length - 1)]; //Sets the path transform to the last wayPoint 
        }
    }
    /*-  Starts when the object is enabled -*/
    private void OnEnable()
    {
        StartCoroutine(UpdateTarget(1f)); //Calls UpdateTarget IEnumerator at 1 second
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Is called every frame -*/
    private void Update()
    {
        //if targetDetected doesn't exist and this unit's behaviour isn't DEFEND
        if(targetDetected == null && stat.unitBehaviour != Behaviour.DEFEND)
        {
            Vector3 dir = path.position - transform.position; //Get the direction of the troop from the paths position and stores it as a Vector 3
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World); //Moves the troop towards the path

            //If the troop has reached a waypoint calculate a new waypoint
            if (Vector3.Distance(transform.position, path.position) <= (.25f))
            {
                GetNextWaypoint(); //Calls the GetNextWaypoint function
            }
        }
        //if targetDetected does exist and this unit's behaviour isn't RANGED
        else if(targetDetected != null && stat.unitBehaviour != Behaviour.RANGED)
        {
            //if this position and targetDetected's position is greater 2
            if(Vector3.Distance(transform.position, targetDetected.position) >= 2f)
            {
                Vector3 dir = targetDetected.position - transform.position; //Get the direction of the troop from the targetDetected position and stores it as a Vector 3
                transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World); //Moves the troop towards the targetDetected
            }
        }
    }
    /*-  Repeatedly updates a target; takes a float for the time -*/
    private IEnumerator UpdateTarget(float time)
    {
        yield return new WaitForSeconds(time); //Waits for time

        //if the unit's behaviour isn't KAMIKAZE
        if(stat.unitBehaviour != Behaviour.KAMIKAZE)
        {
            Targeting(); //Calls the Targeting function
            StartCoroutine(UpdateTarget(1f)); //Recalls Aiming IEnumerator at attackRate
        }
    }

    /*---      FUNCTIONS     ---*/
    /*-  Gets the next waypoint for the troop -*/
    private void GetNextWaypoint()
    {
        //if the troop is an enemy 
        if(stat.isUnitEnemy)
        {
            //if the troop reaches the last waypoint
            if(wavePointIndex >= WayPoints.points.Length - 1)
            {
                this.gameObject.SetActive(false); //deactivate the troop
                return; //Exits if statement
            }
            wavePointIndex++; //Add the waypoint index
            path = WayPoints.points[wavePointIndex]; //Sets path to new waypoint
        }
        else if(!stat.isUnitEnemy) //if the troop isn't an enemy
        {
            //if the troop reaches the first waypoint
            if(wavePointIndex <= 0)
            {
                this.gameObject.SetActive(false); //deactivate the troop
                return; //Exits if statement
            }
            wavePointIndex--; //Subtracts the waypoint index
            path = WayPoints.points[wavePointIndex]; //Sets path to new waypoint
        }
    }
    /*-  Controls targeting -*/
    private void Targeting()
    {
        float shortestDistance = Mathf.Infinity; //Sets the shortestDistance to infinity
        Transform nearestTarget = null; //Sets nearestTarget to null
        
        //For each unit in UnitsList
        foreach(Unit unit in Unit.GetUnitList())
        {
            //A for loop getting each tag
            for(int i = 0; i < stat.targetTags.Length; i++)
            {
                //If the unit's tag is the target tag
                if(unit.gameObject.tag == stat.targetTags[i])
                {
                    float distanceToTarget = Vector3.Distance(transform.position, unit.transform.position); //calculates the distance to that enemy
                    //if the distanceToTarget is lesser than shortestDistance
                    if(distanceToTarget < shortestDistance)
                    {
                        shortestDistance = distanceToTarget; //Set shortestDistance to distanceToTarget
                        nearestTarget = unit.transform; //Set nearestTarget to unitDetected
                    }
                }
            }
        }
        //if the nearestTarget does exist and shortestDistance is less than or equal to the tower's range
        if(nearestTarget != null && shortestDistance <= attackRange)
        {
            targetDetected = nearestTarget; //Set targetDetected to the nearestTarget's transform

            if(stat.unitBehaviour == Behaviour.RANGED)
            {
                targetEngaged = targetDetected.gameObject.GetComponent<IUnitController>(); //Gets the TroopController component and stores it in targetEngaged
                StartCoroutine(Combat(attackRate)); //Calls Combat IEnumerator at attackRate
            }
        }
        else
        {
            targetDetected = null; //Set targetDetected to null
            targetEngaged = null; //Set targetEngaged to null
        }
    }
    /*-  event for something has entered the collider -*/
    private void OnTriggerEnter(Collider other)
    {
        //if the object collider is the tag oncomingBulletTag
        if (other.gameObject.CompareTag(stat.oncomingBulletTag))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>(); //Gets the Bullet component and stores it in bullet
            TakeDamage(bullet.bulletAttack); //Transfer bulletAttack to the this script's TakeDamage function
            bullet.DestroyBullet(); //Calls the bullet's DestroyBullet
        }
        //if the object collider is the tag oncomingTroopTag or oncomingAssassinTag ot oncomingTowerTag
        else if(other.gameObject.CompareTag(stat.oncomingTroopTag) || other.gameObject.CompareTag(stat.oncomingAssassinTag) || other.gameObject.CompareTag(stat.oncomingTowerTag))
        {
            targetEngaged = other.gameObject.GetComponent<IUnitController>(); //Gets the TroopController component and stores it in targetEngaged
            StartCoroutine(Combat(attackRate)); //Calls Combat IEnumerator at attackRate
        }
    }
    /*-  Controls Combat between units for troops -*/
    private IEnumerator Combat(float rate)
    {
        yield return new WaitForSeconds(rate); //Waits for time
        //if targetEngaged does exist
        if(targetEngaged != null)
        {
            targetEngaged.TakeDamage(this.attack); //Transfer the enemy's troop's attack to the this script's TakeDamage function
            StartCoroutine(Combat(attackRange * Random.Range(1, 1.5f))); //Recalls Combat IEnumerator at attackRate * random
        }
    }
    /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    public void TakeDamage(float damage)
    {
        health -= damage; //Subtracts from health with damage
        healthBar.fillAmount = health/stat.unitHealth; //Resets healthBar by dividing health by maxHealth

        //if health is less than or equal to 0
        if(health <= 0)
        {
            this.gameObject.SetActive(false); //deactivate the troop
        }
    }
}