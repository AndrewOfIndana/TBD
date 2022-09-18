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
    private float bulletAttack;
    private Transform targetedEnemy; //Private reference to the targetedEnemy's transform

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Sets targetedEnemy to the tower's enemyDetected -*/
    public void Seek(Transform targ, float atk)
    {
        targetedEnemy = targ; //Sets targetedEnemy to targ
        bulletAttack = atk;
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Is called every frame -*/
    void Update()
    {
        if(targetedEnemy == null)
        {
            DestroyBullet();
            return;
        }

        Vector3 dir = targetedEnemy.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime; //The amount of distance covered by the speed times the time
        transform.Translate(dir.normalized * distanceThisFrame, Space.World); //Moves the bullet towards the enemy
        Invoke("DestroyBullet", .5f);
    }
    /*---      FUNCTIONS     ---*/
        /*-  event for something has entered the collider -*/
    private void OnTriggerEnter(Collider other)
    {
        //if the object collider is a enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyTroopController enemyTroop = other.gameObject.GetComponent<EnemyTroopController>();
            enemyTroop.TakeDamage(bulletAttack);
            DestroyBullet();
        }
    }

    /*-  Destroys the bullet -*/
    void DestroyBullet()
    {
        this.gameObject.SetActive(false); //Disables the bullet
    }
}
