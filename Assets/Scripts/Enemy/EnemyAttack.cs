using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

	public float damage = 5f;
	public float stunDuration = 1f;
	public float attackRadius = 5f;
	public ParticleSystem explosionParticles;           // Stores prefab of Particle System for the explosion

	private ParticleSystem explosionFX;                 // Stores the instance of the explosion Particle System

	public void Punch(GameObject target)
	{
		UnitStats targetHealth = target.GetComponent<UnitStats>();
		targetHealth.TakeDamage(damage);
		PunchEffects();
	}

	public void Slam(GameObject target)
	{		
		AreaOfEffect aoe = new AreaOfEffect();
		aoe.AreaStun(target.transform.position, attackRadius, damage, stunDuration, gameObject);	
		SlamEffects();
	}

	void Awake()
	{
		explosionFX = Instantiate(explosionParticles);
	}
	
	public void Explode(GameObject target)
	{		
		AreaOfEffect aoe = new AreaOfEffect();
		aoe.AreaExplode(target.transform.position, attackRadius, damage, gameObject);
		
		DestructEffects();
		Debug.Log("BOOM!");
	}
	
	// Audio and Visual effects for selfDestruct
	void DestructEffects()
	{
		explosionFX = Instantiate(explosionParticles);
		explosionFX.transform.position = gameObject.transform.position;
		explosionFX.Play();
	}

	// Audio and Visual effects for punching
	void SlamEffects()
	{
		
	}

	// Audio and Visual effects for punching
	void PunchEffects()
	{
		
	}
}
