using UnityEngine;
using System.Collections;

/*
 * USAGE:
 * =============================
 * Defines the logic for attacking a target
 * Used by the unit's AI script to enable the action
 * =============================
 * 
 * Date Created: 27 May 2015
 * Last Modified: 29 May 2015
 * Authors: Andrew Tully
 */

public class BasicAttack : MonoBehaviour 
{
    [SerializeField] public float damage = 5f;
    [SerializeField] public AudioClip punchSound;

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

    public void Punch(GameObject target)
    {
        targetHealth = target.GetComponent<UnitStats>();
        attacking = true;
        targetHealth.TakeDamage(damage);
        PunchEffects();
        attacking = false;
        Debug.Log("Punch!");
    }

    // Audio and Visual effects for punching
    void PunchEffects()
    {

    }
}
