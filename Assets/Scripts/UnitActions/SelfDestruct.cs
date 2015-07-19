using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour 
{
    [SerializeField]
    public float damage = 5f;
    public AudioClip attackSound;
    public float radius = 5f;
    public ParticleSystem explosionParticles;           // Stores prefab of Particle System for the explosion

    private Animator anim;
    private UnitStats targetHealth;
    private bool attacking;
    private float attackRange;
    private ParticleSystem explosionFX;                 // Stores the instance of the explosion Particle System

    void Awake()
    {
        anim = GetComponent<Animator>();
        explosionFX = Instantiate(explosionParticles);
    }

    void Update()
    {

    }

    public void Explode(GameObject target)
    {
        attacking = true;

        AreaOfEffect aoe = new AreaOfEffect();
        aoe.AreaExplode(target.transform.position, radius, damage, gameObject);
        
        DestructEffects();
        attacking = false;
        Debug.Log("BOOM!");
    }

    // Audio and Visual effects for punching
    void DestructEffects()
    {
        explosionFX.transform.position = gameObject.transform.position;
        explosionFX.Play();
    }
}
