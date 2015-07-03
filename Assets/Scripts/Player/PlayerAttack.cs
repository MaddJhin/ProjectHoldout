using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
	
	public float timeBetweenAttacks;        // The time between each shot.
	public float range;                     // The distance the gun can fire.
	public Transform attackTarget;			// Target to shoot
	public Transform shootPoint;

	Ray shootRay;                                   // A ray from the gun end forwards.
	RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
	int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
	ParticleSystem gunParticles;                    // Reference to the particle system.
	LineRenderer gunLine;                           // Reference to the line renderer.
	AudioSource gunAudio;                           // Reference to the audio source.
	Light gunLight;                                 // Reference to the light component.
	
	void Awake ()
	{
		// Create a layer mask for the Shootable layer.
		shootableMask = LayerMask.GetMask ("Enemy");
		
		// Set up the references.
		gunParticles = GetComponent<ParticleSystem> ();
		gunLine = GetComponentInChildren <LineRenderer> ();
		gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponentInChildren<Light> ();
	}
	
	public void DisableEffects ()
	{
		// Disable the line renderer and the light.
		gunLine.enabled = false;
		gunLight.enabled = false;
	}
	
	public void Shoot (float damage)
	{		
		// Set the shootRay so that it starts at the end of the shoot point and points forward from the barrel.
		shootRay.origin = shootPoint.position;
		shootRay.direction = transform.forward;
		
		// Perform the raycast against gameobjects on the shootable layer and if it hits something...
		if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
		{
			// Play the gun shot audioclip.
			gunAudio.Play ();
			
			// Enable the light.
			gunLight.enabled = true;
			
			// Stop the particles from playing if they were, then start the particles.
			//gunParticles.Stop ();
			//gunParticles.Play ();
			
			// Enable the line renderer and set it's first position to be the end of the gun.
			gunLine.enabled = true;
			gunLine.SetPosition (0, shootPoint.position);

			// Try and find an EnemyHealth script on the gameobject hit.
			UnitStats enemyHealth = attackTarget.gameObject.GetComponent<UnitStats>();
			
			// If the EnemyHealth component exist...
			if(enemyHealth != null)
			{
				// ... the enemy should take damage.
				enemyHealth.TakeDamage(damage);
			}
			
			// Set the second position of the line renderer to the point the raycast hit.
			gunLine.SetPosition (1, shootHit.point);
		}
		// If the raycast didn't hit anything on the shootable layer...
		// Nothing Happens

	}
	
	public void Attack(float damage)
	{
		UnitStats enemyHealth = attackTarget.gameObject.GetComponent<UnitStats>();
		enemyHealth.TakeDamage(damage);
	}

	public void Heal(float damage){
		UnitStats friendlyHealth = attackTarget.gameObject.GetComponent<UnitStats>();
		friendlyHealth.TakeDamage(damage);
	}
}
