using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossController : MonoBehaviour, Idamageable, Ieffectable
{
    /*  
        Name: FinalBossController.cs
        Description: This script contains and handles the variables used for the both the behaviour and movement of the final boss 

    */
    /*[Header("Static References")]*/
    GameManager gameManager;
    BossManager bossManager;

    /*[Header("Components")]*/
    private BossBehaviour bossBehaviour;

    [Header("GameObject References")]
    public GameObject tentaclePrefab;
    // private AudioSource audioSource;
    public List<TowerController> tentacles = new List<TowerController>();
    public Stats germStats;

    [Header("Stats")]
    public Stats stat;
    private float attack;
    private float health; 
    private float attackRate;
    private List<StatusEffect> statusEffects = new List<StatusEffect>();

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        bossBehaviour = this.GetComponent<BossBehaviour>();
        // audioSource = this.GetComponent<AudioSource>();
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
        bossManager = BossManager.instance;
        SetUnit();
        StartController();
    }
    /*-  Sets the units stats when the object has spawned from pool using the newStats Stats variables -*/
    public void SetUnit()
    {
        attack = stat.unitAttack;
        health = stat.unitHealth;
        attackRate = stat.unitAttackRate;
        // audioSource.clip = stat.unitUtils.unitsSfx.GetRandomSfx();
        this.gameObject.tag = stat.unitUtils.unitTag;
    }
    public void SpawnTentacle(Transform[] spawnPoints)
    {
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject towerObj = Instantiate(tentaclePrefab, spawnPoints[i].position , Quaternion.identity);
            FinalTowerBehaviour tentacle = towerObj.GetComponent<FinalTowerBehaviour>();
            tentacle.SetController(this);

            if(tentacle != null)
            {
                if(i == 0)
                {
                    tentacle.SetSpawnType(false);
                }
                else
                {
                    tentacle.SetSpawnType(true);
                }
            }
            tentacles.Add(towerObj.GetComponent<TowerController>());
        }
    }
    /*-  Starts the unit's behaviour and movement -*/
    public void StartController()
    {
        bossBehaviour.StartBehaviour(); //Starts the boss's Behaviour
        // audioSource.Play();
    }
    /*---      FUNCTIONS     ---*/
    /*-  Handles applying a status effect for a unit takes a StatusEffect for the applied effect -*/
    public void ApplyEffect(StatusEffect appliedEffect)
    {
        //if statusEffects doesn't contain appliedEffect
        if(!statusEffects.Contains(appliedEffect))
        {
            statusEffects.Add(appliedEffect);
            BuffUnit();
        }
    }
    /*-  Buffs the units stats when applying a status effect -*/
    private void BuffUnit()
    {
        /*  Makes new variables for the new attack  */
        float newAttack = stat.unitAttack;
        float newAttackRate = stat.unitAttackRate;

        /*  Applies each status buff in each status effect for the unit's stats  */
        for(int i = 0; i < statusEffects.Count; i++)
        {
            for(int k = 0; k < statusEffects[i].effects.Count; k++)
            {
                newAttack = newAttack * statusEffects[i].effects[k].GetStatusBonus(BuffedStats.attack);
                newAttackRate = newAttackRate * statusEffects[i].effects[k].GetStatusBonus(BuffedStats.attackRate);
            }
        }

        /*  Sets stat values to newStats values */
        attack = newAttack;
        attackRate = newAttackRate;
        bossManager.UpdateHealthUI();
    }
    /*-  Calls DecayEffect when called from another script -*/
    public void StartDecayEffect(StatusEffect decayingEffect, float lifeTime)
    {
        StartCoroutine(DecayEffect(decayingEffect, lifeTime));
    }
    /*-  Handles the lifetime of a status effect, takes the StatusEffect that is decaying and float for the life time -*/
    public IEnumerator DecayEffect(StatusEffect decayingEffect, float lifeTime)
    {
        //if statusEffects contains decayingEffect
        if(statusEffects.Contains(decayingEffect))
        {
            yield return new WaitForSeconds(lifeTime);
            RemoveEffect(decayingEffect);
        }
    }
    /*-  Handles removing a status effect from a unit takes the StatusEffect that will be removed  -*/
    public void RemoveEffect(StatusEffect removedEffect)
    {
        //if statusEffects contains removedEffect
        if(statusEffects.Contains(removedEffect))
        {
            statusEffects.Remove(removedEffect);
            BuffUnit();
        }
    }
    /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    public void TakeDamage(float damage)
    {
        health -= damage;
        bossManager.UpdateHealthUI();

        //if health is less than or equal to 0
        if(health <= 0 && this.gameObject.activeSelf)
        {
            // AudioSource.PlayClipAtPoint(stat.unitUtils.unitsSfx.deathSfx, this.transform.position, gameManager.GetGameOptions().GetVoiceClipVolume());
            bossManager.EndBoss();
        }
    }
    /*-  changes BossManager to change the string -*/
    public void YellSpecialAttack(string str)
    {
        string sStr = "---" + str + "---";
        bossManager.UpdateSpecialMoveText(sStr);
    }

    /*-  OnDisable is called when the object becomes disabled -*/
    private void OnDisable()
    {
        statusEffects.Clear(); //Clears statusEffects
        // audioSource.Stop();
    }

    /*---      SET/GET FUNCTIONS     ---*/
    /*-  Get Stats -*/
    public Stats GetStats()
    {
        return stat;
    }
    /*-  Get attack -*/
    public float GetAttack()
    {
        return attack;
    }
    /*-  Get health -*/
    public float GetHealth()
    {
        return health;
    }
    /*-  Get attack rate -*/
    public float GetAttackRate()
    {
        return attackRate;
    }
    /*-  Get attack rate -*/
    public Stats GetGermStats()
    {
        return germStats;
    }
}
