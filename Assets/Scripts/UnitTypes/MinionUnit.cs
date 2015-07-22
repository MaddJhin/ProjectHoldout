using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* USES:
 * ==============
 * UnitSight.cs
 * UnitStats.cs
 * MinionPunch.cs
 * ==============
 * 
 * USAGE:
 * ======================================
 * Acts as the "brain" of the minion unit
 * Uses other scripts to perform actions, E.G: seeing the player, attacking
 * Makes calls to other scripts to perform attacks, or to utilise stats
 * Enables modularity
 * ======================================
 * 
 * Date Created: 27 May 2015
 * Last Modified: 22 July 2015
 * Authors: Andrew Tully
 */

[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (NavMeshObstacle))]
[RequireComponent(typeof (UnitSight))]
[RequireComponent(typeof (BasicAttack))]
[RequireComponent(typeof (UnitStats))]

public class MinionUnit : MonoBehaviour 
{
	public float maxHealth = 100.0f;
	public float attackSpeed = 1.0f;
	public float attackRange = 1f;
	public float armor = 0.0f;
	public string defaultTarget;
	public List<string> priorityList = new List<string>();
	
	private NavMeshAgent agent;
    private UnitStats stats;
    private BasicAttack action;
    private UnitSight vision;
    private float elapsedTime;
    private Vector3 targetLoc;
    private NavMeshObstacle obstacle;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<UnitStats>();
        action = GetComponent<BasicAttack>();
        vision = GetComponent<UnitSight>();
        obstacle = GetComponent<NavMeshObstacle>();
    }

	void Start(){
		obstacle.enabled = false;
		agent.enabled = true;
		elapsedTime = 0f;

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
        // Update the target location
		targetLoc = vision.actionTarget.transform.position;
		
		// If unit as at the target, stop moving and block other units
        if (vision.targetDistance <= agent.stoppingDistance)
        {
            agent.Stop();
            agent.enabled = false;
            obstacle.enabled = true;

            if (stats.attackSpeed < elapsedTime)
            {
                elapsedTime = 0f;
                Attack();
            }
        }

        else
        {
            obstacle.enabled = false;
            agent.enabled = true;
            Move();
        }

        elapsedTime += Time.deltaTime;
    }

    void Attack()
    {
        agent.Stop();
        Debug.Log(vision.actionTarget);
        action.Punch(vision.actionTarget);
    }

    void Move()
    {
        agent.Resume();
        agent.SetDestination(targetLoc);
    }
}
