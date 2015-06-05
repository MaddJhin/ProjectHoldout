using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour 
{
    [SerializeField]
    public float damage = 5f;
    public AudioClip attackSound;
    public float radius = 5f;

    private Animator anim;
    private UnitStats targetHealth;
    private bool attacking;
    private float attackRange;

    void Awake()
    {
        anim = GetComponent<Animator>();
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

    }
}
