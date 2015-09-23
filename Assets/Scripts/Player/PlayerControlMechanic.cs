using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* USES:
 * ==============
 * PlayerCharacterMovement
 * PlayerAttack.cs
 * ==============
 * 
 * USAGE:
 * ======================================
 * Acts as the control for the Mechanic player Character
 * Uses other scripts to attack and move 
 * Enables modularity
 * ======================================
 * 
 * Date Created: 27 Jul 2015
 * Last Modified: 	8 Aug 2015
 * Authors: Francisco Carrera
 */

[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (PlayerMovement))]
[RequireComponent(typeof (PlayerAction))]
[RequireComponent(typeof (UnitStats))]
[RequireComponent(typeof (NavMeshObstacle))]

public class PlayerControlMechanic : MonoBehaviour {

	public float health = 100f;
	public float armor = 0f;
	public float sightRange = 10;
	public float healPerHit = 20f;
	public float healRange = 100f;
	public float timeBetweenHeals = 0.15f;
	public float barricadeMaxThether = 10f;
	public List<string> priorityList = new List<string>();	// Stores action target priorty (highest first).
	
	UnitStats stats;								// Unit stat scripts for health assignment
	Transform actionTarget;							// Current Action target
	float timer;                                    // A timer between actions.
	NavMeshAgent agent;								// Nav Agent for moving character
	PlayerMovement playerControl;					// Sets attack target based on priority
	PlayerAction playerAction;						// Script containg player attacks
	bool targetInRange;								// Tracks when target enters and leaves range
	float originalStoppingDistance;					// Used to store preset agent stopping distance
	NavMeshObstacle obstacle;						// Used to indicate other units to avoid this one
    GameObject repairTarget;                        // Target to be repaired
    LayerMask repairLayer;                          // Mask to find barricades

	
	void Awake(){
		agent = GetComponent<NavMeshAgent>();
		playerControl = GetComponent<PlayerMovement>();
		playerAction = GetComponent<PlayerAction>();
		stats = GetComponent<UnitStats>();
		obstacle = GetComponent<NavMeshObstacle>();
        repairLayer = LayerMask.GetMask("Player");
	}
	
	void Start (){
		playerAction.range = healRange;
		playerControl.priorityList = priorityList;
		playerAction.timeBetweenAttacks = timeBetweenHeals;
		originalStoppingDistance = agent.stoppingDistance;
		stats.currentHealth = health;
		playerControl.maxBarricadeDistance = barricadeMaxThether;
		stats.armor = armor;
		playerControl.sightDistance = sightRange;
	}
	
	// Update is called once per frame
	void Update () {
		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;
        repairTarget = playerControl.SetHealTarget(transform.position, healRange, repairLayer);
		
		// If there is nothing to attack, script does nothing.
		if (actionTarget == null) 
		{
			agent.stoppingDistance = originalStoppingDistance;
			return;
		}
		
		if (Vector3.Distance (actionTarget.position, transform.position) <= healRange)
		{
			Stop();
		}
		else
		{
			Move();
		}
		
		// If the target is in range and enough time has passed between attacks, Attack.
		if (timer >= timeBetweenHeals && targetInRange && repairTarget.tag == "Barricade")
		{
			Heal();
		}
	}
	
	void Stop(){
		targetInRange = true;
		
//		if(agent.enabled)
//		{
//			agent.Stop();
//		}
//		agent.enabled = false;
//		obstacle.enabled = true;
	}
	
	void Heal(){
		timer = 0f;
		playerAction.Heal(healPerHit, repairTarget);
	}
	
	void Move (){
		targetInRange = false;
		
//		obstacle.enabled = false;
//		agent.enabled = true;
//		agent.Resume();
	}
}
