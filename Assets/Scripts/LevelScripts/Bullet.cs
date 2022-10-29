using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /*  
        Name: Bullet.cs
        Description: This script controls bullets spawned by towers and the speed of the bullet

    */
    /*[Header("Static References")]*/
    GameManager gameManager;

    [Header("Script Settings")]
    public float speed;
    private float bulletAttack;
    private Transform target;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
    }
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
        //if gameStates isn't playing 
        if(!gameManager.CheckIfPlaying())
        {
            return; //Exits if statement
        }

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

    /*---      SET/GET FUNCTIONS     ---*/
    /*-  Gets attack -*/
    public float GetAttack()
    {
        return bulletAttack;
    }
}
