using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /*  
        Name: Bullet.cs
        Description: This script controls bullets spawned by towers, and units

    */

    public float speed = 70f; //Store the speed of the bullet
    private Transform targetedEnemy; //Private reference to the targetedEnemy's transform

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Sets targetedEnemy to the tower's enemyDetected -*/
    public void Seek(Transform targ)
    {
        targetedEnemy = targ; //Sets targetedEnemy to targ
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Is called every frame -*/
    void Update()
    {
        Vector3 dir; //A Vector3 of the bullet direction
        float distanceThisFrame = speed * Time.deltaTime; //The amount of distance covered by the speed times the time

        //if the targetedEnemy doesn't exist
        if (targetedEnemy == null)
        {
            transform.position += transform.forward * distanceThisFrame; //Moves the bullet forward
            Invoke("DestroyBullet", 5f); //Destroy the bullet after 5 seconds
            return;
        }
        else
        {
            dir = targetedEnemy.position - transform.position; //Gets the bullet's direction from the enemy
            transform.Translate(dir.normalized * distanceThisFrame, Space.World); //Moves the bullet towards the enemy
        }
    }

    /*---      FUNCTIONS     ---*/
    /*-  Destroys the bullet -*/
    void DestroyBullet()
    {
        this.gameObject.SetActive(false); //Disables the bullet
    }
}
