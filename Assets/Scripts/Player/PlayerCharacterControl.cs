using UnityEngine;
using System.Collections;

[RequireComponent(typeof (NavMeshAgent))]

public class PlayerCharacterControl : MonoBehaviour {

	public Transform target; // target to aim for
	public NavMeshAgent agent { get; private set; }

	Animator m_Animator;
	float m_TurnAmount;
	float m_ForwardAmount;
	
	// Use this for initialization
	void Start () {
		agent = GetComponentInChildren<NavMeshAgent>();
		m_Animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		UpdateAnimator(agent.desiredVelocity);
	}

	public void SetTarget(Vector3 target)
	{
		agent.SetDestination(target);
	}

	void UpdateAnimator(Vector3 move)
	{
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
	