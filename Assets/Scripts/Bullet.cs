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
    [HideInInspector]
    public float bulletAttack; //Stores the attack value of the bullet
    private Transform target; //Private reference to the target's transform

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Sets target to the tower's enemyDetected as well as the attack value -*/
    public void Seek(Transform targ, float atk)
    {
        target = targ; //Sets target to targ
        bulletAttack = atk; //Sets bulletAttack to the unit's atk
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Is called every frame -*/
    void Update()
    {
        //if target doesn't exist
        if(target == null)
        {
            DestroyBullet(); //Calls DestroyBullet
            return; //Exits if statement
        }

        Vector3 dir = target.position - transform.position; //Gets direction from target's and this object's position and stores it in a Vector3
        float distanceThisFrame = speed * Time.deltaTime; //The amount of distance covered by the speed times the time
        transform.Translate(dir.normalized * distanceThisFrame, Space.World); //Moves the bullet towards the enemy
        Invoke("DestroyBullet", .5f); //Destroys bullet after half a second
    }
    /*---      FUNCTIONS     ---*/
    /*-  Destroys the bullet -*/
    public void DestroyBullet()
    {
        this.gameObject.SetActive(false); //Disables the bullet
    }
}
