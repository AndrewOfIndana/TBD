using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TroopBehaviour : MonoBehaviour
{
    /*  
        Name: TroopBehaviour.cs
        Description: This script controls the behaviour of a troop and how it reacts to other units

    */
    /*[Header("Static References")]*/
    GameManager gameManager;
    ObjectPool objectPool; 

    /*[Header("Components")]*/
    private TroopController troopController;
    private TroopMovement troopMovement;

    /*[Header("Script Settings")]*/
    private PlayerAvatar playerAvatar;
    private Transform targetDetected; //What the unit detects
    private Transform playerDetected; //When the unit detects a player
    private Idamageable targetEngaged; //What the unit is fighting

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        troopController = this.GetComponent<TroopController>();
        troopMovement = this.GetComponent<TroopMovement>();
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
        objectPool = ObjectPool.instance; 
        playerAvatar = LevelManager.instance.GetPlayerAvatar();
    }
    /*-  Starts the units targeting behaviour -*/
    public void StartBehaviour()
    {
        StartCoroutine(UpdateTarget(1f));
    }

    /*---      FUNCTIONS     ---*/
    /*-  Repeatedly updates a target, takes a float for the time -*/
    private IEnumerator UpdateTarget(float time)
    {
        yield return new WaitForSeconds(time);

        //if gameStates is PLAYING
        if(gameManager.CheckIfPlaying())
        {
            Targeting();
            Engaging();
        }

        //if gameStates isn't WIN or LOSE
        if(!gameManager.CheckIfWinOrLose())
        {
            StartCoroutine(UpdateTarget(1f));
        }
        //if gameStates is WIN or LOSE
        else if(gameManager.CheckIfWinOrLose())
        {
            this.gameObject.SetActive(false);
        }
    }
    /*-  Controls targeting -*/
    private void Targeting()
    {
        float shortestDistance = Mathf.Infinity;
        Transform nearestTarget = null;
        
        foreach(Unit unit in Unit.GetUnitList())
        {
            //If the unit's target tags contain the other units's tag
            if(troopController.GetStats().targetTags.Any(x => x.Contains(unit.gameObject.tag)))
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

        //if the nearestTarget does exist and shortestDistance is less than or equal to the units's attackRange
        if(nearestTarget != null && shortestDistance <= troopController.GetStats().unitAttackRange)
        {
            targetDetected = nearestTarget;

            //if the unit's behaviour is RANGED and targetEngaged doesn't exist
            if(troopController.GetStats().unitBehaviour == Behaviour.RANGED && targetEngaged == null)
            {
                /* STARTS COMBAT FOR RANGED */
                targetEngaged = targetDetected.gameObject.GetComponent<Idamageable>(); 
                StartCoroutine(Combat(troopController.GetAttackRate()));
            }
        }
        //if the unit's behaviour is defend, and this position and playerAvatar's position is greater than the units's attackRange * 2 
        else if(troopController.GetStats().unitBehaviour == Behaviour.DEFEND && Vector3.Distance(transform.position, playerAvatar.transform.position) <= (troopController.GetStats().unitAttackRange * 2))
        {
            /* SETS playerDetected for friendly golems */
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
            if(Vector3.Distance(transform.position, targetDetected.position) <= 2.5f && troopController.GetStats().unitBehaviour != Behaviour.RANGED && targetEngaged == null)
            {
                /* STARTS COMBAT FOR EVERYONE ELSE */
                targetEngaged = targetDetected.gameObject.GetComponent<Idamageable>(); 
                StartCoroutine(Combat(troopController.GetAttackRate()));
            }
        }
    }
    /*-  Controls Combat between units for troops, takes float for an attack rate -*/
    private IEnumerator Combat(float atkRate)
    {
        troopController.animator.SetBool("aAttacking", true);
        yield return new WaitForSeconds(atkRate);

        //if targetEngaged does exists
        if(targetEngaged != null)
        {
            targetEngaged.TakeDamage((troopController.GetAttack() * Random.Range(0.75f, 1.25f))); //Transfer the enemy's troop's attack to the this script's TakeDamage function
        }

        if(troopController.GetStats().unitBehaviour == Behaviour.DEFEND)
        {
            ApplyAreaOfEffect(true);
        }

        Invoke("VoidTargets", 0.5f);
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
    /*-  Controls shooting -*/
    private void ApplyAreaOfEffect(bool isAttack)
    {
        GameObject aoeObj = objectPool.SpawnFromPool("AreaOfEffect", this.transform.position, this.transform.rotation);
        AreaOfEffect aoe = aoeObj.GetComponent<AreaOfEffect>();

        //if this bullet exist
        if(aoe != null)
        {
            if(isAttack)
            {
                aoe.SetAOE(troopController.GetStats().unitAttackRange, troopController.GetStats().isUnitEnemy, !troopController.GetStats().isUnitEnemy, (troopController.GetAttack() * 0.75f)); //calls the bullet's seek function
            }
        }
    }

    /*---      SET/GET FUNCTIONS     ---*/
    /*-  Gets targetDetected  -*/
    public Transform GetTargetDetected()
    {
        return targetDetected;
    }
    /*-  Gets targetEngaged  -*/
    public Idamageable GetTargetEngaged()
    {
        return targetEngaged;
    }
    /*-  Gets playerDetected  -*/
    public Transform GetPlayerDetected()
    {
        return playerDetected;
    }
    /*-  Sets all target to null  -*/
    public void VoidTargets()
    {
        targetDetected = null;
        targetEngaged = null;
        playerDetected = null;
        troopController.animator.SetBool("aAttacking", false);
    }
}


