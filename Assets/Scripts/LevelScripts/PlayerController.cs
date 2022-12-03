using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    /*  
        Name: PlayerController.cs
        Description: This script allows the player to choose what units and cards the player wants to deploy, as well as handle mana and card cost times.

    */
    /*[Header("Static References")]*/
    GameManager gameManager;
    LevelManager levelManager;
    LevelUI levelUI;

    [Header("Controller References")]
    public PlayerSpawner playerSpawner;
    private PlayerTowerDeployer playerTowerDeployer;
    public PlayerAvatar playerAvatar;
    public Aura playerAura;

    [Header("Script Settings")]
    public float mana = 100;
    public float manaRegen = 2;
    private List<Stats> units = new List<Stats>(); 
    public List<CardEffects> cards = new List<CardEffects>();
    public List<StatusEffect> playerPassiveEffect = new List<StatusEffect>();

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        playerTowerDeployer = this.GetComponent<PlayerTowerDeployer>();
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
        levelManager = LevelManager.instance;
        levelUI = LevelUI.instance;

        mana = levelManager.GetLevel().mana;
        manaRegen = levelManager.GetLevel().manaRegen;
        units = levelManager.GetPlayerUnits();
    }
    /*-  StartGame is called when the game has started -*/
    public void StartGame()
    {
        playerAvatar = levelManager.GetPlayerAvatar();
        playerAura = playerAvatar.GetPlayerAura();
        UpdateAura();
        StartCoroutine(RegenerateMana(1f));
        levelUI.UpdateUI(); //Updates UI when the playerStart
    }

    /*---      FUNCTIONS     ---*/
    /*-  Spawns a unit, takes an int from the buttons in the levelUI Script -*/
    public void SpawnUnit(int index)
    {
        //if the mana minus the unitCost isn't less than or equal to 0
        if (CheckManaCost(units[index].unitCost))
        {
            //if the buttons are deploying troop 
            if (units[index].unitUtils.unitType == UnitType.TROOP)
            {
                SpendMana(units[index].unitCost);
                playerSpawner.SpawnTroop(units[index]); //Calls the spawnTroop function in the playerTroopSpawner script
            }
            else if (units[index].unitUtils.unitType == UnitType.TOWER) //if the buttons are deploying tower 
            {
                SpendMana(units[index].unitCost);
                playerTowerDeployer.SetSelectedTower(units[index]); //Calls the SetSelectedTower function in the PlayerTowerDeployer script
            }
        }
    }

    public void AddCard(CardEffects card)
    {
        cards.Add(card);
        playerPassiveEffect.Add(card.passiveEffect);
    }
    public void ActivateCard(CardEffects card, int index)
    {
        if(playerAvatar != null && playerAvatar.isActiveAndEnabled == true)
        {
            playerAvatar.ActivateCardEffect(card.isAppliedToEnemy, card.activeEffect);
        }

        if(cards.Contains(card))
        {
            playerPassiveEffect.Remove(card.passiveEffect);
        }
        UpdateAura();
    }
    public void ReplaceCard(CardEffects card, int index)
    {
        cards[index] =  card;
        playerPassiveEffect.Add(card.passiveEffect);
        UpdateAura();
    }
    public void UpdateAura()
    {
        if(playerAvatar != null && playerAvatar.isActiveAndEnabled == true)
        {
            playerPassiveEffect.Select(e => e).Distinct();
            playerAura.SetAuraEffect(playerPassiveEffect);
        }
    }

    /*-  Repeatedly regenerates mana, takes a float for the time -*/
    private IEnumerator RegenerateMana(float time)
    {
        yield return new WaitForSeconds(time);

        //if gameStates is PLAYING
        if(gameManager.CheckIfPlaying())
        {
            //if the mana plus manaRegen is less than 100
            if ((mana + manaRegen) <= levelManager.GetLevel().GetMaxMana())
            {
                mana += manaRegen;
            }
            levelUI.UpdateUI();
        }

        //if gameStates isn't WIN or LOSE
        if(!gameManager.CheckIfWinOrLose())
        {
            StartCoroutine(RegenerateMana(1f));
        }
    }
    /*-  Spends mana, takes a float for the cost -*/
    private void SpendMana(float cost)
    {
        mana -= cost;
        levelUI.UpdateUI();
    }
    /*-  Checks if the the mana spent is valid, takes a float for the cost, returns a bool -*/
    public bool CheckManaCost(float cost)
    {
        ///if the mana - the cost is greater than 0
        if ((mana - cost) >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*---      SET/GET FUNCTIONS     ---*/
    public float GetMana()
    {
        return mana;
    }
}
