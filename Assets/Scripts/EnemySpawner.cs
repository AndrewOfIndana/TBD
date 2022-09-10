using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    //public float waveIndex;
    public float waveRate;
    
    void Update()
    {
        if(Random.Range(1, 100) < waveRate)
        {
            Vector3 spawnPosition = this.transform.position;

            Instantiate(enemyPrefabs[0], spawnPosition, Quaternion.identity);
        }
    }
}
