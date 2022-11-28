using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class BossBehaviour : MonoBehaviour
{
    /*  
        Name: BossBehaviour.cs
        Description: This script controls the behaviour of a boss and how it reacts to other units and damage

    */
    public delegate void bossDelegate();

    /*[Header("Static References")]*/
    protected GameManager gameManager;
    protected ObjectPool objectPool; 

    /*[Header("Components")]*/
    protected BossController bossController;
    protected PlayerAvatar playerAvatar;

    protected bossDelegate specialAttack;
    protected int bossCounter = 0;
    public int bossCountDown = 45;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    public virtual void Awake()
    {
        bossController = this.GetComponent<BossController>();
    }
    /*-  Start is called before the first frame update -*/
    public void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
        objectPool = ObjectPool.instance; 
        playerAvatar = LevelManager.instance.GetPlayerAvatar();

        specialAttack = GetNextSpecialMove();
    }

    /*-  Starts the units targeting behaviour -*/
    public abstract void StartBehaviour();

    /*-  Controls aoe attack -*/
    public virtual void ApplyAreaOfEffectAttack()
    {
        GameObject aoeObj = objectPool.SpawnFromPool("AreaOfEffect", this.transform.position, this.transform.rotation);
        AreaOfEffect aoe = aoeObj.GetComponent<AreaOfEffect>();

        //if this bullet exist
        if(aoe != null)
        {
            aoe.SetAOE((bossController.GetStats().unitAttackRange * 2), true, false, (bossController.GetAttack() * 0.75f)); //calls the bullet's seek function
        }
    }
    /*-  Controls aoe status effects -*/
    public virtual void ApplyAreaOfEffectStatus(bool toEnemy, StatusEffect appliedEffect)
    {
        GameObject aoeObj = objectPool.SpawnFromPool("AreaOfEffect", this.transform.position, this.transform.rotation);
        AreaOfEffect aoe = aoeObj.GetComponent<AreaOfEffect>();

        //if this bullet exist
        if(aoe != null)
        {
            aoe.SetAOE((bossController.GetStats().unitAttackRange * 2), true, toEnemy, appliedEffect); //calls the bullet's seek function
        }
    }
    /*-  Gets the next special move -*/
    public bossDelegate GetNextSpecialMove()
    {
        int randomNum = Random.Range(0, 3);

        if(randomNum == 0)
        {
            return SpecialAttack_1;
        }
        else if(randomNum == 1)
        {
            return SpecialAttack_2;
        }
        else if(randomNum == 2)
        {
            return SpecialAttack_3;
        }
        return SpecialAttack_1;
    }
    /*-  first Special Attack for boss -*/
    public abstract void SpecialAttack_1();

    /*-  second Special Attack for boss -*/
    public abstract void SpecialAttack_2();

    /*-  third Special Attack for boss -*/
    public abstract void SpecialAttack_3();

    /*---      SET/GET FUNCTIONS     ---*/
    /*-  Gets targetDetected  -*/
    public Transform GetPlayerAvatar()
    {
        return playerAvatar.transform;
    }
    /*-  Gets targetDetected  -*/
    public virtual Transform GetTargetDetected()
    {
        return null;
    }
    /*-  Gets playerDetected  -*/
    public virtual Transform GetPlayerDetected() 
    {
        return null;
    }

    /*-  Sets all target to null  -*/
    public virtual void VoidTargets() 
    {
        Debug.Log("this isn't suppose to run");
    }

}
