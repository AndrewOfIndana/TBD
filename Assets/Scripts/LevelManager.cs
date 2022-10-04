using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum GameStates {SETUP, PLAYING, WIN, LOSE} //The different game states the level could be in

public class LevelManager : MonoBehaviour
{
    /*  
        Name: LevelManager.cs
        Description: This script handles all global variables and states for a scene or level

    */
    public static LevelManager levelManagerInstance;

    [Header("Setup References")]
    public Level level;
    public StatsList playerCatalogue;
    public StatsList enemyCatalogue;

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
    private GameStates gameState = GameStates.SETUP;
    public float respawnTime = 10f;
    private bool hasPlayerRespawned = true;

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
        /* Starts game */

        //CHANGE THIS LATER, if the player hits space and the gameState is at SETUP
        if(Input.GetKeyDown("space") && gameState == GameStates.SETUP)
        {
            ChangeState(GameStates.PLAYING);
            return; //exits if
        }

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
        yield return new WaitForSeconds(waitTime); //Waits for rate
        playerAvatar.gameObject.SetActive(true);
        playerAvatar.transform.position = playerSpawner.transform.position;
        SwitchCameras(0, 1);
        levelUI.UpdateUI();
        hasPlayerRespawned = true;
    }
}
