using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUI : MonoBehaviour
{
    /*  
        Name: LevelUI.cs
        Description: This script controls all global ui elements as well as updating them

    */
    public static LevelUI levelUIinstance;

    [Header("Controller References")]
    public PlayerSpawner playerSpawner;
    public PlayerController playerController;
    public EnemySpawner enemySpawner;
    private LevelManager levelManager;

    [Header("Script References")]
    public GameObject[] gameScreens;

    [Header("SetupUI References")]
    public Button[] selectUnitButtons;
    public TextMeshProUGUI levelNameTxt;
    public TextMeshProUGUI levelUnitCount;

    [Header("GameUI References")]
    public Button[] unitButtons;
    public Image playerHealthBar;
    public Image enemyHealthBar;
    public TextMeshProUGUI manaTxt;

    [Header("KNOB")]
    public Image knoEnraged;
    public GameObject tick;
    float timeE;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        //if another levelUIinstance exists 
        if(levelUIinstance != null)
        {
            return; //exit if statement
        }
        levelUIinstance = this;
        levelManager = this.gameObject.GetComponent<LevelManager>();
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Adds listeners for each buttons the player has */
        for(int i = 0; i < unitButtons.Length; i++)
        {
            AddListeners(unitButtons[i], i);
            AddListeners(selectUnitButtons[i], i);
        }
        
        HideUI(selectUnitButtons, levelManager.levelPlayerUnits.Count);
        UpdateSetUpUI();
        // knoEnraged.fillAmount = LevelManager.levelManagerInstance.HordeEnragedTime/(LevelManager.levelManagerInstance.HordeTime);
    }

    /*---      FUNCTIONS     ---*/
    /*-  Adds listeners to each button, takes a button and the index from the for loop -*/
    private void AddListeners(Button btn, int index)
    {
        btn.onClick.AddListener(() => { OnButtonClick(index); }); //Adds a listeners a button
    }
    public void HideUI(Button[] buttonsList, int listSize)
    {
        for(int i = buttonsList.Length - 1; i >= listSize; i--)
        {
            buttonsList[i].gameObject.SetActive(false);
        }
    }

    ///ADD TURN OFF CERTAIN BUTTONS



    /*-  Checks if a button is clicked, uses an index to indicate which button -*/
    private void OnButtonClick(int index)
    {
        if(levelManager.gameState == GameStates.SETUP)
        {
            levelManager.AddOrRemoveUnit(index);
        }
        if(levelManager.gameState == GameStates.PLAYING)
        {
            playerController.SpawnUnit(index); //Sends index to playerController
        }
    }
    private void UpdateSetUpUI()
    {
        levelNameTxt.text = "Level " + levelManager.levelNum + " - " + levelManager.levelName;
        levelUnitCount.text = "Pick " + levelManager.levelUnitLimit + " units";
    }

    /*-  Updates the Game UI -*/
    public void UpdateUI()
    {
        playerHealthBar.fillAmount = playerSpawner.health/playerSpawner.maxHealth;
        enemyHealthBar.fillAmount = enemySpawner.health/enemySpawner.maxHealth;
        manaTxt.text = "Mana: " + playerController.mana; 
    }
    /*-  Updates the Swaps the screen, takes an index for chosen screen -*/
    public void UpdateScreen(int index)
    {
        //Sets all game screens to false
        for(int i = 0; i < gameScreens.Length; i++)
        {
            gameScreens[i].SetActive(false);
        }
        gameScreens[index].SetActive(true);
    }

    // private void Update()
    // {
        // tick.transform.Rotate(Vector3.forward, (Time.deltaTime * (360f/(LevelManager.levelManagerInstance.HordeCalmTime + LevelManager.levelManagerInstance.HordeEnragedTime))));
        // timeE += Time.deltaTime;
        // Debug.Log(timeE);
    // }
    public void SetKnobs()
    {
        Debug.Log("CKICK");
    }
}
