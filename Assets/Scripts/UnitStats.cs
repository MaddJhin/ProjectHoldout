using UnityEngine;
using System.Collections;


/* USAGE:
 * ===========================
 * Used to define shared attributes & actions of in-game units
 * Allows for easy number tweaking for the design team
 * Used by specific unit scripts to enable stats for that unit
 * ===========================
 * 
 * Date Created: 21 May 2015
 * Last Modified: 25 May 2015
 * Authors: Andrew Tully
 */


public class UnitStats : MonoBehaviour 
{
    // Unit attributes
    [SerializeField] public float health = 100.0f;
    [SerializeField] public float attackSpeed = 1.0f;
    [SerializeField] public float attackRange = 1f;
    [SerializeField] public float armor = 0.0f;

    public enum statusEffects { stun };

	void Update () 
    {
        if (health <= 0)
            KillUnit();
	}

    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
        Debug.Log(health);
    }

    public void KillUnit()
    {
        Destroy(gameObject);
    }

    public void ApplyStatus(statusEffects effect, float duration)
    {
        if (effect == statusEffects.stun)
        {
            Debug.Log("Stun detected for " + duration + " seconds");
            StartCoroutine(ActivateStun(duration));
        }
    }

    IEnumerator ActivateStun(float duration)
    {
        Debug.Log("Activating Stun effect");

        yield return null;
        yield return new WaitForSeconds(duration);

        Debug.Log("Stun Over");
    }
}
