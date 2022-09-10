using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    private Transform targetedEnemy;
    public float speed = 70f;
    private float thisAttack;

    public void Seek(Transform targ)
    {
        targetedEnemy = targ;
    }
    void Update()
    {
        Vector3 dir;
        float distanceThisFrame = speed * Time.deltaTime;

        if (targetedEnemy == null)
        {
            transform.position += transform.forward * distanceThisFrame;
            transform.Translate(Vector3.up * Time.deltaTime, Space.World);
            Invoke("DestroyBullet", 5f);
            return;
        }
        else
        {
            dir = targetedEnemy.position - transform.position;
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }

    }


    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
