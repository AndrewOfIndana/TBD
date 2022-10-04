using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /*  
        Name: Bullet.cs
        Description: This script controls bullets spawned by towers and the speed of the bullet

    */

    public float speed;
    [HideInInspector] public float bulletAttack;
    private Transform target;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Sets target to the tower's enemyDetected as well as bullet's attack value -*/
    public void Seek(Transform targ, float atk)
    {
        target = targ;
        bulletAttack = atk; 
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Update is called once per frame -*/
    private void Update()
    {
        //if target doesn't exist
        if(target == null)
        {
            DestroyBullet();
            return; //Exits if statement
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime; 
        transform.Translate(dir.normalized * distanceThisFrame, Space.World); 
        Invoke("DestroyBullet", .5f); //Destroys bullet after half a second
    }
    
    /*---      FUNCTIONS     ---*/
    /*-  Destroys the bullet -*/
    public void DestroyBullet()
    {
        this.gameObject.SetActive(false); 
    }
}
