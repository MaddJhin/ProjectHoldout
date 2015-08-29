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
    public bool targetInRange;
    public GameObject actionTarget;
    public string defaultTarget;
    public float sightDistance = 10f;

    // Stores priority of GameObject tags
    // First element is the highest priority
    // E.G: if element[0] contains the string "Player", the "Player" tag is highest priority
    public List<string> priorityList = new List<string>();

    public float targetDistance; // Distance between this object and it's target
    private SphereCollider sightRange;
    private UnitStats targetStats;

    // Setting up LayerMask for Player and Barricades
    private int playerMask;

    void Awake()
    {
        sightRange = GetComponentInChildren<SphereCollider>();
        playerMask = LayerMask.GetMask("Player");
    }

	void Start(){
		actionTarget = GameObject.Find(defaultTarget);
	}

    void Update()
    {
        SetTarget();

        // If target doesn't exit, switch to default target
        if (actionTarget == null)
        {
            actionTarget = GameObject.Find(defaultTarget);
        }

        // If the target is inactive, set it to null
        else if (actionTarget.activeInHierarchy == false)
            actionTarget = null;

        else if ((actionTarget.tag == "Player") &
                    (Vector3.Distance(actionTarget.transform.position, gameObject.transform.position) > sightDistance))
        {
            actionTarget = null;
        }

        // Calculate distance every update
        else
        {
            targetDistance = Vector3.Distance(actionTarget.transform.position, gameObject.transform.position);

        }
    }

    void SetTarget()
    {
        float curDistance = sightDistance;
        GameObject tempTarget = null;

        // Grab all available targets around the unit
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, sightDistance, playerMask);

        foreach (string targetTag in priorityList)
        {
            foreach (Collider possibleTarget in possibleTargets)
            {
                if (possibleTarget.gameObject.tag == targetTag)
                {
                    float distance = Vector3.Distance(possibleTarget.transform.position, transform.position);
                    if (distance < curDistance)
                    {
                        if (possibleTarget.gameObject != gameObject)
                        {
                            curDistance = distance;
                            tempTarget = possibleTarget.gameObject;
                        }
                    }
                }
            }
            // If actionTarget is not null, target found
            // Assign target to attack script, return to not continue down priority list.
            if (tempTarget != null)
            {
                actionTarget = tempTarget;
                return;
            }
        }
    }

    /*
    // Checks if a potential target enters range
    void OnTriggerEnter(Collider other)
    {
        // Check each tag considered a priority
        foreach(string targetTag in priorityList)
        {
            // If a match is found then that gameObject becomes the new target
            // The first element is first priority etc
            if (other.gameObject.tag == targetTag && (actionTarget == null || actionTarget == GameObject.Find(defaultTarget)) && other.isTrigger == false)
            {
                actionTarget = other.gameObject;
                targetInRange = true;
                targetStats = actionTarget.GetComponent<UnitStats>();
                targetDistance = Vector3.Distance(actionTarget.transform.position, gameObject.transform.position);
            }
        }      
    }

    // If the current target leaves range, it helps to select a new target
    void OnTriggerStay(Collider other)
    {
        if (actionTarget == null || actionTarget == GameObject.Find(defaultTarget))
        {
            // Check each tag considered a priority
            foreach (string targetTag in priorityList)
            {
                // If a match is found then that gameObject becomes the new target
                // The first element is first priority etc
                if (other.gameObject.tag == targetTag && (actionTarget == null ||actionTarget == GameObject.Find(defaultTarget)))
                {

                    Debug.Log(gameObject + " checking isTrigger");

                    // Checks if the target collider is a trigger
                    // If it's not, then the collider is valid for targetting
                    // Trigger colliders are used to represent a target's vision, thus are ignored
                    if (other.isTrigger == false)
                    {
                        Debug.Log(gameObject + " found no trigger. Assigning target");
                        actionTarget = other.gameObject;
                        targetInRange = true;
                        targetDistance = Vector3.Distance(actionTarget.transform.position, gameObject.transform.position);
                    }

                    else
                    {
                        Debug.Log(gameObject + " found trigger. Ignoring target");
                    }
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
    }*/
}
