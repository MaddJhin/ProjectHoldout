using UnityEngine;
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

public class TrooperUnit : MonoBehaviour {

	public float health = 100f;
	public float damagePerHit = 20f;
	public float attackRange = 2f;
	public float timeBetweenAttacks = 0.15f;
	public List<string> priorityList = new List<string>();	// Stores action target priorty (highest first).
	public Transform attackTarget;							// Target to shoot

	UnitStats stats;								// Unit stat scripts for health assignment
	Transform actionTarget;							// Current Action target
	float timer;                                    // A timer between actions.
	NavMeshAgent agent;								// Nav Agent for moving character
	PlayerCharacterControl playerControl;			// Sets attack target based on priority
	PlayerAttack playerAttack;						// Script containg player attacks
	bool targetInRange;
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
		attackTarget = playerControl.actionTarget;

		// If there is nothing to attack, script does nothing.

        Debug.Log("Checking for Null");
        if (attackTarget == null)
        {
            Debug.Log("Null found");
            agent.stoppingDistance = originalStoppingDistance;
            return;
        }

        // If the attack target isn't active, it's considered null
        else if (attackTarget.gameObject.activeInHierarchy == false)
        {
            Debug.Log("Inactive Found");
            attackTarget = null;
            Debug.Log(attackTarget);
        }

        // Otherwise, if there is a valid target...
        else
        {
            // Set if the target is in range
            if (Vector3.Distance(attackTarget.position, transform.position) <= attackRange)
            {
                targetInRange = true;
                agent.stoppingDistance = attackRange;
            }

            else
            {
                targetInRange = false;
                agent.stoppingDistance = originalStoppingDistance;
            }
        }
		
		// If the target is in range and enough time has passed between attacks, Attack.
		if (timer >= timeBetweenAttacks && targetInRange && attackTarget != null)
		{
			timer = 0f;
			playerAttack.Attack(damagePerHit);
		}
	}
}
