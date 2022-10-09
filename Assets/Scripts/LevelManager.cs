using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum GameStates {SETUP, PLAYING, WIN, LOSE} //The different game states the level could be in
public enum HordeState {CALM, ENRAGED} 

public class LevelManager : MonoBehaviour
{
    /*  
        Name: LevelManager.cs
        Description: This script handles all global variables and states for a scene or level

    */
    public static LevelManager levelManagerInstance;

    [Header("Setup References")]
    public Level level;
    /* Allows for levels to be made without the level scriptable object */
    public StatsList levelPlayerUnitsList;
    public StatsList levelEnemyUnitsList;
    public int levelUnitLimit;
    public float levelEnemyRate;
    [HideInInspector] public int levelNum;
    [HideInInspector] public string levelName;
    /* Setup Variables that actually matter */
    [HideInInspector] public List<Stats> levelPlayerUnits = new List<Stats>(); //List of units that the player has to choose
    [HideInInspector] public List<Stats> playerUnits = new List<Stats>(); //List of units that the player has chosen
    [HideInInspector] public int playerUnitCount = 0;
    [HideInInspector] public List<Stats> levelEnemyUnits = new List<Stats>();

    [Header("Controller References")]
    public PlayerController playerController;
    public PlayerSpawner playerSpawner;
    public EnemySpawner enemySpawner;
    private LevelUI levelUI;

    [Header("Player Avatar References")]
    public GameObject playerPrefab;
    [HideInInspector] public PlayerAvatar playerAvatar;

    [Header("Script References")]
    public CinemachineVirtualCamera topdownCamera; 
    public CinemachineVirtualCamera playerCamera;
    [HideInInspector] public GameStates gameState = GameStates.SETUP;
    public float respawnTime = 10f;
    private bool hasPlayerRespawned = true;

    // public float HordeCalmTime = 120f;
    // public float HordeEnragedTime = 60f;
    // public float HordeTime;

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
        // HordeTime = HordeCalmTime + HordeEnragedTime;
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Sets game to setup */
        gameState = GameStates.SETUP;
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
            if(playerAvatar.isActiveAndEnabled == false && gameState == GameStates.PLAYING && hasPlayerRespawned == true)
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
    public void ChangeState(GameStates newState)
    {
        //if the new state is PLAYING
        if(newState == GameStates.PLAYING)
        {
            /* Starts game */
            gameState = GameStates.PLAYING;
            levelUI.UpdateScreen(1);
            SwitchCameras(0, 1);
            playerController.StartGame();
            enemySpawner.StartGame();
            SpawnPlayer();
        }
        else if(newState == GameStates.WIN) //if the new state is WIN
        {
            /* Player WINS */
            gameState = GameStates.WIN;
            levelUI.UpdateScreen(2);
            SwitchCameras(1, 0);
        }
        else if(newState == GameStates.LOSE) //if the new state is LOSE
        {
            /* Player LOSES */
            gameState = GameStates.LOSE;
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
    /*-  Retrives values from level package and sets variables if they exist  -*/
    private void SetLevel()
    {
        //If level exists
        if(level != null)
        {
            levelPlayerUnits = level.availbleUnits.statsLists;
            levelUnitLimit = level.unitLimit;
            levelEnemyUnits = level.enemyUnits.statsLists;
            levelEnemyRate = level.enemySpawnRate;
            levelNum = level.levelID;
            levelName = level.levelName;
        }
        else if(level == null)
        {
            levelPlayerUnits = levelPlayerUnitsList.statsLists;
            levelEnemyUnits = levelEnemyUnitsList.statsLists;
        }
    }
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
        ChangeState(GameStates.PLAYING);
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
}
