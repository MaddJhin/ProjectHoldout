using UnityEngine;
using System.Collections;

/* USES:
 * ==============
 * UnitSight.cs
 * UnitStats.cs
 * MinionPunch.cs
 * ==============
 * 
 * USAGE:
 * ======================================
 * Acts as the "brain" of the minion unit
 * Uses other scripts to perform actions, E.G: seeing the player, attacking
 * Makes calls to other scripts to perform attacks, or to utilise stats
 * Enables modularity
 * ======================================
 * 
 * Date Created: 27 May 2015
 * Last Modified: 29 May 2015
 * Authors: Andrew Tully
 */

public class MinionUnit : MonoBehaviour 
{
    private NavMeshAgent agent;
    private SphereCollider actionRadius;
    private Animator anim;
    private UnitStats stats;
    private BasicAttack action;
    private UnitSight vision;
    private float elapsedTime;
    private Vector3 targetLoc;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        actionRadius = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        stats = GetComponent<UnitStats>();
        action = GetComponent<BasicAttack>();
        vision = GetComponent<UnitSight>();
        elapsedTime = 0f;
    }

    void Update()
    {
        // Update the target location
        targetLoc = vision.actionTarget.transform.position;

        if (stats.attackSpeed < elapsedTime && vision.targetDistance < agent.stoppingDistance)
        {
            elapsedTime = 0f;
            Attack();
        }

        else
            Move();

        elapsedTime += Time.deltaTime;
    }

    void Attack()
    {
        agent.Stop();
        Debug.Log(vision.actionTarget);
        action.Punch(vision.actionTarget);
    }

    void Move()
    {
        agent.Resume();
        agent.SetDestination(targetLoc);
    }
}
