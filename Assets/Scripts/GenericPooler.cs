using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericPooler : MonoBehaviour 
{
    public static GenericPooler current;
    public GameObject pooledObject;
    public int pooledAmount = 40;
    public Dictionary<string, List<GameObject>> poolDict;
    public List<GameObject> pooledObjects; // List of objects that will be pooled

    // List to hold pool of an object during instantiation
    List<GameObject> objPool; 

    void Awake()
    {
        current = this;
        poolDict = new Dictionary<string, List<GameObject>>();
        objPool = new List<GameObject>();
    }

    void Start()
    {
        foreach (var poolObj in pooledObjects)
        {
            // Clear the temporary list
            List<GameObject> tempPool = new List<GameObject>();

            for (int i = 0; i < pooledAmount; i++)
            {
                GameObject obj = (GameObject)Instantiate(poolObj);
                obj.SetActive(false);
                tempPool.Add(obj);
            }

            poolDict.Add(poolObj.name, tempPool);
            Debug.Log(poolObj.name);
        }       
    }

    public GameObject GetPooledObject(string objType)
    {
        Debug.Log("Pool arg: " + objType);
        List<GameObject> iterList;

        if (poolDict.ContainsKey(objType))
        {
            Debug.Log("Key " + objType + " was found");
            Debug.Log("Fetching value " + poolDict[objType]);

            iterList = poolDict[objType];

            for (int i = 0; i < iterList.Count; i++)
            {
            
                if (!iterList[i].activeInHierarchy)
                    return iterList[i];
            }
        } 

        return null;
    }
}
