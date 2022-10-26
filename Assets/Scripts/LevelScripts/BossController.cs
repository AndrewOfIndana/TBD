using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour, Idamageable, Ieffectable
{
    /*  
        Name: BossController.cs
        Description: This script contains and handles the variables used for the both the behaviour and movement of a boss 

    */
    /*[Header("Static References")]*/
    BossManager bossManager;

    /*[Header("Components")]*/
    private BossBehaviour bossBehaviour;
    private BossMovement bossMovement;

    [Header("GameObject References")]
    public Animator animator;
    public SpriteRenderer thisSprite; 
    public BoxCollider thisCollider;
    private AudioSource audioSource;

    [Header("UI References")]
    public GameObject[] statusUI; //Array of each status effect symbol

    /*[Header("Stats")]*/
    public Stats stat;
    public float attack;
    public float health; 
    public float speed;
    public float attackRate;
    private List<StatusEffect> statusEffects = new List<StatusEffect>();

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        bossMovement = this.GetComponent<BossMovement>();
        bossBehaviour = this.GetComponent<BossBehaviour>();
        audioSource = this.GetComponent<AudioSource>();
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        bossManager = BossManager.instance;
    }
    /*-  Sets the units stats when the object has spawned from pool using the newStats Stats variables -*/
    public void SetUnit(Stats newStats)
    {
        stat = newStats;
        attack = newStats.unitAttack;
        health = newStats.unitHealth;
        speed = newStats.unitSpeed;
        attackRate = newStats.unitAttackRate;
        thisSprite.sprite = newStats.unitSprite;
        thisCollider.size =  newStats.unitSize;
        audioSource.clip = stat.unitsSfx.statSfx1;
        this.gameObject.tag = newStats.unitTag;
    }
    /*-  Starts the unit's behaviour and movement -*/
    public void StartController()
    {
        for(int i = 0; i < statusUI.Length; i++)
        {
            statusUI[i].SetActive(false);
        }
        animator.speed = stat.unitWalkSpeed; //Sets animation speed
        bossBehaviour.StartBehaviour(); //Starts the boss's Behaviour
        audioSource.Play();
    }
    /*---      FUNCTIONS     ---*/
    /*-  Handles applying a status effect for a unit takes a StatusEffect for the applied effect -*/
    public void ApplyEffect(StatusEffect appliedEffect)
    {
        //if statusEffects doesn't contain appliedEffect
        if(!statusEffects.Contains(appliedEffect))
        {
            statusEffects.Add(appliedEffect);
            statusUI[appliedEffect.effectId].SetActive(true);
            BuffUnit();
        }
    }
    /*-  Buffs the units stats when applying a status effect -*/
    private void BuffUnit()
    {
        /*  Makes new variables for the new attack  */
        float newAttack = stat.unitAttack;
        float newHealth = health > stat.unitHealth ? newHealth = stat.unitHealth: newHealth = health; //If health is greater than set newHealth to stat.unitHealth, else set newHealth to health 
        float newSpeed = stat.unitSpeed;
        float newAttackRate = stat.unitAttackRate;

        /*  Applies each status buff in each status effect for the unit's stats  */
        for(int i = 0; i < statusEffects.Count; i++)
        {
            for(int k = 0; k < statusEffects[i].effects.Count; k++)
            {
                newAttack = newAttack * statusEffects[i].effects[k].GetStatusBonus(BuffedStats.attack);
                newHealth = newHealth * statusEffects[i].effects[k].GetStatusBonus(BuffedStats.health);
                newSpeed = newSpeed * statusEffects[i].effects[k].GetStatusBonus(BuffedStats.speed);
                newAttackRate = newAttackRate * statusEffects[i].effects[k].GetStatusBonus(BuffedStats.attackRate);
            }
        }

        /*  Sets stat values to newStats values */
        attack = newAttack;
        health = newHealth;
        speed = newSpeed;
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
            statusUI[removedEffect.effectId].SetActive(false);
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
        if(health <= 0)
        {
            bossBehaviour.VoidTargets();
            bossManager.EndBoss();
        }
    }
    /*-  OnDisable is called when the object becomes disabled -*/
    private void OnDisable()
    {
        statusEffects.Clear(); //Clears statusEffects
        audioSource.Stop();
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
    /*-  Get speed -*/
    public float GetSpeed()
    {
        return speed;
    }
    /*-  Get attack rate -*/
    public float GetAttackRate()
    {
        return attackRate;
    }
}
