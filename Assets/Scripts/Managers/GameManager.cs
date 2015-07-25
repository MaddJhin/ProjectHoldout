﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/*
 * USAGE
 * ========================
 * Singleton design pattern
 * Manages the transitions between levels
 * Does this using the info from the current scene's LevelManager
 * ========================
 * 
 * Date Created: 18 June 2016
 * Last Modified: 18 June 2016
 * Authors: Andrew Tully
 */

public class GameManager : MonoBehaviour
{
    public GameObject[] playerLoadout = new GameObject[7];

    private int objectiveCounter;
    private List<Spawner> spawnList;
    private int inactiveSpawns;
    private GameObject UI_canvas;
    private InputManager IM;
    GameObject test;

    #region Singleton
    private static GameManager _instance;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();

                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {

            Debug.Log("Creating singleton");
            // If this instance is the first in the scene, it becomes the singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }

        else
        {
            // If another Singleton already exists, destroy this object
            if (this != _instance)
            {
                Debug.Log("Destroying invalid singleton");
                Destroy(this.gameObject);
            }
        }

        objectiveCounter = 0;
        inactiveSpawns = 0;
        spawnList = new List<Spawner>();

        UI_canvas = GameObject.Find("Canvas");

        // Get all spawners in the scene and remove the player spawners
        Debug.Log("Fetching Spawners");
        Spawner[] temp = GameObject.FindObjectsOfType<Spawner>();

        foreach (var spawn in temp)
        {
            if (spawn.tag == "Enemy Spawn")
                spawnList.Add(spawn);
        }

        Debug.Log("Spawn List: " + spawnList);

        for (int i = 0; i < 7; i++)
        {
            AssignLoadoutSlot("PlayerCharacter_Marksman", i);
        }

        //AssignLoadoutSlot("PlayerCharacter_Marksman", 0);
        //SpawnPlayerUnits();
        AssignLoadoutUI();
    }
    #endregion

    #region Functionality

    public void SwitchScene(string sceneName)
    {
        Debug.Log("Switching to " + sceneName);
        Application.LoadLevel(sceneName);
    }

    public void AddObjective()
    {
        objectiveCounter++;
        Debug.Log("Objective Count: " + objectiveCounter);
    }

    public void RemoveObjective()
    {
        objectiveCounter--;
        Debug.Log("Objective Count: " + objectiveCounter);

        if (inactiveSpawns == spawnList.Count && objectiveCounter <= 0)
        {
            Debug.Log("Level Complete");
        }
    }

    public void IsSpawnInactive()
    {
        Debug.Log("Checking for inactive Spawners");

        // Check each spawner in the scene to see if it's active and still spawned
        foreach (var spawn in spawnList)
        {
            // If the spawner is finished spawning, add it to the tally of completed spawners
            if (!spawn.isActiveAndEnabled)
            {
                inactiveSpawns++;
            }
        }
    }

    // Assigns a string to an element of the playerLoadout array
    // String is used for dictionary lookup within the ObjectPool when spawning units
    public void AssignLoadoutSlot(string unitName, int slot_no)
    {
        playerLoadout[slot_no] = GenericPooler.current.GetPooledObject(unitName);
        //test = GenericPooler.current.GetPooledObject(unitName);
    }

    // Spawn each player unit from the object pool
    public void SpawnPlayerUnits()
    {

        Debug.Log("Number of Units to Spawn: " + playerLoadout.Length);
        foreach (var unit in playerLoadout)
        {
            GameObject spawnLoc = GameObject.Find("Evac Shuttle");

            unit.transform.position = spawnLoc.transform.position;
            unit.SetActive(true);
            Debug.Log("Spawning: " + unit);
        }
    }

    public void AssignLoadoutUI()
    {
        Button[] b = UI_canvas.GetComponentsInChildren<Button>();
        Debug.Log("Game Manager button refs: ");
        int i = 0;

        foreach (GameObject unit in playerLoadout)
        {
            Debug.Log(b[i]);

            Debug.Log("Adding Listener for: " + unit);
            b[i].onClick.AddListener(() => IM.SetTarget(unit.GetComponent<PlayerCharacterControl>()));

            i++;
        }
    }

    #endregion
}
