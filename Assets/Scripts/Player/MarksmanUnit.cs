﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (PlayerCharacterControl))]
[RequireComponent(typeof (PlayerAttack))]
[RequireComponent(typeof (UnitStats))]

/* USES:
 * ==============

 * ==============
 * 
 * USAGE:
 * ======================================

 * ======================================
 * 
 * Date Created: 	
 * Last Modified: 	
 * Authors: 		
 */

public class MarksmanUnit : MonoBehaviour {

	public float health = 100f;
	public float damagePerHit = 20f;
	public float attackRange = 100f;
	public float timeBetweenAttacks = 0.15f;
	public List<string> priorityList = new List<string>();	// Stores action target priorty (highest first).

	UnitStats stats;								// Unit stat scripts for health assignment
	Transform actionTarget;							// Current Action target
	float timer;                                    // A timer between actions.
	NavMeshAgent agent;								// Nav Agent for moving character
	PlayerCharacterControl playerControl;			// Sets attack target based on priority
	PlayerAttack playerAttack;						// Script containg player attacks
	bool targetInRange;
	float effectsDisplayTime = 0.1f;                // The proportion of the timeBetweenBullets that the effects will display for.
	float originalStoppingDistance;					// Used to store preset agent stopping distance


	void Awake(){
		agent = GetComponent<NavMeshAgent>();
		playerControl = GetComponent<PlayerCharacterControl>();
		playerAttack = GetComponent<PlayerAttack>();
		stats = GetComponent<UnitStats>();
	}

	void Start (){
		playerAttack.range = attackRange;
		playerControl.priorityList = priorityList;
		playerAttack.timeBetweenAttacks = timeBetweenAttacks;
		originalStoppingDistance = agent.stoppingDistance;
		stats.currentHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;
		actionTarget = playerControl.actionTarget;

		// If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
		if(timer >= timeBetweenAttacks * effectsDisplayTime)
		{
			// ... disable the effects.
			playerAttack.DisableEffects ();
		}

		// If there is nothing to attack, script does nothing.
		if (actionTarget == null) 
		{
			agent.stoppingDistance = originalStoppingDistance;
			return;
		}
		
		// Set if the target is in range
		if (Vector3.Distance (actionTarget.position, transform.position) <= attackRange)
		{
			targetInRange = true;
			agent.stoppingDistance = attackRange;
		}
		else
		{
			targetInRange = false;
			agent.stoppingDistance = originalStoppingDistance;
		}

		Debug.Log("Try to shoot");
		// If the target is in range and enough time has passed between attacks, Attack.
		if (timer >= timeBetweenAttacks && targetInRange)
		{
			timer = 0f;
			playerAttack.Shoot(damagePerHit);
			Debug.Log("Shot");
		}
	}
}
