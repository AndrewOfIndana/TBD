using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum EnragedStates {CALM, ENRAGED} 

public class LevelManager : MonoBehaviour
{
    /*  
        Name: LevelManager.cs
        Description: This script handles all global variables and states for a scene or level

    */
    GameManager gameManager;

    public static LevelManager levelManagerInstance;

    [Header("Setup References")]
    public Level level;
    private int levelUnitLimit;
    /* Setup Variables that actually matter */
    private List<Stats> levelPlayerUnits = new List<Stats>(); //List of units that the player has to choose
    private List<Stats> playerUnits = new List<Stats>(); //List of units that the player has chosen
    private int playerUnitCount = 0;
    private List<Stats> levelEnemyUnits = new List<Stats>();

    [Header("Controller References")]
    public PlayerController playerController;
    public PlayerSpawner playerSpawner;
    public EnemySpawner enemySpawner;
    private LevelUI levelUI;

    [Header("Player Avatar References")]
    public GameObject playerPrefab;
    private PlayerAvatar playerAvatar;

    [Header("Script References")]
    public CinemachineVirtualCamera topdownCamera; 
    public CinemachineVirtualCamera playerCamera;
    private float respawnTime = 10f;
    private bool hasPlayerRespawned = true;

    public EnragedStates enragedState = EnragedStates.CALM; 
    public float clockCalmTime = 120f;
    public float clockEnragedTime = 60f;
    public float clockTime = 0;
    public int enragedCount = 0;

    #region
    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        //if another levelManagerInstance exists 
        if(levelManagerInstance != null)
        {
            return; //exit if statement
        }
        levelManagerInstance = this;
        levelUI = this.gameObject.GetComponent<LevelUI>();
        SetLevel(); //Retrieves values from level package if it exist
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        gameManager = GameManager.gameInstance;

        /* Sets game to setup */
        gameManager.SetGameState(GameStates.SETUP);
        levelUI.UpdateScreen(0);
        SwitchCameras(1, 0);
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Update is called once per frame -*/
    private void Update()
    {
        /* Checks if the player is dead */
        //if the player avatar exists
        if(playerAvatar != null)
        {
            //if the player avatar isn;t active, game state is at playing and the hasPlayerRespawned is true
            if(playerAvatar.isActiveAndEnabled == false && gameManager.GetGameState() == GameStates.PLAYING && hasPlayerRespawned == true)
            {
                hasPlayerRespawned = false;
                StartCoroutine(RespawnPlayer(respawnTime));
                return;
            }
        }
    }

