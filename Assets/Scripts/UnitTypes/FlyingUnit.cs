using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* USES:
 * ==============
 * 
 * 
 * 
 * ==============
 * 
 * USAGE:
 * ======================================
 * 
 * 
 * 
 * 
 * ======================================
 * 
 * Date Created:
 * Last Modified:
 * Authors:
 */

[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (UnitSight))]
[RequireComponent(typeof (UnitStats))]

public class FlyingUnit : MonoBehaviour 
{
	public float height = 30f;
	public float maxHealth = 100.0f;
	public float attackSpeed = 1.0f;
	public float attackRange = 1f;
	public float armor = 0.0f;
	public string defaultTarget;
	public List<string> priorityList = new List<string>();

    private NavMeshAgent agent;
    private UnitStats stats;
    private UnitSight vision;
    private Vector3 targetLoc;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<UnitStats>();
        vision = GetComponent<UnitSight>();
    }

	void Start (){		
		// Set values for dependant scripts. Only modify values in one script in inspector
		vision.defaultTarget = defaultTarget;
		vision.priorityList = priorityList;
		stats.maxHealth = maxHealth;
		stats.attackSpeed = attackSpeed;
		stats.attackRange = attackRange;
		stats.armor = armor;
	}

    void Update()
    {
        // Ensures that the flyer remains at a specified height
        transform.position = new Vector3(transform.position.x, height, transform.position.z);

        // Update the target location
        targetLoc = vision.actionTarget.transform.position;
        Move();
    }

    void Move()
    {
        agent.Resume();
        agent.SetDestination(targetLoc);
    }
}
