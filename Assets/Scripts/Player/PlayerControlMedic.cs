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
 * Acts as the control for the medic player Character
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

public class PlayerControlMedic : MonoBehaviour {

	public float health = 100f;
	public float armor = 0f;
	public float sightRange = 10;
	public float healPerHit = 20f;
	public float healRange = 100f;
	public float timeBetweenHeals = 3f;
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
    UnitStats healTarget;                          // Current target to heal
    LayerMask healMask;                             // Layer Mask of heal targets
    ParticleSystem[] m_HealParticleSystem;            // Particles for healing

    Animator m_Animator;
    float m_ForwardAmount;
    float m_TurnAmount;
    bool m_Healing;
	
	void Awake(){
		agent = GetComponent<NavMeshAgent>();
		playerControl = GetComponent<PlayerMovement>();
		playerAction = GetComponent<PlayerAction>();
		stats = GetComponent<UnitStats>();
		obstacle = GetComponent<NavMeshObstacle>();
        m_Animator = GetComponent<Animator>();
        healMask = LayerMask.GetMask("Player");
        m_HealParticleSystem = GetComponentsInChildren<ParticleSystem>();
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
        m_Healing = false;

        foreach (var pfx in m_HealParticleSystem)
            pfx.enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () {

        UpdateAnimator(agent.desiredVelocity);

		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;
        healTarget = playerControl.SetHealTarget(gameObject.transform.position, healRange, healMask, "Player");
        

		// If there is nothing to attack, script does nothing.
		if (healTarget == null) 
		{
			agent.stoppingDistance = originalStoppingDistance;
			return;
		}
		
		// Set if the target is in range
		if (Vector3.Distance (healTarget.transform.position, transform.position) <= healRange)
		{
			Stop();
		}
		else
		{
			Move();
		}

		// If the target is in range and enough time has passed between attacks, Attack.
        if (m_Healing == false && targetInRange && healTarget != null)
        {
            m_Healing = true;
            transform.LookAt(healTarget.transform.position);

            foreach (var pfx in m_HealParticleSystem)
            {
                pfx.transform.position = healTarget.transform.position;
                pfx.enableEmission = true;
            }

            StartCoroutine(playerAction.Heal(healPerHit, healTarget, timeBetweenHeals));
        }

        if (healTarget.currentHealth >= healTarget.healTreshold)
        {
            healTarget = null;
        }


        if ( m_Healing == true && (!targetInRange || healTarget == null))
        {
            m_Healing = false;

            foreach (var pfx in m_HealParticleSystem)
            {
                pfx.enableEmission = false;
            }
        }  
	}

	void Stop()
    {
		targetInRange = true;
	}

	void Move ()
    {
		targetInRange = false;
	}
	
	void Heal()
    {
		timer = 0f;
        foreach (var pfx in m_HealParticleSystem)
        {
            pfx.transform.position = healTarget.transform.position;
            pfx.enableEmission = true;
        }
        
        Debug.Log("Healing: " + m_Healing);
        StartCoroutine(playerAction.Heal(healPerHit, healTarget, timeBetweenHeals));
        Debug.Log("Healing Finished");
        //m_Healing = false;
        Debug.Log("Healing: " + m_Healing);
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
        m_Animator.SetBool("Healing", m_Healing);
    }
}