    /*---      FUNCTIONS     ---*/
    /*--    GAME STATE MANAGEMENT   --*/
    /*-  Changes the game state based on what has happened in game, takes GameState -*/
    public void ChangeState()
    {
        //if the new state is PLAYING
        if(gameManager.GetGameState() == GameStates.PLAYING)
        {
            /* Starts game */
            levelUI.UpdateScreen(1);
            SwitchCameras(0, 1);
            playerController.StartGame();
            enemySpawner.StartGame();
            SpawnPlayer();
            StartCoroutine(EnragedCycle(1f)); //Recalls RegenerateMana IEnumerator at 1 second
        }
        else if(gameManager.GetGameState() == GameStates.WIN) //if the new state is WIN
        {
            /* Player WINS */
            levelUI.UpdateScreen(2);
            SwitchCameras(1, 0);
        }
        else if(gameManager.GetGameState() == GameStates.LOSE) //if the new state is LOSE
        {
            /* Player LOSES */
            levelUI.UpdateScreen(3);
            SwitchCameras(1, 0);
        }
    }
    /*-  Switch the each camera's priority, takes two ints for which cameras should be on or off  -*/
    private void SwitchCameras(int cam1, int cam2)
    {
        topdownCamera.Priority = cam1;
        playerCamera.Priority = cam2;
    }
    /*--    SETUP MANAGEMENT   --*/
    /*-  Adds or removes unit in the playerUnits list and checks if the player is ready or not  -*/
    public void AddOrRemoveUnit(int index)
    {
        //If playerUnits contains this unit
        if(playerUnits.Contains(levelPlayerUnits[index]))
        {
            playerUnitCount--;
            playerUnits.Remove(levelPlayerUnits[index]);
            levelUI.UpdateSetUpUI(); //Updates the UI list of units
        }
        else if(!playerUnits.Contains(levelPlayerUnits[index]) && playerUnitCount < levelUnitLimit) //If playerUnits doesn't contains this unit and playerUnitCount is less than the levelUnitLimit
        {
            playerUnitCount++;
            playerUnits.Add(levelPlayerUnits[index]);
            levelUI.UpdateSetUpUI();  //Updates the UI list of units
        }
        
        //if playerUnitCount is greater than or equal to levelUnitLimit
        if(playerUnitCount >= levelUnitLimit)
        {
            levelUI.startGameButton.SetActive(true); //Sets startGameButton active
        }
        else if(playerUnitCount < levelUnitLimit) //if playerUnitCount is less than levelUnitLimit
        {
            levelUI.startGameButton.SetActive(false); //Sets startGameButton inactive
        }
    }
    /*-  Starts Game  -*/
    public void StartGame()
    {
        gameManager.SetGameState(GameStates.PLAYING);
        ChangeState();
    }
    /*---/--      PLAYER AVATAR MANAGEMENT     --/---*/
    /*-  Spawns the player avatar  -*/
    private void SpawnPlayer()
    {
        GameObject playerAvatarObj = Instantiate(playerPrefab, playerSpawner.transform.position, playerSpawner.transform.rotation); 
        playerAvatar = playerAvatarObj.GetComponent<PlayerAvatar>(); 
        playerCamera.Follow = playerAvatar.transform; //sets player camera's follow to the player's transform
    }
    /*-  respawns the player avatar, takes a float for the respawn time  -*/
    private IEnumerator RespawnPlayer(float waitTime)
    {
        SwitchCameras(1, 0);
        levelUI.UpdatePlayerDeath(false);
        yield return new WaitForSeconds(waitTime); //Waits for rate
        playerAvatar.gameObject.SetActive(true);
        playerAvatar.transform.position = playerSpawner.transform.position;
        SwitchCameras(0, 1);
        levelUI.UpdateUI();
        levelUI.UpdatePlayerDeath(true);
        hasPlayerRespawned = true;
    }

    /*--    ENRAGED SYSTEM MANAGEMENT   --*/
    /*-  Repeatedly regenerates mana, takes a float for the time -*/
    private IEnumerator EnragedCycle(float time)
    {
        yield return new WaitForSeconds(time);

        clockTime++;
        levelUI.UpdateClock();


        if(clockTime <= clockCalmTime)
        {
            enragedState = EnragedStates.CALM; 
        }
        else if(clockTime > clockCalmTime && clockTime <= level.GetTotalTime())
        {
            enragedState = EnragedStates.ENRAGED; 
        }
        else if(clockTime > level.GetTotalTime())
        {
            enragedState = EnragedStates.CALM; 
            clockTime = 0;
            enragedCount++;
        }

        if((gameManager.GetGameState() != GameStates.WIN) || (gameManager.GetGameState() != GameStates.LOSE))
        {
            StartCoroutine(EnragedCycle(1f)); //Recalls RegenerateMana IEnumerator at 1 second
        }
    }

    #endregion

    /*---      SET/GET FUNCTIONS     ---*/
    /*-  Retrives values from level package and sets variables if they exist  -*/
    private void SetLevel()
    {
        levelPlayerUnits = level.availbleUnits.statsLists;
        levelUnitLimit = level.unitLimit;
        levelEnemyUnits = level.enemyUnits.statsLists;
        clockCalmTime = level.enemyCalmTime;
        clockEnragedTime = level.enemyEnragedTime;
    }
    public Level GetLevel()
    {
        return level;
    }
    public List<Stats> GetLevelPlayerUnits()
    {
        return levelPlayerUnits;
    }
    public List<Stats> GetEnemyUnits()
    {
        return levelEnemyUnits;
    }
    public List<Stats> GetPlayerUnits()
    {
        return playerUnits;
    }
    public int GetPlayerUnitsCount()
    {
        return playerUnitCount;
    }
    public PlayerAvatar GetPlayerAvatar()
    {
        return playerAvatar;
    }
}