using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopBehaviour : MonoBehaviour
{
    /*  
        Name: TroopBehaviour.cs
        Description: This script controls the behaviour of a troop and how it reacts to other units and damage

    */

    /*[Header("Script References")]*/
    private TroopController troopController;
    private TroopMovement troopMovement;
    private Transform targetDetected; //What the unit detects
    private Idamageable targetEngaged; //What the unit is fighting

    /*[Header("Golem References")]*/
    private PlayerAvatar playerAvatar;
    private Transform playerDetected;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        troopController = this.GetComponent<TroopController>();
        troopMovement = this.GetComponent<TroopMovement>();
    }
    private void Start()
    {
        playerAvatar = LevelManager.levelManagerInstance.GetPlayerAvatar();
    }

    /*-  Starts the units targeting behaviour -*/
    public void StartBehaviour()
    {
        StartCoroutine(UpdateTarget(1f)); //Calls UpdateTarget IEnumerator at 1 second
    }

   /*---      FUNCTIONS     ---*/
    /*-  Repeatedly updates a target, takes a float for the time -*/
    private IEnumerator UpdateTarget(float time)
    {
        yield return new WaitForSeconds(time);
        Targeting();
        Engaging();
        StartCoroutine(UpdateTarget(1f)); //Recalls Aiming IEnumerator at attackRate
    }
    /*-  Controls targeting -*/
    private void Targeting()
    {
        float shortestDistance = Mathf.Infinity;
        Transform nearestTarget = null;
        
        foreach(Unit unit in Unit.GetUnitList())
        {
            for(int i = 0; i < troopController.GetStats().targetTags.Length; i++)
            {
                //If the unit's tag is the target tag
                if(unit.gameObject.tag == troopController.GetStats().targetTags[i])
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
        if(nearestTarget != null && shortestDistance <= troopController.GetStats().unitAttackRange)
        {
            targetDetected = nearestTarget;

            //if the unit's behaviour is RANGED
            if(troopController.GetStats().unitBehaviour == Behaviour.RANGED && targetEngaged == null)
            {
                troopController.animator.SetBool("aAttacking", true);
                targetEngaged = targetDetected.gameObject.GetComponent<Idamageable>(); 
                StartCoroutine(Combat(troopController.GetAttackRate(), 1.5f, 2f));
            }
        }
        else if(troopController.GetStats().unitBehaviour == Behaviour.DEFEND && Vector3.Distance(transform.position, playerAvatar.transform.position) <= (troopController.GetStats().unitAttackRange * 2))
        {
            playerDetected = playerAvatar.transform;
        }
        else
        {
            VoidTargets();
        }
    }
    /*-  Controls engagement of targets  -*/
    private void Engaging()
    {
        //if targetDetected does exist
        if(targetDetected != null)
        {
            //if this position and targetDetected's position is greater 2 and unit's behaviour isn't RANGED and targetEngaged doesn't exist 
            if(Vector3.Distance(transform.position, targetDetected.position) <= 2f && troopController.GetStats().unitBehaviour != Behaviour.RANGED && targetEngaged == null)
            {
                targetEngaged = targetDetected.gameObject.GetComponent<Idamageable>(); 
                StartCoroutine(Combat(troopController.GetAttackRate(), 1, 1.5f));
            }
        }
    }
    /*-  Controls Combat between units for troops -*/
    private IEnumerator Combat(float rate, float minAtkTime, float maxAtkTime)
    {
        float randomRate = rate * Random.Range(minAtkTime, maxAtkTime);
        yield return new WaitForSeconds(randomRate);

        //if targetEngaged does exist
        if(targetEngaged != null && targetDetected.gameObject.activeSelf)
        {
            targetEngaged.TakeDamage(troopController.GetAttack()); //Transfer the enemy's troop's attack to the this script's TakeDamage function
            StartCoroutine(Combat(rate, minAtkTime, maxAtkTime)); //Recalls Combat IEnumerator at attackRate * random
        }
        else
        {
            VoidTargets();
        }
    }
    /*-  When a GameObject collides with another GameObject, Unity calls OnTriggerEnter. -*/
    private void OnTriggerEnter(Collider other)
    {
        //if the troop collides with an opposing bullet
        if (other.gameObject.CompareTag(troopController.GetStats().sharedTags.oncomingBulletTag))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>(); 
            troopController.TakeDamage(bullet.GetAttack()); //Transfer bulletAttack to the this script's TakeDamage function
            bullet.DestroyBullet();
        }
    }

    /*---      SET/GET FUNCTIONS     ---*/
    public Transform GetTargetDetected()
    {
        return targetDetected;
    }
    public Idamageable GetTargetEngaged()
    {
        return targetEngaged;
    }
    public Transform GetPlayerDetected()
    {
        return playerDetected;
    }
    public void VoidTargets()
    {
        targetDetected = null;
        targetEngaged = null;
        playerDetected = null;
    }
}


