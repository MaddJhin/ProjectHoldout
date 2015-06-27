using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (NavMeshAgent))]

public class PlayerCharacterControl : MonoBehaviour {

	public Transform actionTarget; // target to aim for
	public NavMeshAgent agent;
	public bool targetInRange;
	public List<string> priorityList = new List<string>();	// Stores priority of GameObject tags. First element is the highest priority
	public float targetDistance; // Distance between this object and it's target
	public Transform targetBarricade;

	PlayerAttack attackControl;
	Animator m_Animator;
	float m_TurnAmount;
	float m_ForwardAmount;
	SphereCollider sightRange;
	UnitStats targetStats;

	
	// Use this for initialization
	void Start () {
		agent = GetComponentInChildren<NavMeshAgent>();
		m_Animator = GetComponent<Animator>();
		sightRange = GetComponent<SphereCollider>();
		attackControl = GetComponent<PlayerAttack>();
		targetBarricade = this.transform;
	}
	
	// Update is called once per frame
	void Update () {

		//Update animation and next target every frame.
		UpdateAnimator(agent.desiredVelocity);
		SetTarget();

		// Clear any action target that exceeds sight range.
		if (actionTarget != null && Vector3.Distance(actionTarget.position, gameObject.transform.position) > sightRange.radius) 
		{
			actionTarget = null;
		}

		// If there is an available action target move to that otheriwse go to the last choosen barricade. 
		if (actionTarget != null) Move();
		else SetDestination(targetBarricade);
	}

	void Move()
	{
		// Check to see if the agent is not already moving, if so set a new destination
		if (!agent.pathPending)
		{
			if (agent.remainingDistance <= agent.stoppingDistance)
			{
				if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
				{
					agent.SetDestination (actionTarget.position);
				}
			}
		}
	}

	public void SetDestination (Transform target){
		targetBarricade = target;
		agent.ResetPath();
		agent.SetDestination (target.position);
	}

	void SetTarget (){
		float curDistance = sightRange.radius;
		// Grab all available targets around character
		Collider[] possibleTargets = Physics.OverlapSphere (transform.position, sightRange.radius);

		// Search possible targets, choose clossest one with highest priority first.
		// Stop searching after getting the closest target avaialble of the highest priority is found
		foreach (string targetTag in priorityList)
		{
			foreach (Collider possibleTarget in possibleTargets)
			{
				if (possibleTarget.gameObject.tag == targetTag)
				{
					float distance = Vector3.Distance(possibleTarget.transform.position, transform.position);
					if (distance < curDistance)
					{
						curDistance = distance;
						actionTarget = possibleTarget.transform;
					}
				}
			}
			// If actionTarget is not null, target found
			// Assign target to attack script, return to not continue down priority list.
			if (actionTarget != null) 
			{
				attackControl.attackTarget = actionTarget;
				return;
			}

		}
	}

	void UpdateAnimator(Vector3 move) {
		//Set float values based on nav agent velocity
		if (move.magnitude > 1f) move.Normalize();
		move = transform.InverseTransformDirection(move);
		m_TurnAmount = Mathf.Atan2(move.x, move.z);
		m_ForwardAmount = move.z;
		
		// Update animator float values 
		m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
		m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
	}
}
	