using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (NavMeshObstacle))]
[RequireComponent(typeof (UnitSight))]
[RequireComponent(typeof (EnemyAttack))]
[RequireComponent(typeof (UnitStats))]

public class EnemyControlBrute : MonoBehaviour 
{
	public float maxHealth = 100.0f;
	public float attackSpeed = 1.0f;
	public float attackRange = 1f;
	public float armor = 0.0f;
	public float stunDuration = 1f;
	public float attackRadius = 5f;
	public string defaultTarget;
	public List<string> priorityList = new List<string>();

    private NavMeshAgent agent;
    private UnitStats stats;
    private EnemyAttack action;
    private UnitSight vision;
    private float elapsedTime;
    private Vector3 targetLoc;
    private NavMeshObstacle obstacle;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<UnitStats>();
        action = GetComponent<EnemyAttack>();
        vision = GetComponent<UnitSight>();
        obstacle = GetComponent<NavMeshObstacle>();
    }

	void Start (){
		elapsedTime = 0f;

		// Set values for dependant scripts. Only modify values in one script in inspector
		vision.defaultTarget = defaultTarget;
		vision.priorityList = priorityList;
		stats.maxHealth = maxHealth;
		stats.attackSpeed = attackSpeed;
		stats.attackRange = attackRange;
		stats.armor = armor;
		action.stunDuration = stunDuration;
		action.attackRadius = attackRadius;
	}

    void Update()
    {
        // Update target location
        targetLoc = vision.actionTarget.transform.position;

        // If unit as at the target, stop moving and block other units
        if (vision.targetDistance <= attackRange)
        {
			if(agent.enabled == true)
			{
				agent.Stop();
			}
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
        //agent.Stop();
        Debug.Log(vision.actionTarget);
        action.Slam(vision.actionTarget);
    }

    void Move()
    {
        agent.Resume();
        agent.SetDestination(targetLoc);
    }
}
