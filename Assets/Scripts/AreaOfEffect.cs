﻿using UnityEngine;
using System.Collections;

public class AreaOfEffect : MonoBehaviour
{
    private int colliderIndex;
    private UnitStats cache;

    public void AreaStun(Vector3 center, float radius, float damage, float duration, GameObject source)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        colliderIndex = 0;

        Debug.Log("Potential Targets: " + hitColliders.Length);

        while (colliderIndex < hitColliders.Length)
        {
            if ((hitColliders[colliderIndex].gameObject.layer != source.layer) &&
                (cache = hitColliders[colliderIndex].gameObject.GetComponent<UnitStats>()))
            {
                Debug.Log("Valid Target" + hitColliders[colliderIndex]);
                //hitColliders[colliderIndex].gameObject.GetComponent<UnitStats>().TakeDamage(damage);
                //hitColliders[colliderIndex].gameObject.GetComponent<UnitStats>().ApplyStatus(UnitStats.statusEffects.stun, duration);
                cache.TakeDamage(damage);
                cache.ApplyStatus(UnitStats.statusEffects.stun, duration);
                Debug.Log("Target Attacked");
            }

            colliderIndex++;
        }
    }

    public void AreaExplode(Vector3 center, float radius, float damage, GameObject source)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        colliderIndex = 0;

        

        Debug.Log("Potential Targets: " + hitColliders.Length);

        while (colliderIndex < hitColliders.Length)
        {
            if ((hitColliders[colliderIndex].gameObject.layer != source.layer) &&
                (cache = hitColliders[colliderIndex].gameObject.GetComponent<UnitStats>()))
            {
                Debug.Log("Valid Target" + hitColliders[colliderIndex]);
                //hitColliders[colliderIndex].gameObject.GetComponent<UnitStats>().TakeDamage(damage);
                cache.TakeDamage(damage);
                Debug.Log("Target Attacked");
            }

            colliderIndex++;
        }
    }

}
