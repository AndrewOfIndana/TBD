using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : MonoBehaviour, IUnitController
{
    /*  
        Name: TowerController.cs
        Description: This script controls the towers and how they react to enemies

    */

    ObjectPool objectPool; //Reference to the object pool

    [Header("UnitStats References")]
    public Image healthBar; //Reference to the health bar image of the troop
    public SpriteRenderer thisSprite; //Reference to the SpriteRenderer of the troop
    public BoxCollider thisCollider; //Reference to the box collider of the troop

    /*-  UnitStats Values -*/
    /*-  UnitStats Values -*/
    [HideInInspector] public Stats stat; //Stores the Stats of the troop
    [HideInInspector] public float attack; //Store the attack of the troop
    [HideInInspector] public float health; //Store the health of the troop
    [HideInInspector] public float speed; //Store the speed of the troop
    [HideInInspector] public float attackRate; //Store the attack rate of the troop
    [HideInInspector] public float attackRange; //Store the attack range of the troop

    [Header("Script References")]
    public Transform firingPoint; //Reference to the firing point transform
    private Transform targetDetected; //Private reference to the target the troop is trying to attack

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts on the first frame -*/
    private void Start()
    {
        objectPool = ObjectPool.objectPoolInstance; ///Set objectPool to the objectPool instance 
    }
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
    }    
    /*-  Starts when the object is enabled -*/
    private void OnEnable()
    {
        StartCoroutine(UpdateTarget(attackRate)); //Calls UpdateTarget IEnumerator at attackRate
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Repeatedly updates a target and shoots, takes a float for the time -*/
    private IEnumerator UpdateTarget(float time)
    {
        yield return new WaitForSeconds(time); //Waits for time
        Targeting(); //Calls the UpdateTarget function
        
        //if targetDetected does exist
        if(targetDetected != null)
        {
            Shoot(); //Calls the shoot function
        }
        StartCoroutine(UpdateTarget(attackRate)); //Recalls Aiming IEnumerator at attackRate
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
        }
        else
        {
            targetDetected = null; //Set targetDetected to null
        }
    }
    /*-  Controls shooting -*/
    private void Shoot()
    {
        GameObject bulletObj = objectPool.SpawnFromPool(stat.bulletTag, firingPoint.position, firingPoint.rotation); //Spawn a EnemyBullet from the pool
        Bullet bullet = bulletObj.GetComponent<Bullet>(); //Gets the Bullet component from the spawned bulletObj

        //if this bullet exist
        if(bullet != null)
        {
            bullet.Seek(targetDetected, attack); //calls the bullet's seek function
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
