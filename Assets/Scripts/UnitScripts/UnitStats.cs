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
    public float maxHealth = 100.0f;
    public float currentHealth;
    public float attackSpeed = 1.0f;
    public float attackRange = 1f;
    public float armor = 0.0f;
    public float healTreshold = 80f;

    public enum statusEffects { stun };

    private GameManager gm;

    void Awake()
    {
        currentHealth = maxHealth;
        gm = GameObject.FindObjectOfType<GameManager>();
    }

	void Update () 
    {
        if (currentHealth <= 0)
            KillUnit();
	}

    public void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;
        Debug.Log(gameObject.name + " HP: " + currentHealth);
    }

	public void Heal(float healAmount)
	{
        if (currentHealth < maxHealth)
        {
            currentHealth += healAmount;
            Debug.Log(currentHealth);
        }
	}

    public void KillUnit()
    {
        // Deactivates the unit
		gm.RemoveObjective();
        gameObject.SetActive(false);
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
