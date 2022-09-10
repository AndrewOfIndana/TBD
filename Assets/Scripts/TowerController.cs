using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private Transform targetDetected;
    public Transform firingPoint;
    public GameObject bulletPrefab;
    private float fireCountDown = 0f;
    private float fireRate = 1.1f;
    public float range;

    void Awake()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void Update() 
    {
        if(targetDetected == null)
        {
            return;
        }

        if (fireCountDown <= 0f)
        {
            Shoot();
            fireCountDown = 1f/fireRate;
        }
        fireCountDown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject bulletGameObject = (GameObject)Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        TowerBullet bullet = bulletGameObject.GetComponent<TowerBullet>();

        if(bullet != null)
        {
            bullet.Seek(targetDetected);
        }
    }

    void UpdateTarget() //selects targets
    {
        GameObject[] enemiesDetected = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemyDetected in enemiesDetected)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemyDetected.transform.position);

            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemyDetected;
            }
        }

        if(nearestEnemy != null && shortestDistance <= range)
        {
           targetDetected = nearestEnemy.transform;
        }
        else
        {
            targetDetected = null;
        }
    }
}
