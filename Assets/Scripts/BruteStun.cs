using UnityEngine;
using System.Collections;

public class BruteStun : MonoBehaviour {

    [SerializeField] public float damage = 5f;
    [SerializeField] public AudioClip attackSound;
    [SerializeField] public float duration = 3f;

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

    public void Slam(GameObject target)
    {
        targetHealth = target.GetComponent<UnitStats>();
        attacking = true;
        targetHealth.TakeDamage(damage);
        targetHealth.ApplyStatus(UnitStats.statusEffects.stun, duration);
        SlamEffects();
        attacking = false;
        Debug.Log("Slam!");
    }

    // Audio and Visual effects for punching
    void SlamEffects()
    {

    }
}
