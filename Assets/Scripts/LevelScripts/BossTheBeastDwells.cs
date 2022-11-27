using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTheBeastDwells : BossBehaviour
{
    /*  
        Name: BossTheBeastDwells.cs
        Description: This script controls the behaviour of The Beast Dwells

    */    
    /*[Header("Components")]*/
    private FinalBossController finalBossController;

    [Header("Script Settings")]
    public float spawnRate;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    public override void Awake()
    {
        finalBossController = this.GetComponent<FinalBossController>();
    }
    /*-  Starts the units targeting behaviour -*/
    public override void StartBehaviour()
    {
        StartCoroutine(UpdateTarget(1f));
        StartCoroutine(UngraspableAttack(finalBossController.GetAttackRate()));
        // StartCoroutine(SpawnEnemy(spawnRate));
    }

    /*---      FUNCTIONS     ---*/
    /*-  Repeatedly updates a target and shoots, takes a float for the time -*/
    private IEnumerator UpdateTarget(float time)
    {
        yield return new WaitForSeconds(time); 

        //if gameStates is PLAYING
        if(gameManager.CheckIfPlaying())
        {
            bossCounter++; 

            if(bossCounter == bossCountDown/2)
            {
                specialAttack();
            }
            else if(bossCounter >= bossCountDown)
            {
                bossCounter = 0;
            }
        }
        //if gameStates isn't WIN or LOSE
        if(!gameManager.CheckIfWinOrLose())
        {
            StartCoroutine(UpdateTarget(time));
        }
        //if gameStates is WIN or LOSE
        else if(gameManager.CheckIfWinOrLose())
        {
            this.gameObject.SetActive(false);
        }
    }
    /*-  Repeatedly updates a target and shoots, takes a float for the time -*/
    private IEnumerator UngraspableAttack(float time)
    {
        yield return new WaitForSeconds(time); 

        ApplyAreaOfEffectAttack();
        finalBossController.YellSpecialAttack("Ungraspable Attack");

        //if gameStates isn't WIN or LOSE
        if(!gameManager.CheckIfWinOrLose())
        {
            StartCoroutine(UngraspableAttack(time));
        }
    }
    /*-  Repeatedly spawns Enemy takes a float for the time -*/
    private IEnumerator SpawnEnemy(float rate)
    {
        yield return new WaitForSeconds(rate); 

        //if gameStates is PLAYING
        if(gameManager.CheckIfPlaying())
        {
            GameObject enemyObj = objectPool.SpawnFromPool("Ghost", transform.position, Quaternion.identity);
            TroopController enemy = enemyObj.GetComponent<TroopController>();
            
            //if this enemy exist
            if(enemy != null)
            {
                enemy.SetUnit(finalBossController.GetGermStats());
                enemy.StartController(); //Starts the enemy controller
            }
        }

        //if gameStates isn't WIN or LOSE
        if(!gameManager.CheckIfWinOrLose())
        {
            StartCoroutine(SpawnEnemy(rate)); 
        }
    }
    /*-  Controls aoe attack -*/
    public override void ApplyAreaOfEffectAttack()
    {
        GameObject aoeObj = objectPool.SpawnFromPool("AreaOfEffect", this.transform.position, this.transform.rotation);
        AreaOfEffect aoe = aoeObj.GetComponent<AreaOfEffect>();

        //if this bullet exist
        if(aoe != null)
        {
            aoe.SetAOE((finalBossController.GetStats().unitAttackRange * 2), true, false, (finalBossController.GetAttack() * 0.75f)); //calls the bullet's seek function
        }
    }
    /*-  Controls aoe status effects -*/
    public override void ApplyAreaOfEffectStatus(bool toEnemy, StatusEffect appliedEffect)
    {
        GameObject aoeObj = objectPool.SpawnFromPool("AreaOfEffect", this.transform.position, this.transform.rotation);
        AreaOfEffect aoe = aoeObj.GetComponent<AreaOfEffect>();

        //if this bullet exist
        if(aoe != null)
        {
            aoe.SetAOE((finalBossController.GetStats().unitAttackRange * 2), true, toEnemy, appliedEffect); //calls the bullet's seek function
        }
    }

    /*---      MOVES     ---*/
    public override void SpecialAttack_1()
    {
        Debug.Log("ATTACK ONE");
        finalBossController.YellSpecialAttack("ATTACK ONE");
        specialAttack = GetNextSpecialMove();
    }
    public override void SpecialAttack_2()
    {
        Debug.Log("ATTACK TWO");
        finalBossController.YellSpecialAttack("ATTACK TWO");
        specialAttack = GetNextSpecialMove();
    }
    public override void SpecialAttack_3()
    {
        Debug.Log("ATTACK THREE");
        finalBossController.YellSpecialAttack("ATTACK THREE");
        specialAttack = GetNextSpecialMove();
    }
}
