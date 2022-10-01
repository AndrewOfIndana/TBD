using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum GameStates {SETUP, PLAYING, WIN, LOSE}

public class LevelManager : MonoBehaviour
{
    public Level level;
    public StatsList enemyCatalogue;
    public EnemySpawner enemySpawner;
    public StatsList playerCatalogue;
    public PlayerSpawner playerSpawner;
    public PlayerAvatar playerAvatar;
    public GameObject playerPrefab;
    private LevelUI levelUI;
    private PlayerController playerController;

    public CinemachineVirtualCamera topdownCamera; //Array of Cinemachine Virtual Cameras references
    public CinemachineVirtualCamera playerCamera; //Array of Cinemachine Virtual Cameras references
    private GameStates gameState = GameStates.SETUP;


    private bool hasPlayerRespawned = true;

    private void Awake()
    {
        levelUI = this.gameObject.GetComponent<LevelUI>();
        playerController = this.gameObject.GetComponent<PlayerController>();
    }
    private void Start()
    {
        gameState = GameStates.SETUP;
        levelUI.ChangeCanvas(0);
        SwitchCameras(1, 0);
    }
    private void SwitchCameras(int cam1, int cam2)
    {
        topdownCamera.Priority = cam1;
        playerCamera.Priority = cam2;
    }

    void Update()
    {
        if(Input.GetKeyDown("space") && gameState == GameStates.SETUP)
        {
            ChangeState(GameStates.PLAYING);
            return;
        }
        if(playerSpawner.isActiveAndEnabled == false && gameState == GameStates.PLAYING)
        {
            ChangeState(GameStates.LOSE);
            return;
        }
        if(enemySpawner.isActiveAndEnabled == false && gameState == GameStates.PLAYING)
        {
            ChangeState(GameStates.WIN);
            return;
        }
        if(playerAvatar != null)
        {
            if(playerAvatar.isActiveAndEnabled == false && gameState == GameStates.PLAYING && hasPlayerRespawned == true)
            {
                hasPlayerRespawned = false;
                StartCoroutine(RespawnPlayer(10f));
                return;
            }
        }
    }

    private void ChangeState(GameStates newState)
    {
        if(newState == GameStates.PLAYING)
        {
            gameState = GameStates.PLAYING;
            levelUI.ChangeCanvas(1);
            SwitchCameras(0, 1);
            playerController.StartGame();
            enemySpawner.StartGame();
            SpawnPlayer();
        }
        else if(newState == GameStates.WIN)
        {
            gameState = GameStates.WIN;
            levelUI.ChangeCanvas(2);
            SwitchCameras(1, 0);
        }
        else if(newState == GameStates.LOSE)
        {
            gameState = GameStates.LOSE;
            levelUI.ChangeCanvas(3);
            SwitchCameras(1, 0);
        }
    }

    private void SpawnPlayer()
    {
        GameObject playerAvatarObj = Instantiate(playerPrefab, playerSpawner.transform.position, playerSpawner.transform.rotation); //Spawns player prefab and stores it in a GameObject
        playerAvatar = playerAvatarObj.GetComponent<PlayerAvatar>(); 
        playerCamera.Follow = playerAvatar.transform; //sets player camera's follow to the player's transform
    }
    private IEnumerator RespawnPlayer(float waitTime)
    {
        SwitchCameras(1, 0);
        yield return new WaitForSeconds(waitTime); //Waits for rate
        playerAvatar.gameObject.SetActive(true);
        playerAvatar.transform.position = playerSpawner.transform.position;
        SwitchCameras(0, 1);
        hasPlayerRespawned = true;
    }
}
