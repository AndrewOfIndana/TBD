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
    public Animator bodyAnimator;
    public Animator featuresAnimator;
    public GameObject beastModel;

    [Header("Spawn Points")]
    public Transform bossSpawnPoint;
    public Transform[] tentacleSpawnPoints;

    /*[Header("Script Settings")]*/
    private FinalBossController finalBoss;

    /*-  Starts boss fight, Spawns the boss and changes the UI of the game -*/
    public override void StartBoss()
    {
        bodyAnimator.SetTrigger("Start");
        featuresAnimator.SetTrigger("Start");
        enemySpawnerUI.SetActive(false);
        levelManager.enemySpawner.gameObject.SetActive(false);
        bossUI.SetActive(true);
        ClearSpecialMoveText();
        gameManager.GetGameOptions().SetColorFilter(new Color(1, .4f, .4f));
        StartCoroutine(StartFinalBoss(10f));
    }
    private IEnumerator StartFinalBoss(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject bossObj = Instantiate(bossPrefab, bossSpawnPoint.position , Quaternion.identity);
        finalBoss = bossObj.GetComponent<FinalBossController>();
        finalBoss.SpawnTentacle(tentacleSpawnPoints);
    }
    /*-  Updates the health bar of a unit -*/
    public override void UpdateHealthUI()
    {
        //if boss's health is greater than the boss's stat.unitHealth and levelManager's isEnraged is true
        if(finalBoss.GetHealth() > bossStat.unitHealth && levelManager.GetIsEnraged())
        {
            bossHealthBar.color = enragedColor;
        }
        //if boss's health is greater than the boss's stat.unitHealth and levelManager's isEnraged is flase
        else if(finalBoss.GetHealth() > bossStat.unitHealth && !levelManager.GetIsEnraged())
        {
            bossHealthBar.color = buffedHpColor;
        }
        else
        {
            bossHealthBar.color = healthColor;
            bossHealthBar.fillAmount = finalBoss.GetHealth()/bossStat.unitHealth;
        }
    }
    /*-  Ends boss fight, changes the the gameState to win -*/
    public override void EndBoss()
    {
        finalBoss.gameObject.SetActive(false);   
        beastModel.SetActive(false);
        gameManager.GetGameOptions().SetColorFilter(new Color(1, 1, 1));
        StartCoroutine(FinishBoss(5f));
    }
}
