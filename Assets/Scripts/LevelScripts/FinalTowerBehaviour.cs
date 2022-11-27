using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTowerBehaviour : TowerBehaviour
{
    /*  
        Name: FinalTowerBehaviour.cs
        Description: This script controls the behaviour of a tentacle and how it reacts to other units

    */
    private LevelManager levelManager;

    public float spawnRate;
    private bool areEnemiesPathFindingNormal = false;
    private List<Stats> typesOfEnemies = new List<Stats>();
    private FinalBossController finalBossController;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
        levelManager = LevelManager.instance;
        objectPool = ObjectPool.instance; 

        /* Gets and sets variables form the level manager */
        typesOfEnemies = levelManager.GetEnemyUnits();
    }
    public void SetController(FinalBossController controller)
    {
        finalBossController = controller;
    }
    public void SetSpawnType(bool pathFind)
    {
        areEnemiesPathFindingNormal = pathFind;
    }
    /*-  Starts the units targeting behaviour -*/
    public override void StartBehaviour()
    {
        StartCoroutine(UpdateTarget(towerController.GetAttackRate()));
        StartCoroutine(SpawnEnemy(spawnRate));
    }

    /*---      FUNCTIONS     ---*/
    /*-  Repeatedly updates a target and shoots, takes a float for the time -*/
    protected override IEnumerator UpdateTarget(float time)
    {
        yield return new WaitForSeconds(time); 

        //if gameStates is PLAYING
        if(gameManager.CheckIfPlaying())
        {
            Targeting();

            //if targetDetected does exist
            if(targetDetected != null)
            {
                Shoot();
            }
        }

        //if gameStates isn't WIN or LOSE
        if(!gameManager.CheckIfWinOrLose())
        {
            StartCoroutine(UpdateTarget(towerController.GetAttackRate()));
        }
        //if gameStates is WIN or LOSE
        else if(gameManager.CheckIfWinOrLose())
        {
            this.gameObject.SetActive(false);
        }
    }
    /*-  Repeatedly spawns Enemy takes a float for the time -*/
    private IEnumerator SpawnEnemy(float rate)
    {
        yield return new WaitForSeconds(rate); 

        //if gameStates is PLAYING
        if(gameManager.CheckIfPlaying())
        {
            if(areEnemiesPathFindingNormal)
            {
                GameObject enemyObj = objectPool.SpawnFromPool("Enemy", transform.position, Quaternion.identity);
                TroopController enemy = enemyObj.GetComponent<TroopController>();

                //if this enemy exist
                if(enemy != null)
                {
                    enemy.SetUnit(typesOfEnemies[Random.Range(0, typesOfEnemies.Count)]); //Sets enemy type and stats based on random number generator
                    enemy.StartController(); //Starts the enemy controller
                }
            }
            else if(!areEnemiesPathFindingNormal)
            {
                GameObject enemyObj = objectPool.SpawnFromPool("FinalEnemy", transform.position, Quaternion.identity);
                TroopController enemy = enemyObj.GetComponent<TroopController>();

                //if this enemy exist
                if(enemy != null)
                {
                    enemy.SetUnit(typesOfEnemies[Random.Range(0, typesOfEnemies.Count)]); //Sets enemy type and stats based on random number generator
                    enemy.StartController(); //Starts the enemy controller
                }

            }
        }

        //if gameStates isn't WIN or LOSE
        if(!gameManager.CheckIfWinOrLose())
        {
            StartCoroutine(SpawnEnemy(rate)); 
        }
    }

    private void OnDisable()
    {
        finalBossController.TakeDamage(1f);
    }
}
