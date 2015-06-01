using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* USAGE:
 * =========================
 * Used to detect when a player object is in range
 * Sets that object as the current target
 * Selected target used by the unit's AI script
 * =========================
 * 
 * NOTES:
 * =========================
 * May need to be removed and merged with specific unit AI scripts
 * Will allow for better control over how unit's prioritise targets
 * General solution may be too broad
 * =========================
 * 
 * Date Created: 27 May 2015
 * Last Modified: 29 May 2015
 * Authors: Andrew Tully
 */

public class UnitSight : MonoBehaviour 
{
    [SerializeField] public bool targetInRange;
    [SerializeField] public GameObject actionTarget;

    // Stores priority of GameObject tags
    // First element is the highest priority
    // E.G: if element[0] contains the string "Player", the "Player" tag is highest priority
    [SerializeField] public List<string> priorityList = new List<string>();
    private SphereCollider sightRange;
    private UnitStats targetStats;

    void Awake()
    {
        sightRange = GetComponent<SphereCollider>();
    }

    void Update()
    {
        // If target doesn't exit, it is not in range
        if (actionTarget == null)
        {
            targetInRange = false;
        }
    }

    // Checks if a potential target enters range
    void OnTriggerEnter(Collider other)
    {
        // Check each tag considered a priority
        foreach(string targetTag in priorityList)
        {
            // If a match is found then that gameObject becomes the new target
            // The first element is first priority etc
            if (other.gameObject.tag == targetTag && actionTarget == null)
            {
                actionTarget = other.gameObject;
                targetInRange = true;
                targetStats = actionTarget.GetComponent<UnitStats>();
            }
        }      
    }

    // If the current target leaves range, it helps to select a new target
    void OnTriggerStay(Collider other)
    {
        if (actionTarget == null)
        {
            // Check each tag considered a priority
            foreach (string targetTag in priorityList)
            {
                // If a match is found then that gameObject becomes the new target
                // The first element is first priority etc
                if (other.gameObject.tag == targetTag && actionTarget == null)
                {
                    actionTarget = other.gameObject;
                    targetInRange = true;
                    targetStats = actionTarget.GetComponent<UnitStats>();
                }
            }  
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == actionTarget)
        {
            actionTarget = null;
            targetInRange = false;
        }
    }
}
