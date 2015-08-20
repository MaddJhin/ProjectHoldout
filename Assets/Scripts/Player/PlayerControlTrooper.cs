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
 * Acts as the control for the trooper player Character
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

public class PlayerControlTrooper : MonoBehaviour {

	public float health = 100f;
	public float armor = 0f;
	public float sightRange = 10;
	public float damagePerHit = 20f;
	public float attackRange = 2f;
	public float timeBetweenAttacks = 0.15f;
	public float barricadeMaxThether = 10f;
	public List<string> priorityList = new List<string>();	// Stores action target priorty (highest first).

	Transform actionTarget;							// Target to shoot
	UnitStats stats;								// Unit stat scripts for health assignment
	float timer;                                    // A timer between actions.
	NavMeshAgent agent;								// Nav Agent for moving character
	PlayerMovement playerControl;					// Sets attack target based on priority
	PlayerAction playerAction;						// Script containg player attacks
	bool targetInRange;								// Tracks when target enters and leaves range
	float originalStoppingDistance;					// Used to store preset agent stopping distance
	NavMeshObstacle obstacle;						// Used to indicate other units to avoid this one
	
	void Awake(){
		agent = GetComponent<NavMeshAgent>();
		playerControl = GetComponent<PlayerMovement>();
		playerAction = GetComponent<PlayerAction>();
		stats = GetComponent<UnitStats>();
		obstacle = GetComponent<NavMeshObstacle>();
	}
	
	void Start (){
		playerAction.range = attackRange;
		playerControl.priorityList = priorityList;
		playerAction.timeBetweenAttacks = timeBetweenAttacks;
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
		actionTarget = playerControl.actionTarget;

		// If there is nothing to attack, script does nothing.
        if (actionTarget == null)
        {
            agent.stoppingDistance = originalStoppingDistance;
            return;
        }

        // Set if the target is in range
		if (Vector3.Distance (actionTarget.position, transform.position) <= attackRange)
		{
			Stop();
		}
		else
		{
			Move();
		}
		
		// If the target is in range and enough time has passed between attacks, Attack.
		if (timer >= timeBetweenAttacks && targetInRange)
		{
			Attack();
		}
	}

	void Stop(){
		targetInRange = true;

		if(agent.enabled)
		{
			agent.Stop();
		}
		agent.enabled = false;
		obstacle.enabled = true;
	}

	void Attack(){
		timer = 0f;
		playerAction.Attack(damagePerHit);
	}

	void Move (){
		targetInRange = false;

		obstacle.enabled = false;
		agent.enabled = true;
		agent.Resume();
	}
}
