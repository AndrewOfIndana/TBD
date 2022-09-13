using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    /*  
        Name: TowerController.cs
        Description: This script controls the player's towers and how they react toe enemies

    */

    ObjectPool objectPool; //Reference to the object pool
    public Transform firingPoint; //Reference to the firing point transform
    public float fireRate = 1.1f; //Stores the fire rate of the tower
    public float range = 10f; //Store the range of the tower
    private float fireCountDown; //Stores how long it takes for the tower to fire again
    private Transform targetDetected; //Private reference to the target the tower is trying to fire at

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts on the first frame -*/
    public void Start()
    {
        objectPool = ObjectPool.objectPoolInstance; ///Set objectPool to the objectPool instance 
    }
    /*-  Starts when the script is awake -*/
    void Awake()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f); //Repeat the UpdateTarget function every half a second
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Is called every frame -*/
    void Update() 
    {
        //if there are no target in range
        if(targetDetected == null)
        {
            return; //return
        }

        //if fireCountDown is less than or equal to 0
        if (fireCountDown <= 0f)
        {
            Shoot(); //Calls the shoot function
            fireCountDown = 1f/fireRate; //Resets fireCountDown to 1 over the fireRate
        }
        fireCountDown -= Time.deltaTime; //fireCountDown count down from every second
    }

    /*---      FUNCTIONS     ---*/
    /*-  Controls shooting -*/
    void Shoot()
    {
        GameObject bulletObj = objectPool.SpawnFromPool("Bullet", firingPoint.position, firingPoint.rotation); //Spawn a bullet from the pool
        Bullet bullet = bulletObj.GetComponent<Bullet>(); //Gets the Bullet component from the spawned bulletObj

        //if this bullet exist
        if(bullet != null)
        {
            bullet.Seek(targetDetected); //calls the bullet's seek function
        }
    }
    /*-  Controls targeting -*/
    void UpdateTarget()
    {
        GameObject[] enemiesDetected = GameObject.FindGameObjectsWithTag("Enemy"); //A array of gameobjects of all gameobjects that contain the tag enemy
        float shortestDistance = Mathf.Infinity; //Sets the shortestDistance to infinity
        GameObject nearestEnemy = null; //Sets nearestEnemy to null

        //For every enemy detected in enemiesDetected
        foreach (GameObject enemyDetected in enemiesDetected)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemyDetected.transform.position); //calculates the distance to that enemy

            //if the distanceToEnemy is lesser than shortestDistance
            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy; //Set shortestDistance to distanceToEnemy
                nearestEnemy = enemyDetected; //Set nearestEnemy to enemyDetected
            }
        }

        //if the nearestEnemy does exist and shortestDistance is less than or equal to the tower's range
        if(nearestEnemy != null && shortestDistance <= range)
        {
           targetDetected = nearestEnemy.transform; //Set targetDetected to the nearestEnemy's transform
        }
        else
        {
            targetDetected = null; //Set targetDetected to null
        }
    }
}
