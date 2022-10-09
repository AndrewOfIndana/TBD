using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    /*  
        Name: PlayerController.cs
        Description: This script allows the player to choose what units and cards the player wants to deploy, as well as handle mana and card cost times.

    */
    /*[Header("Static Variables")]*/
    LevelManager levelManager;
    LevelUI levelUI;

    [Header("Controller References")]
    public PlayerSpawner playerSpawner;
    private PlayerTowerDeployer playerTowerDeployer;

    [Header("PlayerController Variables")]
    public float mana = 100;
    public float manaRegen = 2;
    private List<Stats> units = new List<Stats>(); 

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
        levelManager = LevelManager.levelManagerInstance;
        levelUI = LevelUI.levelUIinstance;

        /* Gets and sets variables form the level manager */
        units = levelManager.playerUnits;
    }
    /*-  StartGame is called when the game has started -*/
    public void StartGame()
    {
        StartCoroutine(RegenerateMana(1f)); //Calls RegenerateMana IEnumerator at 1 second
        levelUI.UpdateUI();
    }

    /*---      FUNCTIONS     ---*/
    /*-  Spawns a unit, takes an int from the buttons in the levelUI Script -*/
    public void SpawnUnit(int index)
    {
        //if the mana minus the unitCost isn't less than or equal to 0
        if (CheckManaCost(units[index].unitCost))
        {
            //if the buttons are deploying troop 
            if (units[index].unitType == UnitType.TROOP)
            {
                SpendMana(units[index].unitCost);
                playerSpawner.SpawnTroop(units[index]); //Calls the spawnTroop function in the playerTroopSpawner script
            }
            else if (units[index].unitType == UnitType.TOWER) //if the buttons are deploying tower 
            {
                SpendMana(units[index].unitCost);
                playerTowerDeployer.SetSelectedTower(units[index]); //Calls the SetSelectedTower function in the PlayerTowerDeployer script
            }
        }
    }
    /*-  Repeatedly regenerates mana, takes a float for the time -*/
    private IEnumerator RegenerateMana(float time)
    {
        yield return new WaitForSeconds(time);

        //if the mana plus manaRegen is less than 100
        if ((mana + manaRegen) <= 100)
        {
            mana += manaRegen;
        }
        levelUI.UpdateUI();
        StartCoroutine(RegenerateMana(1f)); //Recalls RegenerateMana IEnumerator at 1 second
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
}
