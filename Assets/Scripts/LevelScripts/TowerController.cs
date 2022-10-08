using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : MonoBehaviour, Idamageable
{
    /*  
        Name: TowerController.cs
        Description: This script controls the towers and how react to other unit
        
    */
    /*[Header("Static Variables")]*/
    ObjectPool objectPool; 

    [Header("GameObject References")]
    public Image healthBar; 
    public SpriteRenderer thisSprite; 
    public BoxCollider thisCollider; 

    /*[Header("Stats Variables")]*/
    [HideInInspector] public Stats stat;
    [HideInInspector] public float attack;
    [HideInInspector] public float health; 
    [HideInInspector] public float speed;
    [HideInInspector] public float attackRate;
    [HideInInspector] public float attackRange;

    [Header("Script References")]
    public Transform firingPoint; 
    private Transform targetDetected;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        objectPool = ObjectPool.objectPoolInstance; 
    }
    /*-  Sets the units stats when the object has spawned from pool using the newStats Stats variables -*/
    public void SetUnit(Stats newStats)
    {
        /* Gets the Stats from stats and store them in Stats Variables */
        stat = newStats;
        attack = newStats.unitAttack; 
        health = newStats.unitHealth;
        attackRate = newStats.unitAttackRate;
        attackRange = newStats.unitAttackRange;
        thisSprite.sprite = newStats.unitSprite;
        thisCollider.size =  newStats.unitSize;
        healthBar.fillAmount = health/newStats.unitHealth;
    }    
    /*-  OnEnable is called when the object becomes enabled -*/
    private void OnEnable()
    {
        StartCoroutine(UpdateTarget(attackRate)); //Calls UpdateTarget IEnumerator at attackRate
    }

    /*---      FUNCTIONS     ---*/
    /*-  Repeatedly updates a target and shoots, takes a float for the time -*/
    private IEnumerator UpdateTarget(float time)
    {
        yield return new WaitForSeconds(time); 
        Targeting();
        
        //if targetDetected does exist
        if(targetDetected != null)
        {
            Shoot();
        }
        StartCoroutine(UpdateTarget(attackRate)); //Recalls Aiming IEnumerator at attackRate
    }
    /*-  Controls targeting -*/
    private void Targeting()
    {
        float shortestDistance = Mathf.Infinity; 
        Transform nearestTarget = null;

        foreach(Unit unit in Unit.GetUnitList())
        {
            for(int i = 0; i < stat.targetTags.Length; i++)
            {
                //If the unit's tag is the target tag
                if(unit.gameObject.tag == stat.targetTags[i])
                {
                    float distanceToTarget = Vector3.Distance(transform.position, unit.transform.position); //calculates the distance to that enemy

                    //if the distanceToTarget is lesser than shortestDistance
                    if(distanceToTarget < shortestDistance)
                    {
                        shortestDistance = distanceToTarget; 
                        nearestTarget = unit.transform;
                    }
                }
            }
        }
        //if the nearestTarget does exist and shortestDistance is less than or equal to the tower's range
        if(nearestTarget != null && shortestDistance <= attackRange)
        {
            targetDetected = nearestTarget; 
        }
        else
        {
            targetDetected = null;
        }
    }
    /*-  Controls shooting -*/
    private void Shoot()
    {
        GameObject bulletObj = objectPool.SpawnFromPool(stat.sharedTags.bulletTag, firingPoint.position, firingPoint.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        //if this bullet exist
        if(bullet != null)
        {
            bullet.Seek(targetDetected, attack); //calls the bullet's seek function
        }
    }
    /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health/stat.unitHealth; //Resets healthBar

        //if health is less than or equal to 0
        if(health <= 0)
        {
            this.gameObject.SetActive(false); 
        }
    }
}
