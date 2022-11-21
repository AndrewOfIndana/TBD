using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossManager : MonoBehaviour
{
    /*  
        Name: BossManager.cs
        Description: This script handles spawning bosses, manages the ui of boss health, and handles hijacking the level ends

    */
    public static BossManager instance;

    /*[Header("Static References")]*/
    GameManager gameManager;
    LevelManager levelManager;

    [Header("Script Settings")]
    public GameObject bossPrefab;
    public Stats bossStat;
    private BossController boss;

    [Header("Boss UI References")]
    public GameObject enemySpawnerUI; //A reference to the enemySpawner health bar
    public GameObject bossUI; //A reference to the boss UI
    public TextMeshProUGUI bossName;
    public TextMeshProUGUI bossMoveName;
    public Image bossHealthBar; 
    private Color healthColor;
    private Color buffedHpColor = Color.yellow;
    private Color enragedColor = new Color(1f, 0.16f, 0.14f);

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        /* SINGLETON PATTERN */
        //if the instance does exist and the instance isn't this
        if (instance != null && instance != this) 
        { 
            return;
        } 
        else 
        { 
            instance = this; 
        } 
        healthColor = bossHealthBar.color;
    }
    /*-  Start is called before the first frame update -*/
    public void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
        levelManager = LevelManager.instance;
        UpdateSetUpText();
    }
    /*-  Starts boss fight, Spawns the boss and changes the UI of the game -*/
    public void StartBoss()
    {
        GameObject bossObj = Instantiate(bossPrefab, levelManager.enemySpawner.transform.position, Quaternion.identity);
        boss = bossObj.GetComponent<BossController>();
        enemySpawnerUI.SetActive(false);
        bossUI.SetActive(true);
        ClearSpecialMoveText();
    }
    /*-  Updates the setup texts and even the unit select thumbnails, should only be called once   -*/
    private void UpdateSetUpText()
    {
        bossName.text = bossStat.unitName;
        bossHealthBar.fillAmount = 1;
        bossUI.SetActive(false);
    }
    /*-  Updates the setup texts and even the unit select thumbnails, should only be called once   -*/
    public void UpdateSpecialMoveText(string str)
    {
        bossMoveName.text = str;
        Invoke("ClearSpecialMoveText", 2f);
    }
    private void ClearSpecialMoveText()
    {
        bossMoveName.text = "";
    }

    /*-  Updates the health bar of a unit -*/
    public void UpdateHealthUI()
    {
        //if boss's health is greater than the boss's stat.unitHealth and levelManager's isEnraged is true
        if(boss.GetHealth() > bossStat.unitHealth && levelManager.GetIsEnraged())
        {
            bossHealthBar.color = enragedColor;
        }
        //if boss's health is greater than the boss's stat.unitHealth and levelManager's isEnraged is flase
        else if(boss.GetHealth() > bossStat.unitHealth && !levelManager.GetIsEnraged())
        {
            bossHealthBar.color = buffedHpColor;
        }
        else
        {
            bossHealthBar.color = healthColor;
            bossHealthBar.fillAmount = boss.GetHealth()/bossStat.unitHealth;
        }
    }
    /*-  Ends boss fight, changes the the gameState to win -*/
    public void EndBoss()
    {
        gameManager.SetGameState(GameStates.WIN); //Sets GameStates to WIN
        levelManager.ChangeState(); //Changes State for level
        boss.gameObject.SetActive(false);
        levelManager.enemySpawner.gameObject.SetActive(false);
    }
}
