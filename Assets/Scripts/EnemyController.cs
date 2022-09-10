using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //UnitStats

    public float bugSpeed;
    private Transform target;
    private int wavePointIndex = 0;

    void Awake()
    {
        target = WayPoints.points[0];
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * bugSpeed * Time.deltaTime, Space.World);
        transform.LookAt(WayPoints.points[wavePointIndex]);

        if (Vector3.Distance(transform.position, target.position) <= (bugSpeed * .01))
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if(wavePointIndex >= WayPoints.points.Length - 1)
        {
            Destroy(this.gameObject);
            return;
        }
        wavePointIndex++;
        target = WayPoints.points[wavePointIndex];
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

}
