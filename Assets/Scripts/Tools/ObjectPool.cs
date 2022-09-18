using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    /*  
        Name: ObjectPool.cs
        Description: This script allows for object pooling, or reusing the same objects instead of creating and deleting new ones.

    */

    /*  A class called pool stores what the objects name is what it is instantiating from the pool and how many of it is in the pool */
    [System.Serializable] //Allows a class to be edited in editor
    public class Pool 
    {
        public string objName; 
        public GameObject objPrefab;
        public int objNum;
    }

    public static ObjectPool objectPoolInstance; //A static object that can be accessed in any script 
    public List<Pool> pools; //A listArray that stores how many object pools there should be
    public Dictionary<string, Queue<GameObject>> poolDictionary; //A dictionary that stores the name and a gameObject

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts when script is awake -*/
    public void Awake()
    {
        objectPoolInstance = this; //Set objectPoolInstance to this gameObject 
    }
    /*-  Starts on the first frame -*/
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>(); //Instantiates a new poolDictionary

        //for every pool in the list of pools
        foreach(Pool pool in pools)
        {
            Queue<GameObject> pooledObj = new Queue<GameObject>(); //Instantiates a new Queue of objects named pooledObj

            //for loop that makes and enqueue each new gameObject in the pool
            for(int i = 0; i < pool.objNum; i++)
            {
                GameObject obj = Instantiate(pool.objPrefab); //Instantiates the gameObject from the pools prefab
                obj.SetActive(false); //Disables object by default
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
            return null; //return nothing
        }

        GameObject spawnedObj = poolDictionary[name].Dequeue(); //Dequeue the object from the pool
        spawnedObj.SetActive(true); //enables the object
        spawnedObj.transform.position = spawnPosition; //sets the object's position to spawnPosition
        spawnedObj.transform.rotation = spawnRotation; //sets the object's rotation to spawnRotation

        IObjectPoolFunctions spawnedObjStart = spawnedObj.GetComponent<IObjectPoolFunctions>(); //get the IObjectPoolFunctions component from the spawnedObj as a reference;

        //if spawnedObjStart does exist
        if(spawnedObjStart != null)
        {
            spawnedObjStart.OnObjectSpawn(); //Calls ObObjectSpawn function
        }

        poolDictionary[name].Enqueue(spawnedObj); //Readds spawnedObj to the poolDictionary
        return spawnedObj; //Returns spawnedObj as a GameObject
    }
}
