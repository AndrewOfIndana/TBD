using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    /*  
        Name: ObjectPool.cs
        Description: This script allows for object pooling, or reusing the same objects instead of creating and deleting new ones.

    */
    public static ObjectPool instance;

    /*  A class called pool stores what the objects name is what it is instantiating from the pool and how many of it is in the pool  */
    [System.Serializable] //Allows the class to be edited in editor
    public class Pool 
    {
        public string objName; 
        public GameObject objPrefab;
        public int objNum;
    }

    [Header("Script Settings")]
    public List<Pool> pools; //A listArray that stores how many object pools there should be
    public Dictionary<string, Queue<GameObject>> poolDictionary; //A dictionary that stores the name and a gameObject

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        /* SINGLETON PATTERN */
        //if the instance does exist and the instance isn't this
        if (instance != null && instance != this) 
        { 
            return;
        } 
        else 
        { 
            instance = this; 
        } 
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>(); 

        foreach(Pool pool in pools)
        {
            Queue<GameObject> pooledObj = new Queue<GameObject>(); //Instantiates a new Queue of objects named pooledObj

            //for loop that makes and enqueue each new gameObject in the pool
            for(int i = 0; i < pool.objNum; i++)
            {
                GameObject obj = Instantiate(pool.objPrefab); 
                obj.SetActive(false); 
                pooledObj.Enqueue(obj); //Adds object the pooledObj queue
            }

            poolDictionary.Add(pool.objName, pooledObj); //Adds objectQueue to the poolDictionary
        }
    }

    /*---       FUNCTIONS     ---*/
    /*-  Spawns a gameObject from a pool, needs the pooled object's name, the spawn position, and spawn rotation-*/
    public GameObject SpawnFromPool(string name, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        //If there are no pools that contain the name
        if(!poolDictionary.ContainsKey(name))
        {
            Debug.Log("Pool with the name: " + name + " doesn't exist");
            return null;
        }

        GameObject spawnedObj = poolDictionary[name].Dequeue(); //Dequeue the object from the pool
        spawnedObj.SetActive(true);
        spawnedObj.transform.position = spawnPosition; 
        spawnedObj.transform.rotation = spawnRotation;

        poolDictionary[name].Enqueue(spawnedObj); //Readds spawnedObj to the poolDictionary
        return spawnedObj; //Returns spawnedObj as a GameObject
    }
}
