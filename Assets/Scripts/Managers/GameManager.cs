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
    public GameObject spawnPoint;
    public float spawnOffset = 1f;                                     // Distance between player units being spawned

    private int objectiveCounter;
    private List<Spawner> spawnList;
    private int inactiveSpawns;
    private GameObject UI_canvas;
    private InputManager IM;
    GameObject test;


    // Following region ensures that there is only ever one game manager
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

        Debug.Log("Fetching Spawners");
        Spawner[] temp = GameObject.FindObjectsOfType<Spawner>();   // Get all spawners in scene

        // Ignore Player tagged spawners
        foreach (var spawn in temp)
        {
            if (spawn.tag == "Enemy Spawn")
                spawnList.Add(spawn);
        }

        Debug.Log("Spawn List: " + spawnList);

        // Temporary
        // Used for testing purposes
        

        Debug.Log("Preparing to Spawn Units");
        
        Debug.Log("Spawn attempted complete");                             
        
    }
    #endregion

    void Start()
    {
        IM = GameObject.FindObjectOfType<InputManager>();

        AssignLoadoutSlot("PlayerCharacter_Marksman", 0);
        AssignLoadoutSlot("PlayerCharacter_Trooper", 1);
        AssignLoadoutSlot("PlayerCharacter_Medic", 2);
        AssignLoadoutSlot("PlayerCharacter_Mechanic", 3);
        AssignLoadoutSlot("PlayerCharacter_Trooper", 4);
        AssignLoadoutSlot("PlayerCharacter_Medic", 5);
        AssignLoadoutSlot("PlayerCharacter_Trooper", 6);

        AssignLoadoutUI();
        SpawnPlayerUnits();
    }

    // Following region handles the tracking of objectives and transition conditions
    #region Objectives & Transitions

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
    #endregion 

    // Following region allows for units to be assigned to a loadout, and spawned
    #region Player Unit Management

    /* Function: Assigns a unit to the appropriate loadout slot
     * Parameters: The name of the unit to be spawned, The slow position
     * Returns: void
     */
    public void AssignLoadoutSlot(string unitName, int slot_no)
    {
        playerLoadout[slot_no] = GenericPooler.current.GetPooledObject(unitName);   // Get the unit from the pool
        playerLoadout[slot_no].SetActive(true);                                     // Activate it
        Debug.Log("Slot " + slot_no + ": " + playerLoadout[slot_no]);
    }

    /* Function: Spawns all units in the loadout into the scene
     * Parameters: None
     * Returns: None
     */
    public void SpawnPlayerUnits()
    {
        Debug.Log("Number of Units to Spawn: " + playerLoadout.Length);
        

        // Sets the player units' location to a specific point on the map
        foreach (var unit in playerLoadout)
        {
            spawnPoint.transform.position = new Vector3(spawnPoint.transform.position.x + spawnOffset, 
                                                        spawnPoint.transform.position.y, 
                                                        spawnPoint.transform.position.z);

            unit.transform.position = spawnPoint.transform.position;
            Debug.Log("Spawning: " + unit);
        }
    }

    /* Function: Gives each element of the control UI a reference to it's relevant unit
     * Parameters: None
     * Returns: None
     */
    public void AssignLoadoutUI()
    {
        Button[] b = UI_canvas.GetComponentsInChildren<Button>();                                       // Gets each button in the canvas
        Debug.Log("Game Manager button refs: ");
        int i;

        for (i = 0; i < playerLoadout.Length; i++)
        {
            Debug.Log(b[i]);

            Debug.Log("Adding Listener for: " + playerLoadout[i] + " at position " + i);

			PlayerMovement param = playerLoadout[i].GetComponent<PlayerMovement>();     // Cache the character controller to be added

            b[i].onClick.RemoveAllListeners();                                                          // Remove all previous listeners
            b[i].onClick.AddListener(delegate { IM.SetTarget(param); });                                // Add a new listener with the cached controller
        }
    }

    #endregion
}
