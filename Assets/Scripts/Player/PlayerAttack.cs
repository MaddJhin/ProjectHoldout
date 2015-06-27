using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

	public float damage = 5f;
	public AudioClip punchSound;
	public Transform attackTarget;

	private Animator anim;
	private NavMeshAgent agent;
	private UnitStats targetHealth;
	private UnitStats playerStats;

	float nextAttack = 0f;
	public bool targetInRange = false;
	
	void Awake()
	{
		agent = GetComponent<NavMeshAgent>();

		anim = GetComponent<Animator>();
		playerStats = GetComponent<UnitStats>();
	}
	
	void Update()
	{
		// If there is nothing to attack, script does nothing.
		if (attackTarget == null) return;

		// Set if the target is in range
		if (Vector3.Distance (attackTarget.position, transform.position) <= agent.stoppingDistance)
		{
			targetInRange = true;
		}
		else
		{
			targetInRange = false;
		}

		// If the target is in range and enough time has passed between attacks, Attack.
		if (Time.time > nextAttack && targetInRange)
		{
			nextAttack = Time.time + playerStats.attackSpeed;;
			Attack();
		}
	}
	
	void Attack()
	{
		targetHealth = attackTarget.gameObject.GetComponent<UnitStats>();
		targetHealth.TakeDamage(damage);
	}
}
