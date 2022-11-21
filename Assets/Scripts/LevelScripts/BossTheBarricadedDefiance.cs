using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossTheBarricadedDefiance : BossBehaviour
{
    /*  
        Name: BossTheBarricadeDefiance.cs
        Description: This script controls the behaviour of The Barricaded Defiance

    */

    /*[Header("Script Settings")]*/
    private Transform targetDetected; //What the unit detects

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts the units targeting behaviour -*/
    public override void StartBehaviour()
    {
        //SUMMON 4 OTHER TOWERS
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
        //if the nearestTarget does exist, shortestDistance is less than or equal to the units's attackRange
        if(nearestTarget != null && shortestDistance <= bossController.GetStats().unitAttackRange)
        {
            targetDetected = nearestTarget;
            bossController.animator.SetBool("aAttacking", true);
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
            Shoot();
        }
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
    /*-  Controls shooting -*/
    private void Shoot()
    {
        GameObject bulletObj = objectPool.SpawnFromPool(bossController.GetStats().unitUtils.sharedTags.bulletTag, new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z), Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        //if this bullet exist
        if(bullet != null)
        {
            bullet.Seek(targetDetected, bossController.GetAttack()); //calls the bullet's seek function
        }
    }

    /*---      MOVES     ---*/
    public override void SpecialAttack_1()
    {
        //PETRIFY
        ApplyAreaOfEffectStatus(false, bossController.GetStats().unitUtils.unitKeyEffects[0]);
        bossController.YellSpecialAttack(bossController.GetStats().unitUtils.unitKeyEffects[0].effectName);
        specialAttack = GetNextSpecialMove();
    }
    public override void SpecialAttack_2()
    {
        //TOWER RALLY
        ApplyAreaOfEffectStatus(true, bossController.GetStats().unitUtils.unitKeyEffects[1]);
        bossController.YellSpecialAttack(bossController.GetStats().unitUtils.unitKeyEffects[1].effectName);
        specialAttack = GetNextSpecialMove();
    }
    public override void SpecialAttack_3()
    {
        //DEMORALIZE
        ApplyAreaOfEffectStatus(false, bossController.GetStats().unitUtils.unitKeyEffects[2]);
        bossController.YellSpecialAttack(bossController.GetStats().unitUtils.unitKeyEffects[2].effectName);
        specialAttack = GetNextSpecialMove();
    }

    /*---      SET/GET FUNCTIONS     ---*/
    public override Transform GetTargetDetected()
    {
        return targetDetected;
    }
    /*-  Sets all target to null  -*/
    public override void VoidTargets()
    {
        targetDetected = null;
        bossController.animator.SetBool("aAttacking", false);
    }
}
