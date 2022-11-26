using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinalBossManager : BossManager
{
    /*  
        Name: FinalBossManager.cs
        Description: This script handles spawning the final boss, manages the ui of boss health, and handles hijacking the level ends

    */
    [Header("Final Boss Settings")]
    public Transform bossSpawnPoint;
    public GameObject tentaclePrefab;
    public Transform[] tentacleSpawnPoints;
    public Animator bodyAnimator;
    public Animator featuresAnimator; 

    /*-  Starts boss fight, Spawns the boss and changes the UI of the game -*/
    public override void StartBoss()
    {
        bodyAnimator.SetTrigger("Start");
        featuresAnimator.SetTrigger("Start");
        enemySpawnerUI.SetActive(false);
        bossUI.SetActive(true);
        ClearSpecialMoveText();
        StartCoroutine(StartFinalBoss(20f));
    }
    private IEnumerator StartFinalBoss(float time)
    {
        yield return new WaitForSeconds(time);
        // GameObject bossObj = Instantiate(bossPrefab, bossSpawnPoint.position , Quaternion.identity);
        // boss = bossObj.GetComponent<BossController>();
        
        // for(int i = 0; i < tentacleSpawnPoints.Length; i++)
        // {
        //     GameObject tentacleObj = Instantiate(tentaclePrefab, tentacleSpawnPoints[i].position , Quaternion.identity);
        // }
    }
}
