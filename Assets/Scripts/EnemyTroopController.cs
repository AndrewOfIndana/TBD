using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTroopController : MonoBehaviour, IObjectPoolFunctions
{
    /*  
        Name: EnemyTroopController.cs
        Description: This script allows for object pooling, or reusing the same objects instead of creating and deleting new ones.

    */
    public Image healthBar;
    public SpriteRenderer enemySprite;
    public BoxCollider enemyCollider;
    private float enemyAttack; //Store the attack of the enemy
    private float enemyHealth; //Store the health of the enemy
    private float enemySpeed; //Store the speed of the enemy
    // private float enemyAttackRate;
    // private float enemyAttackRange;

    private Transform target; //A reference to the target transform
    private int wavePointIndex = 0; //Keeps track of which waypoint the enemy is at
    private float enemyHealthMax;

    /*---      SETUP FUNCTIONS     ---*/
    public void SetUnit(Stats newStats)
    {
        enemyAttack = newStats.unitAttack;
        enemyHealth = newStats.unitHealth;
        enemySpeed = newStats.unitSpeed;
        enemyHealthMax = newStats.unitHealth;
        //enemyAttackRate = newStats.unitAttackRate;
        //enemyAttackRange = newStats.unitAttackRange;
        enemySprite.sprite = newStats.unitEnemySprite;
        enemyCollider.size =  newStats.unitSize;
        healthBar.fillAmount = enemyHealth/enemyHealthMax;
    }
    /*-  Starts when the object has spawned from pool -*/
    public void OnObjectSpawn()
    {
        wavePointIndex = 0; //Resets wayPointIndex to 0 when the enemy spawns
        target = WayPoints.points[0]; //Sets the target transform to the first wayPoint 

    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Is called every frame -*/
    void Update()
    {
        Vector3 dir = target.position - transform.position; //Get the direction of the enemy from the targets position and stores it as a Vector 3
        transform.Translate(dir.normalized * enemySpeed * Time.deltaTime, Space.World); //Moves the enemy towards the target

        //If the enemy has reached a waypoint calculate a new waypoint
        if (Vector3.Distance(transform.position, target.position) <= (enemySpeed * .01))
        {
            GetNextWaypoint(); //Calls the GetNextWaypoint function
        }
    }

    /*---      FUNCTIONS     ---*/
    /*-  Gets the next waypoint for the enemy -*/
    void GetNextWaypoint()
    {
        //if the enemy reaches the last waypoint
        if(wavePointIndex >= WayPoints.points.Length - 1)
        {
            this.gameObject.SetActive(false); //deactivate the enemy
            return; //Return
        }
        wavePointIndex++; //Add the waypoint index
        target = WayPoints.points[wavePointIndex]; //Sets target to new waypoint
    }
    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        healthBar.fillAmount = enemyHealth/enemyHealthMax;

        if(enemyHealth <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
