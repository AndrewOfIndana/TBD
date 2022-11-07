using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossBehaviour : MonoBehaviour
{
    /*  
        Name: BossBehaviour.cs
        Description: This script controls the behaviour of a boss and how it reacts to other units and damage

    */
    public delegate void bossDelegate();

    /*[Header("Static References")]*/
    GameManager gameManager;
    ObjectPool objectPool; 

    /*[Header("Components")]*/
    private BossController bossController;
    private BossMovement bossMovement;

    /*[Header("Script Settings")]*/
    private PlayerAvatar playerAvatar;
    private Transform targetDetected; //What the unit detects
    private Transform playerDetected; //When the unit detects a player
    private Idamageable targetEngaged; //What the unit is fighting

    private bossDelegate specialAttack;
    private int bossCounter = 5;

    #region 
    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        bossController = this.GetComponent<BossController>();
        bossMovement = this.GetComponent<BossMovement>();
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
        objectPool = ObjectPool.instance; 
        playerAvatar = LevelManager.instance.GetPlayerAvatar();

        specialAttack = GetNextSpecialMove();
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
            bossCounter--;

            if(bossCounter <= 0f)
            {
                specialAttack();
                bossCounter = 45;
            }
            
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
            if(bossController.GetStats().targetTags.Any(x => x.Contains(unit.gameObject.tag)))
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
        //if the unit's behaviour is defend, and this position and playerAvatar's position is greater than the units's attackRange * 2 
        if(bossController.GetStats().unitBehaviour == Behaviour.DEFEND && Vector3.Distance(transform.position, playerAvatar.transform.position) <= (bossController.GetStats().unitAttackRange * 2))
        {
            playerDetected = playerAvatar.transform;
            bossController.animator.SetBool("aAttacking", true);
        }
        //if the nearestTarget does exist, shortestDistance is less than or equal to the units's attackRange and playerDetected doesn't exist
        else if(nearestTarget != null && shortestDistance <= bossController.GetStats().unitAttackRange && playerDetected == null)
        {
            targetDetected = nearestTarget;
            bossController.animator.SetBool("aAttacking", true);
        }
        else
        {
            VoidTargets();
            bossController.animator.SetBool("aAttacking", false);
        }
    }
    /*-  Controls engagement of targets  -*/
    private void Engaging()
    {
        //if playerDetected does exist
        if(playerDetected != null)
        {
            //Override targetDetected if player is nearby
            targetDetected = playerDetected;
        }

        //if targetDetected does exist
        if(targetDetected != null)
        {
            if(bossController.GetStats().unitType == UnitType.TOWER)
            {
                Shoot();
            }
            else if(bossController.GetStats().unitType == UnitType.TROOP)
            {
                //if this position and targetDetected's position is greater 3.5 and targetEngaged doesn't exist 
                if(Vector3.Distance(transform.position, targetDetected.position) <= 3.5f && targetEngaged == null)
                {
                    /* STARTS COMBAT FOR RANGED */
                    targetEngaged = targetDetected.gameObject.GetComponent<Idamageable>(); 
                    StartCoroutine(Combat(bossController.GetAttackRate()));
                }
            }
        }
    }
    /*-  Controls Combat between units for bosses, takes float for an attack rate -*/
    private IEnumerator Combat(float atkRate)
    {
        bossController.animator.SetBool("aAttacking", true);
        yield return new WaitForSeconds(atkRate);

        //if targetEngaged does exists
        if(targetEngaged != null)
        {
            targetEngaged.TakeDamage((bossController.GetAttack() * Random.Range(0.75f, 1.25f))); //Transfer the enemy's troop's attack to the this script's TakeDamage function
        }

        ApplyAreaOfEffectAttack();

        Invoke("VoidTargets", 0.5f);
    }
    /*-  When a GameObject collides with another GameObject, Unity calls OnTriggerEnter. -*/
    private void OnTriggerEnter(Collider other)
    {
        //if the boss collides with an opposing bullet
        if (other.gameObject.CompareTag(bossController.GetStats().sharedTags.oncomingBulletTag))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>(); 
            bossController.TakeDamage(bullet.GetAttack()); //Transfer bulletAttack to the this script's TakeDamage function
            bullet.DestroyBullet();
        }
    }
    /*-  Controls shooting -*/
    private void Shoot()
    {
        Debug.Log("shoot");
        GameObject bulletObj = objectPool.SpawnFromPool(bossController.GetStats().sharedTags.bulletTag, new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z), Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        //if this bullet exist
        if(bullet != null)
        {
            bullet.Seek(targetDetected, bossController.GetAttack()); //calls the bullet's seek function
        }
    }
    /*-  Controls shooting -*/
    private void ApplyAreaOfEffectAttack()
    {
        GameObject aoeObj = objectPool.SpawnFromPool("AreaOfEffect", this.transform.position, this.transform.rotation);
        AreaOfEffect aoe = aoeObj.GetComponent<AreaOfEffect>();

        //if this bullet exist
        if(aoe != null)
        {
            aoe.SetAOE((bossController.GetStats().unitAttackRange * 2), true, false, (bossController.GetAttack() * 0.75f)); //calls the bullet's seek function
        }
    }
    private void ApplyAreaOfEffectStatus(bool toEnemy, StatusEffect appliedEffect)
    {
        GameObject aoeObj = objectPool.SpawnFromPool("AreaOfEffect", this.transform.position, this.transform.rotation);
        AreaOfEffect aoe = aoeObj.GetComponent<AreaOfEffect>();

        //if this bullet exist
        if(aoe != null)
        {
            aoe.SetAOE((bossController.GetStats().unitAttackRange * 2), true, toEnemy, appliedEffect); //calls the bullet's seek function
        }
    }
    #endregion
    private bossDelegate GetNextSpecialMove()
    {
        int randomNum = Random.Range(0, 3);

        if(bossController.GetStats().unitBehaviour == Behaviour.DEFEND)
        {
            if(randomNum == 0)
            {
                return GolemAttack_1;
            }
            else if(randomNum == 1)
            {
                return GolemAttack_2;
            }
            else if(randomNum == 2)
            {
                return GolemAttack_3;
            }
        }
        return GolemAttack_1;
    }


    /*---      MOVES     ---*/
    private void GolemAttack_1()
    {
        //HIGH GEAR
        bossController.ApplyEffect(bossController.GetStats().unitStartEffects[0]);
        bossController.StartDecayEffect(bossController.GetStats().unitStartEffects[0], bossController.GetStats().unitStartEffects[0].effectLifetime);
        specialAttack = GetNextSpecialMove();
    }
    private void GolemAttack_2()
    {
        //STONE COLD
        ApplyAreaOfEffectStatus(false, bossController.GetStats().unitStartEffects[1]);
        specialAttack = GetNextSpecialMove();
    }
    private void GolemAttack_3()
    {
        //ARMOR UP
        ApplyAreaOfEffectStatus(true, bossController.GetStats().unitStartEffects[2]);
        specialAttack = GetNextSpecialMove();
    }


    /*---      SET/GET FUNCTIONS     ---*/
    /*-  Gets targetDetected  -*/
    public Transform GetPlayerAvatar()
    {
        return playerAvatar.transform;
    }
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
        bossController.animator.SetBool("aAttacking", false);
    }
}
