using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AreaOfEffect : MonoBehaviour
{
    /*[Header("Static References")]*/
    GameManager gameManager;

    /*[Header("Components")]*/
    private SphereCollider aoeCollider;

    [Header("Script Settings")]
    public GameObject[] aoeParticles;
    private bool isAppliedByEnemy;
    private bool isAppliedToEnemy;
    private float aoeAttack;
    private StatusEffect aoeEffect;
    private bool isAoeAttack;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        aoeCollider = this.GetComponent<SphereCollider>();
        ClearParticleEffects();
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
    }
    
    private void OnEnable()
    {
        Invoke("DestroyAOE", 1f); //Destroys bullet after half a second
    }

    public void SetAOE(float spread, bool fromEnemy, bool toEnemy, float atk)
    {
        aoeCollider.radius = spread;
        isAppliedByEnemy = fromEnemy;
        isAppliedToEnemy = toEnemy;
        aoeAttack = atk;

        if(!fromEnemy)
        {
            UpdateParticleEffect(0, spread);
        }
        else if(fromEnemy)
        {
            UpdateParticleEffect(1, spread);
        }
    }
    /*-  Sets target to the tower's enemyDetected as well as bullet's attack value -*/
    public void SetAOE(float spread, bool fromEnemy, bool toEnemy, StatusEffect statEffect)
    {
        aoeCollider.radius = spread;
        isAppliedByEnemy = fromEnemy;
        isAppliedToEnemy = toEnemy;
        aoeEffect = statEffect;

        if(!fromEnemy)
        {
            UpdateParticleEffect(2, spread);
        }
        else if(fromEnemy)
        {
            UpdateParticleEffect(3, spread);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Unit otherUnit = other.GetComponent<Unit>();

        if(otherUnit != null)
        {
            if(isAppliedByEnemy && !isAppliedToEnemy && !otherUnit.IsEnemy() && aoeEffect == null)
            {
                SpreadDamage(other);
            }
            else if(!isAppliedByEnemy && isAppliedToEnemy && otherUnit.IsEnemy() && aoeEffect == null)
            {
                SpreadDamage(other);
            }
            else if(isAppliedByEnemy && !isAppliedToEnemy && !otherUnit.IsEnemy() && aoeEffect != null)
            {
                SpreadEffect(other);
            }
            else if(!isAppliedByEnemy && isAppliedToEnemy && otherUnit.IsEnemy() && aoeEffect != null)
            {
                SpreadEffect(other);
            }
            else if(!isAppliedByEnemy && !isAppliedToEnemy && !otherUnit.IsEnemy() && aoeEffect != null)
            {
                SpreadEffect(other);
            }
        }
    }

    private void SpreadDamage(Collider other)
    {
        Idamageable otherOpponent = other.GetComponent<Idamageable>();
        if(otherOpponent != null)
        {
            otherOpponent.TakeDamage(aoeAttack);
        }
    }
    private void SpreadEffect(Collider other)
    {
        Ieffectable otherStatus = other.GetComponent<Ieffectable>();
        if(otherStatus != null)
        {
            otherStatus.ApplyEffect(aoeEffect);
            otherStatus.StartDecayEffect(aoeEffect, aoeEffect.effectLifetime);
        }
    }

    /*---      FUNCTIONS     ---*/
    private void ClearParticleEffects()
    {
        for(int i = 0; i < aoeParticles.Length; i++)
        {
            aoeParticles[i].transform.localScale = new Vector3(1, 1, 1);
            aoeParticles[i].SetActive(false);
        }
    }

    private void UpdateParticleEffect(int index, float spread)
    {

        aoeParticles[index].transform.localScale = new Vector3(spread, 1, spread);
        aoeParticles[index].SetActive(true);
    }
    /*-  Destroys the bullet -*/
    public void DestroyAOE()
    {
        aoeEffect = null;
        aoeAttack = 0;
        ClearParticleEffects();
        this.gameObject.SetActive(false); 
    }

    /*---      SET/GET FUNCTIONS     ---*/
}
