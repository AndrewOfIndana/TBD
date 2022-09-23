using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : MonoBehaviour, IObjectPoolFunctions
{
    /*  
        Name: PlayerTowerController.cs
        Description: This script controls the player's towers and how they react to enemies

    */

    ObjectPool objectPool; //Reference to the object pool

    [Header("UnitStats References")]
    public Image healthBar; //Reference to the health bar image of the tower
    public SpriteRenderer towerSprite; //Reference to the SpriteRenderer of the tower
    public BoxCollider towerCollider; //Reference to the box collider of the tower

    /*-  UnitStats Values -*/
    [HideInInspector]
    public float attack; //Store the attack of the tower
    private float health; //Store the health of the tower
    private float maxHealth; //Stores the max health of the tower
    private float attackRate; //Store the attack rate of the tower
    private float attackRange; //Store the attack range of the tower
    private bool isEnemy; //Is the troop an enemy

    [Header("Script References")]
    public Transform firingPoint; //Reference to the firing point transform
    private Transform targetDetected; //Private reference to the target the tower is trying to fire at
    private string targetTag; //Stores the string of what type of unit should be targeted
    private string bulletTag; //Store the string of what type of bullet should be spawned
    private string oncomingBulletTag; //Stores the string of oncoming bullet type tag used for damage calculations
    private string oncomingTroopTag; //Stores the string of oncoming troop type tag used for damage calculations
    private string oncomingAssassinTag; //Stores the string of oncoming assassin type tag used for damage calculations

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts on the first frame -*/
    public void Start()
    {
        objectPool = ObjectPool.objectPoolInstance; ///Set objectPool to the objectPool instance 
    }
    /*---      SETUP FUNCTIONS     ---*/
    /*-  Sets the units stats when the object has spawned from pool using the newStats Stats variables -*/
    public void SetUnit(Stats newStats)
    {
        attack = newStats.unitAttack; //Sets attack to the newStats's unitAttack
        health = newStats.unitHealth; //Sets health to the newStats's unitHealth
        maxHealth = newStats.unitHealth; //Sets maxHealth to the newStats's unitHealth
        attackRate = newStats.unitAttackRate; //Sets attackRate to the newStats's unitAttackRate
        attackRange = newStats.unitAttackRange; //Sets attackRange to the newStats's unitAttackRange
        towerSprite.sprite = newStats.unitSprite; //Sets towerSprite's sprite to the newStats's unitSprite
        towerCollider.size =  newStats.unitSize; //Sets towerCollider's size to the newStats's collider's unitSize
        isEnemy = newStats.isUnitEnemy; //Sets isEnemy to the newStats's isUnitEnemy bool
        healthBar.fillAmount = health/maxHealth; //Resets healthBar by dividing health by maxHealth

        //if the troop is an enemy 
        if(isEnemy)
        {
            targetTag = "AllyTroop"; //Sets targetTag to register the opponents troops
            bulletTag = "EnemyBullet"; //Sets the type of bullets that need to be spawned
            oncomingBulletTag = "AllyBullet"; //Sets oncomingBulletTag to register player's bullets
            oncomingTroopTag = "AllyTroop"; //Sets oncomingTroopTag to register player's troop's attack
            oncomingAssassinTag = "AllyAssassin"; //Sets oncomingAssassinTag to register player's assassin's attack
        }
        else if(!isEnemy) //if the troop isn't an enemy
        {
            targetTag = "EnemyTroop"; //Sets targetTag to register the opponents troops
            bulletTag = "AllyBullet"; //Sets the type of bullets that need to be spawned
            oncomingBulletTag = "EnemyBullet"; //Sets oncomingBulletTag to register enemy bullets
            oncomingTroopTag = "EnemyTroop"; //Sets oncomingTroopTag to register enemy troop's attack
            oncomingAssassinTag = "EnemyAssassin"; //Sets oncomingAssassinTag to register enemy assassin's attack
        }
    }    
    /*-  Starts when the object has spawned from pool -*/
    public void OnObjectSpawn()
    {
        StartCoroutine(Aiming(attackRate)); //Calls Aiming IEnumerator at attackRate
    }

    /*---      FUNCTIONS     ---*/
    /*-  Repeatedly updates a target and shoots, takes a float for the time -*/
    private IEnumerator Aiming(float time)
    {
        yield return new WaitForSeconds(time); //Waits for time
        UpdateTarget(); //Calls the UpdateTarget function
        
        if(targetDetected != null)
        {
            Shoot(); //Calls the shoot function
        }
        StartCoroutine(Aiming(attackRate)); //Recalls Aiming IEnumerator at attackRate
    }
    /*-  Controls targeting -*/
    private void UpdateTarget()
    {
        GameObject[] unitsDetected = GameObject.FindGameObjectsWithTag(targetTag); //Finds all GameObjects with the troopTag and stores it in the unitsDetected array
        float shortestDistance = Mathf.Infinity; //Sets the shortestDistance to infinity
        GameObject nearestTarget = null; //Sets nearestTarget to null

        //For every enemy detected in unitsDetected
        foreach (GameObject unitDetected in unitsDetected)
        {
            float distanceToTarget = Vector3.Distance(transform.position, unitDetected.transform.position); //calculates the distance to that enemy

            //if the distanceToTarget is lesser than shortestDistance
            if(distanceToTarget < shortestDistance)
            {
                shortestDistance = distanceToTarget; //Set shortestDistance to distanceToTarget
                nearestTarget = unitDetected; //Set nearestTarget to unitDetected
            }
        }

        //if the nearestTarget does exist and shortestDistance is less than or equal to the tower's range
        if(nearestTarget != null && shortestDistance <= attackRange)
        {
            targetDetected = nearestTarget.transform; //Set targetDetected to the nearestTarget's transform
        }
        else
        {
            targetDetected = null; //Set targetDetected to null
        }
    }
    /*-  Controls shooting -*/
    private void Shoot()
    {
        GameObject bulletObj = objectPool.SpawnFromPool(bulletTag, firingPoint.position, firingPoint.rotation); //Spawn a EnemyBullet from the pool
        Bullet bullet = bulletObj.GetComponent<Bullet>(); //Gets the Bullet component from the spawned bulletObj

        //if this bullet exist
        if(bullet != null)
        {
            bullet.Seek(targetDetected, attack); //calls the bullet's seek function
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
