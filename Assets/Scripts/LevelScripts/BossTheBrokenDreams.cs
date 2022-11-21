using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossTheBrokenDreams : BossBehaviour
{
    /*  
        Name: BossTheBrokenDreams.cs
        Description: This script controls the behaviour of The Broken Dreams

    */

    /*[Header("Script Settings")]*/
    private Transform targetDetected; //What the unit detects
    private Transform playerDetected; //When the unit detects a player
    private Idamageable targetEngaged; //What the unit is fighting

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts the units targeting behaviour -*/
    public override void StartBehaviour()
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
            bossCounter++;

            if(bossCounter >= bossCountDown)
            {
                specialAttack();
                bossCounter = 0;
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
            if(bossController.GetStats().unitUtils.targetTags.Any(x => x.Contains(unit.gameObject.tag)))
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
        //if the playerAvatar's position is greater than the units's attackRange * 2 
        if(Vector3.Distance(transform.position, playerAvatar.transform.position) <= (bossController.GetStats().unitAttackRange * 2))
        {
            playerDetected = playerAvatar.transform;
        }
        //if the nearestTarget does exist, shortestDistance is less than or equal to the units's attackRange and playerDetected doesn't exist
        else if(nearestTarget != null && shortestDistance <= bossController.GetStats().unitAttackRange && playerDetected == null)
        {
            targetDetected = nearestTarget;
        }
        else
        {
            VoidTargets();
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
            //if this position and targetDetected's position is greater 3.5 and targetEngaged doesn't exist 
            if(Vector3.Distance(transform.position, targetDetected.position) <= 3.5f && targetEngaged == null)
            {
                /* STARTS COMBAT FOR RANGED */
                targetEngaged = targetDetected.gameObject.GetComponent<Idamageable>(); 
                StartCoroutine(Combat(bossController.GetAttackRate()));
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
        if (other.gameObject.CompareTag(bossController.GetStats().unitUtils.sharedTags.oncomingBulletTag))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>(); 
            bossController.TakeDamage(bullet.GetAttack()); //Transfer bulletAttack to the this script's TakeDamage function
            bullet.DestroyBullet();
        }
    }

    /*---      MOVES     ---*/
    public override void SpecialAttack_1()
    {
        //HIGH GEAR
        bossController.ApplyEffect(bossController.GetStats().unitUtils.unitKeyEffects[0]);
        bossController.StartDecayEffect(bossController.GetStats().unitUtils.unitKeyEffects[0], bossController.GetStats().unitUtils.unitKeyEffects[0].effectLifetime);
        bossController.YellSpecialAttack(bossController.GetStats().unitUtils.unitKeyEffects[0].effectName);
        specialAttack = GetNextSpecialMove();
    }
    public override void SpecialAttack_2()
    {
        //STONE COLD
        ApplyAreaOfEffectStatus(false, bossController.GetStats().unitUtils.unitKeyEffects[1]);
        bossController.YellSpecialAttack(bossController.GetStats().unitUtils.unitKeyEffects[1].effectName);
        specialAttack = GetNextSpecialMove();
    }
    public override void SpecialAttack_3()
    {
        //ARMOR UP
        ApplyAreaOfEffectStatus(true, bossController.GetStats().unitUtils.unitKeyEffects[2]);
        bossController.YellSpecialAttack(bossController.GetStats().unitUtils.unitKeyEffects[2].effectName);
        specialAttack = GetNextSpecialMove();
    }

    /*---      SET/GET FUNCTIONS     ---*/
    /*-  Gets targetDetected  -*/
    public override Transform GetTargetDetected()
    {
        return targetDetected;
    }
    /*-  Gets targetEngaged  -*/
    public Idamageable GetTargetEngaged()
    {
        return targetEngaged;
    }
    /*-  Gets playerDetected  -*/
    public override Transform GetPlayerDetected()
    {
        return playerDetected;
    }
    /*-  Sets all target to null  -*/
    public override void VoidTargets()
    {
        targetDetected = null;
        targetEngaged = null;
        playerDetected = null;
        bossController.animator.SetBool("aAttacking", false);
    }
}
