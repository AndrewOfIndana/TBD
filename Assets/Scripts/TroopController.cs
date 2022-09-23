using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopController : MonoBehaviour
{
    /*  
        Name: TroopController.cs
        Description: This script controls the basic behavior of all troops

    */

    [Header("UnitStats References")]
    public Image healthBar; //Reference to the health bar image of the troop
    public SpriteRenderer troopSprite; //Reference to the SpriteRenderer of the troop
    public BoxCollider troopCollider; //Reference to the box collider of the troop

    /*-  UnitStats Values -*/
    [HideInInspector]
    public float attack; //Store the attack of the troop
    private float health; //Store the health of the troop
    private float maxHealth; //Stores the max health of the troop
    private float speed; //Store the speed of the troop
    // private float attackRate; //Store the attack rate of the troop
    // private float attackRange; //Store the attack range of the troop
    private bool isEnemy; //Is the troop an enemy
    private string oncomingBulletTag; //Stores the string of oncoming bullet type tag used for damage calculations
    private string oncomingTroopTag; //Stores the string of oncoming troop type tag used for damage calculations
    private string oncomingAssassinTag; //Stores the string of oncoming assassin type tag used for damage calculations

    /*-  Script References -*/
    private Transform target; //A reference to the target transform
    private int wavePointIndex; //Keeps track of which waypoint the troop is at

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Sets the units stats when the object has spawned from pool using the newStats Stats variables -*/
    public void SetUnit(Stats newStats)
    {
        attack = newStats.unitAttack; //Sets attack to the newStats's unitAttack
        health = newStats.unitHealth; //Sets health to the newStats's unitHealth
        maxHealth = newStats.unitHealth; //Sets maxHealth to the newStats's unitHealth
        speed = newStats.unitSpeed; //Sets speed to the newStats's unitSpeed
        //attackRate = newStats.unitAttackRate; //Sets attackRate to the newStats's unitAttackRate
        //attackRange = newStats.unitAttackRange; //Sets attackRange to the newStats's unitAttackRange
        troopSprite.sprite = newStats.unitSprite; //Sets troopSprite's sprite to the newStats's unitSprite
        troopCollider.size =  newStats.unitSize; //Sets troopCollider's size to the newStats's collider's unitSize
        isEnemy = newStats.isUnitEnemy; //Sets isEnemy to the newStats's isUnitEnemy bool
        healthBar.fillAmount = health/maxHealth; //Resets healthBar by dividing health by maxHealth

        //if the troop is an enemy 
        if(isEnemy)
        {
            wavePointIndex = 0; //Resets wayPointIndex to 0 when the troop spawns
            target = WayPoints.points[0]; //Sets the target transform to the first wayPoint 
            oncomingBulletTag = "AllyBullet"; //Sets oncomingBulletTag to register player's bullets
            oncomingTroopTag = "AllyTroop"; //Sets oncomingTroopTag to register player's troop's attack
            oncomingAssassinTag = "AllyAssassin"; //Sets oncomingAssassinTag to register player's assassin's attack
        }
        else if(!isEnemy) //if the troop isn't an enemy
        {
            wavePointIndex = WayPoints.points.Length; //Resets wayPointIndex to last wayPoint index when the troop spawns
            target = WayPoints.points[(WayPoints.points.Length - 1)]; //Sets the target transform to the last wayPoint 
            oncomingBulletTag = "EnemyBullet"; //Sets oncomingBulletTag to register enemy bullets
            oncomingTroopTag = "EnemyTroop"; //Sets oncomingTroopTag to register enemy troop's attack
            oncomingAssassinTag = "EnemyAssassin"; //Sets oncomingAssassinTag to register enemy assassin's attack
        }
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Is called every frame -*/
    void Update()
    {
        Vector3 dir = target.position - transform.position; //Get the direction of the troop from the targets position and stores it as a Vector 3
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World); //Moves the troop towards the target

        //If the troop has reached a waypoint calculate a new waypoint
        if (Vector3.Distance(transform.position, target.position) <= (speed * .01))
        {
            GetNextWaypoint(); //Calls the GetNextWaypoint function
        }
    }

    /*---      FUNCTIONS     ---*/
    /*-  Gets the next waypoint for the troop -*/
    private void GetNextWaypoint()
    {
        //if the troop is an enemy 
        if(isEnemy)
        {
            //if the troop reaches the last waypoint
            if(wavePointIndex >= WayPoints.points.Length - 1)
            {
                this.gameObject.SetActive(false); //deactivate the troop
                return; //Exits if statement
            }
            wavePointIndex++; //Add the waypoint index
            target = WayPoints.points[wavePointIndex]; //Sets target to new waypoint
        }
        else if(!isEnemy) //if the troop isn't an enemy
        {
            //if the troop reaches the first waypoint
            if(wavePointIndex <= 0)
            {
                this.gameObject.SetActive(false); //deactivate the troop
                return; //Exits if statement
            }
            wavePointIndex--; //Subtracts the waypoint index
            target = WayPoints.points[wavePointIndex]; //Sets target to new waypoint
        }
    }
    /*-  event for something has entered the collider -*/
    private void OnTriggerEnter(Collider other)
    {
        //if the object collider is the tag oncomingBulletTag
        if (other.gameObject.CompareTag(oncomingBulletTag))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>(); //Gets the Bullet component and stores it in bullet
            TakeDamage(bullet.bulletAttack); //Transfer bulletAttack to the this script's TakeDamage function
            bullet.DestroyBullet(); //Calls the bullet's DestroyBullet
        }
        //if the object collider is the tag oncomingTroopTag or oncomingAssassinTag
        else if(other.gameObject.CompareTag(oncomingTroopTag) || other.gameObject.CompareTag(oncomingAssassinTag))
        {
            TroopController troop = other.gameObject.GetComponent<TroopController>(); //Gets the TroopController component and stores it in troop
            troop.TakeDamage(troop.attack); //Transfer the enemy's troop's attack to the this script's TakeDamage function
        }
    }
    /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    public void TakeDamage(float damage)
    {
        health -= damage; //Subtracts from health with damage
        healthBar.fillAmount = health/maxHealth; //Resets healthBar by dividing health by maxHealth

        //if health is less than or equal to 0
        if(health <= 0)
        {
            this.gameObject.SetActive(false); //deactivate the troop
        }
    }
}
