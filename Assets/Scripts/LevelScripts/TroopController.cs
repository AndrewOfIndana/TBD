using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopController : MonoBehaviour, IUnitController
{
    /*  
        Name: TroopController.cs
        Description: This script controls the troops and how they move and react to other unit

    */
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

    /*[Header("Script References")]*/
    private TroopMovement troopMovement;
    [HideInInspector] public Transform targetDetected; //What the unit detects
    private IUnitController targetEngaged; //What the unit is fighting

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        troopMovement = this.GetComponent<TroopMovement>();
    }
    /*-  Sets the units stats when the object has spawned from pool using the newStats Stats variables -*/
    public void SetUnit(Stats newStats)
    {
        stat = newStats;
        attack = newStats.unitAttack;
        health = newStats.unitHealth;
        speed = newStats.unitSpeed;
        attackRate = newStats.unitAttackRate;
        attackRange = newStats.unitAttackRange;
        thisSprite.sprite = newStats.unitSprite;
        thisCollider.size =  newStats.unitSize;
        healthBar.fillAmount = health/newStats.unitHealth;
        troopMovement.StartMovement(); //Starts the troop's Movement
    }
    /*-  OnEnable is called when the object becomes enabled -*/
    private void OnEnable()
    {
        StartCoroutine(UpdateTarget(1f)); //Calls UpdateTarget IEnumerator at 1 second
    }

    /*---      FUNCTIONS     ---*/
    /*-  Repeatedly updates a target, takes a float for the time -*/
    private IEnumerator UpdateTarget(float time)
    {
        yield return new WaitForSeconds(time);

        //if the unit's behaviour isn't KAMIKAZE
        if(stat.unitBehaviour != Behaviour.KAMIKAZE)
        {
            Targeting();
            StartCoroutine(UpdateTarget(1f)); //Recalls Aiming IEnumerator at attackRate
        }
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

            //if the unit's behaviour isn't RANGED
            if(stat.unitBehaviour == Behaviour.RANGED)
            {
                targetEngaged = targetDetected.gameObject.GetComponent<IUnitController>(); 
                StartCoroutine(Combat(attackRate * Random.Range(.75f, 1.25f))); //Calls Combat IEnumerator at attackRate * random
            }
        }
        else
        {
            targetDetected = null;
            targetEngaged = null;
        }
    }
    /*-  When a GameObject collides with another GameObject, Unity calls OnTriggerEnter. -*/
    private void OnTriggerEnter(Collider other)
    {
        //if the troop collides with an opposing bullet
        if (other.gameObject.CompareTag(stat.sharedTags.oncomingBulletTag))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>(); 
            TakeDamage(bullet.bulletAttack); //Transfer bulletAttack to the this script's TakeDamage function
            bullet.DestroyBullet();
        }
        foreach(string tag in stat.sharedTags.oncomingTags)
        {
            //if the troop collides with something from their target tags
            if(other.gameObject.CompareTag(tag))
            {
                targetEngaged = other.gameObject.GetComponent<IUnitController>();
                StartCoroutine(Combat(attackRate * Random.Range(1, 1.5f))); //Calls Combat IEnumerator at attackRate * random
            }
        }
    }
    /*-  Controls Combat between units for troops -*/
    private IEnumerator Combat(float rate)
    {
        yield return new WaitForSeconds(rate);

        //if targetEngaged does exist
        if(targetEngaged != null)
        {
            targetEngaged.TakeDamage(this.attack); //Transfer the enemy's troop's attack to the this script's TakeDamage function
            StartCoroutine(Combat(attackRate * Random.Range(1, 1.5f))); //Recalls Combat IEnumerator at attackRate * random
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